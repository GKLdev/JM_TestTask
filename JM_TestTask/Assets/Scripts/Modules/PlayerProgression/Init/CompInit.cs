using Modules.PlayerProgression_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.PlayerProgression
{
    public static class CompInit
    {
        // *****************************
        // Init
        // *****************************
        public static void Init(State _state)
        {
            // Initialize the stats dictionary
            _state.dynamicData.stats = new Dictionary<string, PlayerStatContainer>();

            // Populate stats from config
            foreach (StatConfig statConfig in _state.config.P_Stats)
            {
                string alias = statConfig.P_Alias;

                // Check for duplicate aliases
                bool aliasExists = _state.dynamicData.stats.ContainsKey(alias);
                if (aliasExists)
                {
                    Debug.Assert(false, $"Duplicate stat alias found in config: {alias}");
                    return;
                }

                // Create new stat with default value (minValue)
                PlayerStatContainer stat = new PlayerStatContainer { value = statConfig.P_MinValue };
                stat.min = statConfig.P_MinValue;
                stat.max = statConfig.P_MaxValue;

                _state.dynamicData.stats[alias] = stat;
            }

            _state.dynamicData.isInitialized = true;
        }
    }
}
