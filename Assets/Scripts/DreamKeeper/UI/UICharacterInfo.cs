using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UICharacterInfo:ViewBase
    {
        public Text Name;
        public Text Rank;
        public Text Spec;
        public Text Hp;
        public Text Sp;
        public Text Att;
        public Text Def;
        public RectTransform Fit;
        public Button CloseBtn;
        public Image Info;
        public Text ItemName;
        public Text ItemDetail;

        protected UIFit[] Fits = new UIFit[6];    // Slots，用数组最好，可以和Player存档统一
        protected IPlayer player;

        protected virtual void Awake()
        {
            base.UIForm_Type = UIFormType.PopUp;
            base.UIForm_ShowMode = UIFormShowMode.ReverseChange;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;
            player = GameMainProgram.Instance.playerMgr.CurrentPlayer;
            // 由本类给子物体初始化
            Initialize();

            CloseBtn.onClick.AddListener(OnButtonClose);
            // 更新装备，因为拖拽后Fit对应的ID会改变，如果每次打开时都ChangeFit的话那么后面装备上后前面的E会关掉，所以只在初始化时更新Fit
        }

        protected virtual void Initialize()
        {
            int i = 0;
            foreach (RectTransform r in Fit)
            {
                Fits[i]=r.GetComponent<UIFit>();
                Fits[i].UiCharacterInfo = this;
                Fits[i].FitNum = i;
                Fits[i].Initialize();
                i++;
            }
        }

        /// <summary>
        /// 更新装备和角色信息
        /// </summary>
        private void OnEnable()
        {
            UpdateCharacterInfo();
            for(int i=0;i<Fits.Length;i++)
                Fits[i].UpdateFit(player.Fit[i]);   // 设置装备
        }

        public void ShowItemData(int fitId, Vector3 pos)
        {
            if (fitId < 0 || fitId > Fits.Length)
                return;
            IEquip equip = Fits[fitId].Equip;
            string str=string.Empty;
            if (equip == null)
                return;
            ItemName.text = equip.Name;
            if (equip.Detail != string.Empty)
                str += equip.Detail;
            if (equip.Attack > 0)
                str += "\n" + "<color=#62BEFFFF>ATT</color> +" + equip.Attack;
            if (equip.Defend > 0)
                str += "\n" + "<color=#62BEFFFF>DEF</color> +" + equip.Defend;
            if (equip.HP > 0)
                str += "\n" + "<color=#62BEFFFF>HP</color> +" + equip.HP;
            if (equip.SP > 0)
                str += "\n" + "<color=#62BEFFFF>SP</color> +" + equip.SP;
            ItemDetail.text = str;
            Info.transform.position = pos;
            Info.gameObject.SetActive(true);
        }

        public void CloseItemData()
        {
            Info.gameObject.SetActive(false);
        }

        /// <summary>
        /// 更新WindowCharacter的信息，均来源于player
        /// </summary>
        private void UpdateCharacterInfo()
        {
            if (player == null)
                return;
            Rank.text = player.Rank.ToString();
            Spec.text = player.Special.ToString("F");
            Hp.text = player.MaxHP.ToString();
            Sp.text = player.MaxSP.ToString();
            Att.text = player.AttackPoint.ToString();
            Def.text = player.DefendPoint.ToString();
        }

        protected virtual void OnButtonClose()
        {
            GameMainProgram.Instance.uiManager.CloseUIForms("CharacterInfo");
        }

    }
}