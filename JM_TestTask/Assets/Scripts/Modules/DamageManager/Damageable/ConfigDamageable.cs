﻿using Modules.ReferenceDb_Public;
using System.Collections;
using UnityEngine;

namespace Modules.DamageManager_Public
{
    [CreateAssetMenu(fileName = "ConfigDamageable", menuName = "Configs/Damage/Damageable")]
    public class ConfigDamageable : DbEntryBase
    {
        [Tooltip("Default health")]
        public int Health = 1;

        [Tooltip("Should we randomize health?")]
        public bool NeedRandomizeHealth = false;

        [Tooltip("Range for hp randomization")]
        public int HealthRandomizationMin;
        public int HealthRandomizationMax;

        [Tooltip("This object cannnot receive any damage and will not die on zero health")]
        public bool IsImmortalObject = false;

        [Tooltip("Damageable Faction")]
        public Faction DamageableFaction = Faction.AI;
    }
}