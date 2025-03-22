using System;
using System.Collections;
using System.Collections.Generic;
using Modules.ReferenceDb_Public;
using Modules.UI.Screens.HUD_Public;
using Modules.UI.UIController_Public;
using UnityEngine;
using Zenject;

namespace Modules.UI.Screens.HUD
{
    public class UIPresenterHUD : ScreenPresenterBase<IUIScreenViewHUD>, IUIPresenterHUD
    {
        [Inject] 
        private IReferenceDb reference;
        
        [Inject] 
        private ReferenceDbAliasesConfig aliases;
        
        private ConfigHUD config;
        
        // *****************************
        // OnInitialised
        // *****************************
        protected override void OnInitialised()
        {
            base.OnInitialised();

            //int id = aliases.CONFIG_HUD
            //config = reference.GetEntry<ConfigHUD>((int)CATEGORY_SCREEN_CONFIGS.CONFIG_HUD);
        }

        // *****************************
        // ShowHint
        // *****************************
        public void ShowHint(bool _show, HUDHintType _type = HUDHintType.Undefined)
        {
            Sprite image = null;
            string text  = default;
            
            switch (_type)
            {
                case HUDHintType.Undefined:
                    break;
                case HUDHintType.ItemPickup:
                    image = config.IconPickupItem;
                    text = "Pickup item";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_type), _type, null);
            }
            
            view.ToggleHint(_show, image, text);
        }

        // *****************************
        // Dispose
        // *****************************
        public override void Dispose()
        {
            base.Dispose();
            
            reference = null;
            aliases   = null;
            config    = null;
        }
    }
}
