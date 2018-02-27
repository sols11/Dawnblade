using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SFramework;

namespace DreamKeeper
{
    /// <summary>
    /// 控制整个Slot，是背包的组成部分，存放Prop
    /// 只存放ID，这样以后改变物品对应的ID也能适用
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class UIDragGrid : MonoBehaviour, IPointerEnterHandler
    {
        public UIInventory UiInventory { get; set; }
        public int ID { get; set; }
        public Transform E { get; private set; }
        public bool HasItem { get; set; }   // 目前是否有存放物品（武器或道具）
        public bool Selected { get; set; }    // 该格子的物品是否装备了

        private Image img; // Image显示
        private Button btn;
        private Text count;
        private int Num { get; set; }   // 记录数量，提高效率

        public void Initialize()
        {
            count = transform.parent.GetChild(1).GetComponent<Text>();
            E = transform.parent.GetChild(2);
            Num = -10;
            btn = transform.parent.GetComponent<Button>();
            btn.onClick.AddListener(OnButtonClick);
            img = GetComponent<Image>();
        }
        
        private void OnButtonClick()
        {
            if (HasItem && ID <= 7)
            {
                UiInventory.ChangeCarry(ID);
                Selected = true;
                E.gameObject.SetActive(true);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // 显示详细信息
            if (HasItem)
                UiInventory.ShowItemData(ID);
            else
                UiInventory.ShowItemData(-1);
        }

        /// <summary>
        /// 更新显示的UI，只在数量变化时改变
        /// </summary>
        public void UpdateImage(int num)
        {
            E.gameObject.SetActive(Selected);
            if (num == Num)
                return;
            Num = num;
            // 数目>=0和数目<0的不同显示
            if (num < 0)
            {
                img.sprite = null;
                img.color = Color.clear;
                HasItem = false;
                count.gameObject.SetActive(false);
            }
            else
            {
                IProp prop = UiInventory.Props[ID];
                if (prop == null)
                {
                    Debug.LogError("数据库中没有这个道具");
                    return;
                }
                img.sprite = prop.GetIcon();
                HasItem = true;
                img.color = Color.white;
                count.gameObject.SetActive(true);
                count.text = num.ToString();
            }
        }
    }
}