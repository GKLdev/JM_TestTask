using Modules.PlayerProgression_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.PlayerProgression
{
    public static class CompStatManagement
    {
        // *****************************
        // SetStatValue
        // *****************************
        public static void SetStatValue(State _state, int _val, string _alias, PlayerProgression _self)
        {
            // Check if the stat already exists
            bool statExists = _state.dynamicData.stats.ContainsKey(_alias);

            if (!statExists)
            {
                Debug.Assert(false, $"Stat with alias {_alias} not found in config");
                return;
            }

            var statContainer = _state.dynamicData.stats[_alias];

            // Clamp the value between min and max
            int minValue = statContainer.min;
            int maxValue = statContainer.max;
            int clampedValue = Mathf.Clamp(_val, minValue, maxValue);

            bool needEvent = _state.dynamicData.stats[_alias].value != clampedValue;

            // Update existing stat
            _state.dynamicData.stats[_alias].value = clampedValue;

            // Shoot event
            if (needEvent)
            {
                _self.RaiseStatChangedEvent(_alias, clampedValue);
            }
        }

        // *****************************
        // GetStatValue
        // *****************************
        public static int GetStatValue(State _state, string _alias)
        {
            // Check if the stat exists
            bool statExists = _state.dynamicData.stats.ContainsKey(_alias);

            if (statExists)
            {
                return _state.dynamicData.stats[_alias].value;
            }

            // Return 0 if stat doesn't exist
            return 0;
        }
    }
}
