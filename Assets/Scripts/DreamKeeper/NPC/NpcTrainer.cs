using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SFramework;
using UnityEngine;

namespace DreamKeeper
{
    public class NpcTrainer : INPC
    {
        public override void Initialize()
        {
            base.Initialize();
            dialogKey = "Trainer";
            maxDistance = 2;
        }

        /// <summary>
        /// 对话结束时的事件，需要取消方法监听
        /// </summary>
        protected override void OnDialogComplete()
        {
            base.OnDialogComplete(); 
            GameLoop.Instance.sceneStateController.SetState(SceneState.TrainScene, true);
        }


    }
}