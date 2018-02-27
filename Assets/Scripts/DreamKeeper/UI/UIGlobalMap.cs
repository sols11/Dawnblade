using System;
using System.Collections;
using System.Collections.Generic;
using SFramework;
using UnityEngine;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UIGlobalMap : ViewBase
    {
        public RectTransform LocationRect;
        public Button BtnMonster;
        public Button BtnClose;

        private void Awake()
        {
            //定义本窗体的性质(弹出窗体)
            base.UIForm_Type = UIFormType.Normal;
            base.UIForm_ShowMode = UIFormShowMode.HideOther;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;
        }

        private void Start()
        {
            BtnMonster.onClick.AddListener(OnBtnMonster);
            BtnClose.onClick.AddListener(OnBtnClose);
        }

        private void OnBtnMonster()
        {
            UIDialog.IsTalking = false;
            GameLoop.Instance.sceneStateController.SetState(SceneState.BattleMonster, true,true);
        }

        private void OnBtnClose()
        {
            GameMainProgram.Instance.uiManager.CloseUIForms("GlobalMap");
            // 结束对话，可以移动
            UIDialog.IsTalking = false;
            GameMainProgram.Instance.playerMgr.CurrentPlayer.CanMove = true;
        }
    }
}
