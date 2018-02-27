using System;
using System.Collections;
using System.Collections.Generic;
using SFramework;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DreamKeeper
{
    /// <summary>
    /// 每次出现新任务，就向TasksData Insert新的TaskData
    /// UI每次打开时检查添加的新任务
    /// </summary>
    public class UITasksMenu : ViewBase
    {
        // 一条Task的UI
        class UiTask
        {
            public int Id { get; set; }
            public GameObject TaskGo { get; set; }
            public Image Complete { get; set; }
            public Text Place { get; set; }
            public Text Name { get; set; }
            public Text Award { get; set; }
            public Button Battle { get; set; }

            public UiTask(GameObject go)
            {
                TaskGo = go;
                Complete = TaskGo.transform.GetChild(0).GetComponent<Image>();
                Place = TaskGo.transform.GetChild(1).GetComponent<Text>();
                Name = TaskGo.transform.GetChild(2).GetComponent<Text>();
                Award = TaskGo.transform.GetChild(3).GetComponent<Text>();
                Battle = TaskGo.transform.GetChild(4).GetComponent<Button>();
            }
        }

        public RectTransform TaskRectTransform;
        public GameObject TaskPrefab;
        public Button CloseBtn;

        private List<UiTask> TaskList;
        private IPlayer Player { get; set; }
        private List<Task> Tasks { get; set; }
        private Color UnCompleteColor = new Color(0.51f,0.51f,0.51f,0);
        private Color CompleteColor = new Color(0.78f, 1, 0, 1);
        private Color AcColor = new Color( 1, 0.44f, 0.44f, 1);

        private void Awake()
        {
            //定义本窗体的性质(弹出窗体)
            base.UIForm_Type = UIFormType.PopUp;
            base.UIForm_ShowMode = UIFormShowMode.ReverseChange;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;
            TaskList = new List<UiTask>();
            Player = GameMainProgram.Instance.playerMgr.CurrentPlayer;
            Tasks = GameMainProgram.Instance.dataBaseMgr.Tasks;
            TaskPrefab.SetActive(false);
            CloseBtn.onClick.AddListener(OnBtnClose);
            Player.AddTask(0);
            Player.AddTask(1);
            Player.AddTask(2);
        }

        /// <summary>
        /// 每次打开时检查任务是不是又多了
        /// </summary>
        private void OnEnable()
        {
            foreach (TaskData t in Player.TasksData)
            {
                if(t.Id>=Tasks.Count)
                    continue;
                UpdateTask(t);
            }
        }

        private void UpdateTask(TaskData td)
        {
            if (td.Id < TaskList.Count) // 判断存在,存在就不用再克隆了
                return;
            // 克隆UI
            GameObject go = Instantiate(TaskPrefab);
            go.transform.parent = TaskRectTransform;
            go.SetActive(true);
            UiTask ut =new UiTask(go);
            TaskList.Add(ut);
            ut.Id = td.Id;
            ut.Place.text = Tasks[td.Id].TaskPlace;
            ut.Name.text = Tasks[td.Id].TaskName;
            ut.Award.text = Tasks[td.Id].TaskAward.ToString();
            AddBattleListener(td.Id, ut.Battle);
            // 完成情况
            if(td.HasCompletedRank3)
                ut.Complete.color = AcColor;
            else if (td.HasCompleted)
                ut.Complete.color = CompleteColor;
            else
                ut.Complete.color = UnCompleteColor;
        }

        /// <summary>
        /// 要添加任务就加一个case
        /// </summary>
        /// <param name="id"></param>
        /// <param name="btn"></param>
        private void AddBattleListener(int id,Button btn)
        {
            switch (id)
            {
                case 0:
                    btn.onClick.AddListener(() => GameLoop.Instance.sceneStateController.SetState(SceneState.BattleMonster, true, true));
                    break;
                case 1:
                    btn.onClick.AddListener(() => GameLoop.Instance.sceneStateController.SetState(SceneState.BattleEyeball, true, true));
                    break;
                case 2:
                    btn.onClick.AddListener(() => GameLoop.Instance.sceneStateController.SetState(SceneState.BattleFantasy, true, true));
                    break;
                default:
                    break;
            }
        }

        private void OnBtnClose()
        {
            GameMainProgram.Instance.uiManager.CloseUIForms("TasksMenu");
        }

    }
}
