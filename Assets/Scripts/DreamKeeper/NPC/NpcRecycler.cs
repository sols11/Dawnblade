using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SFramework;
using UnityEngine;

namespace DreamKeeper
{
    public class NpcRecycler : INPC
    {
        public override void Initialize()
        {
            base.Initialize();
            dialogKey = "Recycler";
            maxDistance = 2;
        }

        protected override void OnDialogComplete()
        {
            //base.OnDialogComplete();
            transform.DOLocalRotate(initDir, 1);
            GameMainProgram.Instance.uiManager.ShowUIForms("StoreSale");
        }
    }
}