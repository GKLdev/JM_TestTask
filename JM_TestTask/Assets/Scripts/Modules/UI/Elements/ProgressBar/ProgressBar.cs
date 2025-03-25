using System.Collections;
using System.Collections.Generic;
using Modules.UI.Screens.Main.ProgressBar_Public;
using UnityEngine;

namespace Modules.UI.Screens.Main.ProgressBar
{
    public class ProgressBar : LogicBase, IProgressBar
    {
        [SerializeField]
        private GameObject scalable;

        [SerializeField]
        private ScaleAxisType scaleAxis;
        
        // *****************************
        // SetData
        // *****************************
        public void SetData(int _value, int _maxValue)
        {
            int max = Mathf.Clamp(_maxValue, 0, int.MaxValue);
            int val = Mathf.Clamp(_value, 0, _maxValue);

            bool     maxIsZero        = max == 0;
            float    scaleNormalized  = maxIsZero ? 0f : (float)val / (float)max;

            scalable.transform.localScale = scaleAxis == ScaleAxisType.Horizontal ? new Vector3(scaleNormalized, 1f, 1f) : new Vector3(1f, scaleNormalized, 1f);
        }
    }   
}
