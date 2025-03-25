using Modules.UI.UIController_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI.Screens.Upgrades_Public
{
    public interface IUIPresenterUpgrades : IScreenPresenter
    {
        void ReportStatChange(int _delta, string _stat);
        void ConfirmPointsChange();
        void CloseScreen();
    }
}