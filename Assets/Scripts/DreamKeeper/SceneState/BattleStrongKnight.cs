using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    public class BattleStrongKnight : ISceneState
    {
        private GameMainProgram gameMainProgram;//主程序

        public BattleStrongKnight(SceneStateController controller) : base(controller)
        {
            this.StateName = "BattleDungeon";
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
            // 相机边界控制
            CameraCtrl.Instance.SetAreaLimit(1.7f, -1.5f, 7, -2.5f);
            // 创建Monster
            //gameMainProgram.enemyMgr.AddEnemy(new EnemyStrongKnight(gameMainProgram.resourcesMgr.LoadAsset
              //  (@"Enemys\StrongKnight", false, new Vector3(0, 0, 7), Quaternion.Euler(0, -180, 0))));
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