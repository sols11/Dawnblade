using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using UnityEngine.UI;

namespace DreamKeeper
{
    /// <summary>
    /// 背包UI，管理其下的所有UI
    /// 获取数据，接受操作，保存数据
    /// </summary>
    public class UIInventory : ViewBase
    {
        #region Variables
        public RectTransform PropSlots;
        public Text Gold;
        public Image ItemImg;
        public Text ItemDetail;
        public Button CloseBtn;

        public IProp[] Props { get; private set; }
        public IPlayer Player { get; set; }
        private UIDragGrid[] PropArray = new UIDragGrid[32];
        #endregion

        /// <summary>
        /// Awake：初始化所有子物体。第一次打开时从Player获取背包信息，然后更新装备
        /// </summary>
        private void Awake()
        {
            base.UIForm_Type = UIFormType.PopUp;
            base.UIForm_ShowMode = UIFormShowMode.ReverseChange;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;
            Player = GameMainProgram.Instance.playerMgr.CurrentPlayer;
            Props = GameMainProgram.Instance.dataBaseMgr.Props;
            // 由本类给子物体初始化
            Initialize();

            CloseBtn.onClick.AddListener(ButtonClose);
        }

        /// <summary>
        /// 初始化子物体并获取
        /// </summary>
        private void Initialize()
        {
            Gold.text = Player.Gold.ToString();
            //遍历获取初始化
            int i = 0;
            foreach (RectTransform r in PropSlots)
            {
                PropArray[i] = r.GetChild(0).GetComponent<UIDragGrid>();
                PropArray[i].UiInventory = this;
                PropArray[i].ID = i;
                PropArray[i].Initialize();
                i++;
            }
        }

        /// <summary>
        /// 每次启动时自动根据Player的信息更新背包，外部只需要修改Player的Data，不需要控制背包
        /// </summary>
        private void OnEnable()
        {
            // 进行数据初始化
            PropArray[Player.MedicineID].Selected = true;
            for (int i = 0; i < Player.PropNum.Length; i++)
                PropArray[i].UpdateImage(Player.PropNum[i]);
        }

        public void ChangeCarry(int Id)
        {
            PropArray[Player.MedicineID].Selected = false;
            PropArray[Player.MedicineID].E.gameObject.SetActive(false);
            Player.MedicineID = Id;
        }

        /// <summary>
        /// 更新ItemData，提供给Grid调用
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="pos"></param>
        public void ShowItemData(int id)
        {
            if (id < 0 || id > PropArray.Length)
            {
                ItemImg.color = Color.clear;
                ItemDetail.text = string.Empty;
            }
            else
            {
                ItemImg.sprite = Props[id].GetIcon();
                ItemImg.color = Color.white;
                ItemDetail.text = Props[id].Name + "\n背包上限：" + Props[id].MaxNum + "\n" + Props[id].Detail;
            }
        }

        private void ButtonClose()
        {
            GameMainProgram.Instance.uiManager.CloseUIForms("Inventory");
        }
    }
}