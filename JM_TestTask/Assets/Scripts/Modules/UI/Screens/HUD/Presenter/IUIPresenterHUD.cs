using System.Collections;
using System.Collections.Generic;
using Modules.UI.UIController_Public;
using UnityEngine;

namespace Modules.UI.Screens.HUD_Public
{
    public interface IUIPresenterHUD : IScreenPresenter
    {
    }

    public enum HUDHintType
    {
        Undefined  = -1,
        ItemPickup = 0
    }
}