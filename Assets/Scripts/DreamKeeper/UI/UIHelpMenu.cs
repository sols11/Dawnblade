using System.Text;
using UnityEngine;
using SFramework;
using UnityEngine.UI;

namespace DreamKeeper
{
    public class UIHelpMenu : ViewBase
    {
        [SerializeField]
        private RectTransform HelpTab;
        [SerializeField]
        private Button CloseBtn;
        [SerializeField]
        private Text helpText;
        [SerializeField]
        private Text subText;
        [SerializeField]
        private Image logo;
        // 各个分页按钮
        private Button[] tabBtn = new Button[6];
        // Tab下的内容
        private string[] tabHelpText = new string[6];
        private string[] tabSubText = new string[6];

        void Awake()
        {
            //定义本窗体的性质(默认数值，可以不写)
            base.UIForm_Type = UIFormType.PopUp;
            base.UIForm_ShowMode = UIFormShowMode.ReverseChange;
            base.UIForm_LucencyType = UIFormLucenyType.Lucency;
            int i = 0;
            foreach (RectTransform r in HelpTab)
            {
                tabBtn[i] = r.GetComponent<Button>();
                //if (tabBtn[i])
                //{
                //    //Text sender = TabText[i];   // 需要单独定义参数，这样每次传入的内存地址才不同
                //    tabBtn[i].onClick.AddListener(delegate() { SelectTab(ref i); });    // 用匿名方法实现方法调用
                //}
                i++;
            }
            // 用匿名方法实现方法调用,值类型似乎必须要写常量才能实现
            tabBtn[0].onClick.AddListener(delegate() { SelectTab(0); });
            tabBtn[1].onClick.AddListener(delegate() { SelectTab(1); });
            tabBtn[2].onClick.AddListener(delegate() { SelectTab(2); });
            tabBtn[3].onClick.AddListener(delegate() { SelectTab(3); });
            tabBtn[4].onClick.AddListener(delegate() { SelectTab(4); });

            //tabBtn[0].Select(); // 默认选中
            CloseBtn.onClick.AddListener(Close);
            // 控制
            tabHelpText[0] = "控制\n水平方向移动\n竖直方向移动\n攻击1-太刀-扫\n攻击2-太刀-刺\n攻击3-太刀-斩\n技能1-残像-瞬移\n技能2-太刀-跳斩\n使用物品\n翻滚闪避\n对话\n呼出菜单\n\n键位可在运行游戏前设置";
            tabSubText[0] = "\nA D\nW S\nJ\nK\nL\nU\nI\nO\nSpace\nJ\nEsc";
            // 技能（UI建议在play下调整，text在程序中编写）
            StringBuilder sb = new StringBuilder(); // 处理大量字符串修改，用AppendLine使输入后换行
            sb.AppendLine(@"太刀-扫 <color=#D69200FF>灵巧快速的刀技，伤害较低但稳定</color>");
            sb.AppendLine(@"太刀-刺 <color=#D69200FF>可施展刺击三连，伤害可观，对于某些敌人有特殊效果</color>");
            sb.AppendLine(@"太刀-斩 <color=#D69200FF>武士太刀流连技，攻击力高，对于某些敌人有特殊效果</color>");
            sb.AppendLine("残像/瞬移\xa0"+ "<color=#D69200FF>制造出一个残像，再次使用技能时可以瞬移到残像位置。需要消耗SP</color>");
            sb.AppendLine("太刀-跳斩\xa0"+ "<color=#D69200FF>快速释放跳跃斩击对手，伤害一般，但是可以打断其他技能造成的硬直。需要消耗SP</color>");
            sb.AppendLine("使用物品\xa0"+ "<color=#D69200FF>可以使用携带的物品，战斗时只能携带背包中的一种物品。</color>");
            sb.AppendLine("翻滚闪避\xa0"+ "<color=#D69200FF>快速闪避敌人的攻击，可以打断一些灵巧技能的硬直。需要消耗SP</color>");
            sb.AppendLine("\ndemo中技能初始即可使用，正式版中将会逐步学会技能，并且会有更多丰富的技能变化，敬请期待~");
            tabHelpText[1] = sb.ToString();
            tabHelpText[2] = "背包\n可以在村子中按Esc呼出菜单，然后查看自己的背包\n背包中的道具分3类：\n携带道具 （可以携带到战斗中）加工素材\n任务道具\n可以到商店处购买道具，也可以到回收处出售道具。道具对于战斗的帮助是显而易见的，所以不要忘了选择合适的携带道具~\n\ndemo中的道具种类尚且有限，正式游戏中将会随流程逐步增加，商店也会在不同阶段出售不同的道具";
            tabHelpText[3] = "版本说明\n本版本为游戏开发中版本，游戏中尚有部分未授权素材未完成替换，但大部分是原创素材和授权素材。本版本表现效果不代表最终版本\n原创部分包括场景，角色，音乐，脚本，架构，原画等等\n授权部分包括动画，UI，场景，角色，插件等等\n游戏目前展示内容仅为完整版本的一部分，更多策划案中的设计还有待实现，敬请期待~\n最终解释权属于 Sunset Game 制作组";
            tabHelpText[4] = "关于\n感谢您体验我们制作的游戏，初次制作，经验尚缺，我们会努力改善，还望海涵\nSunset\xa0Game\xa0游戏制作组是一个由大学生组成的独立游戏开发团队，专注但不限于动作游戏、RPG等类型游戏。目标是“简单地做不简单的游戏”，一点一点实现游戏梦想\n如果对我们的游戏感兴趣，欢迎加入游戏群\xa0"+"398740933，反馈您的体验，并了解更多信息";
            helpText.text = tabHelpText[0];
            subText.text = tabSubText[0];
        }

        private void SelectTab(int i)
        {
            if (i >= 0 && i < tabHelpText.Length)
            {
                helpText.text = tabHelpText[i];
                subText.text = tabSubText[i];
            }
            if (i == 4)
                logo.gameObject.SetActive(true);
            else 
                logo.gameObject.SetActive(false);
            //if (currentTab == text)
            //    return;
            //currentTab.gameObject.SetActive(false);
            //text.gameObject.SetActive(true);
            //currentTab = text;
        }

        private void Close()
        {
            GameMainProgram.Instance.uiManager.CloseUIForms("HelpMenu");
        }
    }
}