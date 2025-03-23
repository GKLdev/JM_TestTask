using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

namespace Modules.PlayerCharacterView {
    public static class CompHead
    {
        // *****************************
        // OnUpdate
        // *****************************
        public static void OnUpdate(State _state)
        {
            ApplyRotation(_state);

            // Reset relative rotation
            _state.dynamicData.headRelativeRotation = Vector2.zero;
        }

        // *****************************
        // ApplyRotation
        // *****************************
        static void ApplyRotation(State _state)
        {
            // Check if angles are significant
            if (_state.dynamicData.headRelativeRotation.sqrMagnitude < _state.config.P_FloatPrecision)
            {
                return;
            }

            // Apply rotation
            _state.head.rotation *= Quaternion.Euler(_state.dynamicData.headRelativeRotation.y, 0f, 0f);

            // Clamp vertical rotation
            Vector3 newFwd = _state.head.forward;
            float maxAngle = _state.config.P_MaxVerticalLookAngle;

            float   angle       = Vector3.SignedAngle(newFwd, _state.root.forward, _state.root.right);
            bool    needClamp   = Mathf.Abs(angle) > maxAngle;
            if (needClamp)
            {
                _state.head.rotation = _state.root.rotation * Quaternion.Euler(-Mathf.Sign(angle) * maxAngle, 0f, 0f);
            }
        }
    }
}
