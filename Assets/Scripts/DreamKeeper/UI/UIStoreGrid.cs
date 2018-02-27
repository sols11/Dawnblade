using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    [RequireComponent(typeof(Image))]
    public class UIStoreGrid : MonoBehaviour, IPointerEnterHandler
    {
        public UIStore UiStore { get; set; }

        public int ID { get; set; }

        private bool HasProp { get; set; }
        private Text Price { get; set; }

        private Image img; 
        private Button btn;

        /// <summary>
        /// 由UIStore进行初始化
        /// </summary>
        public void Initialize()
        {
            Price = transform.parent.GetChild(1).GetComponent<Text>();
            btn = transform.parent.GetComponent<Button>();
            btn.onClick.AddListener(OnButtonClick);
            img = GetComponent<Image>();
        }

        /// <summary>
        /// 只在初始化时执行一次
        /// </summary>
        /// <param name="hasProp"></param>
        public void InitializeBuy(bool hasProp)
        {
            // 更新显示
            HasProp = hasProp;
            if (!hasProp)
                return;
            IProp prop = UiStore.Props[ID];
            if (prop == null)
            {
                Debug.LogError("数据库中没有这个道具");
                return;
            }
            img.sprite = prop.GetIcon();
            img.color = Color.white;
            Price.gameObject.SetActive(true);
            Price.text = prop.Price.ToString();
        }

        /// <summary>
        /// 更新显示的UI，Sale
        /// </summary>
        public void InitializeSale(int num)
        {
            // 只有可出售且数目>0的才会显示出来
            if (num <= 0 || !UiStore.Props[ID].CanSale)
            {
                img.sprite = null;
                img.color = Color.clear;
                HasProp = false;
                Price.gameObject.SetActive(false);
            }
            else
            {
                IProp prop = UiStore.Props[ID];
                if (prop == null)
                {
                    Debug.LogError("数据库中没有这个道具");
                    return;
                }
                img.sprite = prop.GetIcon();
                HasProp = true;
                img.color = Color.white;
                Price.gameObject.SetActive(true);
                Price.text = prop.SalePrice.ToString();
            }
        }

        /// <summary>
        /// 每次打开时由UiStore进行检查
        /// </summary>
        /// <param name="gold"></param>
        //public void PriceOnEnable(int gold)
        //{
        //    if (!HasProp)
        //        return;
        //    if (gold < UiStore.Props[ID].Price)
        //        Price.color = Color.red;
        //    else
        //        Price.color = Color.white;
        //}

        public void OnPointerEnter(PointerEventData eventData)
        {
            // 显示详细信息
            if (HasProp)
                UiStore.ShowItemData(ID);
            else
                UiStore.ShowItemData(-1);
        }

        /// <summary>
        /// 指针点击
        /// </summary>
        /// <param name="eventData"></param>
        private void OnButtonClick()
        {
            if (HasProp)
                UiStore.OpenBox(ID);
        }



    }
}