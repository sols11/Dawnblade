using System;
using System.Collections.Generic;
using SFramework;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UINormalMenu:ViewBase
    {
        public Button Resume;
        public Button Character;
        public Button Inventory;
        public Button Tasks;
        public Button Setting;
        public Button Help;
        public Button SaveAndExit;

        void Awake()
        {
            //定义本窗体的性质(弹出窗体)
            base.UIForm_Type = UIFormType.PopUp;
            base.UIForm_ShowMode = UIFormShowMode.ReverseChange;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;
        }

        private void Start()
        {
            Resume.onClick.AddListener(OnBtnResume);
            Character.onClick.AddListener(OnBtnCharacter);
            Inventory.onClick.AddListener(OnBtnInventory);
            Tasks.onClick.AddListener(OnBtnTasks);
            Setting.onClick.AddListener(OnBtnSetting);
            Help.onClick.AddListener(OnBtnHelp);
            SaveAndExit.onClick.AddListener(OnBtnSaveAndExit);
        }

        private void OnBtnResume()
        {
            GameMainProgram.Instance.courseMgr.OnBtnCanCel();
        }

        private void OnBtnCharacter()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("CharacterInfo");
        }

        private void OnBtnInventory()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("Inventory");
        }

        private void OnBtnTasks()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("TasksMenu");
        }

        private void OnBtnSetting()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("Setting");
        }

        private void OnBtnHelp()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("HelpMenu");
        }

        private void OnBtnSaveAndExit()
        {
            GameMainProgram.Instance.gameDataMgr.Save(GameMainProgram.Instance.playerMgr.CurrentPlayer);
            GameLoop.Instance.sceneStateController.SetState(SceneState.StartState);
        }
    }
}
