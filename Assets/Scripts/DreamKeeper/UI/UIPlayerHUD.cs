using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UIPlayerHUD : ViewBase
    {
        public Image uiHP;
        public Image uiSP;
        [SerializeField]
        private float minWidthHP = 0;
        [SerializeField]
        private float maxWidthHP = 100;
        [SerializeField]
        private float minWidthSP = 0;
        [SerializeField]
        private float maxWidthSP = 100;

        private IPlayer player;

        void Awake()
        {
            //定义本窗体的性质(默认数值，可以不写)
            base.UIForm_Type = UIFormType.Normal;
            base.UIForm_ShowMode = UIFormShowMode.Normal;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;
            // 观察者注册
            player = GameMainProgram.Instance.playerMgr.CurrentPlayer;
            GameMainProgram.Instance.eventMgr.StartListening(EventName.PlayerHP_SP, this.UpdateUI);
            UpdateUI(); // 进行初始化
        }

        void OnDestroy()
        {
            GameMainProgram.Instance.eventMgr.StopListening(EventName.PlayerHP_SP, this.UpdateUI);
        }

        public override void UpdateUI()
        {
            // 计算出fillAmount
            if (uiHP != null)
            {
                uiHP.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                    Mathf.Round(minWidthHP + ((maxWidthHP - minWidthHP) * player.HPpercent)));
                if (player.HPpercent > 0.66f)
                    uiHP.color = new Color(0, 1, 0, 0.78f);
                else if (player.HPpercent > 0.33f)
                    uiHP.color = new Color(1, 1, 0, 0.78f);
                else
                    uiHP.color = new Color(1, 0, 0, 0.78f);
                //uiHP.color = new Color(-2 * player.HPpercent + 2, 1, 0, 0.78f);  // 1~0.5->0~1
                //uiHP.color = new Color(1, 2 * player.HPpercent, 0, 0.78f);   // 0.5~0->1~0
            }
            if (uiSP != null)
            {
                uiSP.rectTransform.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Horizontal,
                    Mathf.Round(minWidthSP + ((maxWidthSP - minWidthSP) * player.SPpercent)));
            }
        }
    
    }
}
