using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.PlayerProgression_Public
{
    [CreateAssetMenu(fileName = "ConfigPlayerProgression", menuName = "Configs/PlayerProgression", order = 1)]
    public class ConfigPlayerProgression : ScriptableObject
    {
        [SerializeField] 
        private List<StatConfig> stats = new();

        public List<StatConfig> P_Stats => stats;
    }

    [System.Serializable]
    public class StatConfig
    {
        [Tooltip("The alias of the stat used to identify it")]
        [SerializeField] private string alias = "";

        [Tooltip("The minimum value the stat can have")]
        [SerializeField] private int minValue = 0;

        [Tooltip("The maximum value the stat can have")]
        [SerializeField] private int maxValue = 100;

        public string P_Alias => alias;
        public int P_MinValue => minValue;
        public int P_MaxValue => maxValue;
    }
}
