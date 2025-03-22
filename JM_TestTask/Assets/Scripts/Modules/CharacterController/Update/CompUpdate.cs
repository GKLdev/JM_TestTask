using Modules.CharacterController_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.CharacterController
{
    public static class CompUpdate
    {
        // *****************************
        // OnUpdate
        // *****************************
        public static void OnUpdate(State _state)
        {
            // Update time delta
            GetDeltaTIme(_state);

            // Update movement based on navigation mode
            if (_state.dynamicData.movementData.navigationMode == NavigationMode.Navmesh)
            {
                CompNavmesh.UpdateNavmeshMovement(_state);
            }
            else
            {
                CompMovement.UpdateMovement(_state);
            }

            // Update rotation
            CompRotation.UpdateRotation(_state);
        }

        // *****************************
        // GetDeltaTIme
        // *****************************
        private static void GetDeltaTIme(State _state)
        {
            _state.dynamicData.generalData.deltaTime = _state.dynamicData.generalData.timeMgr.GetDeltaTime(_state.dynamicData.generalData.timeLayer);
        }
    }
}