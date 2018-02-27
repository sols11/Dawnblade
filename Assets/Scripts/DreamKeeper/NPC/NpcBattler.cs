using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SFramework;
using UnityEngine;

namespace DreamKeeper
{
    public class NpcBattler : INPC
    {
        public override void Initialize()
        {
            base.Initialize();
            dialogKey = "Battler";
            maxDistance = 2;
        }

        /// <summary>
        /// 对话结束时的事件，需要取消方法监听
        /// </summary>
        protected override void OnDialogComplete()
        {
            // base.OnDialogComplete(); 因为不是对话完就结束，而要打开UI，所以不调用基类
            transform.DOLocalRotate(initDir, 1);
            GameMainProgram.Instance.uiManager.ShowUIForms("StageMenu");
        }


    }
}