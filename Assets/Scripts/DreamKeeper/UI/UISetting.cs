
using SFramework;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UISetting:ViewBase
    {
        private SettingData SettingSaveData { get; set; }    // 存储
        public Slider BgmSlider;
        public Slider SfxSlider;
        public Toggle ToggleCN;
        public Toggle ToggleEN;
        public Button BtnAcceptBox;
        public Button BtnCloseBox;

        private AudioMgr audioMgr;

        private void Awake()
        {
            base.UIForm_Type = UIFormType.PopUp;
            base.UIForm_ShowMode = UIFormShowMode.ReverseChange;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;

            SettingSaveData = GameMainProgram.Instance.gameDataMgr.SettingSaveData;
            audioMgr = GameMainProgram.Instance.audioMgr;
        }

        private void Start()
        {
            //ToggleCN.onValueChanged.AddListener(OnToggleValueChanged);
            BtnAcceptBox.onClick.AddListener(OnBtnAccept);
            BtnCloseBox.onClick.AddListener(OnBtnClose);
        }

        // 每次打开时都更新数据
        private void OnEnable()
        {
            BgmSlider.value = SettingSaveData.MusicVolume;
            SfxSlider.value = SettingSaveData.SoundVolume;
            ToggleCN.isOn = SettingSaveData.IsChinese;
            ToggleEN.isOn = !SettingSaveData.IsChinese;
        }

        private void OnBtnAccept()
        {
            audioMgr.ChangeMusicVolume((int)BgmSlider.value);
            audioMgr.ChangeSoundVolume((int)SfxSlider.value);
            SettingSaveData.IsChinese = ToggleCN.isOn;  // 更新语言选择
            GameMainProgram.Instance.gameDataMgr.SaveSetting(); //SaveSetting
            GameMainProgram.Instance.uiManager.CloseUIForms("Setting");
        }

        private void OnBtnClose()
        {
            GameMainProgram.Instance.uiManager.CloseUIForms("Setting");
        }

    }
}
