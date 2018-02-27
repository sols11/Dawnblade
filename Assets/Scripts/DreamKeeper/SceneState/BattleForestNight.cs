using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    public class BattleForestNight : ISceneState
    {
        private GameMainProgram gameMainProgram;//主程序
        private float zTrigger = 135;
        private bool hasTrigger = false;

        public BattleForestNight(SceneStateController controller) : base(controller)
        {
            this.StateName = "BattleForestNight";
        }

        public override void StateBegin()
        {
            gameMainProgram = GameMainProgram.Instance;
            gameMainProgram.Initialize();
            // 启用Mgr
            gameMainProgram.courseMgr.Enable = true;
            // 场景初始化
            gameMainProgram.playerMgr.BuildPlayer(new Vector3(123, 0, 187), Quaternion.Euler(0,90,0));
            //gameMainProgram.playerMgr.BuildPlayer(new Vector3(100, 0, 130), Quaternion.identity);
            gameMainProgram.playerMgr.CanInput = true;  // 接受输入
            // 锁定player的y轴以防和dragon碰撞Bug
            gameMainProgram.playerMgr.CurrentPlayer.Rgbd.constraints = (RigidbodyConstraints)(4 + 16 + 64);

            // UI在后面初始化
            gameMainProgram.uiManager.ShowUIForms("PlayerHUD");
            gameMainProgram.uiManager.ShowUIForms("MedicineHUD");
            GameMainProgram.Instance.uiManager.ShowUIForms("FadeOut");
            gameMainProgram.audioMgr.PlayMusic("Night");
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
            // 触发Dragon出现事件
            if(!hasTrigger&& gameMainProgram.playerMgr.CurrentPlayer.GameObjectInScene.transform.position.z<zTrigger)
            {
                hasTrigger = true;
                // 创建Monster
                gameMainProgram.enemyMgr.AddEnemy(new EnemyDragon(gameMainProgram.resourcesMgr.LoadAsset
                    (@"Enemys\DragonBoss", false, new Vector3(100, 8, 160), Quaternion.Euler(0, -180, 0))));
                CoroutineMgr.Instance.StartCoroutine(PlayBGM());
            }
        }

        private IEnumerator PlayBGM()
        {
            yield return new WaitForSeconds(1f);
            gameMainProgram.audioMgr.PlayMusic("HWComplex5");
        }

        public override void FixedUpdate()
        {
            gameMainProgram.FixedUpdate();
        }

    }
}