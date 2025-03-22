using System.Collections;
using System.Collections.Generic;
using GDTUtils.Extensions;
using Modules.UI.Screens.HUD_Public;
using Modules.UI.UIController;
using Modules.UI.UIController_Public;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI.Screens.HUD
{
    public class UIScreenHUDView : ScreenViewBase<IUIPresenterHUD>, IUIScreenViewHUD
    {
        [SerializeField]
        private GameObject hintRoot;

        [SerializeField]
        private TextMeshProUGUI hintText;

        [SerializeField]
        private Image hintIcon;
        
        // *****************************
        // Show
        // *****************************
        public override void Show()
        {
            base.Show();
            presenter.ReportScreenShown();
            hintRoot.SetActive(false);
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
        // ToggleHint
        // *****************************
        public void ToggleHint(bool _show, Sprite _image = null, string _text = null)
        {
            hintRoot.SetActive(_show);
            if (!_show)
            {
                return;
            }

            bool assignIcon = _image != null;
            if (assignIcon)
            {
                hintIcon.sprite = _image;
            }
            
            hintIcon.gameObject.SetActive(assignIcon);

            bool assignText = !_text.NullOrEmpty();
            if (assignText)
            {
                hintText.text = _text;
            }
            
            hintText.gameObject.SetActive(assignText);
        }
    }
}