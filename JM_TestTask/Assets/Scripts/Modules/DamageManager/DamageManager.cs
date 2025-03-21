using GDTUtils;
using Modules.DamageManager_Public;
using Modules.ModuleManager_Public;
using Modules.ReferenceDb_Public;
using Modules.TimeManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Modules.DamageManager
{
    public class DamageManager : LogicBase, IDamageManager
    {
        [SerializeField]
        State state;

        [Inject]
        IModuleManager moduleMgr;

        // *****************************
        // InitModule
        // *****************************
        public void InitModule()
        {
            if (state.initialized)
            {
                return;
            }

            CompInit.Init(state, moduleMgr);
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
        }

        // *****************************
        // RegiterDamageable
        // *****************************
        public void RegisterDamageable(IDamageable _component, Collider _collider)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.initialized);
            CompDamageableRegistration.Register(state, _component, _collider);
        }

        // *****************************
        // UnregisterDamageable
        // *****************************
        public void UnregisterDamageable(IDamageable _component)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.initialized);
            CompDamageableRegistration.Unregister(state, _component);
        }

        // *****************************
        // GetDamageableFromCollision
        // *****************************
        public IDamageable GetDamageableByCollision(Collider _collider)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.initialized);

            return CompDamageableRegistration.GetDamageable(state, _collider);
        }

        // *****************************
        // Dispose
        // *****************************
        public void Dispose()
        {
            if (!state.initialized)
            {
                return;
            }

            moduleMgr = null;
            CompInit.Dispose(state);
        }
    }

    // *****************************
    // State
    // *****************************
    [System.Serializable]
    public class State
    {
        public bool initialized = false;

        public ConfigDamageManager  config;
        public Transform            fallbackRoot;
        public Transform[]          poolRoots;
        public DynamicData          dynamic = new();

        // *****************************
        // DynamicData
        // *****************************
        [System.Serializable]
        public class DynamicData
        {
            public Dictionary<Collider, IDamageable> colliderToDamageable = new();
            public Dictionary<IDamageable, Collider> damageableToCollider = new();

            public ReferenceDbAliasesConfig         referenceConfig;
            public ITimeManager                     timeMng;
            public TimeManager_Public.TimeLayerType timeLayer;
        }
    }
}