
using System.Diagnostics;
using SFramework;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UILogin : ViewBase
    {
        public InputField IdInputField;
        public InputField PwdInputField;
        public Button BtnRegister;
        public Button BtnAcceptBox;
        public Button BtnCloseBox;

        private SqlMgr sqlMgr;

        private void Awake()
        {
            base.UIForm_Type = UIFormType.PopUp;
            base.UIForm_ShowMode = UIFormShowMode.ReverseChange;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;

        }

        private void Start()
        {
            sqlMgr = GameMainProgram.Instance.sqlMgr;
            //SqlMgr.OpenSql();
            BtnRegister.onClick.AddListener(OnBtnRegister);
            //BtnAcceptBox.onClick.AddListener(OnBtnAccept);
            //BtnCloseBox.onClick.AddListener(OnBtnClose);
        }


        /// <summary>
        /// 注册新用户到数据库
        /// 数据库中已经定义好属性，password为binary查询以区分大小写
        /// </summary>
        private void OnBtnRegister()
        {
            // 执行：CREATE TABLE  PlayerDataBase(id int,username text,password text)
            // sqlMgr.CreateTable("PlayerDataBase", new string[] {"id", "username", "password"},new string[]{"int","text","text"});
            // 执行：INSERT INTO PlayerDataBase (id, username, password) VALUES ('2', 'player', '123321')

            if (string.IsNullOrEmpty(IdInputField.text) || string.IsNullOrEmpty(PwdInputField.text))
            {
                UIMessageBox.AddMessage("请输入用户名和密码");
                return;
            }

            // 查询并保存结果
            /*DataSet ds = sqlMgr.Select("PlayerDataBase", new string[] { "username" }, new string[] { "username" }, new string[] { "=" }, new string[] { IdInputField.text });
            if (ds != null)
            {
                sqlMgr.ReadDs(ds);
                UIMessageBox.AddMessage("该用户名已注册，尝试登录？");
                return;
            }
            sqlMgr.InsertInto("PlayerDataBase", new string[] {"username", "password"},new string[] { IdInputField.text, PwdInputField.text });
            OnBtnClose();*/
        }

        /*private void OnBtnAccept()
        {
            // 检查满足条件
            if (string.IsNullOrEmpty(IdInputField.text) || PwdInputField.text.IsNullOrEmpty())
            {
                UIMessageBox.AddMessage("请输入用户名和密码");
                return;
            }

            DataSet ds = sqlMgr.Select("PlayerDataBase", new string[] { "username", "password" }, new string[] { "username", "password" }, new string[] { "=", "=" }, new string[] { IdInputField.text, PwdInputField.text });
            sqlMgr.ReadDs(ds);
            if (ds == null)
            {
                UIMessageBox.AddMessage("用户名或密码输错了？");
                return;
            }

            sqlMgr.Close();
            GameMainProgram.Instance.uiManager.CloseUIForms("Login");
        }

        private void OnBtnClose()
        {
            sqlMgr.Close();
            GameMainProgram.Instance.uiManager.CloseUIForms("Login");
        }*/

    }
}
