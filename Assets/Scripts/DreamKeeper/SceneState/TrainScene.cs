using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    public class TrainScene :ISceneState
    {
        private GameMainProgram gameMainProgram;    //主程序

        public TrainScene(SceneStateController controller) : base(controller)
        {
            this.StateName = "Train";
        }

        public override void StateBegin()
        {
            gameMainProgram = GameMainProgram.Instance;
            gameMainProgram.Initialize();
            // 启用Mgr
            gameMainProgram.courseMgr.Enable = true;
            // 场景初始化
            gameMainProgram.playerMgr.BuildPlayer(Vector3.zero, Quaternion.identity);
            gameMainProgram.playerMgr.CanInput = true;  // 接受输入
            // Npc在Player之后
            //gameMainProgram.npcMgr.CreateNpc("Merchant");

            // UI在后面初始化
            gameMainProgram.uiManager.ShowUIForms("PlayerHUD");
            //【测试时先关闭，打包需要删除】GameMainProgram.Instance.uiManager.ShowUIForms("FadeOut");
            //gameMainProgram.audioMgr.PlayMusic(2);

        }

        public override void StateEnd()
        {
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
