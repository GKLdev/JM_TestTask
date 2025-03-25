using System.Collections;
using System.Collections.Generic;
using Modules.UI.UIController_Public;
using UnityEngine;
using Zenject;

namespace Modules.UI.UIController_Public
{
    public abstract class ScreenViewBase<TPresenter> : LogicBase, IScreenView
        where TPresenter : IScreenPresenter 
    {
        [SerializeField]
        private Canvas root;

        protected bool       isInitialised => initialised;
        private   bool       initialised   = false;

        protected TPresenter presenter;
        
        // *****************************
        // Init
        // *****************************
        void IScreenView.Init(IScreenPresenter _presenter)
        {
            if (initialised)
            {
                return;
            }

            presenter = (TPresenter)_presenter;
            
            OnInitialisation();
            initialised = true;
            OnInitialised();
        }
                
        // *****************************
        // OnInitialised
        // *****************************
        protected virtual void OnInitialisation()
        {
        }

        // *****************************
        // OnInitialised
        // *****************************
        protected virtual void OnInitialised()
        {
            
        }

        // *****************************
        // Show
        // *****************************
        public virtual void Show()
        {
            LibModuleExceptions.ExceptionIfNotInitialized(initialised);
            root.enabled = true;
        }

        // *****************************
        // InitModule
        // *****************************
        public virtual void Hide()
        {
            LibModuleExceptions.ExceptionIfNotInitialized(initialised);
            root.enabled = false;
        }

        // *****************************
        // Dispose
        // *****************************
        public virtual void Dispose()
        {
            GameObject.Destroy(gameObject);
        }
    }
}