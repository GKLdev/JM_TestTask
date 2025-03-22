using System.Collections;
using System.Collections.Generic;
using Modules.UI.UIController_Public;
using UnityEngine;

namespace Modules.UI.Screens.HUD_Public
{
    public interface IUIPresenterHUD : IScreenPresenter
    {
        void ShowHint(bool _show, HUDHintType _type = HUDHintType.Undefined);
    }

    public enum HUDHintType
    {
        Undefined  = -1,
        ItemPickup = 0
    }
}