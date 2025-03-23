using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.PlayerCharacterView
{
    public static class CompUpdate
    {
        // *****************************
        // OnUpdate
        // *****************************
        public static void OnUpdate(State _state)
        {
            // Update head logic
            CompHead.OnUpdate( _state );
        }
    }
}
