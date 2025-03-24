using Modules.CharacterController_Public;
using Modules.CharacterControllerView_Public;
using Modules.DamageManager_Public;
using Modules.ModuleManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Modules.CharacterControllerView
{
    public class CharacterControllerView : LogicBase, ICharacterControllerView
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
            CompVisualState.SetVisualState(state, _type);
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
                SetVisualState(VisualState.Idle);
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
        public void SetHeadAngle(float _angle)
        {
            throw new System.NotImplementedException();
        }

        // *****************************
        // OnDeath
        // *****************************
        public void OnDeath()
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);
            SetVisualState(VisualState.Dead);
        }

        // *****************************
        // OnDamage
        // *****************************
        public void OnDamage(IDamageable _damageable)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);
        }
    }

    [System.Serializable]
    public class State
    {
        public Transform root;

        // Array of state roots, indexed by VisualState enum (Idle = 0, Dead = 1)
        [SerializeField]
        private UnityEngine.GameObject[] stateRoots;

        public DynamicData dynamicData = new();

        public class DynamicData
        {
            public bool isInitialized = false;

            // Current visual state
            public VisualState          currentState        = VisualState.Idle;
            public ICharacterController characterController = null;

            public void Reset()
            {
                characterController = null;
                currentState        = VisualState.Idle;
            }
        }

        // Getter for accessing state roots array
        public UnityEngine.GameObject[] StateRoots => stateRoots;
    }
}
