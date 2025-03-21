using Modules.AIBrain_Public;
using Modules.AIManager_Public;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.AIManager
{
    public class AIManager : LogicBase, IAIManager
    {
        [SerializeField]
        State state;

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
            if (!state.initialized)
            {
                return;
            }

            CompUpdate.Update(state);
        }

        // *****************************
        // Register 
        // *****************************
        public void Register(IAIBrain _brain)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.initialized);
            state.dynamic.addQueue.Add(_brain);
            state.dynamic.addQueueTriggered = true;
        }

        // *****************************
        // Unregister 
        // *****************************
        public void Unregister(IAIBrain _brain)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.initialized);
            state.dynamic.removeQueue.Add(_brain);
            state.dynamic.removeQueueTriggered = false;
        }
    }

    // *****************************
    // State
    // *****************************
    [System.Serializable]
    public class State
    {
        [HideInInspector]
        public bool         initialized     = false;
        public DynamicData  dynamic         = new();

        // *****************************
        // DynamicData
        // *****************************
        //[System.Serializable]
        public class DynamicData
        {
            public List<IAIBrain> addQueue      = new();
            public List<IAIBrain> removeQueue   = new();
            public List<IAIBrain> updateList    = new();

            public bool addQueueTriggered       = false;
            public bool removeQueueTriggered    = false;
        }
    }
}