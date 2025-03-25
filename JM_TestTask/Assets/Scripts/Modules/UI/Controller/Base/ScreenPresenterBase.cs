using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Modules.UI.UIController_Public
{
    public class ScreenPresenterBase<TView> : IScreenPresenter
        where TView : IScreenView
    {
        private   ScreenType screenType = ScreenType.UNDEFINED;
        protected bool       isInitialised => initialised;
        private   bool       initialised   = false;

        [Inject]
        protected IUIController uiController;
        protected TView         view;
        
        // *****************************
        // Init
        // *****************************
        /// <summary>
        /// Is called only when View is instantiated
        /// </summary>
        /// <param name="_view"></param>
        void IScreenPresenter.Init(IScreenView _view, ScreenType _screenType)
        {
            view = (TView)_view;
            
            if (initialised)
            {
                return;
            }

            screenType = _screenType;
            OnInitialisation();
            initialised = true;
            OnInitialised();
        }
                
        // *****************************
        // OnInitialised
        // *****************************
        /// <summary>
        /// Pre initialization
        /// </summary>
        protected virtual void OnInitialisation()
        {
        }

        // *****************************
        // OnInitialised
        // *****************************
        /// <summary>
        /// Post initialization
        /// </summary>
        protected virtual void OnInitialised()
        {
            
        }
        
        // *****************************
        // Show
        // *****************************
        public virtual void Show()
        {
            view.Show();
        }

        // *****************************
        // Hide
        // *****************************
        public virtual void Hide()
        {
            view.Hide();
        }

        // *****************************
        // ReportScreenShown
        // *****************************
        /// <summary>
        /// Must be reported manually.
        /// </summary>
        void IScreenPresenter.ReportScreenShown()
        {
            uiController.ReportScreenShown(screenType);
        }

        // *****************************
        // ReportScreenHidden
        // *****************************
        /// <summary>
        /// Must be reported manually.
        /// </summary>
        void IScreenPresenter.ReportScreenHidden()
        {
            uiController.ReportScreeHidden(screenType);
        }
        
        // *****************************
        // Dispose
        // *****************************
        public virtual void Dispose()
        {
        }
    }
}