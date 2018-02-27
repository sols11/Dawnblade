using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UITestMenu : ViewBase
    {
        public Button e1;
        public Button e2;
        public Button e3;
        public Button e4;
        public Button e5;
        public Button e6;
        public Button e7;
        public Button e8;
        public Button e9;
        public Button e10;


        void Awake()
        {
            //定义本窗体的性质
            base.UIForm_Type = UIFormType.Normal;
            base.UIForm_ShowMode = UIFormShowMode.Normal;
            base.UIForm_LucencyType = UIFormLucenyType.Pentrate;
        }

        void Start()
        {
            e1.onClick.AddListener(E1);
            e2.onClick.AddListener(E2);
            //e3.onClick.AddListener(Train);
            //e4.onClick.AddListener(Pack);
            //e5.onClick.AddListener(Store);
            //e6.onClick.AddListener(Make);
            //e7.onClick.AddListener(Setting);
            //e8.onClick.AddListener(Help);
            //e9.onClick.AddListener(Exit);
            //e10.onClick.AddListener(Sale);
        }

        /// <summary>
        /// 数据库测试（登录界面）
        /// </summary>
        private void E1()
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("Login");

        }

        private void E2()
        {
            
        }


    }
}