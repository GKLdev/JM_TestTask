using System;
using System.Collections;
using System.Collections.Generic;
using GDTUtils;
using Modules.ModuleManager_Public;
using Modules.UI.Screens.HUD_Public;
using Modules.UI.UIController_Public;
using UnityEngine;
using Zenject;
using Object = System.Object;

namespace Tests
{
    // TODO: mb need a way to inject dependency manually
    public class TEST_UIController : MonoBehaviour
    {
        [Inject]
        private IModuleManager  moduleManager;
        
        [Inject]
        private IUIController   uiController;
        
        //[Inject]
        //private IUIPresenterHUD hud;


        private Dependencies dependencies;
        
        // *****************************
        // Update
        // *****************************
        private void Update()
        {
            TestHUD();
        }
        
        // *****************************
        // TestHUD
        // *****************************
        void TestHUD()
        {
            if (Input.GetKeyDown("t"))
            {
                uiController.ShowScreen(ScreenType.HUD, false);
            }

            if (Input.GetKeyDown("y"))
            {
                if (dependencies == null)
                {
                    //moduleManager.Container.Inject(this);
                    Type type = typeof(Dependencies);
                    dependencies = moduleManager.Container.Instantiate(type) as Dependencies;
                }
                
                dependencies.hud.ShowHint(true, HUDHintType.ItemPickup);
            }

            if (Input.GetKeyDown("u"))
            {
                dependencies.hud.ShowHint(false);
            }
        }
                
        // *****************************
        // Dependencies
        // *****************************
        public class Dependencies
        {
            [Inject]
            public IUIPresenterHUD hud;
        }
    }
}