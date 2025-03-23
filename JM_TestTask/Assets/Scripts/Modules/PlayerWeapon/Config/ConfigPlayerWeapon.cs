using Modules.DamageManager_Public;
using Modules.TimeManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.PlayerWeapon_Public
{
    [CreateAssetMenu(fileName = "ConfigPlayerWeapon", menuName = "Configs/ConfigPlayerWeapon")]
    public class ConfigPlayerWeapon : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Layer mask for raycast to detect targets")]
        private LayerMask targetLayerMask = 0;

        [SerializeField]
        [Tooltip("Maximum distance for the raycast")]
        private float maxDistance = 100f;

        [SerializeField]
        [Tooltip("Damage dealt to the target on hit")]
        private float damage = 10f;

        [SerializeField]
        [Tooltip("Delay between shots while firing.")]
        private float delayBetweenShots = 0.2f;

        [SerializeField]
        [Tooltip("Weapon holder's default faction")]
        private Faction faction = Faction.Player;

        [SerializeField]
        [Tooltip("Time layer used for cooldown")]
        private TimeLayerType timeLayer = TimeLayerType.PlayerWeapon;

        public LayerMask P_TargetLayerMask => targetLayerMask;
        public float P_MaxDistance => maxDistance;
        public float P_Damage => damage;
        public float P_DelayBetweenShots => delayBetweenShots;
        public Faction P_Faction => faction;
        public TimeLayerType P_TimeLayer => timeLayer;
    }
}
