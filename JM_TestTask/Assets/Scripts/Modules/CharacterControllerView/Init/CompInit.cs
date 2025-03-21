using Modules.CharacterControllerView_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Modules.CharacterControllerView
{
    public static class CompInit
    {
        // *****************************
        // Init
        // *****************************
        public static void Init(State _state)
        {
            // Check that all state roots are assigned
            UnityEngine.Debug.Assert(_state.StateRoots != null, _state.AssertMsgStateRootsNullValue);
            for (int i = 0; i < _state.StateRoots.Length; i++)
            {
                UnityEngine.Debug.Assert(_state.StateRoots[i] != null, string.Format(_state.AssertMsgStateRootNullValue, i));
            }

            // Reset dynamic state
            _state.dynamicData.Reset();

            // Show default state (Idle, index 0)
            CompVisualState.ShowState(_state, VisualState.Idle);

            // Mark the module as initialized
            _state.dynamicData.isInitialized = true;
        }
    }
}
