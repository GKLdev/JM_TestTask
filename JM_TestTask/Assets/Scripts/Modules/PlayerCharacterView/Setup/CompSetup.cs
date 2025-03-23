using Modules.CharacterControllerView_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.PlayerCharacterView
{
    public static class CompSetup
    {
        // *****************************
        // Setup
        // *****************************
        public static void Setup(State _state, CharacterControllerViewSetupData _data)
        {
            // Check if the module is initialized
            LibModuleExceptions.ExceptionIfNotInitialized(_state.dynamicData.isInitialized);

            // Store the character controller reference
            _state.dynamicData.characterController = _data.characterController;
        }
    }
}
