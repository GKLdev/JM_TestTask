using GDTUtils;
using Modules.DamageManager_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.DamageManager.DamageableVisualizer
{
    public class DamageableVisualizerTest : LogicBase
    {
        public SerializedInterface<IDamageable> target;

        public GameObject IdleState;
        public GameObject DestroyedState;

        public bool autoLinkToDamageable = false;


        // *****************************
        // Start
        // *****************************
        private void Start()
        {
            if (target.Value == null)
            {
                return;
            }

            if (autoLinkToDamageable)
            {
                target.Value.OnDamageApplied += OnDamage;
            }
        }

        // *****************************
        // OnDamage
        // *****************************
        public void OnDamage(bool _isDead, IDamageable _damagable)
        {
            IdleState.SetActive(!_isDead);
            DestroyedState.SetActive(_isDead);
        }
    }
}