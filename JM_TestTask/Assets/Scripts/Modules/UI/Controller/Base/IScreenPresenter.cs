using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI.UIController_Public
{
    public interface IScreenPresenter : IDisposable
    {
        void Init(IScreenView _view);
        void Show();
        void Hide();
        
        /// <summary>
        /// Must be reported manually.
        /// </summary>
        void ReportScreenShown();
        
        /// <summary>
        /// Must be reported manually.
        /// </summary>
        void ReportScreenHidden();
    }
}