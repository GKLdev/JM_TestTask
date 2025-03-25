using Modules.InputManager_Public;
using Modules.PlayerWeapon;
using Modules.PlayerWeapon_Public;
using Modules.UI.UIController_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.InputManager
{
    public static class CompUpdate
    {
        // *****************************
        // OnUpdate
        // *****************************
        public static void OnUpdate(State _state)
        {
            // Handle update based on current context
            switch (_state.dynamicData.currentContext)
            {
                case InputContext.Undef:
                    UpdateUndefContext(_state);
                    break;
                case InputContext.World:
                    UpdateWorldContext(_state);
                    break;
                case InputContext.UI:
                    UpdateUIContext(_state);
                    break;
            }
        }

        // *****************************
        // UpdateUndefContext
        // *****************************
        private static void UpdateUndefContext(State _state)
        {
        }

        // *****************************
        // UpdateWorldContext
        // *****************************
        private static void UpdateWorldContext(State _state)
        {
            // move to stat switch logic
            if (Input.GetKeyDown(KeyCode.B))
            {
                _state.dynamicData.ui.ShowScreen(ScreenType.Upgrades, true);
                CompContextHandler.SetContext(_state, InputContext.UI);
                return;
            }

            if (_state.dynamicData.player == null)
            {
                _state.dynamicData.player = _state.dynamicData.characterMgr.GetPlayer();

                if (_state.dynamicData.player == null)
                {
                    return;
                }
            }

            // Movement
            // Get movement input from keyboard (WASD or arrow keys)
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector2 movementInput = new Vector2(horizontalInput, verticalInput).normalized;

            // Send movement input to the character controller
            _state.dynamicData.player.P_Controller.Move(movementInput);

            // Mouse look
            // Get mouse input for look direction (relative angles)
            float mouseX = Input.GetAxis("Mouse X") * _state.mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * _state.mouseSensitivity;

            // Calculate relative look direction based on mouse input
            Vector2 relativeLookDir = new Vector2(mouseX, -mouseY);
            _state.dynamicData.player.P_Controller.LookDirectionRelative(relativeLookDir);


            if (_state.dynamicData.playerWeapon == null)
            {
                _state.dynamicData.playerWeapon = _state.dynamicData.moduleMgr.Container.Resolve<IPlayerWeapon>();
            }

            // Firing weapon
            if (Input.GetButton("Fire1"))
            {
                _state.dynamicData.playerWeapon?.Shoot();
            }
        }

        // *****************************
        // UpdateUIContext
        // *****************************
        private static void UpdateUIContext(State _state)
        {
            // Placeholder for UI context update
        }
    }
}
