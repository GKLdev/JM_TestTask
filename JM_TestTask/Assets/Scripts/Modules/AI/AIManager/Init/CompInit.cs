using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.AIManager
{
    public static class CompInit
    {
        // *****************************
        // Init 
        // *****************************
        public static void Init(State _state)
        {
            _state.dynamic.addQueue.Clear();
            _state.dynamic.removeQueue.Clear();
            _state.dynamic.updateList.Clear();

           _state.initialized = true;
        }
    }
}