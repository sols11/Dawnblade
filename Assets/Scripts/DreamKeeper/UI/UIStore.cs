using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFramework;

namespace DreamKeeper
{
    public class UIStore:ViewBase
    {
        public RectTransform PropSlots;
        public Text Gold;
        public Button CloseBtn;
        public Image ItemImg;
        public Text ItemDetail;
        public bool isBuy = true;   // 购买界面还是出售界面
        public AudioClip AudioBuy;
        public AudioClip AudioWarn;

        public IProp[] Props { get; private set; }
        private bool[] HasProp { get; set; }    // 存储商店售卖哪些道具
        private UIStoreGrid[] PropArray = new UIStoreGrid[32];    //道具数组
        private IPlayer player;
        private AudioSource audioSource;

        [Header("QuantityBox")]
        public RectTransform QuantityBox;
        public Text PriceText;  // 总价
        public Text ValueText;  // 购买数目
        public Text TextValueLeft;  // 下限
        public Text TextValueRight; // 上限
        public Slider ValueSlider;
        public Button BtnAcceptBox;
        public Button BtnCloseBox;

        private int minValue = 1;
        private int maxValue = 33;
        private int realValue = 1;
        private int propId = 0; // 购买或出售的道具Id
        private int propPrice = 0;
        private int finalPrice = 0;

        private void Awake()
        {
            base.UIForm_Type = UIFormType.PopUp;
            base.UIForm_ShowMode = UIFormShowMode.ReverseChange;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;
            player = GameMainProgram.Instance.playerMgr.CurrentPlayer;
            HasProp = player.HasProp;
            Props = GameMainProgram.Instance.dataBaseMgr.Props;
            audioSource = GetComponent<AudioSource>();
            GameMainProgram.Instance.audioMgr.AddSound(audioSource);

            int i = 0;
            foreach (RectTransform r in PropSlots)
            {
                PropArray[i] = r.GetChild(0).GetComponent<UIStoreGrid>();
                PropArray[i].UiStore = this;
                PropArray[i].ID = i;
                PropArray[i].Initialize();
                if(isBuy)
                    PropArray[i].InitializeBuy(HasProp[i]);
                i++;
            }

            CloseBtn.onClick.AddListener(OnButtonClose);
            // QuantityBox
            ValueSlider.onValueChanged.AddListener(OnValueChanged);
            // 购买出售监听不同方法
            if(isBuy)
                BtnAcceptBox.onClick.AddListener(OnBuyBoxAccept);
            else
                BtnAcceptBox.onClick.AddListener(OnSaleBoxAccept);
            BtnCloseBox.onClick.AddListener(OnQuantityBoxClose);
        }

        private void OnEnable()
        {
            Gold.text = player.Gold.ToString();
            // Sale界面在每次打开时更新
            if (!isBuy)
                for (int i = 0; i < PropArray.Length; i++)
                    PropArray[i].InitializeSale(player.PropNum[i]);
        }

        private void OnDestroy()
        {
            GameMainProgram.Instance.audioMgr.RemoveSound(audioSource);
        }

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

        public void OpenBox(int id)
        {
            if (id < 0 || id > Props.Length)
                return;
            propId = id;
            if (isBuy)
            {
                propPrice = Props[id].Price;
                // 不可购买
                if (player.Gold < propPrice || Props[id].MaxNum <= player.PropNum[id])
                {
                    audioSource.clip = AudioWarn;
                    audioSource.Play();
                    //UIMessageBox.AddMessage("你的金币不够~");
                    return;
                }
                // maxValue = 能购买且不超上限的最大值
                maxValue = player.Gold / propPrice;
                int curNum = Mathf.Max(player.PropNum[id], 0); // Player所持道具数>=0
                maxValue = Mathf.Min(maxValue, Props[id].MaxNum - curNum);
                // minValue就不改了
            }
            else
            {
                propPrice = Props[id].SalePrice;
                maxValue = player.PropNum[id];
            }
            // UpdateUI
            TextValueRight.text = maxValue.ToString();
            // 因为无法通过修改Value移动Slider的UI位置，所以只能每次更新当前value对应的值咯
            OnValueChanged(ValueSlider.value);
            QuantityBox.gameObject.SetActive(true);
        }

        private void OnValueChanged(float value)
        {
            // 公式，因为没有0值所以我们+minValue
            realValue=(int)((maxValue - minValue) * value+ minValue);
            ValueText.text = realValue.ToString();
            finalPrice = propPrice * realValue;
            PriceText.text = finalPrice.ToString();
        }

        private void OnBuyBoxAccept()
        {
            player.AddProp(propId, realValue);
            player.Gold -= finalPrice;
            QuantityBox.gameObject.SetActive(false);
            audioSource.clip = AudioBuy;
            audioSource.Play();
            OnEnable(); // 更新
        }

        private void OnSaleBoxAccept()
        {
            player.RemoveProp(propId, realValue);
            player.Gold += finalPrice;
            QuantityBox.gameObject.SetActive(false);
            audioSource.clip = AudioBuy;
            audioSource.Play();
            OnEnable();
        }

        private void OnQuantityBoxClose()
        {
            QuantityBox.gameObject.SetActive(false);
        }

        private void OnButtonClose()
        {
            QuantityBox.gameObject.SetActive(false);
            if(isBuy)
                GameMainProgram.Instance.uiManager.CloseUIForms("Store");
            else
                GameMainProgram.Instance.uiManager.CloseUIForms("StoreSale");
            GameMainProgram.Instance.eventMgr.InvokeEvent(EventName.DialogActionComplete);
            // 存档
        }
    }
}