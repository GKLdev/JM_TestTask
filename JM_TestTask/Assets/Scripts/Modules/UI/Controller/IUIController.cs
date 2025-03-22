using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI.UIController_Public
{
    public interface IUIController : IModuleInit, IModuleUpdate
    {
        public void ShowScreen(ScreenType _screenType, bool _hideOthers);
        public void HideScreen(ScreenType _screenType, bool _despawnView = false);
        public void HideAll(bool _despawnViews = false);
        
        public void ReportScreenShown(ScreenType _screenType);
        public void ReportScreeHidden(ScreenType _screenType);

        /// <summary>
        /// Tells ui view using that property that ui screens are blocked.
        /// </summary>
        bool UIIsBlocked { get; set; }
    }
}