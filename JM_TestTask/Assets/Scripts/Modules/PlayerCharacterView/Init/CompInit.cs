using Modules.CharacterControllerView_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.PlayerCharacterView
{
    public static class CompInit
    {
        // *****************************
        // Init
        // *****************************
        public static void Init(State _state)
        {
            // Reset dynamic state
            _state.dynamicData.Reset();

            _state.debugWeapon.Value.InitModule();

            // Mark the module as initialized
            _state.dynamicData.isInitialized = true;
        }
    }
}