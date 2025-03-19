using GDTUtils;
using Modules.CharacterController_Public;
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
            float floatPrecision = state.config.P_FloatPrecision;
            if (_dir.sqrMagnitude < floatPrecision)
            {
                throw new CharacterControllerException("Look direction cannot be zero.");
            }
            state.dynamicData.rotationData.targetLookDirection = _dir;
        }
    }

    [System.Serializable]
    public class State
    {
        public Transform                    transform;
        public Transform                    verticalLookTransform;
        public ConfigCharacterController    config;
        public NavMeshAgent                 navAgent;
        public Collider                     characterCollider;

        public DynamicData dynamicData = new();

        public class DynamicData
        {
            public GeneralData      generalData     = new();
            public MovementData     movementData    = new();
            public RotationData     rotationData    = new();
            public CollisionData    collisionData   = new();

            public class GeneralData
            {
                public bool isInitialized = false;
                public bool isEnabled = false;
            }

            public class MovementData
            {
                public IDynamicAxis movementAxisX;
                public IDynamicAxis movementAxisZ;
                public Vector3 targetPosition;
                public bool hasTargetPosition = false;
                public Vector3 currentVelocity = Vector3.zero;
                public Vector3 desiredPosition;
                public Vector2 inputDirection = Vector2.zero;
                public NavigationMode navigationMode = NavigationMode.DirectControl;
                public bool isGrounded = false;
                public float verticalVelocity = 0f;
            }

            public class RotationData
            {
                public IDynamicAxis rotationAxis;   // For angular rotation
                public Vector3 targetLookDirection = Vector3.zero;
                public float verticalLookAngle = 0f;
            }

            public class CollisionData
            {
                public Collider[] colliderBuffer;  // Buffer for colliders to avoid allocations
            }
        }
    }
}
