using Modules.CharacterController_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Modules.CharacterController
{
    public static class CompNavmesh
    {
        // *****************************
        // UpdateNavmeshAgentState
        // *****************************
        public static void UpdateNavmeshAgentState(State _state)
        {
            // Enable or disable NavMeshAgent based on navigation mode
            bool isNavmeshMode = _state.dynamicData.movementData.navigationMode == NavigationMode.Navmesh;
            _state.navAgent.enabled = isNavmeshMode;

            // Reset velocity and input when switching modes
            if (!isNavmeshMode)
            {
                _state.dynamicData.movementData.currentVelocity = Vector3.zero;
                _state.dynamicData.movementData.inputDirection = Vector2.zero;
            }
        }

        // *****************************
        // UpdateNavmeshMovement
        // *****************************
        public static void UpdateNavmeshMovement(State _state)
        {
            // Skip if not in Navmesh mode
            if (_state.dynamicData.movementData.navigationMode != NavigationMode.Navmesh && _state.navAgent.path.status != NavMeshPathStatus.PathInvalid)
            {
                return;
            }

            // Set destination if target position is set
            bool hasTarget = _state.dynamicData.movementData.hasTargetPosition;
            if (hasTarget)
            {
                Vector3 targetPos   = _state.dynamicData.movementData.targetPosition;
                bool    success     = _state.navAgent.SetDestination(targetPos);
                if (!success)
                {
                    return;
                }
            }

            // Check if we've reached the destination
            bool hasReachedDestination = !_state.navAgent.pathPending && _state.navAgent.remainingDistance <= _state.config.P_NavmeshStoppingDistance;
            if (hasReachedDestination)
            {
                // Stop sharply at the destination
                _state.navAgent.isStopped = true;
                _state.navAgent.velocity = Vector3.zero;
                _state.dynamicData.movementData.currentVelocity = Vector3.zero;
                _state.dynamicData.movementData.hasTargetPosition = false;
                return;
            }

            // Resume movement if not stopped
            _state.navAgent.isStopped = false;
        }

        // *****************************
        // GetNavmeshLookDirection
        // *****************************
        public static Vector3 GetNavmeshLookDirection(State _state)
        {
            // Use the agent's desired velocity as the look direction
            Vector3 desiredVelocity = _state.navAgent.desiredVelocity;
            return desiredVelocity;
        }
    }
}
