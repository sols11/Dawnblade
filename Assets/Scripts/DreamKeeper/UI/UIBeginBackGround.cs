using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using UnityEngine.UI;


namespace DreamKeeper
{
    public class UIBeginBackGround:ViewBase
    {
        public Button btnNewGame;
        public Button btnLoad;
        public Button btnExit;
        private AudioSource audioSource;

        private void Awake()
        {
            base.UIForm_Type = UIFormType.Normal;
            base.UIForm_ShowMode = UIFormShowMode.Normal;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            btnNewGame.onClick.AddListener(NewGame);
            btnLoad.onClick.AddListener(Load);
            btnExit.onClick.AddListener(Exit);
        }

        void NewGame()
        {
            audioSource.Play();
            GameMainProgram.Instance.uiManager.ShowUIForms("FadeIn");
            Invoke("StartGame", 2);
            // 删除存档
            GameMainProgram.Instance.gameDataMgr.DeleteSaveData();
        }

        void StartGame()
        {
            // 切换Scene
            GameLoop.Instance.sceneStateController.SetState(SceneState.TutorialScene, true);
        }

        void Load()
        {
            audioSource.Play();
            GameMainProgram.Instance.uiManager.ShowUIForms("FadeIn");
            Invoke("LoadGame", 2);
        }

        void LoadGame()
        {
            // 切换Scene
            GameLoop.Instance.sceneStateController.SetState(SceneState.VillageScene, true);
        }

        void Exit()
        {
            audioSource.Play();
            GameLoop.Instance.sceneStateController.ExitGame();
        }
    }
}