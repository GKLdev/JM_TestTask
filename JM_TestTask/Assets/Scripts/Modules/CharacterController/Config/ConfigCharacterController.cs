using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.CharacterController_Public
{
    [CreateAssetMenu(fileName = "ConfigCharacterController", menuName = "Configs/ConfigCharacterController")]
    public class ConfigCharacterController : ScriptableObject
    {
        [Tooltip("Maximum speed of the character")]
        [SerializeField]
        private float maxSpeed = 5f;

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

        [Tooltip("Speed of acceleration for movement")]
        [SerializeField]
        private float movementUpSpeed = 10f;

        [Tooltip("Speed of deceleration for movement")]
        [SerializeField]
        private float movementDownSpeed = 10f;

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

        public float P_MaxSpeed => maxSpeed;
        public float P_RotationSpeed => rotationSpeed;
        public LayerMask P_CollisionLayer => collisionLayer;
        public float P_FloatPrecision => floatPrecision;
        public float P_AnglePrecision => anglePrecision;
        public float P_CollisionCheckRadius => collisionCheckRadius;
        public float P_MovementUpSpeed => movementUpSpeed;
        public float P_MovementDownSpeed => movementDownSpeed;
        public float P_RotationUpSpeed => rotationUpSpeed;
        public float P_RotationDownSpeed => rotationDownSpeed;
        public bool P_UseSmoothHorizontalRotation => useSmoothHorizontalRotation;
        public bool P_UseStepCollisionResolve => useStepCollisionResolve;
        public float P_CollisionStepDistance => collisionStepDistance;
        public float P_NavmeshStoppingDistance => navmeshStoppingDistance;
        public float P_GravityForce => gravityForce;
        public float P_GroundCheckExtraDistance => groundCheckExtraDistance;
    }
}
