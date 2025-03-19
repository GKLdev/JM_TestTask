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
            // Extract target direction and precision
            Vector3 targetDir = _state.dynamicData.rotationData.targetLookDirection;
            float floatPrecision = _state.config.P_FloatPrecision;

            // Check if direction is significant
            bool hasSignificantDirection = targetDir.sqrMagnitude >= floatPrecision;
            if (!hasSignificantDirection)
            {
                // In Navmesh mode, use the agent's desired velocity as the look direction
                if (_state.dynamicData.movementData.navigationMode == NavigationMode.Navmesh)
                {
                    targetDir = CompNavmesh.GetNavmeshLookDirection(_state);
                    hasSignificantDirection = targetDir.sqrMagnitude >= floatPrecision;
                    if (hasSignificantDirection)
                    {
                        _state.dynamicData.rotationData.targetLookDirection = targetDir;
                    }
                }

                if (!hasSignificantDirection)
                {
                    return;
                }
            }

            // Extract vertical angle
            ExtractVerticalAngle(_state, floatPrecision);

            // Apply vertical rotation to child transform
            ApplyVerticalRotation(_state);

            // Smoothly rotate in the horizontal plane
            SmoothRotate(_state);
        }

        // *****************************
        // ExtractVerticalAngle
        // *****************************
        private static void ExtractVerticalAngle(State _state, float _floatPrecision)
        {
            // Extract target direction
            Vector3 targetDir = _state.dynamicData.rotationData.targetLookDirection;
            Vector3 flatDir   = targetDir;    
            flatDir.y = 0f;

            float magnitude = flatDir.magnitude;

            // Check if direction is significant
            if (magnitude > _floatPrecision)
            {
                float verticalAngle = Mathf.Asin(targetDir.y / targetDir.magnitude) * Mathf.Rad2Deg;
                _state.dynamicData.rotationData.verticalLookAngle = verticalAngle;
            }
        }

        // *****************************
        // ApplyVerticalRotation
        // *****************************
        private static void ApplyVerticalRotation(State _state)
        {
            // Extract vertical angle
            float verticalAngle = _state.dynamicData.rotationData.verticalLookAngle;

            // Apply rotation around local X axis
            _state.verticalLookTransform.localRotation = Quaternion.Euler(verticalAngle, 0f, 0f);
        }

        // *****************************
        // SmoothRotate
        // *****************************
        public static void SmoothRotate(State _state)
        {
            // Extract target direction and config values
            Vector3 targetDir = _state.dynamicData.rotationData.targetLookDirection;
            targetDir.y = 0f;

            float deltaTime         = Time.deltaTime;
            float floatPrecision    = _state.config.P_FloatPrecision;
            float anglePrecision    = _state.config.P_AnglePrecision;
            bool  useSmoothRotation = _state.config.P_UseSmoothHorizontalRotation;

            // Check if direction is significant
            bool hasSignificantDirection = targetDir.sqrMagnitude >= floatPrecision;
            if (!hasSignificantDirection)
            {
                _state.dynamicData.rotationData.rotationAxis.SetTarget(0f);
                return;
            }

            // Calculate angle difference
            Vector3 currentDir = _state.transform.forward;
            currentDir.y = 0f;
            float angleDiff = Vector3.SignedAngle(currentDir, targetDir, Vector3.up);

            // Snap to target if angle difference is small
            bool isAngleSmall = Mathf.Abs(angleDiff) < anglePrecision;
            if (isAngleSmall)
            {
                _state.transform.rotation = Quaternion.LookRotation(targetDir);
                _state.dynamicData.rotationData.rotationAxis.SetTarget(0f);
                return;
            }

            // Handle horizontal rotation based on config
            if (useSmoothRotation)
            {
                // Update rotation axis with target angular speed
                _state.dynamicData.rotationData.rotationAxis.SetTarget(angleDiff);

                // Update axis to get current angular speed
                float angularSpeed = _state.dynamicData.rotationData.rotationAxis.UpdateAxis(deltaTime);

                // Apply rotation
                _state.transform.Rotate(Vector3.up, angularSpeed * deltaTime, Space.World);
            }
            else
            {
                // Apply immediate rotation
                _state.transform.rotation = Quaternion.LookRotation(targetDir);
                _state.dynamicData.rotationData.rotationAxis.SetTarget(0f);
            }
        }
    }
}