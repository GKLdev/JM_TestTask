using Modules.GameDirector_Public;
using Modules.ModuleManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Modules.GameDirector
{
    public class GameDirector : LogicBase, IGameDirector
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
            CompInit.Init(state, moduleMgr.Container);
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
    }

    [System.Serializable]
    public class State
    {

        // Dynamic data
        public DynamicData dynamicData = new();

        public class DynamicData
        {
            public DiContainer  container;
            public bool         isInitialized = false;

            public GDTUtils.StateMachine.IStateMachine<GameDirectorNodeType, State> stateMachine;
        }
    }

    public enum GameDirectorNodeType
    {
        Null,
        SimpleGameplayTest
    }
}
