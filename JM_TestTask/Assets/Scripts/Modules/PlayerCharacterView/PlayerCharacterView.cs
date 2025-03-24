using CharacterControllerView_Public;
using GDTUtils;
using Modules.CharacterController_Public;
using Modules.CharacterControllerView_Public;
using Modules.DamageManager_Public;
using Modules.ModuleManager_Public;
using Modules.PlayerCharacterView_Public;
using Modules.PlayerWeapon_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Modules.PlayerCharacterView
{
    public class PlayerCharacterView : LogicBase, ICharacterControllerView, IPlayerView
    {
        public GameObject P_GameObjectAccess => state.root.gameObject;

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
            if (!state.dynamicData.isInitialized)
            {
                return;
            }

            CompUpdate.OnUpdate(state);
        }

        // *****************************
        // SetVisualState
        // *****************************
        public void SetVisualState(VisualState _type)
        {
        }

        // *****************************
        // Setup
        // *****************************
        public void Setup(CharacterControllerViewSetupData _data)
        {
            CompSetup.Setup(state, _data);
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
            bool needSetActive = !gameObject.activeInHierarchy;
            if (needSetActive)
            {
                gameObject.SetActive(true);
            }
        }

        // *****************************
        // OnSlept
        // *****************************
        public void OnSlept()
        {
            state.dynamicData.Reset();

            bool needSetInnactive = gameObject.activeInHierarchy;
            if (needSetInnactive)
            {
                gameObject.SetActive(false);
            }
        }

        // *****************************
        // Dispose
        // *****************************
        public void Dispose()
        {
        }

        // *****************************
        // SetHeadAngle
        // *****************************
        public void SetHeadAngles(Vector2 _angles)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);
            state.dynamicData.headRelativeRotation = _angles;
        }

        // *****************************
        // OnDeath
        // *****************************
        public void OnDeath()
        {
        }

        // *****************************
        // OnDamage
        // *****************************
        public void OnDamage(IDamageable _damageable)
        {
        }
    }

    [System.Serializable]
    public class State
    {
        public Transform root;
        public Transform head;

        public ConfigPlayerCharacterView            config;
        public SerializedInterface<IPlayerWeapon>   debugWeapon;

        public DynamicData dynamicData = new();

        public class DynamicData
        {
            public bool isInitialized = false;

            public ICharacterController characterController     = null;
            public Vector2              headRelativeRotation    = Vector2.zero;

            public void Reset()
            {
                characterController = null;
                headRelativeRotation = Vector2.zero;
            }
        }
    }
}
