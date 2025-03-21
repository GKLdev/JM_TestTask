using GDTUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Modules.CharacterController
{
    public static class CompInit
    {
        // *****************************
        // Init
        // *****************************
        public static void Init(State _state)
        {
            // Validate config
            if (_state.config == null)
            {
                Debug.Assert(false, "CharacterControllerConfig is not assigned.");
                return;
            }

            // Setup state
            if (_state.transform == null)
            {
                Debug.Assert(false, "Transform component is missing on the GameObject.");
                return;
            }

            // Validate vertical look transform
            if (_state.verticalLookTransform == null)
            {
                Debug.Assert(false, "Vertical look transform is not assigned.");
                return;
            }

            // Validate character collider
            if (_state.characterCollider == null)
            {
                Debug.Assert(false, "Character collider is not assigned.");
                return;
            }

            // Initialize NavMeshAgent
            _state.navAgent = _state.transform.gameObject.GetComponent<NavMeshAgent>();
            if (_state.navAgent == null)
            {
                _state.navAgent = _state.transform.gameObject.AddComponent<NavMeshAgent>();
            }
            _state.navAgent.enabled = false;
            _state.navAgent.speed = _state.config.P_MaxSpeed;
            _state.navAgent.angularSpeed = _state.config.P_RotationSpeed;
            _state.navAgent.stoppingDistance = _state.config.P_NavmeshStoppingDistance;
            _state.navAgent.autoBraking = false;

            // Initialize collider buffer
            _state.dynamicData.collisionData.colliderBuffer = new Collider[16]; // Adjust size as needed

            // Initialize movement axes
            FactoryDynamicAxis factory = new FactoryDynamicAxis();
            _state.dynamicData.movementData.movementAxisX = factory.Produce();
            _state.dynamicData.movementData.movementAxisZ = factory.Produce();
            _state.dynamicData.rotationData.rotationAxis = factory.Produce();

            // Strafe (X-axis) parameters
            float strafeMaxSpeed = _state.config.P_StrafeMaxSpeed;
            _state.dynamicData.movementData.movementAxisX.InitAxis(-strafeMaxSpeed, strafeMaxSpeed, _state.config.P_StrafeUpSpeed, _state.config.P_StrafeDownSpeed);

            // Forward/Backward (Z-axis) parameters
            float maxSpeed = _state.config.P_MaxSpeed;
            _state.dynamicData.movementData.movementAxisZ.InitAxis(-maxSpeed, maxSpeed, _state.config.P_MovementUpSpeed, _state.config.P_MovementDownSpeed);

            // Initialize rotation axis
            float maxRotationSpeed = _state.config.P_RotationSpeed;
            _state.dynamicData.rotationData.rotationAxis.InitAxis(-maxRotationSpeed, maxRotationSpeed, _state.config.P_RotationUpSpeed, _state.config.P_RotationDownSpeed);

            // Mark as initialized
            _state.dynamicData.generalData.isInitialized = true;
            _state.dynamicData.generalData.isEnabled = true;
        }
    }
}
