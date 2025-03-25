using Modules.InputManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.InputManager
{
    public static class CompContextHandler
    {
        // *****************************
        // SetContext
        // *****************************
        public static void SetContext(State _state, InputContext _context)
        {
            // Update current context
            _state.dynamicData.currentContext = _context;

            // Handle context switch
            switch (_context)
            {
                case InputContext.Undef:
                    HandleUndefContext(_state);
                    break;
                case InputContext.World:
                    HandleWorldContext(_state);
                    break;
                case InputContext.UI:
                    HandleUIContext(_state);
                    break;
            }
        }

        // *****************************
        // HandleUndefContext
        // *****************************
        private static void HandleUndefContext(State _state)
        {
            // Placeholder for Undef context handling
        }

        // *****************************
        // HandleWorldContext
        // *****************************
        private static void HandleWorldContext(State _state)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // *****************************
        // HandleUIContext
        // *****************************
        private static void HandleUIContext(State _state)
        {
            Cursor.lockState    = CursorLockMode.Confined;
            Cursor.visible      = true;
        }
    }
}
