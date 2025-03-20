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
            bool isRelativeRotation = _state.dynamicData.rotationData.hasRelativeLookAngles;
            if (isRelativeRotation)
            {
                MakeRelativeRotation(_state);
            }
            else
            {
                MakeLookAtRotation(_state);
            }

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
            _state.verticalLookTransform.rotation *= Quaternion.Euler(_state.dynamicData.rotationData.relativeLookAngles.y, 0f, 0f);

            // Clamp vertical rotation
            Vector3 newFwd = _state.verticalLookTransform.forward;
            float maxAngle = _state.config.P_MaxVerticalLookAngle;

            float angle = Vector3.SignedAngle(newFwd, _state.transform.forward, _state.transform.right);
            bool needClamp = Mathf.Abs(angle) > maxAngle;
            if (needClamp)
            {
                _state.verticalLookTransform.rotation = _state.transform.rotation * Quaternion.Euler(-Mathf.Sign(angle) * maxAngle, 0f, 0f);
            }
        }

        // *****************************
        // MakeLookAtRotation
        // *****************************
        private static void MakeLookAtRotation(State _state)
        {
            // Extract target direction
            Vector3 targetDir = _state.dynamicData.rotationData.targetLookDirection;
            float floatPrecision = _state.config.P_FloatPrecision;

            // Check if direction is significant
            if (targetDir.sqrMagnitude < floatPrecision)
            {
                return;
            }

            // Project targetLookDirection onto the horizontal plane
            Vector3 targetHorizontalDir = Vector3.ProjectOnPlane(targetDir, _state.transform.up);
            targetHorizontalDir.Normalize();

            // Check if the projected direction is significant
            if (targetHorizontalDir.sqrMagnitude < floatPrecision)
            {
                return;
            }

            // Update vertical angle based on the target direction
            Vector3 flatDir = targetDir;

            // Queue smooth rotation if flag is enabled
            bool useSmoothRotation = _state.config.P_UseSmoothHorizontalRotation;
            if (useSmoothRotation)
            {
                // Calculate the angle difference for smooth rotation
                Vector3 currentHorizontalDir = _state.transform.forward;
                currentHorizontalDir.y = 0f;
                currentHorizontalDir.Normalize();
                float angleDiff = Vector3.SignedAngle(currentHorizontalDir, targetHorizontalDir, Vector3.up);
                SmoothRotate(_state, angleDiff);
            }
            else
            {
                // Apply immediate rotation
                _state.transform.rotation = Quaternion.LookRotation(targetHorizontalDir);
                _state.dynamicData.rotationData.rotationAxis.SetTarget(0f);
            }
        }

        // *****************************
        // SmoothRotate
        // *****************************
        private static void SmoothRotate(State _state, float _angleDiff)
        {
            float anglePrecision = _state.config.P_AnglePrecision;

            // Snap to target if angle difference is small
            if (Mathf.Abs(_angleDiff) < anglePrecision)
            {
                _state.dynamicData.rotationData.rotationAxis.SetTarget(0f);
                return;
            }

            // Update rotation axis with target angular speed
            _state.dynamicData.rotationData.rotationAxis.SetTarget(_angleDiff);

            // Update axis to get current angular speed
            float deltaTime = Time.deltaTime;
            float angularSpeed = _state.dynamicData.rotationData.rotationAxis.UpdateAxis(deltaTime);

            // Apply rotation
            _state.transform.rotation *= Quaternion.Euler(0f, angularSpeed * deltaTime, 0f);
        }

        // *****************************
        // ResetRotationData
        // *****************************
        private static void ResetRotationData(State _state)
        {
            // Reset the target direction and relative angles to prevent continuous rotation
            _state.dynamicData.rotationData.relativeLookAngles = Vector2.zero;
            _state.dynamicData.rotationData.hasRelativeLookAngles = false;
        }
    }
}