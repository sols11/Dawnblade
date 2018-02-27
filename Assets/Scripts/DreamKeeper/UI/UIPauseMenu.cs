using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UIPauseMenu : ViewBase
    {
        public Button Resume;
        public Button Setting;
        public Button Help;
        public Button BackMain;

        void Awake()
        {
            //定义本窗体的性质(弹出窗体)
            base.UIForm_Type = UIFormType.PopUp;
            base.UIForm_ShowMode = UIFormShowMode.ReverseChange;
            base.UIForm_LucencyType = UIFormLucenyType.Translucence;
        }

        void Start()
        {
            Resume.onClick.AddListener(OnBtnResume);
            Setting.onClick.AddListener(OnBtnSetting);
            Help.onClick.AddListener(OnBtnHelp);
            BackMain.onClick.AddListener(OnBtnBackMain);
        }

        private void OnBtnResume()
        {
            GameMainProgram.Instance.courseMgr.PauseGame();
        }

        private void OnBtnSetting()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("Setting");
        }

        private void OnBtnHelp()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("HelpMenu");
        }

        private void OnBtnBackMain()
        {
            OnBtnResume();
            GameLoop.Instance.sceneStateController.SetState(SceneState.VillageScene);
        }

    }
}