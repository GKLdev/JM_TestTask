using GDTUtils;
using Modules.CharacterController_Public;
using Modules.CharacterStatsSystem_Public;
using Modules.DamageManager_Public;
using Modules.ModuleManager_Public;
using Modules.TimeManager_Public;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Modules.DamageManager.DamageableCharacter
{
    public class DamageableCharacter : LogicBase, IDamageable, IEntityModifcation
    {
        [SerializeField]
        State state;

        [Inject]
        IModuleManager moduleMgr;

        public event Action<bool, IDamageable> OnDamageApplied;


        // *****************************
        // Start
        // *****************************
        private void Start()
        {
            if (state.registerOnStart)
            {
                OnCreated();
            }

            if (state.toggleOnStart)
            {
                ToggleActive(true);
            }
        }

        // *****************************
        // OnCreated
        // *****************************
        public void OnCreated()
        {
            if (state.dynamic.initialized)
            {
                Debug.LogWarning($"Damageable={name}.OnCreated() attempted to call twice! You should probably disable 'registerOnStart'!");
                return;
            }

            state.dynamic.initialized   = true;
            state.dynamic.damageMgr     = moduleMgr.Container.TryResolve<IDamageManager>();

            if (state.dynamic.damageMgr == null)
            {
                UnityEngine.Debug.LogError($"Failed to get damageable manager Object={name}!");
                return;
            }

            state.dynamic.damageMgr.RegisterDamageable(this, state.collision);

            ResetDamageable();
        }

        // *****************************
        // Toggle
        // *****************************
        public void ToggleActive(bool _val)
        {
            state.dynamic.isActive = _val;
        }

        // *****************************
        // Dispose
        // *****************************
        public void Dispose()
        {
            state.dynamic.damageMgr.UnregisterDamageable(this);
            OnDamageApplied = null;
        }

        // *****************************
        // OnDamage
        // *****************************
        public bool OnDamage(DamageSource _source, DamageType _type, float _value)
        {
            bool result = false;

            bool ignore = !state.dynamic.isActive || state.dynamic.isDead || state.dynamic.isImmortalObject;
            if (ignore)
            {
                return result;
            }

            state.dynamic.health = Mathf.Clamp(state.dynamic.health - _value, 0f, float.MaxValue);
            result = true;

            if (state.debug)
            {
                Debug.Log($"Object={gameObject.name} is damaged: damage={_value}, Hp={state.dynamic.health}/{state.dynamic.maxHealth}");
            }

            bool isDead = Mathf.Approximately(state.dynamic.health, 0f);
            if (isDead)
            {
                OnDeath();
            }

            state.dynamic.isDead = isDead;
            OnDamageApplied?.Invoke(isDead, this);

            return result;
        }

        // *****************************
        // ResetDamageable
        // *****************************
        public void ResetDamageable()
        {
            state.dynamic.health            = state.config.Health;
            state.dynamic.maxHealth         = state.config.Health;
            state.dynamic.isImmortalObject  = state.config.IsImmortalObject;
            state.dynamic.isDead            = false;

            RandomizeHealth();
        }

        // *****************************
        // OnDeath
        // *****************************
        void OnDeath()
        {
            if (state.debug)
            {
                Debug.Log($"Object is Dead={gameObject.name}");
            }
        }

        // *****************************
        // GetFaction
        // *****************************
        public Faction GetFaction()
        {
            return state.config.DamageableFaction;
        }

        // *****************************
        // GetFaction
        // *****************************
        public void SetupConfig(ConfigDamageable _config)
        {
            if (state.dynamic.initialized)
            {
                Debug.Assert(false, "Cant assign config is damageable is already initialized!");
            }

            state.config = _config;
        }

        // *****************************
        // IsDead
        // *****************************
        public bool IsDead()
        {
            return state.dynamic.isDead;
        }

        // *****************************
        // ModifyEntity
        // *****************************
        public void ModifyEntity(EntityModifcationType _stat, float _value)
        {
            switch (_stat)
            {
                case EntityModifcationType.Health:
                    bool atMaxHealth = state.dynamic.maxHealth == state.dynamic.health;
                    state.dynamic.maxHealth = state.config.Health + _value;

                    if (atMaxHealth)
                    {
                        state.dynamic.health = state.dynamic.maxHealth;
                    }

                    break;
                default:
                    break;
            }
        }

        // *****************************
        // GetCurrentHealth
        // *****************************
        public float GetCurrentHealth()
        {
            return state.dynamic.health;
        }

        // *****************************
        // GetMaxHealth
        // *****************************
        public float GetMaxHealth()
        {
            return state.dynamic.maxHealth;
        }

        // *****************************
        // RandomizeHealth
        // *****************************
        private void RandomizeHealth()
        {
            int min = state.config.HealthRandomizationMin;
            int max = state.config.HealthRandomizationMax;

            bool ignore = !state.config.NeedRandomizeHealth || min < 1 || max < min;
            if (ignore)
            {
                return;
            }

            int value = GDTRandom.generalRng.Next(min, max);

            state.dynamic.health    = value;
            state.dynamic.maxHealth = value;
        }
    }

    // *****************************
    // State
    // *****************************
    [System.Serializable]
    public class State
    {
        public bool             debug           = false;
        public bool             registerOnStart = false;
        public bool             toggleOnStart   = false;
        public Collider         collision;
        public ConfigDamageable config;

        public DynamicData dynamic = new();

        // *****************************
        // DynamicData
        // *****************************
        public class DynamicData
        {
            public float    health;
            public float    maxHealth;
            public bool     isImmortalObject    = false;
            public bool     isDead              = false;
            public bool     isActive            = false;

            public bool initialized = false;

            public IDamageManager damageMgr;
        }
    }

}