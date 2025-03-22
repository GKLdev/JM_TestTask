using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.GameDirector
{
    public static class CompUpdate
    {
        // *****************************
        // OnUpdate
        // *****************************
        public static void OnUpdate(State _state)
        {
            // Update the state machine
            _state.dynamicData.stateMachine.Update();
        }
    }
}
