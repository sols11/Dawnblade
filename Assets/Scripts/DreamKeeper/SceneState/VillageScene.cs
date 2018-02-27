using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    public class VillageScene : ISceneState
    {
        private GameMainProgram gameMainProgram;    //主程序

        public VillageScene(SceneStateController controller) : base(controller)
        {
            this.StateName = "Village";
        }

        public override void StateBegin()
        {
            gameMainProgram = GameMainProgram.Instance;
            gameMainProgram.Initialize();
            gameMainProgram.courseMgr.Enable = false;
            gameMainProgram.courseMgr.EnableNormalMenu = true;
            // 场景初始化
            gameMainProgram.playerMgr.BuildPlayerPeace(new Vector3(-4, 0, -6), Quaternion.Euler(0, -95, 0));
            gameMainProgram.playerMgr.CanInput = true;  // 接受输入
            // Npc在Player之后

            gameMainProgram.npcMgr.CreateNpc("Merchant");
            gameMainProgram.npcMgr.CreateNpc("Recycler");
            gameMainProgram.npcMgr.CreateNpc("Battler");
            gameMainProgram.npcMgr.CreateNpc("Trainer");

            GameMainProgram.Instance.uiManager.ShowUIForms("FadeOut");
            gameMainProgram.audioMgr.PlayMusic("Medieval");
        }

        public override void StateEnd()
        {
            // 切换场景时，可能NPC对话仍未结束
            gameMainProgram.eventMgr.InvokeEvent(EventName.DialogActionComplete);
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
