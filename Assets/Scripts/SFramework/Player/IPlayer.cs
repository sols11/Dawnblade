using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SFramework
{
    /// <summary>
    /// + Player的所有属性
    /// + PlayerMeditor
    /// + 数据修改
    /// + 数据存取
    /// Player的数据层
    /// Player的属性由装备和基础值决定
    /// </summary>
	public class IPlayer : ICharacter
	{
        protected int m_MedicineNum;

        /// <summary>
        /// m_CanMove只是单纯的控制是否能进行空间上的移动，而不是是否可以输入移动
        /// </summary>
		protected bool m_CanMove = false;  
		protected float h;
		protected float v;
        protected GameData gameData;

		public float Speed { get; set; }    // AvoidSpeed用MoveSpeed*2代替

        //Player的HP,SP需要更新UI
        public override int CurrentHP
        {
            get { return base.CurrentHP; }
            set { base.CurrentHP = value;
                UpdateHP_SP();
            }
        }
        public override int CurrentSP
        {
            get { return base.CurrentSP; }
            set
            {
                base.CurrentSP = value;
                UpdateHP_SP();
            }
        }
        public int Gold { get; set; }
        public int MedicineID { get; set; } // 携带道具的ID，保证任何时候都有对应的道具
	    // 技能开关
	    public bool CanAvoid { get; set; }
	    public bool CanAttack2 { get; set; }
	    public bool CanAttack3 { get; set; }
	    public bool CanDush { get; set; }
        // 装备和道具
        public IEquip[] Fit { get; set; }
        public int[] PropNum { get; set; }
	    // 商店数据
	    public bool[] HasProp { get; set; }
	    // 任务数据
	    public List<TaskData> TasksData { get; set; }
        // 
        /// <summary>
        /// 在设置时将相关值赋为0，主要是设置false时需要
        /// </summary>
        public virtual bool CanMove
		{
			get { return m_CanMove; }
			set { h = 0; v = 0; m_CanMove = value; }
		}
		public bool CanRotate { get; set; }
        public PlayerMediator PlayerMedi { get; set; }
        
        protected IProp[] Props { get; set; }

        /// <summary>
        /// Initialize只在第一次创建时执行初始化代码，之后切换Scene时都不用再次初始化，所以Data也没有改变
        /// </summary>
        /// <param name="gameObject"></param>
        public IPlayer(GameObject gameObject):base(gameObject)
        {
            Props = GameMainProgram.Instance.dataBaseMgr.Props;
            if (GameObjectInScene != null)
            {
                animator = GameObjectInScene.GetComponent<Animator>();
                Rgbd = GameObjectInScene.GetComponent<Rigidbody>();
                // 关联中介者
                PlayerMedi = new PlayerMediator(this);
                PlayerMedi.Initialize();
            }
        } 

        public override void Release()
        {
            PlayerMedi.PlayerMono.Release();
        }

        public virtual void Hurt(PlayerHurtAttr _playerHurtAttr) { }
        public virtual void Roared() { }
        public virtual void BeDownGround() { }

        public override void Dead()
        {
            base.Dead();
            GameMainProgram.Instance.eventMgr.InvokeEvent(EventName.PlayerDead);    // 触发死亡事件
        }

        public bool AddProp(int id,int num=1)
        {
            if (id >= 32)
            {
                Debug.LogError("Id超过32！");
                return false;
            }
            if (Props[id] == null)
            {
                Debug.LogError("数据库中没有这个道具");
                return false;
            }
            if (PropNum[id] < 0)
                PropNum[id] = 0;
            if (PropNum[id]+num > Props[id].MaxNum)
            {
                Debug.Log("背包物品达到上限，不可添加");
                return false;
            }
            PropNum[id] += num;
            return true;
        }

        public bool RemoveProp(int id,int num=1)
        {
            if (id >= 32)
            {
                Debug.LogError("Id超过32！");
                return false;
            }
            if (PropNum[id] < num)
                return false;
            PropNum[id] -= num;
            return true;
        }

        /// <summary>
        /// 单纯的添加Id下标的任务，不进行检查
        /// </summary>
        /// <param name="id"></param>
	    public void AddTask(int id)
	    {
            TaskData td =new TaskData();
	        td.Id = id;
	        TasksData.Insert(id,td);
	    }

        private void UpdateHP_SP()
        {
            GameMainProgram.Instance.eventMgr.InvokeEvent(EventName.PlayerHP_SP);
        }

    }
}