using GDTUtils;
using Modules.ModuleManager_Public;
using Modules.ReferenceDb_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace Modules.DamageManager
{
    public static class CompInit
    {
        // *****************************
        // Init 
        // *****************************
        public static void Init(State _state, IModuleManager _moduleMgr)
        {
            _state.dynamic.referenceConfig = _moduleMgr.Container.Resolve<ReferenceDbAliasesConfig>();

            // finish
            _state.initialized = true;
        }

        // *****************************
        // Dispose 
        // *****************************
        public static void Dispose(State _state)
        {
            _state.dynamic.colliderToDamageable.Clear();
            _state.dynamic.damageableToCollider.Clear();

            _state.initialized = false;
        }
    }
}