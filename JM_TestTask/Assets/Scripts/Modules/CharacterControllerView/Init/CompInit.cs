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
            if (_state.StateRoots == null)
            {
                Debug.Assert(false, "StateRoots array is not assigned in CharacterControllerView.");
                return;
            }

            for (int i = 0; i < _state.StateRoots.Length; i++)
            {
                if (_state.StateRoots[i] == null)
                {
                    Debug.Assert(false, $"State root at index {i} is not assigned in CharacterControllerView.");
                    return;
                }
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
