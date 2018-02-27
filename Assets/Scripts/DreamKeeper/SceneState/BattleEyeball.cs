using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    public class BattleEyeball : ISceneState
    {
        private GameMainProgram gameMainProgram;//主程序

        public BattleEyeball(SceneStateController controller) : base(controller)
        {
            this.StateName = "BattleEyeball";
        }

        public override void StateBegin()
        {
            gameMainProgram = GameMainProgram.Instance;
            gameMainProgram.Initialize();
            // 启用Mgr
            gameMainProgram.courseMgr.Enable = true;
            // 场景初始化
            gameMainProgram.playerMgr.BuildPlayer(new Vector3(0, 0, -10), Quaternion.identity);
            gameMainProgram.playerMgr.CanInput = true;  // 接受输入

            // 创建Monster
            gameMainProgram.enemyMgr.AddEnemy(new EnemyEyeball(gameMainProgram.resourcesMgr.LoadAsset
                (@"Enemys\Eyeball", false, new Vector3(0, 2.16f, 10), Quaternion.Euler(0, 180, 0))));
            // UI在后面初始化
            gameMainProgram.uiManager.ShowUIForms("PlayerHUD");
            gameMainProgram.uiManager.ShowUIForms("MedicineHUD");
            //gameMainProgram.audioMgr.PlayMusic("FinalBattle");

        }

        public override void StateEnd()
        {
            gameMainProgram.audioMgr.StopMusic();
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