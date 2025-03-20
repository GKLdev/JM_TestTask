using GDTUtils;
using Modules.CharacterController_Public;
using Modules.ModuleManager_Public;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Modules.CharacterController
{
    public class CharacterController : MonoBehaviour, ICharacterController
    {
        [Inject]
        private IModuleManager moduleMgr;

        [SerializeField]
        private State state = new();

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
        // ToggleNavigationMode
        // *****************************
        public void ToggleNavigationMode(NavigationMode _mode)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.generalData.isInitialized);
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
            }

            public class RotationData
            {
                public IDynamicAxis rotationAxis;                   // For angular rotation
                public Vector3 targetLookDirection = Vector3.zero;  // Absolute look direction
                public Vector2 relativeLookAngles = Vector2.zero;   // Relative Euler angles (x: yaw, y: pitch)
                public bool hasRelativeLookAngles = false;          // Flag to indicate if relative angles are set
                public float verticalLookAngle = 0f;                // Current vertical look angle
            }

            public class CollisionData
            {
                public Collider[] colliderBuffer;  // Buffer for colliders to avoid allocations
            }
        }
    }
}