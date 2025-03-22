using Modules.CharacterController_Public;
using Modules.CharacterControllerView_Public;
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
    }

    [System.Serializable]
    public class State
    {
        public Transform root;

        // Cached assert messages to avoid GC allocations
        private const string AssertMsgStateRootsNull = "StateRoots array is not assigned in CharacterControllerView.";
        private const string AssertMsgStateRootNull = "State root at index {0} is not assigned in CharacterControllerView.";

        // Array of state roots, indexed by VisualState enum (Idle = 0, Dead = 1)
        [SerializeField]
        private UnityEngine.GameObject[] stateRoots;

        public DynamicData dynamicData = new();

        public class DynamicData
        {
            public bool isInitialized = false;
            // References to dependencies
            public ICharacterController characterController = null;
            // Current visual state
            public VisualState currentState = VisualState.Idle;

            public void Reset()
            {
                characterController = null;
                currentState = VisualState.Idle;
            }
        }

        // Getter for accessing state roots array
        public UnityEngine.GameObject[] StateRoots => stateRoots;

        // Getter for assert messages
        public string AssertMsgStateRootsNullValue => AssertMsgStateRootsNull;
        public string AssertMsgStateRootNullValue => AssertMsgStateRootNull;
    }
}
