using CharacterControllerView_Public;
using GDTUtils;
using Modules.CharacterController_Public;
using Modules.CharacterControllerView_Public;
using Modules.CharacterFacade_Public;
using Modules.CharacterManager_Public;
using Modules.CharacterStatsSystem_Public;
using Modules.DamageManager_Public;
using Modules.ModuleManager_Public;
using Modules.TimeManager_Public;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Modules.CharacterController
{
    public class CharacterController : LogicBase, ICharacterController, IEntityModifcation
    {
        public Vector3 P_Position { get => state.transform.position; set => state.transform.position = value; }
        public Quaternion P_Rotation { get => state.transform.rotation; set => state.transform.rotation = value; }
        public Vector3 P_Orientation { get => state.transform.forward; set => state.transform.forward = value; }

        [Inject]
        private IModuleManager moduleMgr;

        [SerializeField]
        private State state = new();

        // *****************************
        // InitModule
        // *****************************
        public void InitModule()
        {
            CompInit.Init(state, moduleMgr.Container);
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
            state.transform.gameObject.SetActive(true);
        }

        // *****************************
        // OnSlept
        // *****************************
        public void OnSlept()
        {
            CompInit.ResetState(state);
            state.transform.gameObject.SetActive(false);
        }

        // *****************************
        // Dispose
        // *****************************
        public void Dispose()
        {
            // TODO
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

            _view.P_GameObjectAccess.transform.position = P_Position;
            _view.P_GameObjectAccess.transform.rotation = P_Rotation;
            _view.P_GameObjectAccess.transform.SetParent(state.viewRoot);

            state.dynamicData.generalData.view = _view;
            state.dynamicData.generalData.view.InitModule();

            // Try get PlayerView
            state.dynamicData.generalData.playerView = state.dynamicData.generalData.view as IPlayerView;
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

            return state.dynamicData.generalData.view;
        }

        // *****************************
        // ModifyEntity
        // *****************************
        public void ModifyEntity(EntityModifcationType _stat, float _value)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.generalData.isInitialized);

            switch (_stat)
            {
                case EntityModifcationType.Speed:
                    state.dynamicData.modficationData.speedMod = _value;
                    break;
                default:
                    break;
            }
        }

        // *****************************
        // OnDeath
        // *****************************
        public void OnDeath()
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.generalData.isInitialized);
            SetNavigationMode(NavigationMode.DirectControl);
            Toggle(false);
            state.dynamicData.generalData.view.OnDeath();

            // may be caled after some delay to player death anim beforehand.
            state.dynamicData.generalData.characterMgr.RemoveCharacter(state.facade.Value);
        }

        // *****************************
        // OnDamage
        // *****************************
        public void OnDamage(IDamageable _damageable)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.generalData.isInitialized);
            state.dynamicData.generalData.view.OnDamage(_damageable);
        }
    }

    [System.Serializable]
    public class State
    {
        public Transform    transform;    
        public Collider     characterCollider;
        
        public ConfigCharacterController config;
        public NavMeshAgent navAgent;

        public Transform viewRoot;

        public SerializedInterface<ICharacterFacade> facade;

        public DynamicData dynamicData = new();

        public class DynamicData
        {
            public GeneralData generalData = new();
            public MovementData movementData = new();
            public RotationData rotationData = new();
            public CollisionData collisionData = new();
            public ModificationData modficationData = new();
            public class GeneralData
            {
                public bool                     isInitialized = false;
                public bool                     isEnabled = false;
                public TimeLayerType            timeLayer;
                public ITimeManager             timeMgr;
                public ICharacterManager        characterMgr;
                public float                    deltaTime;
                public ICharacterControllerView view;
                public IPlayerView              playerView;
            }

            public class MovementData
            {
                public IDynamicAxis movementAxisX;
                public IDynamicAxis movementAxisZ;
                public Vector3 targetPosition;
                public bool hasTargetPosition = false;
                public Vector3 currentVelocity = Vector3.zero;
                public float verticalVelocity = 0f;
                public bool isGrounded = false;
                public Vector3 desiredPosition;
                public Vector2 inputDirection = Vector2.zero;
                public NavigationMode navigationMode = NavigationMode.DirectControl;

                public float currentMaxSpeed;
                public float currentMaxStrafeSpeed;

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
                    currentMaxSpeed = 0f;
                    currentMaxStrafeSpeed = 0f;

            }
        }

            public class RotationData
            {
                public IDynamicAxis rotationAxis;
                public Vector3 targetLookDirection = Vector3.zero;
                public Vector2 relativeLookAngles = Vector2.zero;
                public float verticalLookAngle = 0f;

                // Reset all rotation data
                public void Reset()
                {
                    rotationAxis?.ResetAxis();
                    targetLookDirection = Vector3.zero;
                    relativeLookAngles = Vector2.zero;
                    verticalLookAngle = 0f;
                }
            }

            public class ModificationData
            {
                public float speedMod = 0f;

                // Reset mods data
                public void Reset()
                {
                    speedMod  = 0f;
                }
            }

            public class CollisionData
            {
                public Collider[] colliderBuffer;  // Buffer for colliders to avoid allocations
            }
        }
    }
}