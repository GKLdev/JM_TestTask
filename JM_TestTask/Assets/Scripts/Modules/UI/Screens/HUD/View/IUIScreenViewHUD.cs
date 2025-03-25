using System.Collections;
using System.Collections.Generic;
using Modules.UI.UIController_Public;
using UnityEngine;

namespace Modules.UI.Screens.HUD_Public
{
    public interface IUIScreenViewHUD : IScreenView
    {
        void SetUpgradesAvailable(int _count);
        void UpdaterHPBar(int _current, int _max);
    }
}