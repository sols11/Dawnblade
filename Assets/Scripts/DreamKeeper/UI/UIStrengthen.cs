using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UIStrengthen:UICharacterInfo
    {
        [Header("UpgradeBox")]
        public RectTransform UpgradeBox;
        public UIFit WeaponLeft;
        public UIFit WeaponRight;

        public Text PriceText;
        public Button BtnAcceptBox;
        public Button BtnCloseBox;

        protected override void Awake()
        {
            base.Awake();
            BtnAcceptBox.onClick.AddListener(OnBtnAccept);
            BtnCloseBox.onClick.AddListener(OnBtnClose);
        }

        protected override void Initialize()
        {
            int i = 0;
            foreach (RectTransform r in Fit)
            {
                Fits[i] = r.GetComponent<UIFit>();
                Fits[i].UiCharacterInfo = this;
                Fits[i].FitNum = i;
                Fits[i].Initialize();
                Fits[i].InitStrengthen();   // Init
                i++;
            }
        }

        public void OpenBox(int fitNum)
        {
            if (fitNum < 0 || fitNum > Fits.Length)
                return;

            UpgradeBox.gameObject.SetActive(true);

        }

        private void OnBtnAccept()
        {
            
        }

        private void OnBtnClose()
        {
            UpgradeBox.gameObject.SetActive(false);
        }

        protected override void OnButtonClose()
        {
            GameMainProgram.Instance.uiManager.CloseUIForms("Strengthen");
        }
    }
}