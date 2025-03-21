using Modules.CharacterControllerView_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.CharacterControllerView
{
    public static class CompVisualState
    {
        // *****************************
        // SetVisualState
        // *****************************
        public static void SetVisualState(State _state, VisualState _type)
        {
            // Check if the module is initialized
            LibModuleExceptions.ExceptionIfNotInitialized(_state.dynamicData.isInitialized);

            // If the state hasn't changed, do nothing
            if (_state.dynamicData.currentState == _type)
            {
                return;
            }

            // Update the state
            _state.dynamicData.currentState = _type;

            // Show the new state
            ShowState(_state, _type);
        }

        // *****************************
        // ShowState
        // *****************************
        public static void ShowState(State _state, VisualState _stateToShow)
        {
            for (int i = 0; i < _state.StateRoots.Length; i++)
            {
                bool isActive = i == (int)_stateToShow;
                if (_state.StateRoots[i] != null)
                {
                    _state.StateRoots[i].SetActive(isActive);
                }
            }
        }
    }
}