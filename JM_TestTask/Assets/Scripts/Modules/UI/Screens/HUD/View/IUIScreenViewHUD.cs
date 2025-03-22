using System.Collections;
using System.Collections.Generic;
using Modules.UI.UIController_Public;
using UnityEngine;

namespace Modules.UI.Screens.HUD_Public
{
    public interface IUIScreenViewHUD : IScreenView
    {
        void ToggleHint(bool _show, Sprite _image = null, string _text = null);
    }
}