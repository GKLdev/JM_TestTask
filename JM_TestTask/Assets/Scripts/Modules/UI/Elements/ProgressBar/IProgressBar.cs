using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI.Screens.Main.ProgressBar_Public
{
    public interface IProgressBar
    {
        void SetData(int _value, int _maxValue);
    }
    
    // *****************************
    // ScaleAxisType
    // *****************************
    public enum ScaleAxisType
    {
        Horizontal = 0,
        Vertical
    }
}