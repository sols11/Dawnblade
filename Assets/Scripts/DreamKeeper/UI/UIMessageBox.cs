using System.Collections;
using System.Collections.Generic;
using SFramework;
using UnityEngine;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UIMessageBox : ViewBase
    {
        public Text Word;
        private static Queue<string> MessageQueue = new Queue<string>();

        private void Awake()
        {
            base.UIForm_Type = UIFormType.PopUp;
            base.UIForm_ShowMode = UIFormShowMode.ReverseChange;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;

        }

        /// <summary>
        /// 调用这个来开启消息
        /// </summary>
        /// <param name="str"></param>
        public static void AddMessage(string str)
        {
            MessageQueue.Enqueue(str);
            GameMainProgram.Instance.uiManager.ShowUIForms("MessageBox");
        }

        private void OnEnable()
        {
            CoroutineMgr.Instance.StartCoroutine(ShowMessage());
        }

        private IEnumerator ShowMessage()
        {
            // 队列显示
            while(MessageQueue.Count > 0)
            {
                Word.text = MessageQueue.Dequeue();
                yield return new WaitForSeconds(1);
            }
            GameMainProgram.Instance.uiManager.CloseUIForms("MessageBox");
        }


    }
}