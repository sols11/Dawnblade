using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    /// <summary>
    /// 存放Equip
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class UIFit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public UICharacterInfo UiCharacterInfo { get; set; }
        public IEquip Equip { get; set; }
        public int FitNum { get; set; }
        private Image img;
        private Image gridImg;  // 子物体grid的img
        private Color normalColor=new Color(1,1,1,0.47f);     // 该物品格子的正常颜色
        private Color highLightColor = Color.white;   // 该物品格子的高亮颜色
        private bool hasItem = false;
        // Strengthen用
        public UIStrengthen UiStrengthen { get; set; }
        private Button btn;

        public void Initialize()
        {
            img = GetComponent<Image>();
            gridImg = transform.GetChild(0).GetComponent<Image>();
        }

        public void InitStrengthen()
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(OnBtnClick);
        }

        private void OnBtnClick()
        {
            // 检查是否可强化后再强化
            UiStrengthen.OpenBox(FitNum);
        }

        /// <summary>
        /// 切换装备，如果没有装备则不显示，效率快
        /// </summary>
        /// <param name="equip"></param>
        public void UpdateFit(IEquip equip)
        {
            if (equip == null)  // 为空
            {
                gameObject.SetActive(false);
                hasItem = false;
                return;
            }
            if (equip == Equip)    // 未修改
                return;
            Equip = equip;
            gridImg.sprite = Equip.GetIcon();
            hasItem = true;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 指针进入
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            // 显示详细信息
            if (hasItem)
            {
                img.color = highLightColor;
                UiCharacterInfo.ShowItemData(FitNum, transform.position);
            }
        }

        /// <summary>
        /// 指针离开
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            img.color = normalColor;
            UiCharacterInfo.CloseItemData();
        }

    }
}