using Modules.InputManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.InputManager
{
    public static class CompInit
    {
        // *****************************
        // Init
        // *****************************
        public static void Init(State _state)
        {
            // Mark module as initialized
            _state.dynamicData.isInitialized = true;
            _state.dynamicData.currentContext = InputContext.Undef;
        }
    }
}
