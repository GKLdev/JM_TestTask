using System.Collections;
using System.Collections.Generic;
using GDTUtils;
using GDTUtils.Extensions;
using Modules.UI.Button_Public;
using Modules.UI.Common;
using Modules.UI.Screens.HUD_Public;
using Modules.UI.Screens.Main.ProgressBar_Public;
using Modules.UI.UIController;
using Modules.UI.UIController_Public;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Modules.UI.Screens.HUD
{
    public class UIScreenHUDView : ScreenViewBase<IUIPresenterHUD>, IUIScreenViewHUD
    {
        [SerializeField]
        private TextMeshProUGUI                     textHP;
        [SerializeField]
        private TextMeshProUGUI                     textUpgrades;
        [SerializeField]
        private SerializedInterface<IProgressBar>   progressBar;
        [SerializeField]
        private SerializedInterface<Button<ButtonResponseDataBase, ButtonInputDataBase>> buttonOpenOpgradeScreen;

        [SerializeField]
        private RectTransform   pointsIndicatorRoot;
        [SerializeField]
        private TextMeshProUGUI pointsIndicatorText;

        [Header("Text strings")]
        [SerializeField]
        private string localeHealthBar = "HP";

        // *****************************
        // Show
        // *****************************
        public override void Show()
        {
            base.Show();
            presenter.ReportScreenShown();
            SetUpgradesAvailable(0);
            UpdaterHPBar(-1, -1);

            textUpgrades.text = $"Press [B] to levelup!";
        }

        // *****************************
        // Hide
        // *****************************
        public override void Hide()
        {
            base.Hide();
            presenter.ReportScreenHidden();
        }

        // *****************************
        // SetUpgradesAvailable
        // *****************************
        public void SetUpgradesAvailable(int _count)
        {
            bool available = _count > 0;
            textUpgrades.gameObject.SetActive(available);
            pointsIndicatorRoot.gameObject.SetActive(available);

            if (available)
            {
                pointsIndicatorText.text = _count.ToString();
            }
        }

        // *****************************
        // UpdaterHPBar
        // *****************************
        public void UpdaterHPBar(int _current, int _max)
        {
            // Can be optimzed be using StringBuilder or Span, but using simple approach for now
            textHP.text = $"{localeHealthBar}: {_current}/{_max}";
            progressBar.Value.SetData(_current, _max);
        }
    }
}