using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.PlayerProgression_Public
{
    public interface IPlayerProgression : IModuleInit
    {
        /// <summary>
        /// Sets the value of a stat identified by the given alias.
        /// </summary>
        void SetStatValue(int _val, string _alias);

        /// <summary>
        /// Gets the value of a stat identified by the given alias.
        /// </summary>
        int GetStatValue(string _alias);

        /// <summary>
        /// Gets limits of a give stat.
        /// </summary>
        void GetStatLimits(string _alias, out int _min, out int _max);

        /// <summary>
        /// Adds points which can be traded for upgrades.
        /// </summary>
        void AddUpgradePoints(int _val);

        /// <summary>
        /// Returns count of free upgrade points.
        /// </summary>
        int GetAvailableUpgradePointsCount();

        /// <summary>
        /// Use upgrade points to increase given stat.
        /// </summary>
        void SpendUpgradePoints(int _val, string _alias);

        /// <summary>
        /// Get upgrade points back from given stat.
        /// </summary>
        void RefundUpgradePoints(int _val, string _alias);

        /// <summary>
        /// Resets upgrade points data. But not modifications data!
        /// </summary>
        void ResetUpgradePoints();

        /// <summary>
        /// Gets Called after any stat was changed
        /// </summary>
        event System.Action<StatChangeData> OnPlayerStatChanged;

        /// <summary>
        /// Gets Called after player receved or spent an upgrade point.
        /// </summary>
        event Action OnUpgradePointsCountChanged;
    }

    public struct StatChangeData
    {
        public string   alias;
        public int      currentValue;
    }
}