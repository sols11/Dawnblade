using System.Collections;
using SFramework;
using UnityEngine;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UIPromptBox : ViewBase
    {
        public Text word;
        public Button btn;
        public static int wordID = 0;   // 修改显示内容
        public static bool isPause = false;   // 是否暂停
        private string[] wordText =
        {
            "卸下装备后才可出售",
            "背包物品已满",
            "你的金币不够",
            "该物品不可出售",
            "尚未完成，敬请期待"
        };

        private void Awake()
        {
            base.UIForm_Type = UIFormType.PopUp;
            base.UIForm_ShowMode = UIFormShowMode.ReverseChange;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;

            btn.onClick.AddListener(Close);
        }

        private void OnEnable()
        {
            word.text = wordText[wordID];
            if(isPause)
                Time.timeScale = 0;

        }

        private void Close()
        {
            if (isPause)
            {
                isPause = false;
                Time.timeScale = 1;
            }
            GameMainProgram.Instance.uiManager.CloseUIForms("PromptBox");
        }

    }
}