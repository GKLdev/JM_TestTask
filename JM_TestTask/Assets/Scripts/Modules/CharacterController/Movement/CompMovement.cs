using Modules.CharacterController_Public;
using UnityEngine;

namespace Modules.CharacterController
{
    public static class CompMovement
    {
        // *****************************
        // UpdateMovement
        // *****************************
        public static void UpdateMovement(State _state)
        {
            // Skip if not in DirectControl mode
            if (_state.dynamicData.movementData.navigationMode == NavigationMode.Navmesh)
            {
                return;
            }

            // Initialize desiredPosition with the current transform position
            _state.dynamicData.movementData.desiredPosition = _state.transform.position;

            // Calculate target velocity based on input
            Vector3 targetVelocity = CalculateTargetVelocity(_state);

            // Smoothly adjust current velocity using DynamicAxis
            _state.dynamicData.movementData.currentVelocity = SmoothVelocity(_state, targetVelocity);

            // Apply gravity if not grounded
            ApplyGravity(_state);

            // Calculate the desired position with collision resolution
            ApplyMovementWithCollision(_state);

            // Apply final position to transform
            ApplyPositionToTransform(_state);
        }

        // *****************************
        // CalculateTargetVelocity
        // *****************************
        private static Vector3 CalculateTargetVelocity(State _state)
        {
            // Extract input and transform directions
            Vector2 inputDir = _state.dynamicData.movementData.inputDirection;
            float maxSpeed          = _state.dynamicData.movementData.currentMaxSpeed;
            float strafeMaxSpeed    = _state.dynamicData.movementData.currentMaxStrafeSpeed;

            // Calculate target velocity with different speeds for strafe (X) and forward/backward (Z)
            Vector3 targetVelocity = new Vector3(inputDir.x * strafeMaxSpeed, 0f, inputDir.y * maxSpeed);
            return targetVelocity;
        }

        // *****************************
        // SmoothVelocity
        // *****************************
        public static Vector3 SmoothVelocity(State _state, Vector3 _targetVelocity)
        {
            // Extract components
            float deltaTime = _state.dynamicData.generalData.deltaTime;

            // Get current velocities directly using GetProgress()
            float currentVelocityX = _state.dynamicData.movementData.movementAxisX.GetProgress();
            float currentVelocityZ = _state.dynamicData.movementData.movementAxisZ.GetProgress();

            // Check if we should reset axes on direction change
            bool resetAxisOnDirectionChange = _state.config.P_ResetAxisOnDirectionChange;

            if (resetAxisOnDirectionChange)
            {
                // Reset strafe axis (X) if direction reverses
                if (currentVelocityX * _targetVelocity.x < 0f) // Different signs indicate direction reversal
                {
                    _state.dynamicData.movementData.movementAxisX.ResetAxis();
                }

                // Reset forward/backward axis (Z) if direction reverses
                if (currentVelocityZ * _targetVelocity.z < 0f) // Different signs indicate direction reversal
                {
                    _state.dynamicData.movementData.movementAxisZ.ResetAxis();
                }
            }

            // Update axes with target values
            _state.dynamicData.movementData.movementAxisX.SetTarget(_targetVelocity.x);
            _state.dynamicData.movementData.movementAxisZ.SetTarget(_targetVelocity.z);

            // Update axes to get current velocity components
            float velocityX = _state.dynamicData.movementData.movementAxisX.UpdateAxis(deltaTime);
            float velocityZ = _state.dynamicData.movementData.movementAxisZ.UpdateAxis(deltaTime);

            // Construct new velocity
            Vector3 newVelocity = _state.transform.forward * velocityZ + _state.transform.right * velocityX;
            return newVelocity;
        }

        // *****************************
        // ApplyGravity
        // *****************************
        private static void ApplyGravity(State _state)
        {
            // Skip if not in DirectControl mode
            if (_state.dynamicData.movementData.navigationMode != NavigationMode.DirectControl)
            {
                return;
            }

            // Update grounded state
            _state.dynamicData.movementData.isGrounded = IsGrounded(_state);

            if (!_state.dynamicData.movementData.isGrounded)
            {
                // Apply gravity to vertical velocity
                float gravityForce = _state.config.P_GravityForce;
                float deltaTime = _state.dynamicData.generalData.deltaTime;
                _state.dynamicData.movementData.verticalVelocity -= gravityForce * deltaTime;
            }
            else
            {
                // Reset vertical velocity if grounded
                _state.dynamicData.movementData.verticalVelocity = 0f;
            }
        }

        // *****************************
        // IsGrounded
        // *****************************
        private static bool IsGrounded(State _state)
        {
            // Use a raycast to check if the character is grounded
            float checkDistance = _state.config.P_CollisionCheckRadius + _state.config.P_GroundCheckExtraDistance;
            Vector3 rayStart = _state.transform.position + Vector3.up * _state.config.P_CollisionCheckRadius;
            return Physics.Raycast(rayStart, Vector3.down, checkDistance, _state.config.P_CollisionLayer, QueryTriggerInteraction.Ignore);
        }

        // *****************************
        // ApplyMovementWithCollision
        // *****************************
        private static void ApplyMovementWithCollision(State _state)
        {
            // Calculate total displacement
            Vector3 velocity = _state.dynamicData.movementData.currentVelocity;
            float deltaTime = _state.dynamicData.generalData.deltaTime;
            Vector3 displacement = velocity * deltaTime;

            // Add vertical displacement from gravity
            displacement += _state.transform.up * _state.dynamicData.movementData.verticalVelocity * deltaTime;

            float totalDistance = displacement.magnitude;

            // Check if we need to use step-based collision resolution
            bool useStepResolve = _state.config.P_UseStepCollisionResolve;
            bool hasSignificantDistance = totalDistance > _state.config.P_FloatPrecision;
            if (!useStepResolve || !hasSignificantDistance)
            {
                // Move directly to the target position and check collisions once
                _state.dynamicData.movementData.desiredPosition += displacement;
                ResolveCollisions(_state);
                return;
            }

            // Use step-based collision resolution
            float stepDistance = _state.config.P_CollisionStepDistance;
            Vector3 direction = displacement.normalized;

            // Calculate the number of full steps and the remainder
            int fullSteps = Mathf.FloorToInt(totalDistance / stepDistance);
            float remainingDistance = totalDistance - (fullSteps * stepDistance);
            Vector3 fullStepDisplacement = direction * stepDistance;

            // Perform full steps
            for (int i = 0; i < fullSteps; i++)
            {
                // Move by a full step
                _state.dynamicData.movementData.desiredPosition += fullStepDisplacement;

                // Resolve collisions at this step
                ResolveCollisions(_state);
            }

            // Perform the remaining step if necessary
            bool hasRemainingStep = remainingDistance > _state.config.P_FloatPrecision;
            if (hasRemainingStep)
            {
                Vector3 remainingStep = direction * remainingDistance;
                _state.dynamicData.movementData.desiredPosition += remainingStep;
                ResolveCollisions(_state);
            }
        }

        // *****************************
        // ResolveCollisions
        // *****************************
        private static void ResolveCollisions(State _state)
        {
            // Extract config values
            float checkRadius = _state.config.P_CollisionCheckRadius;

            // Check for collisions using non-allocating method
            int colliderCount = Physics.OverlapSphereNonAlloc(
                _state.dynamicData.movementData.desiredPosition,
                checkRadius,
                _state.dynamicData.collisionData.colliderBuffer,
                _state.config.P_CollisionLayer,
                QueryTriggerInteraction.Ignore);
            for (int i = 0; i < colliderCount; i++)
            {
                Collider collider = _state.dynamicData.collisionData.colliderBuffer[i];
                // Skip if the collider is the character's own collider
                if (collider == _state.characterCollider)
                {
                    continue;
                }

                if (Physics.ComputePenetration(
                    collider, collider.transform.position, collider.transform.rotation,
                    _state.characterCollider, _state.dynamicData.movementData.desiredPosition, _state.transform.rotation,
                    out Vector3 direction, out float distance))
                {
                    // Adjust desired position to resolve collision (direction is reversed)
                    _state.dynamicData.movementData.desiredPosition += -direction * distance;
                }
            }
        }

        // *****************************
        // ApplyPositionToTransform
        // *****************************
        private static void ApplyPositionToTransform(State _state)
        {
            // Apply desired position to transform
            _state.transform.position = _state.dynamicData.movementData.desiredPosition;
        }
    }
}