using UnityEngine;
using SFramework;
using System.Collections;

namespace DreamKeeper
{
    /// <summary>
    /// 【暂时不用的场景，以后需要修改】
    /// </summary>
    public class TutorialScene : ISceneState
    {
        private GameMainProgram gameMainProgram;    //主程序

        public TutorialScene(SceneStateController controller) : base(controller)
        {
            this.StateName = "Tutorial";
        }

        public override void StateBegin()
        {
            gameMainProgram = GameMainProgram.Instance;
            gameMainProgram.Initialize();
            // 启用Mgr
            gameMainProgram.courseMgr.Enable = true;
            // 场景初始化
            //CameraCtrl.Instance.EnableAutoCam(false);
            gameMainProgram.playerMgr.BuildPlayer(new Vector3(-6,0,3), Quaternion.Euler(0,120,0));
            gameMainProgram.playerMgr.CanInput = true;  // 接受输入
            // 创建Enemy
            gameMainProgram.enemyMgr.AddEnemy(new EnemyNormal(gameMainProgram.resourcesMgr.LoadAsset
                (@"Enemys\Scarecrow", false)));

            // UI在后面初始化
            gameMainProgram.uiManager.ShowUIForms("PlayerHUD");
            gameMainProgram.uiManager.ShowUIForms("FadeOut");
            gameMainProgram.audioMgr.PlayMusic("Torch");
            CoroutineMgr.Instance.StartCoroutine(PlayerStart());
        }

        private IEnumerator PlayerStart()
        {
            gameMainProgram.playerMgr.CurrentPlayer.animator.Play("Break");
            yield return new WaitForSeconds(2);
            gameMainProgram.playerMgr.CurrentPlayer.animator.Play("BreakToIdle");
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
