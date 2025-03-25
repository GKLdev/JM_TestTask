using GDTUtils.Extensions;
using Modules.InputManager_Public;
using Modules.PlayerProgression_Public;
using Modules.UI.Screens.HUD_Public;
using Modules.UI.Screens.Upgrades_Public;
using Modules.UI.UIController_Public;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using Zenject;

namespace Modules.UI.Screens.Upgrades
{
    public class UIPresenterUpgrades : ScreenPresenterBase<IUIScreenViewUpgrades>, IUIPresenterUpgrades
    {
        [Inject]
        IPlayerProgression progression;

        [Inject]
        IUIController ui;

        [Inject]
        IInputManager input;

        private Dictionary<string, StatContainer> dictStats = new();
        private List<string> trackedStats = new()
        {
            PlayerProgressionAliases.health,
            PlayerProgressionAliases.speed,
            PlayerProgressionAliases.damage
        };

        private int totalSparePoints = 0;

        // *****************************
        // OnInitialised
        // *****************************
        protected override void OnInitialised()
        {
            base.OnInitialised();

            // very fast implementation
            dictStats.Clear();

            for (int i = 0; i < trackedStats.Count; i++)
            {
                var container = new StatContainer();
                container.alias = trackedStats[i];

                dictStats.Add(container.alias, container);
            }
        }

        // *****************************
        // Show
        // *****************************
        public override void Show()
        {
            base.Show();
            UpdateData(true);
            UpdateView();
        }

        // *****************************
        // Hide
        // *****************************
        public override void Hide()
        {
            base.Hide();
        }

        // *****************************
        // ReportStatChange
        // *****************************
        public void ReportStatChange(int _delta, string _stat)
        {
            var cont                = dictStats[_stat];
            int clampedDelta        = Mathf.Clamp(_delta, cont.points - cont.desiredPointsValue, cont.maxLimit - cont.desiredPointsValue);
            int realSparePoints     = progression.GetAvailableUpgradePointsCount();

            if (clampedDelta > 0)
            {
                clampedDelta = Mathf.Clamp(clampedDelta, 0, totalSparePoints);
            }
            else
            {
                clampedDelta = Mathf.Clamp(clampedDelta, -realSparePoints, 0);
            }

            totalSparePoints -= clampedDelta;
            cont.desiredPointsValue += clampedDelta;

            // Update view
            view.UpdateTotalPointsCounter(totalSparePoints);
            UpdateStatView(_stat);
        }

        // *****************************
        // ConfirmPointsChange
        // *****************************
        public void ConfirmPointsChange()
        {
            dictStats.ForEach(x => {
                progression.SpendUpgradePoints(x.Value.desiredPointsValue - x.Value.points, x.Value.alias);
            });

            CloseScreen();
        }

        // *****************************
        // CloseScreen
        // *****************************
        public void CloseScreen()
        {
            ui.ShowScreen(ScreenType.HUD, true);
            input.SetContext(InputContext.World);
        }

        // *****************************
        // UpdateData
        // *****************************
        public void UpdateData(bool _resetDesiredPoints = false)
        {
            dictStats.ForEach(x => {
                progression.GetStatLimits(x.Value.alias, out int min, out int max);

                x.Value.points = progression.GetStatValue(x.Value.alias);
                x.Value.minLimit = min;
                x.Value.maxLimit = max;

                if (_resetDesiredPoints)
                {
                    x.Value.desiredPointsValue = x.Value.points;
                }
            });

            totalSparePoints = progression.GetAvailableUpgradePointsCount();
        }

        // *****************************
        // UpdateView
        // *****************************
        public void UpdateView()
        {
            view.UpdateTotalPointsCounter(totalSparePoints);
            dictStats.ForEach(x => {
                UpdateStatView(x.Value.alias);
            });
        }

        // *****************************
        // UpdateStatView
        // *****************************
        private void UpdateStatView(string _stat)
        {
            var cont = dictStats[_stat];
            view.UpdateStatBlock(_stat, cont.desiredPointsValue, cont.maxLimit);
        }
    }

    public class StatContainer
    {
        public int      points           = 0;
        public int      minLimit         = 0;
        public int      maxLimit         = 0;
        public int      desiredPointsValue    = 0;
        public string   alias;
    }
}