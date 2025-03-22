using System.Collections;
using System.Collections.Generic;
using GDTUtils;
using Modules.ModuleManager_Public;
using Modules.UI.UIController_Public;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Serialization;
using Zenject;

namespace Modules.UI.UIController
{
    public class UIController : LogicBase, IUIController
    {
        public bool UIIsBlocked { get => state.dynamicData.uiIsBlocked; set => state.dynamicData.uiIsBlocked = true; }

        [field: SerializeField]
        private State state = new();

        [Inject] 
        private IModuleManager moduleManager;
        
        // *****************************
        // InitModule
        // *****************************
        public void InitModule()
        {
            if (state.dynamicData.initialized)
            {
                throw new System.Exception("UIController: cannot be initialized twice!");
            }
            
            CompInit.Init(state, moduleManager);
        }

        // *****************************
        // OnUpdate
        // *****************************
        public void OnUpdate()
        {
        }

        // *****************************
        // OnUpdate
        // *****************************
        public void ShowScreen(ScreenType _screenType, bool _hideOthers)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.initialized);
            CompScreenVisibility.ShowScreen(state, _screenType, _hideOthers);
        }

        // *****************************
        // HideScreen
        // *****************************
        public void HideScreen(ScreenType _screenType, bool _despawnView = false)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.initialized);
            CompScreenVisibility.HideScreen(state, _screenType, _despawnView);
        }

        // *****************************
        // HideAll
        // *****************************
        public void HideAll(bool _despawnViews = false)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.initialized);
            CompScreenVisibility.HideAll(state,_despawnViews);
        }

        // *****************************
        // ReportScreenShown
        // *****************************
        public void ReportScreenShown(ScreenType _screenType)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.initialized);
            CompScreenVisibility.OnScreenShown(state, _screenType);
        }

        // *****************************
        // ReportScreeHidden
        // *****************************
        public void ReportScreeHidden(ScreenType _screenType)
        {
            LibModuleExceptions.ExceptionIfNotInitialized(state.dynamicData.initialized);
            CompScreenVisibility.OnScreenHidden(state, _screenType);
        }
    }

    [System.Serializable]
    public class State
    {
        public ConfigUIScreens configScreens;
        public Transform screensParent;
        
        public DynamicData dynamicData = new();
        
        [System.Serializable]
        public class DynamicData
        {
            /// <summary>
            /// enabled when screen is being shown or hidden
            /// </summary>
            public bool awaitingScreen = false;
            public bool initialized    = false;
            public bool debug          = false;

            public IModuleManager                          moduleManager;
            public Dictionary<ScreenType, ScreenContainer> dictScreens = new();
            public bool                                    uiIsBlocked = false;
            
            [System.Serializable]
            public class ScreenContainer
            {
                public ScreenType                            screenType;
                public ScreenState                           screenState;
                public ConfigUIScreens.ScreenConfigContainer config;

                public bool PresenterLoaded   => presenter != null;
                public bool ViewLoaded        => screenView != null;
                public bool ScreenIsUsable    => PresenterLoaded && ViewLoaded;
                
                public IScreenView         screenView;
                public IScreenPresenter    presenter;

                public bool viewMarkedForDespawn = false;
            }
            
            public enum ScreenState
            {
                Undefined = 0,
                Shown,
                Hidden,
                Transition
            }
        }
    }
}