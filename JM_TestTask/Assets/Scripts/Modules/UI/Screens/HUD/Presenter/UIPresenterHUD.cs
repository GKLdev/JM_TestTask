using System;
using System.Collections;
using System.Collections.Generic;
using Modules.CharacterManager_Public;
using Modules.DamageManager_Public;
using Modules.PlayerProgression_Public;
using Modules.ReferenceDb_Public;
using Modules.UI.Screens.HUD_Public;
using Modules.UI.UIController_Public;
using UnityEngine;
using Zenject;

namespace Modules.UI.Screens.HUD
{
    public class UIPresenterHUD : ScreenPresenterBase<IUIScreenViewHUD>, IUIPresenterHUD
    {
        [Inject] 
        private IReferenceDb reference;
        
        [Inject] 
        private ReferenceDbAliasesConfig aliases;

        [Inject]
        private ICharacterManager characterMgr;
        
        [Inject]
        private IPlayerProgression playerProgression;

        private CharacterFacade_Public.ICharacterFacade player;

        // *****************************
        // OnInitialised
        // *****************************
        protected override void OnInitialised()
        {
            base.OnInitialised();
        }

        // *****************************
        // OnInitialised
        // *****************************
        public override void Show()
        {
            base.Show();

            player = characterMgr.GetPlayer();
            if (player != null)
            {
                UpdateHPBar(player.P_Damageable.GetCurrentHealth(), player.P_Damageable.GetMaxHealth());
                player.P_Damageable.OnDamageApplied += OnPlayerDamaged;
            }

            playerProgression.OnUpgradePointsCountChanged += OnPlayerStatChanged;
            OnPlayerStatChanged();
        }

        // *****************************
        // Hide
        // *****************************
        public override void Hide()
        {
            base.Hide();

            if (player != null)
            {
                player.P_Damageable.OnDamageApplied -= OnPlayerDamaged;
            }

            playerProgression.OnUpgradePointsCountChanged -= OnPlayerStatChanged;
            player = null;
        }

        // *****************************
        // Dispose
        // *****************************
        public override void Dispose()
        {
            base.Dispose();
            
            reference = null;
            aliases   = null;
        }

        // *****************************
        // OnPlayerDamaged
        // *****************************
        private void OnPlayerDamaged(bool _isDead, IDamageable _damagable)
        {
            UpdateHPBar(_damagable.GetCurrentHealth(), _damagable.GetMaxHealth());
        }

        // *****************************
        // UpdateHPBar
        // *****************************
        private void UpdateHPBar(float _current, float _max)
        {
            view.UpdaterHPBar(Mathf.FloorToInt(_current), Mathf.FloorToInt(_max));
        }

        // *****************************
        // OnPlayerStatChanged
        // *****************************
        private void OnPlayerStatChanged()
        {
            int sparePoints = playerProgression.GetAvailableUpgradePointsCount();
            UpdateUpgradeSection(sparePoints);
        }

        // *****************************
        // UpdateUpgradeSection
        // *****************************
        private void UpdateUpgradeSection(int _points)
        {
            view.SetUpgradesAvailable(_points);
        }
    }
}
