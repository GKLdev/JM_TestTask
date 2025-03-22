using System.Collections;
using System.Collections.Generic;
using Editor.CodeGen.UIController;
using UnityCodeGen;
using UnityEditor;
using UnityEngine;

namespace Editor.UIController
{
    [CustomEditor(typeof(Modules.UI.UIController.ConfigUIScreens))]
    public class UIScreensConfigEditor : UnityEditor.Editor
    {
        private Modules.UI.UIController.ConfigUIScreens config;
        
        // *****************************
        // OnInspectorGUI 
        // *****************************
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            config = target as Modules.UI.UIController.ConfigUIScreens;
            
            bool setupDatabaseIsPressed = GUILayout.Button("Setup");
            if (setupDatabaseIsPressed)
            {
                GenerateCode();
            }
        }

        void GenerateCode()
        {
            // TODO: make separate utility if there will be other generators
            // TODO: make path and file name as settings
            ScreensConfigGenerator.config     = config;
            ScreensConfigGenerator.fileName   = "ScreenType";
            ScreensConfigGenerator.outputPath = "Assets/Scripts/Modules/UI/Controller/Config";

            UnityCodeGenUtility.Generate();
        }
    }
}