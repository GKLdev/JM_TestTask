using GDTUtils;
using Modules.UI.Button_Public;
using Modules.UI.Common;
using Modules.UI.Screens.Main.ProgressBar_Public;
using Modules.UI.UIController_Public;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Modules.UI.Screens.Upgrades
{
    public class StatBlockView : LogicBase, IViewElement
    {
        public string   P_StatName  { set => OnSetName(value); }
        public int      P_CurrentPoints  { set { pointsCurrent = value; OnPointsChanged(); } }
        public int      P_TotalPoints    { set { pointsTotal = value; OnPointsChanged(); } }

        public Action OnIncreased;
        public Action OnDecreased;

        [SerializeField]
        private SerializedInterface<Button<ButtonResponseDataBase, ButtonInputDataBase>> buttonIncrease;

        [SerializeField]
        private SerializedInterface<Button<ButtonResponseDataBase, ButtonInputDataBase>> buttonDecrease;

        [SerializeField]
        private TextMeshProUGUI textStatName;

        [SerializeField]
        private TextMeshProUGUI textStatProgress;

        [SerializeField]
        private SerializedInterface<IProgressBar> progressBar;

        private int pointsCurrent   = 0;
        private int pointsTotal     = 0;
        
        // *****************************
        // InitModule
        // *****************************
        public void InitModule()
        {
        }

        // *****************************
        // Show
        // *****************************
        public void Show()
        {
            buttonIncrease.Value.P_ButtonCallback += OnIncreaseButton;
            buttonDecrease.Value.P_ButtonCallback += OnDecreaseButton;
        }

        // *****************************
        // Hide
        // *****************************
        public void Hide()
        {
            buttonIncrease.Value.P_ButtonCallback = null;
            buttonDecrease.Value.P_ButtonCallback = null;
        }


        // *****************************
        // OnIncresedButton
        // *****************************
        private void OnIncreaseButton(ButtonResponseDataBase _data)
        {
            OnIncreased?.Invoke();
        }


        // *****************************
        // OnDecreaseButton
        // *****************************
        private void OnDecreaseButton(ButtonResponseDataBase _data)
        {
            OnDecreased?.Invoke();
        }

        // *****************************
        // OnSetName
        // *****************************
        private void OnSetName(string _name)
        {
            textStatName.text = _name;
        }

        // *****************************
        // OnSetPoints
        // *****************************


        // *****************************
        // OnPointsChanged
        // *****************************
        private void OnPointsChanged()
        {;
            textStatProgress.text = $"{pointsCurrent}/{pointsTotal}";
            progressBar.Value.SetData(pointsCurrent, pointsTotal);
        }
    }
}