using GDTUtils;
using Modules.CharacterController_Public;
using Modules.CharacterControllerView_Public;
using Modules.CharacterStatsSystem_Public;
using Modules.DamageManager_Public;
using Modules.ModuleManager_Public;
using Modules.PlayerProgression_Public;
using Modules.PlayerWeapon_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Zenject;
using static UnityEngine.GraphicsBuffer;

namespace Modules.CharacterStatsSystem
{
    public class CharacterStatsSystem : LogicBase, ICharacterStatsSystem
    {
        [Inject]
        private IModuleManager moduleMgr;

        [SerializeField]
        private State state = new();

        // *****************************
        // InitModule 
        // *****************************
        public void InitModule()
        {
            state.dynamicData.playerProgression = moduleMgr.Container.Resolve<IPlayerProgression>();
            state.dynamicData.isInitialized = true;
        }

        // *****************************
        // OnAdded 
        // *****************************
        public void OnAdded()
        {
        }

        // *****************************
        // OnAwake 
        // *****************************
        public void OnAwake()
        {
        }

        // *****************************
        // OnSlept 
        // *****************************
        public void OnSlept()
        {
            Toggle(false);
        }

        // *****************************
        // Toggle 
        // *****************************
        public void Toggle(bool _val)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.isInitialized);

            state.dynamicData.isActive = _val;
            if (_val)
            {
                state.dynamicData.playerProgression.OnPlayerStatChanged += OnStatChanged;
            }
            else
            {
                state.dynamicData.playerProgression.OnPlayerStatChanged -= OnStatChanged;
            }
        }

        // *****************************
        // Dispose 
        // *****************************
        public void Dispose()
        {
            state.dynamicData.playerProgression.OnPlayerStatChanged -= OnStatChanged;
        }

        // *****************************
        // OnStatChanged 
        // *****************************
        void OnStatChanged(StatChangeData _data)
        {
            if (!state.dynamicData.isActive)
            {
                return;
            }

            switch (_data.alias)
            {
                case PlayerProgressionAliases.health:
                    state.damageable.Value?.ModifyEntity(EntityModifcationType.Health, _data.currentValue);
                    break;
                case PlayerProgressionAliases.speed:
                    state.character.Value?.ModifyEntity(EntityModifcationType.Speed, _data.currentValue);
                    break;
                case PlayerProgressionAliases.damage:
                    state.weapon.Value?.ModifyEntity(EntityModifcationType.Damage, _data.currentValue);
                    break;
                default:
                    break;
            }
        }
    }

    [System.Serializable]
    public class State
    {
        public SerializedInterface<IEntityModifcation>  character;
        public SerializedInterface<IEntityModifcation>  damageable;
        public SerializedInterface<IEntityModifcation>  weapon;

        public DynamicData dynamicData = new();

        public class DynamicData
        {
            public bool isInitialized   = false;
            public bool isActive        = false;

            //public 

            public IPlayerProgression playerProgression;
        }
    }
}