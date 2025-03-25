using GDTUtils.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI.Button_Public
{
    // TODO: mb add second generic type for InputData
    public interface IButton<TResponse> : IGameObjectAccess 
        where TResponse : ButtonResponseDataBase
    {
        /// <summary>
        /// Set callback
        /// </summary>
        Action<TResponse> P_ButtonCallback { get; set; }
        
        /// <summary>
        /// Toggle lock
        /// </summary>
        bool P_ButtonIsLocked  { get; set; }
        
        /// <summary>
        /// Set custom data
        /// </summary>
        /// <param name="_data"></param>
        void SetData(ButtonInputDataBase _data);

        void SetResponseData(TResponse _responseData);
    }

    public class ButtonResponseDataBase
    {
        
    }

    public class ButtonInputDataBase
    {
        
    }
}