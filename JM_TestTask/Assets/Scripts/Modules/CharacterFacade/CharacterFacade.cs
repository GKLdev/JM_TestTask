using GDTUtils;
using Modules.AIBrain;
using Modules.AIBrain_Public;
using Modules.AITemplate_Public;
using Modules.CharacterController_Public;
using Modules.CharacterControllerView_Public;
using Modules.CharacterFacade_Public;
using Modules.DamageManager_Public;
using Modules.ReferenceDb_Public;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.CharacterFacade
{
    /// <summary>
    /// Helps to init, update and to get access to all character components.
    /// </summary>

    // TODO: add statmanager and ability to change it adn enable it.
    public class CharacterFacade : LogicBase, ICharacterFacade
    {
        public int Id { get; set; }

        public ICharacterController         P_Controller => controller.Value;
        public ICharacterStatsSystem        P_StatsSystem => statsSystem.Value;

        public CATEGORY_CHARACTERS          P_ElementType { get; set; }
        public GameObject                   P_GameObjectAccess => gameObject;

        public SerializedInterface<ICharacterController>        controller;
        public SerializedInterface<IDamageable>                 damageable;
        public SerializedInterface<IAIBrain>                    aiBrain;
        public SerializedInterface<ICharacterStatsSystem>       statsSystem;

        bool initialized    = false;
        bool isDisposed     = false;

        DamageSource damageSource = new();

        // *****************************
        // InitModule 
        // *****************************
        public void InitModule(ICharacterControllerView _visual)
        {
            if (initialized)
            {
                return;
            }

            controller.Value.InitModule();
            controller.Value.AttachVisual(_visual);
            damageable.Value.OnCreated();
            damageable.Value.OnDamageApplied += OnDamage;
            aiBrain.Value.InitModule(this);
            statsSystem.Value.InitModule();

            initialized = true;

            // damageable
            damageSource.obj        = this;
            damageSource.faction    = damageable.Value.GetFaction();
        }


        // *****************************
        // SetupCharacter 
        // *****************************
        public void SetupCharacter(ConfigCharacterController configController, ConfigDamageable configDamageable, IAITemplate _template = null)
        {
            if (initialized)
            {
                Debug.Assert(false, "SetupCharacter() can only be called BEFORE 'InitModule'! ");
            }

            controller.Value.SetupConfig(configController);
            damageable.Value.SetupConfig(configDamageable);
            aiBrain.Value.SetupTemplate(_template);
        }

        // *****************************
        // OnUpdate 
        // *****************************
        public void OnUpdate()
        {
            if (!initialized)
            {
                return;
            }

            controller.Value.OnUpdate(); // updates visual internally
        }

        // *****************************
        // GetDamageSource 
        // *****************************
        public DamageSource GetDamageSource()
        {
            return damageSource;
        }

        // *****************************
        // OnDamage 
        // *****************************
        void OnDamage(bool _isDead) {
            
            // TODO
            if (_isDead)
            {
                Debug.Log($"Character={this.name} is dead!");
                // dead logic
                // TODO: subscrive controller and view to OnDamage event
            }
            else {
                // on damage logic
                Debug.Log($"Character={this.name} is damaged! HP={damageable.Value.GetCurrentHealth()}/{damageable.Value.GetMaxHealth()}");
            }
        }

        // *****************************
        // MakeAIControlled 
        // *****************************
        public void MakeAIControlled()
        {
            P_Controller.MoveToTarget(P_Controller.P_Position);
            aiBrain.Value.ToggleAIBrain(true);
            P_Controller.SetNavigationMode(NavigationMode.Navmesh);
        }


        // *****************************
        // MakePlayerControlled 
        // *****************************
        public void MakePlayerControlled()
        {
            aiBrain.Value.ToggleAIBrain(false);
            P_Controller.SetNavigationMode(NavigationMode.DirectControl);
        }

        // *****************************
        // IsDead 
        // *****************************
        public bool IsDead()
        {
            return damageable.Value.IsDead();
        }

        //------------------------------
        // Poolable:
        //------------------------------

        // *****************************
        // Activate 
        // *****************************
        public void Activate()
        {
            controller.Value.OnAwake();
            controller.Value.GetView().OnAwake();
            damageable.Value.ToggleActive(true);
            aiBrain.Value.OnAwake();
            statsSystem.Value.OnAwake();
        }

        // *****************************
        // Deactivate 
        // *****************************
        public void Deactivate()
        {
            controller.Value.GetView().OnSlept();
            controller.Value.OnSlept();
            damageable.Value.ResetDamageable();
            damageable.Value.ToggleActive(false);
            aiBrain.Value.OnSlept();
            statsSystem.Value.OnSlept();
        }

        // *****************************
        // OnAdded 
        // *****************************
        public void OnAdded()
        {
            controller.Value.OnAdded();
            controller.Value.GetView().OnAdded();
            aiBrain.Value.OnAdded();
            statsSystem.Value.OnAdded();
        }

        // *****************************
        // Dispose 
        // *****************************
        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;
            controller.Value.GetView().Dispose();
            controller.Value.Dispose();
            damageable.Value.Dispose();
            aiBrain.Value.Dispose();
            statsSystem.Value.Dispose();

            damageable.Value.OnDamageApplied -= OnDamage;
        }
    }
}