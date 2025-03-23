using GDTUtils;
using Modules.CharacterController_Public;
using Modules.DamageManager_Public;
using Modules.ModuleManager_Public;
using Modules.PlayerWeapon_Public;
using Modules.TimeManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Modules.PlayerWeapon
{
    public class PlayerWeapon : LogicBase, IPlayerWeapon, IEntityModifcation
    {
        [Inject]
        private IModuleManager moduleMgr;

        [UnityEngine.SerializeField]
        private State state = new();

        // *****************************
        // InitModule
        // *****************************
        public void InitModule()
        {
            state.dynamicData.damageMgr = moduleMgr.Container.Resolve<IDamageManager>();
            state.dynamicData.timeMgr   = moduleMgr.Container.Resolve<ITimeManager>();
            CompInit.Init(state, this);
            moduleMgr.Container.Bind<IPlayerWeapon>().FromInstance(this);
        }

        // *****************************
        // OnUpdate
        // *****************************
        public void OnUpdate()
        {
            if (!state.dynamicData.isInitialized)
            {
                return;
            }

            CompShoot.OnUpdate(state);
        }

        // *****************************
        // Shoot
        // *****************************
        public void Shoot()
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);
            CompShoot.Shoot(state);
        }

        // *****************************
        // ModifyEntity
        // *****************************
        public void ModifyEntity(EntityModifcationType _stat, float _value)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);

            switch (_stat)
            {
                case EntityModifcationType.Damage:
                    state.dynamicData.currentDamageMod = _value;
                    break;
                default:
                    break;
            }
        }
    }

    [System.Serializable]
    public class State
    {
        public UnityEngine.Transform shootPointTransform;
        public ConfigPlayerWeapon config;

        public SerializedInterface<ICharacterStatsSystem> statsSystem;

        public DynamicData dynamicData = new();

        public class DynamicData
        {
            public IDamageManager   damageMgr;
            public ITimeManager     timeMgr;
            public DamageSource     damageSource = new();

            public float currentDamageValue = 0f;
            public float currentDamageMod   = 0f;

            public int shotCooldown = -1;

            public bool isInitialized = false;
        }
    }
}
