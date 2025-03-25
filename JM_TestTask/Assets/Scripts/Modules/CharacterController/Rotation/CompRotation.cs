using CharacterControllerView_Public;
using Modules.CharacterController_Public;
using UnityEngine;

namespace Modules.CharacterController
{
    public static class CompRotation
    {
        // *****************************
        // UpdateRotation
        // *****************************
        public static void UpdateRotation(State _state)
        {
            // Ignore rotation if moving by navmesh
            bool ignore = _state.dynamicData.movementData.navigationMode == NavigationMode.Navmesh;
            if (ignore)
            {
                return;
            }

            MakeRelativeRotation(_state);

            // Reset rotation data to prevent continuous rotation
            ResetRotationData(_state);
        }

        // *****************************
        // MakeRelateiveRotation
        // *****************************
        private static void MakeRelativeRotation(State _state)
        {
            Vector2 relativeAngles = _state.dynamicData.rotationData.relativeLookAngles;
            float floatPrecision = _state.config.P_FloatPrecision;

            // Check if angles are significant
            if (relativeAngles.sqrMagnitude < floatPrecision)
            {
                return;
            }

            // Apply rotation
            _state.transform.rotation *= Quaternion.Euler(0f, _state.dynamicData.rotationData.relativeLookAngles.x, 0f);

            // Head rotation
            bool isPlayer = _state.dynamicData.generalData.playerView != null;
            if (isPlayer)
            {
                _state.dynamicData.generalData.playerView.SetHeadAngles(new Vector2(0f, _state.dynamicData.rotationData.relativeLookAngles.y));
            }
        }

        // *****************************
        // ResetRotationData
        // *****************************
        private static void ResetRotationData(State _state)
        {
            // Reset the target direction and relative angles to prevent continuous rotation
            _state.dynamicData.rotationData.relativeLookAngles = Vector2.zero;
        }
    }
}