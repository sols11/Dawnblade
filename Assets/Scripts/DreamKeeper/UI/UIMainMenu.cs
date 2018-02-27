using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UIMainMenu : ViewBase
    {
        public Button characterInfo;
        public Button pack;
        public Button store;
        public Button make;
        public Button battle;
        public Button train;
        public Button setting;
        public Button help;
        public Button exit;
        public Button sale;

        void Awake()
        {
            //定义本窗体的性质
            base.UIForm_Type = UIFormType.Normal;
            base.UIForm_ShowMode = UIFormShowMode.Normal;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;
            //ShowText(battleText,"Battle",2);
        }

        void Start()
        {
            characterInfo.onClick.AddListener(CharacterInfo);
            battle.onClick.AddListener(Battle);
            train.onClick.AddListener(Train);
            pack.onClick.AddListener(Pack);
            store.onClick.AddListener(Store);
            make.onClick.AddListener(Make);
            setting.onClick.AddListener(Setting);
            help.onClick.AddListener(Help);
            exit.onClick.AddListener(Exit);
            sale.onClick.AddListener(Sale);
        }

        private void CharacterInfo()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("CharacterInfo");
        }

        private void Battle()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("StageMenu");
        }

        private void Train()
        {
            GameLoop.Instance.sceneStateController.SetState(SceneState.TrainScene, true);
        }

        private void Pack()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("Inventory");
        }

        private void Store()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("Store");
        }

        private void Sale()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("StoreSale");
        }

        private void Help()
        {

        }

        private void Make()
        {
            UIPromptBox.wordID = 8;
            GameMainProgram.Instance.uiManager.ShowUIForms("PromptBox");
        }

        private void Setting()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("Setting");
        }

        private void Exit()
        {
            GameLoop.Instance.sceneStateController.ExitGame();
        }

    }
}