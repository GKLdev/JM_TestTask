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
        /// Gets Called after any stat was changed
        /// </summary>
        event System.Action<StatChangeData> OnPlayerStatChanged;
    }

    public struct StatChangeData
    {
        public string   alias;
        public int      currentValue;
    }
}