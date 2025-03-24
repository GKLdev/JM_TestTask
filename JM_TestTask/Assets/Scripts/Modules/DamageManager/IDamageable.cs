using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.DamageManager_Public
{
    public interface IDamageable : IDisposable
    {
        /// <summary>
        /// Callback on damage.
        /// </summary>
        /// <param name="_source"></param>
        /// <param name="_type"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        bool OnDamage(DamageSource _source, DamageType _type, float _value);

        /// <summary>
        /// Callback to init damageable
        /// </summary>
        void OnCreated();

        /// <summary>
        /// Setup custom config
        /// </summary>
        /// <param name="_config"></param>
        void SetupConfig(ConfigDamageable _config);

        /// <summary>
        /// Reset to dfault values. Active state not affected.
        /// </summary>
        void ResetDamageable();

        /// <summary>
        /// Set active/innactive. Damageable will receive only while active.
        /// </summary>
        /// <param name="_val"></param>
        void ToggleActive(bool _val);

        Faction GetFaction();

        bool IsDead();

        float GetCurrentHealth();
        float GetMaxHealth();

        event System.Action<bool, IDamageable> OnDamageApplied; 
    }

    // *****************************
    // DamageSource
    // *****************************
    public class DamageSource
    {
        public Faction          faction;
        public System.Object    obj;
    }

    // *****************************
    // Team
    // *****************************
    public enum Faction
    {
        Player = 0,
        AI
    }

    // *****************************
    // DamageType
    // *****************************
    public enum DamageType
    {
        Physical = 0
    }
}