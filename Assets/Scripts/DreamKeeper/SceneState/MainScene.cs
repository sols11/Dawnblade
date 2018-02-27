using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    /// <summary>
    /// 【以后要修改】
    /// </summary>
    public class MainScene : ISceneState
    {
        private GameMainProgram gameMainProgram;//主程序

        public MainScene(SceneStateController controller) : base(controller)
        {
            this.StateName = "Main";
        }

        public override void StateBegin()
        {
            gameMainProgram = GameMainProgram.Instance;
            gameMainProgram.Initialize();
            gameMainProgram.courseMgr.Enable = false;
            gameMainProgram.courseMgr.EnableNormalMenu = true;
            // Player
            gameMainProgram.playerMgr.BuildPlayer(Vector3.zero, Quaternion.identity);

            gameMainProgram.playerMgr.CanInput = false;  // 禁止输入
 
            //UI初始化
            //gameMainProgram.uiManager.ShowUIForms("FadeOut");
            gameMainProgram.uiManager.ShowUIForms("MainMenu");
            //gameMainProgram.audioMgr.PlayMusic(1);

        }

        public override void StateEnd()
        {
            gameMainProgram.gameDataMgr.Save(gameMainProgram.playerMgr.CurrentPlayer);  // Save
            // State切换时，GameMainProgram及其各个Mgr并没有销毁，而是调用Release，然后重新Initialize
            gameMainProgram.Release();
        }

        public override void StateUpdate()
        {
            gameMainProgram.Update();
        }

        public override void FixedUpdate()
        {
            gameMainProgram.FixedUpdate();
        }

    }
}