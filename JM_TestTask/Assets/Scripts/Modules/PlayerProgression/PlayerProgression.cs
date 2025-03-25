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
        public event Action                 OnUpgradePointsCountChanged;

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

        // *****************************
        // RaiseStatChangedEvent
        // *****************************
        public void AddUpgradePoints(int _val)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);

            _val = Mathf.Abs(_val);
            state.dynamicData.totalUpgradePoints += _val;
            state.dynamicData.spareUpgradePoints += _val;

            OnUpgradePointsCountChanged?.Invoke();
        }

        // *****************************
        // RaiseStatChangedEvent
        // *****************************
        public int GetAvailableUpgradePointsCount()
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);
            return state.dynamicData.spareUpgradePoints;
        }

        // *****************************
        // SpendUpgradePoints
        // *****************************
        public void SpendUpgradePoints(int _val, string _alias)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);
            CompStatManagement.SpendUpgradePoints(state, _val, _alias, this);
            OnUpgradePointsCountChanged?.Invoke();
        }

        // *****************************
        // RefundUpgradePoints
        // *****************************
        public void RefundUpgradePoints(int _val, string _alias)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);
            CompStatManagement.RefundUpgradePoints(state, _val , _alias , this);
            OnUpgradePointsCountChanged?.Invoke();
        }

        // *****************************
        // ResetUpgradePoints
        // *****************************
        public void ResetUpgradePoints()
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);
            state.dynamicData.totalUpgradePoints = 0;
            state.dynamicData.spareUpgradePoints = 0;
        }

        // *****************************
        // GetStatLimits
        // *****************************
        public void GetStatLimits(string _alias, out int _min, out int _max)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);
            CompStatManagement.GetStatLimits(state, _alias, out _min, out _max);
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

            public int totalUpgradePoints;
            public int spareUpgradePoints;

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
