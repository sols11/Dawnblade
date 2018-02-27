using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework
{
	public class StartState : ISceneState
	{
		private GameMainProgram gameMainProgram;//主程序

		public StartState(SceneStateController controller) : base(controller)
		{
			this.StateName = "Start";
		}

		public override void StateBegin()
		{
			gameMainProgram=GameMainProgram.Instance;
			gameMainProgram.Initialize();
            //UI初始化
            gameMainProgram.uiManager.ShowUIForms("FadeOut");
            gameMainProgram.uiManager.ShowUIForms("BeginBackground");
            CoroutineMgr.Instance.StartCoroutine(PlayBgm());
        }

        IEnumerator PlayBgm()
        {
            // 等待渐变结束，懒得写成事件了
            yield return new WaitForSeconds(1);
            gameMainProgram.audioMgr.PlayMusic("Opening");
        }

        public override void StateEnd()
		{
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
