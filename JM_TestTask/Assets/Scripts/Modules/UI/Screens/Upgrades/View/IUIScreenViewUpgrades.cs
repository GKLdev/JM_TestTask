using Modules.UI.UIController_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIScreenViewUpgrades : IScreenView
{
    void UpdateStatBlock(string _alias, int _currPoints, int _totalpoints);
    void UpdateTotalPointsCounter(int _count);
}
