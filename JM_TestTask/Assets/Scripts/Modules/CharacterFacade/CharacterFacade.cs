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
    public class CharacterFacade : LogicBase, ICharacterFacade
    {
        public int Id { get; set; }

        public ICharacterController         P_Controller => controller.Value;
        public CATEGORY_CHARACTERS          P_ElementType { get; set; }
        public GameObject                   P_GameObjectAccess => gameObject;

        public SerializedInterface<ICharacterController>        controller;
        public SerializedInterface<IDamageable>                 damageable;
        public SerializedInterface<IAIBrain>                    aiBrain;

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
            }
            else {
                // on damage logic
                Debug.Log($"Character={this.name} is damaged!");
            }
        }

        // *****************************
        // MakeAIControlled 
        // *****************************
        public void MakeAIControlled()
        {
            P_Controller.MoveToTarget(P_Controller.P_Position); // sets controller to path mode and ensured it will not move anywhere
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
        }

        // *****************************
        // OnAdded 
        // *****************************
        public void OnAdded()
        {
            controller.Value.OnAdded();
            controller.Value.GetView().OnAdded();
            aiBrain.Value.OnAdded();
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
        }
    }
}