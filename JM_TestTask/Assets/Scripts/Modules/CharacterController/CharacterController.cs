using GDTUtils;
using Modules.CharacterController_Public;
using Modules.CharacterControllerView_Public;
using Modules.ModuleManager_Public;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Modules.CharacterController
{
    public class CharacterController : LogicBase, ICharacterController
    {
        [Inject]
        private IModuleManager moduleMgr;

        [SerializeField]
        private State state = new();

        public Vector3 P_Position       { get => state.transform.position; set => state.transform.position = value; }
        public Quaternion P_Rotation    { get => state.transform.rotation; set => state.transform.rotation = value; }
        public Vector3 P_Orientation    { get => state.transform.forward; set => state.transform.forward = value; }

        // *****************************
        // InitModule
        // *****************************
        public void InitModule()
        {
            CompInit.Init(state);
        }

        // *****************************
        // OnUpdate
        // *****************************
        public void OnUpdate()
        {
            if (!state.dynamicData.generalData.isInitialized || !state.dynamicData.generalData.isEnabled)
            {
                return;
            }

            CompUpdate.OnUpdate(state);
        }

        // *****************************
        // Move
        // *****************************
        public void Move(Vector2 _dir)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.generalData.isInitialized);
            state.dynamicData.movementData.inputDirection = _dir;
        }

        // *****************************
        // MoveToTarget
        // *****************************
        public void MoveToTarget(Vector3 _pos)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.generalData.isInitialized);
            state.dynamicData.movementData.targetPosition = _pos;
            state.dynamicData.movementData.hasTargetPosition = true;
        }

        // *****************************
        // Toggle
        // *****************************
        public void Toggle(bool _val)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.generalData.isInitialized);
            state.dynamicData.generalData.isEnabled = _val;
        }

        // *****************************
        // SetNavigationMode
        // *****************************
        public void SetNavigationMode(NavigationMode _mode)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.generalData.isInitialized);

            // Reset movement and rotation data before switching modes
            state.dynamicData.movementData.Reset();
            state.dynamicData.rotationData.Reset();

            // Set the new navigation mode and update Navmesh agent state
            state.dynamicData.movementData.navigationMode = _mode;
            CompNavmesh.UpdateNavmeshAgentState(state);
        }

        // *****************************
        // LookDirection
        // *****************************
        public void LookDirection(Vector3 _dir)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.generalData.isInitialized);

            bool atNavmeshMode = state.dynamicData.movementData.navigationMode == NavigationMode.Navmesh;
            if (atNavmeshMode)
            {
                return;
            }

            state.dynamicData.rotationData.targetLookDirection = _dir.normalized;
            state.dynamicData.rotationData.hasRelativeLookAngles = false;
        }

        // *****************************
        // LookDirectionRelative
        // *****************************
        public void LookDirectionRelative(Vector2 _angles)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.generalData.isInitialized);

            bool atNavmeshMode = state.dynamicData.movementData.navigationMode == NavigationMode.Navmesh;
            if (atNavmeshMode)
            {
                return;
            }

            state.dynamicData.rotationData.relativeLookAngles = _angles;
            state.dynamicData.rotationData.hasRelativeLookAngles = true;
        }

        // *****************************
        // OnAdded
        // *****************************
        public void OnAdded()
        {
        }

        // *****************************
        // OnAwake
        // *****************************
        public void OnAwake()
        {
        }

        // *****************************
        // OnSlept
        // *****************************
        public void OnSlept()
        {
        }

        // *****************************
        // Dispose
        // *****************************
        public void Dispose()
        {
        }

        // *****************************
        // AttachVisual
        // *****************************
        public void AttachVisual(ICharacterControllerView _view)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.generalData.isInitialized);

            if (_view == null)
            {
                Debug.Assert(false, "_view is Null.");
            }

            //_view.P_GameObjectAccess.transform.position = P_Position;
            //_view.P_GameObjectAccess.transform.rotation = P_Rotation;
            //_view.P_GameObjectAccess.transform.SetParent(state.viewRoot);

            state.dynamicData.generalData.view = _view;
            state.dynamicData.generalData.view.InitModule();
        }

        // *****************************
        // SetupConfig
        // *****************************
        public void SetupConfig(ConfigCharacterController _config)
        {
            if (state.dynamicData.generalData.isInitialized)
            {
                Debug.Assert(false, "Cant setup config if character controller is initialized!");
            }

            state.config = _config;
        }

        // *****************************
        // GetView
        // *****************************
        public ICharacterControllerView GetView()
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.generalData.isInitialized);

            throw new System.NotImplementedException();
        }
    }

    [System.Serializable]
    public class State
    {
        public Transform transform;
        
        [SerializeField]
        public Transform verticalLookTransform;
        
        [SerializeField]
        public Collider characterCollider;
        
        [SerializeField]
        public ConfigCharacterController config;
        public NavMeshAgent navAgent;

        public Transform viewRoot;

        public DynamicData dynamicData = new();

        public class DynamicData
        {
            public GeneralData generalData = new();
            public MovementData movementData = new();
            public RotationData rotationData = new();
            public CollisionData collisionData = new();

            public class GeneralData
            {
                public bool isInitialized = false;
                public bool isEnabled = false;

                public ICharacterControllerView view;
            }

            public class MovementData
            {
                public IDynamicAxis movementAxisX;      // For velocity along X axis
                public IDynamicAxis movementAxisZ;      // For velocity along Z axis
                public Vector3 targetPosition;          // Target position for Navmesh mode
                public bool hasTargetPosition = false;  // Indicates if target position is set
                public Vector3 currentVelocity = Vector3.zero; // Current movement velocity
                public float verticalVelocity = 0f;     // Vertical velocity for gravity
                public bool isGrounded = false;         // Indicates if the character is grounded
                public Vector3 desiredPosition;         // Desired position after movement and collision resolution
                public Vector2 inputDirection = Vector2.zero;
                public NavigationMode navigationMode = NavigationMode.DirectControl;

                // Reset all movement data
                public void Reset()
                {
                    movementAxisX?.ResetAxis();
                    movementAxisZ?.ResetAxis();
                    targetPosition = Vector3.zero;
                    hasTargetPosition = false;
                    currentVelocity = Vector3.zero;
                    verticalVelocity = 0f;
                    isGrounded = false;
                    desiredPosition = Vector3.zero;
                    inputDirection = Vector2.zero;
                    navigationMode = NavigationMode.DirectControl;
                }
            }

            public class RotationData
            {
                public IDynamicAxis rotationAxis;                   // For angular rotation
                public Vector3 targetLookDirection = Vector3.zero;  // Absolute look direction
                public Vector2 relativeLookAngles = Vector2.zero;   // Relative Euler angles (x: yaw, y: pitch)
                public bool hasRelativeLookAngles = false;          // Flag to indicate if relative angles are set
                public float verticalLookAngle = 0f;                // Current vertical look angle

                // Reset all rotation data
                public void Reset()
                {
                    rotationAxis?.ResetAxis();
                    targetLookDirection = Vector3.zero;
                    relativeLookAngles = Vector2.zero;
                    hasRelativeLookAngles = false;
                    verticalLookAngle = 0f;
                }
            }

            public class CollisionData
            {
                public Collider[] colliderBuffer;  // Buffer for colliders to avoid allocations
            }
        }
    }
}