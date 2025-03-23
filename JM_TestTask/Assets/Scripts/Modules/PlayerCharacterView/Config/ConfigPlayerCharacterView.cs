using Modules.TimeManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Modules.PlayerCharacterView_Public
{
    [CreateAssetMenu(fileName = "ConfigPlayerCharacterView", menuName = "Configs/ConfigPlayerCharacterView")]
    public class ConfigPlayerCharacterView : ScriptableObject
    {
        [Tooltip("Maximum vertical look angle (pitch) in degrees")]
        [SerializeField]
        private float maxVerticalLookAngle = 45f;

        [Tooltip("Precision for velocity and direction magnitude comparisons")]
        [SerializeField]
        private float floatPrecision = 0.01f;

        public float P_MaxVerticalLookAngle => maxVerticalLookAngle;
        public float P_FloatPrecision => floatPrecision;
    }
}