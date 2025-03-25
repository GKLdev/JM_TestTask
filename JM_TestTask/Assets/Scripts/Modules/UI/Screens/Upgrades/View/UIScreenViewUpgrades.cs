using GDTUtils;
using Modules.PlayerProgression_Public;
using Modules.UI.Button_Public;
using Modules.UI.Common;
using Modules.UI.Screens.Upgrades_Public;
using Modules.UI.UIController_Public;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Modules.UI.Screens.Upgrades
{
    public class UIScreenViewUpgrades : ScreenViewBase<IUIPresenterUpgrades>, IUIScreenViewUpgrades
    {
        [SerializeField]
        private SerializedInterface<Button<ButtonResponseDataBase, ButtonInputDataBase>> buttonConfirm;

        [SerializeField]
        private SerializedInterface<Button<ButtonResponseDataBase, ButtonInputDataBase>> buttonEscape;

        // very fast implementation
        public List<StatContainer>  statBlocks = new();
        public TextMeshProUGUI      textTotalPoints;
        public TextMeshProUGUI      textConfirmButton;

        [Header("Text strings")]
        public string localeConfirm     = "Confirm";
        public string localeTotaPoints  = "Points available";

        private int deltaChange = 1;
        private Dictionary<string, StatContainer> dictStatBlocks = new();

        // *****************************
        // OnInitialised
        // *****************************
        protected override void OnInitialised()
        {
            base.OnInitialised();

            for (int i = 0; i < statBlocks.Count; i++) {
                var cont = statBlocks[i];
                dictStatBlocks.Add(cont.alias, cont);
            }
        }

        // *****************************
        // Show
        // *****************************
        public override void Show()
        {
            base.Show();
            Setup(true);
            textConfirmButton.text = localeConfirm;
            presenter.ReportScreenShown();
        }

        // *****************************
        // QueueHideScreen
        // *****************************
        public override void Hide()
        {
            base.Hide();
            Setup(false);
            presenter.ReportScreenHidden();
        }

        // *****************************
        // UpdateStatBlock
        // *****************************
        public void UpdateStatBlock(string _alias, int _currPoints, int _totalpoints)
        {
            bool success = dictStatBlocks.TryGetValue(_alias, out StatContainer cont);
            if (success)
            {
                cont.block.P_CurrentPoints      = _currPoints;
                cont.block.P_TotalPoints        = _totalpoints;
                cont.block.P_StatName           = cont.localizedName;
            }
        }

        // *****************************
        // UpdateTotalPointsCounter
        // *****************************
        public void UpdateTotalPointsCounter(int _count)
        {
            textTotalPoints.text = $"{localeTotaPoints}: {_count}";
        }

        // *****************************
        // Setup
        // *****************************
        private void Setup(bool _val)
        {
            if (_val)
            {
                // Stat blocks
                foreach (var item in statBlocks)
                {
                    SetupStat(item.block, item.alias);
                    item.block.Show();
                }

                // Navigation buttons
                buttonConfirm.Value.P_ButtonCallback += (x) => presenter.ConfirmPointsChange();
                buttonEscape.Value.P_ButtonCallback += (x) => presenter.CloseScreen();
            }
            else
            {
                // Stat blocks
                foreach (var item in statBlocks)
                {
                    item.block.Hide();
                    ClearStatSetup(item.block);
                }

                // Navigation buttons
                buttonConfirm.Value.P_ButtonCallback    = null;
                buttonEscape.Value.P_ButtonCallback     = null;
            }

            void SetupStat(StatBlockView _view, string _alias)
            {
                _view.OnIncreased += () => presenter.ReportStatChange(deltaChange, _alias);
                _view.OnDecreased += () => presenter.ReportStatChange(-deltaChange, _alias);
            }

            void ClearStatSetup(StatBlockView _view)
            {
                _view.OnIncreased = null;
                _view.OnDecreased = null;
            }
        }


        [System.Serializable]
        public class StatContainer {
            public StatBlockView    block;
            public string           alias;
            public string           localizedName;
        }

    }
}