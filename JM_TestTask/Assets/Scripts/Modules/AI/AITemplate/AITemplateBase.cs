using GDTUtils.StateMachine;
using Modules.AIBrain_Public;
using Modules.CharacterFacade_Public;
using Modules.ReferenceDb_Public;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Zenject;

namespace Modules.AITemplate_Public
{
    public interface IAITemplate
    {
        void Init(IAIBrain _brain, ICharacterFacade _character, DiContainer _container);
        void UpdateTemplate(float _delta);
        void ToggleActive(bool _val);
        void ResetTemplate();
        IAITemplate Clone();
    }

    public abstract class AITemplateBase : DbEntryBase, IAITemplate
    {
        private bool isActive = false;
        private bool initialized = false;
        
        protected IAIBrain          brain;
        protected ICharacterFacade  character;
        protected DiContainer       diContainer;

        // *****************************
        // Init 
        // *****************************
        public void Init(IAIBrain _brain, ICharacterFacade _character, DiContainer _container)
        {
            brain       = _brain;
            character   = _character;
            diContainer = _container;
            OnInit();
            initialized = true;
        }

        // *****************************
        // OnInit 
        // *****************************
        protected virtual void OnInit()
        {
            
        }

        // *****************************
        // UpdateTemplate 
        // *****************************
        public void UpdateTemplate(float _delta)
        {
            if (!initialized || !isActive)
            {
                return;
            }

            OnUpdate(_delta);
        }

        // *****************************
        // OnUpdate 
        // *****************************
        protected virtual void OnUpdate(float _delta)
        {

        }

        // *****************************
        // ToggleActive 
        // *****************************
        public void ToggleActive(bool _val)
        {
            isActive = _val;

            if (_val)
            {
                OnTemplateActivated();
            }
            else
            {
                OnTemplateDeactivated();
            }
        }

        // *****************************
        // OnTemplateActivated 
        // *****************************
        protected virtual void OnTemplateActivated()
        {

        }

        // *****************************
        // OnTemplateDeactivated 
        // *****************************
        protected virtual void OnTemplateDeactivated()
        {

        }

        // *****************************
        // ResetTemplate 
        // *****************************
        public void ResetTemplate()
        {
            OnResetTemplate();
            isActive = false;
        }

        // *****************************
        // OnResetTemplate 
        // *****************************
        protected virtual void OnResetTemplate()
        {

        }

        // *****************************
        // Clone 
        // *****************************
        public IAITemplate Clone()
        {
            if (initialized)
            {
                Debug.Assert(false, "Cant clone template which was initialized!");
            }

            return ScriptableObject.Instantiate(this);
        }
    }
}