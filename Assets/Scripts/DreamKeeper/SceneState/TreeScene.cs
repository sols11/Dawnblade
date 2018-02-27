using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    /// <summary>
    /// 【暂时不用的场景，以后需要修改】
    /// </summary>
    public class TreeScene : ISceneState
    {
        private GameMainProgram gameMainProgram;    //主程序

        public TreeScene(SceneStateController controller) : base(controller)
        {
            this.StateName = "Tree";
        }

        public override void StateBegin()
        {
            gameMainProgram = GameMainProgram.Instance;
            gameMainProgram.Initialize();
            gameMainProgram.courseMgr.Enable = false;
            gameMainProgram.courseMgr.EnableNormalMenu = true;
            // 场景初始化
            gameMainProgram.playerMgr.BuildPlayerPeace(new Vector3(237,0,261), Quaternion.Euler(0,135,0));
            gameMainProgram.playerMgr.CanInput = true;  // 接受输入
            // Npc在Player之后

            gameMainProgram.npcMgr.CreateNpc("Merchant");
            gameMainProgram.npcMgr.CreateNpc("Recycler");
            gameMainProgram.npcMgr.CreateNpc("Battler");
            gameMainProgram.npcMgr.CreateNpc("Trainer");

            GameMainProgram.Instance.uiManager.ShowUIForms("FadeOut");
            gameMainProgram.audioMgr.PlayMusic("WhiteLie");
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
