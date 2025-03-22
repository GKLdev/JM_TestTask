using System;
using System.Collections;
using System.Collections.Generic;
using Modules.ModuleManager_Public;
using Modules.UI.UIController_Public;
using UnityEngine;

namespace Modules.UI.UIController
{
    public static class CompInit
    {
        // *****************************
        // Init
        // *****************************
        public static void Init(State _state, IModuleManager _moduleManager)
        {
            _state.dynamicData.moduleManager = _moduleManager;

            int id = -1;
            // init screen containers
            _state.configScreens.screens.ForEach(x =>
            {
                id++;
                ScreenType screenType = (ScreenType)id;
                
                State.DynamicData.ScreenContainer screenContainer = new();
                screenContainer.screenType  = screenType;
                screenContainer.config      = x;
                screenContainer.screenState = State.DynamicData.ScreenState.Undefined;
                
                // add to screens
                _state.dynamicData.dictScreens.Add(screenType, screenContainer);
            });

            _state.dynamicData.initialized = true;
        }
    }
}
