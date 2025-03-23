using Modules.ModuleManager_Public;
using Modules.PlayerProgression_Public;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Modules.PlayerProgression
{
    public class PlayerProgression : LogicBase, IPlayerProgression
    {
        [Inject]
        private IModuleManager moduleMgr;

        [SerializeField]
        private State state = new();

        public event Action<StatChangeData> OnPlayerStatChanged;

        // *****************************
        // InitModule
        // *****************************
        public void InitModule()
        {
            CompInit.Init(state);
        }

        // *****************************
        // SetStatValue
        // *****************************
        void IPlayerProgression.SetStatValue(int _val, string _alias)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);
            CompStatManagement.SetStatValue(state, _val, _alias, this);
        }

        // *****************************
        // GetStatValue
        // *****************************
        int IPlayerProgression.GetStatValue(string _alias)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);
            return CompStatManagement.GetStatValue(state, _alias);
        }

        // *****************************
        // RaiseStatChangedEvent
        // *****************************
        public void RaiseStatChangedEvent(string _alias, int _value)
        {
            OnPlayerStatChanged?.Invoke(new StatChangeData() { alias = _alias , currentValue = _value });
        }
    }

    [System.Serializable]
    public class State
    {
        public ConfigPlayerProgression config;

        public DynamicData dynamicData = new();

        public class DynamicData
        {
            public bool isInitialized = false;
            public Dictionary<string, PlayerStatContainer> stats = new();

            public void Reset()
            {
                stats.Clear();
            }
        }
    }

    public class PlayerStatContainer
    {
        public int value;
        public int min;
        public int max;
    }
}
