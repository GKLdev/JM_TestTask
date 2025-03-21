using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.CharacterController_Public
{
    [CreateAssetMenu(fileName = "ConfigCharacterController", menuName = "Configs/ConfigCharacterController")]
    public class ConfigCharacterController : ScriptableObject
    {
        [Tooltip("Maximum speed of the character for forward/backward movement")]
        [SerializeField]
        private float maxSpeed = 5f;

        [Tooltip("Maximum speed of the character for strafing (sideways movement)")]
        [SerializeField]
        private float strafeMaxSpeed = 4f;

        [Tooltip("Speed of character rotation")]
        [SerializeField]
        private float rotationSpeed = 5f;

        [Tooltip("Layer mask for collision detection")]
        [SerializeField]
        private LayerMask collisionLayer;

        [Tooltip("Precision for velocity and direction magnitude comparisons")]
        [SerializeField]
        private float floatPrecision = 0.01f;

        [Tooltip("Precision for angle comparisons")]
        [SerializeField]
        private float anglePrecision = 0.1f;

        [Tooltip("Radius for collision overlap check")]
        [SerializeField]
        private float collisionCheckRadius = 0.5f;

        [Tooltip("Speed of acceleration for forward/backward movement")]
        [SerializeField]
        private float movementUpSpeed = 10f;

        [Tooltip("Speed of deceleration for forward/backward movement")]
        [SerializeField]
        private float movementDownSpeed = 10f;

        [Tooltip("Speed of acceleration for strafing (sideways movement)")]
        [SerializeField]
        private float strafeUpSpeed = 8f;

        [Tooltip("Speed of deceleration for strafing (sideways movement)")]
        [SerializeField]
        private float strafeDownSpeed = 8f;

        [Tooltip("Speed of rotation acceleration")]
        [SerializeField]
        private float rotationUpSpeed = 5f;

        [Tooltip("Speed of rotation deceleration")]
        [SerializeField]
        private float rotationDownSpeed = 5f;

        [Tooltip("Use smooth rotation for horizontal look")]
        [SerializeField]
        private bool useSmoothHorizontalRotation = true;

        [Tooltip("Use step-based collision resolution")]
        [SerializeField]
        private bool useStepCollisionResolve = true;

        [Tooltip("Reset movement axes when direction reverses (e.g., forward to backward or strafe left to right)")]
        [SerializeField]
        private bool resetAxisOnDirectionChange = true;

        [Tooltip("Distance per step for collision resolution")]
        [SerializeField]
        private float collisionStepDistance = 0.2f;

        [Tooltip("Stopping distance for NavMeshAgent")]
        [SerializeField]
        private float navmeshStoppingDistance = 0.1f;

        [Tooltip("Gravity force applied to the character in DirectControl mode")]
        [SerializeField]
        private float gravityForce = 9.81f;

        [Tooltip("Extra distance added to collision check radius for ground detection")]
        [SerializeField]
        private float groundCheckExtraDistance = 0.1f;

        [Tooltip("Maximum vertical look angle (pitch) in degrees")]
        [SerializeField]
        private float maxVerticalLookAngle = 45f;

        public float P_MaxSpeed => maxSpeed;
        public float P_StrafeMaxSpeed => strafeMaxSpeed;
        public float P_RotationSpeed => rotationSpeed;
        public LayerMask P_CollisionLayer => collisionLayer;
        public float P_FloatPrecision => floatPrecision;
        public float P_AnglePrecision => anglePrecision;
        public float P_CollisionCheckRadius => collisionCheckRadius;
        public float P_MovementUpSpeed => movementUpSpeed;
        public float P_MovementDownSpeed => movementDownSpeed;
        public float P_StrafeUpSpeed => strafeUpSpeed;
        public float P_StrafeDownSpeed => strafeDownSpeed;
        public float P_RotationUpSpeed => rotationUpSpeed;
        public float P_RotationDownSpeed => rotationDownSpeed;
        public bool P_UseSmoothHorizontalRotation => useSmoothHorizontalRotation;
        public bool P_UseStepCollisionResolve => useStepCollisionResolve;
        public bool P_ResetAxisOnDirectionChange => resetAxisOnDirectionChange;
        public float P_CollisionStepDistance => collisionStepDistance;
        public float P_NavmeshStoppingDistance => navmeshStoppingDistance;
        public float P_GravityForce => gravityForce;
        public float P_GroundCheckExtraDistance => groundCheckExtraDistance;
        public float P_MaxVerticalLookAngle => maxVerticalLookAngle;
    }
}