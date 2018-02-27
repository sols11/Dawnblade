using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
	public class PlayerYuka:IPlayer
	{
	    public override bool CanMove
	    {
	        get { return m_CanMove; }
	        set { h = 0; v = 0;
                m_CanMove = value;
                if(!m_CanMove)
                    animator.SetFloat(aniSpeed, 0);
            }
        }

	    //动画参数名
		private string aniSpeed = "Speed";
		private string aniAvoid = "Avoid";
        private string aniParried = "Parried";
        private string aniRoared = "Roared";
        private string aniDush = "Dush";
        private string aniDrink = "Drink";
        private string aniCast = "Cast";
        private string aniInstant = "InstantMove";
        private string aniHurt = "Hurt";
        private string aniDefeated = "Defeated";
        private string aniDead = "Dead";
        private string aniLAttack = "LightAttack";
		private string aniLAttack1 = "LightAttack1";//第一击是trigger
		private string aniHAttack = "HeavyAttack";
		private string aniHAttack1 = "HeavyAttack1";//第一击是trigger
		private string aniSAttack = "StrongAttack";
		private string aniSAttack1 = "StrongAttack1";
        private int aniDownGround = Animator.StringToHash("DownGround");
        //计算需要的字段
        private int lightAttack = 0;
		private int heavyAttack = 0;
		private int strongAttack = 0;
        private float recoveryTime = 0.3f; // SP自动回复所需时间
        private float recoveryTimer = 0.3f; // 使用计时器而不使用协程实现
        private float recoveryWait = 1; // 要实现使用SP1秒后才能开始回复的效果
        private bool canCast = true;    // 同时也是是否持有MagicBall
		private Vector3 targetDirection;//输入的方向
		private Vector3 forwardDirection;//存储输入后的朝向
        //获取Mono的引用
		private KatanaAnimEvent katanaAnimEvent;
        // 残影技能
        private SkillAfterImage skillAfterImage;

        /// <summary>
        /// 创建时的初始化
        /// </summary>
        /// <param name="gameObject"></param>
		public PlayerYuka(GameObject gameObject):base(gameObject)
		{
            katanaAnimEvent = PlayerMedi.PlayerMono as KatanaAnimEvent;
            skillAfterImage = new SkillAfterImage(1, GameObjectInScene);
            if(katanaAnimEvent) // 引用传递
                katanaAnimEvent.AfterImage = skillAfterImage;
            // 以下是其他属性
            m_CanMove = true;
            CanRotate = true;
            recoveryTime = 0.05f;
            recoveryTimer = recoveryTime;
        }

        /// <summary>
        /// 每次切换场景时执行
        /// </summary>
		public override void Initialize() {
        }

        public override void Release()
        {
            base.Release();
        }

	    public override void FixedUpdate()
	    {
	        // 当Avoid转向其他状态时也可以移动
	        if (CanMove)
	            if (stateInfo.IsName("Idle") || stateInfo.IsName("Run") || stateInfo.IsName("AtoR"))
	                GroundMove(h, v);
	    }

        public override void Update() {
			stateInfo = animator.GetCurrentAnimatorStateInfo(0);

			MoveInput();
            AfterImageInput();
            AllState();

            //自动回复
            recoveryTimer -= Time.deltaTime;
            if (recoveryTimer <= 0)
            {
                recoveryTimer = recoveryTime;
                CurrentSP+=1;
            }
        }

		/// <summary>
		/// 触发动画，伤害计算，死亡判断
		/// </summary>
		/// <param name="damage"></param>
		public override void Hurt(PlayerHurtAttr playerHurtAttr)
		{
            if (stateInfo.IsName("Dead") || stateInfo.IsName("Dush"))
                return;
            Rgbd.velocity = playerHurtAttr.TransformForward * playerHurtAttr.VelocityForward;
            animator.applyRootMotion = false; // 关闭rootmotion
            // 击飞
            if (playerHurtAttr.CanDefeatedFly) 
            {
                if (playerHurtAttr.TransformForward != Vector3.zero)
                    GameObjectInScene.transform.forward = -playerHurtAttr.TransformForward;
                animator.SetTrigger(aniDefeated);
            }
            // 击倒
            else if (playerHurtAttr.DefeatedDown)   
            {
                // 由于y轴可能不为0
                if (playerHurtAttr.TransformForward != Vector3.zero)
                    GameObjectInScene.transform.forward = new Vector3(-playerHurtAttr.TransformForward.x, 0, -playerHurtAttr.TransformForward.z);
                animator.SetTrigger(aniDownGround);
                // 切换layer,到动画结束后再切换回来
                GameObjectInScene.layer = (int)ObjectLayer.Without;
            }
            // 受伤
            else
            {
                animator.SetTrigger(aniHurt);
            }
            //int damage = _playerHurtAttr.Attack * _playerHurtAttr.Attack /(_playerHurtAttr.Attack + DefendPoint);
		    int damage = playerHurtAttr.Attack - DefendPoint;
            CurrentHP -= damage;
            Debug.Log("PlayerHurt:" + damage);
            katanaAnimEvent.TrailSwitch(0); // close Trail
		}
        public override void Dead()
        {
            base.Dead();
            animator.SetTrigger(aniDead);

        }

        #region 用于外部调用
        /// <summary>
        /// 被弹开，提供给AnimEvent调用
        /// </summary>
        public void Parried()
        {
            animator.SetTrigger(aniParried);
        }
        /// <summary>
        /// 被吼叫，提供给Enemy触发
        /// </summary>
        public override void Roared()
        {
            animator.SetTrigger(aniRoared);
        }
        /// <summary>
        /// 喝药，提供给AnimEvent调用
        /// </summary>
        public void DrinkMedicine()
        {
            CurrentHP += Props[MedicineID].HP;
        }
        #endregion

        /// <summary>
        /// 处理方向键、闪避输入、冲刺攻击，进行移动和旋转
        /// </summary>
        void MoveInput()
		{
            h = Input.GetAxisRaw("Horizontal");
			v = Input.GetAxisRaw("Vertical");
			targetDirection = new Vector3(h, 0, v).normalized;

			//移动状态时使用平滑旋转
			if (stateInfo.IsName("Idle") || stateInfo.IsName("Run") || stateInfo.IsName("AtoR"))
			{
				if (CanRotate)
					Rotating();
				//用于回到了Idle或Run状态则恢复输入，但是如果仍在Transition未过渡到attack等状态时不要打开开关（刚刚在attack关闭开关）
				//if (!animator.IsInTransition(0)) 用rootmotion写不写都一样
				{
					m_CanMove = true;//仅打开开关
				}
                DrinkInput();
            }
            //注意下面这段代码要放在canRotate=true下面(执行顺序)
            if(CanAvoid)
                AvoidInput();
            if (CanDush)
                DushInput();

            //是否切换为Run
            if (Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1)
				Speed = 1;
			else
				Speed = 0;
			animator.SetFloat(aniSpeed, Speed);
		}
		/// <summary>
		/// 平滑旋转
		/// </summary>
		/// <param name="h"></param>
		/// <param name="v"></param>
		void Rotating()
		{
			//计算出旋转
			if (targetDirection != Vector3.zero)
			{
				//目标方向的旋转角度
				Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
				//平滑插值
				Quaternion newRotation = Quaternion.Slerp(Rgbd.rotation, targetRotation, RotSpeed * Time.deltaTime);
				Rgbd.MoveRotation(newRotation);
			}
		}
		/// <summary>
		/// 地面移动
		/// </summary>
		/// <param name="h"></param>
		/// <param name="v"></param>
		void GroundMove(float h, float v)
		{
			if (Speed != 0)
				Rgbd.MovePosition(GameObjectInScene.transform.position + targetDirection * MoveSpeed * Time.deltaTime);
		}

        void DrinkInput()
        {
            // 只要Num>=1，就说明装备了可以使用的道具
            if(Input.GetButtonDown("Drink")&& PropNum[MedicineID] >= 1)
            {
                PropNum[MedicineID]-=1;
                GameMainProgram.Instance.eventMgr.InvokeEvent(EventName.MedicineNum);    // UI
                animator.SetTrigger(aniDrink);
            }
        }

        void AfterImageInput()
        {
            if (Input.GetButtonDown("Locate"))
            {
                // 目前先做成只有1个残影和1次瞬移
                if (skillAfterImage.CanInstantMove())   // 瞬移
                {
                    CurrentSP -= 25;
                    recoveryTimer = 2*recoveryWait; // 因为CD长所以回复也要变慢
                    animator.SetTrigger(aniInstant);
                    animator.applyRootMotion = false;   // 关闭rootmotion，因为要转向
                    katanaAnimEvent.TrailSwitch(0);     // 关闭col和trail
                    if (targetDirection != Vector3.zero)//如果不是向前方闪避，那么先转向
                        Rgbd.MoveRotation(Quaternion.LookRotation(targetDirection, Vector3.up));
                }
                // 残像
                else
                {
                    skillAfterImage.CreateAfterImage();
                    skillAfterImage.KatanaAfterImage();
                }
            }
        }

        //void CastInput()
        //{
        //    // 只要Num>=1，就说明装备了可以使用的道具
        //    if (Input.GetButtonDown("Avoid"))
        //    {
        //        animator.applyRootMotion = false;   // 关闭rootmotion，因为要转向
        //        katanaAnimEvent.TrailSwitch(0);     // 关闭col和trail
        //        if (targetDirection != Vector3.zero)//如果不是向前方闪避，那么先转向
        //        {
        //            Rgbd.MoveRotation(Quaternion.LookRotation(targetDirection, Vector3.up));
        //        }



        //        释放magicball
        //        if (canCast)
        //        {
        //            canCast = false;
        //            animator.SetTrigger(aniCast);
        //            katanaAnimEvent.MagicBall.Cast();
        //        }
        //        需要等球停下
        //        else if (katanaAnimEvent.MagicBall.Rgbd.velocity.magnitude < 1)
        //        {
        //            canCast = true;
        //            animator.SetTrigger(aniInstant);
        //        }
        //    }
        //}

        void AvoidInput()
        {
            //如果是Transition状态（如avoid->run,run->avoid）或正在avoid则不允许输入
            if (!animator.IsInTransition(0) && (stateInfo.IsName("Idle") || stateInfo.IsName("Run") || stateInfo.IsName("Att1") ||
                stateInfo.IsName("Att2") || stateInfo.IsName("Att3") || stateInfo.IsName("Att4")))
                if (Input.GetButtonDown("Avoid") && CurrentSP >= 25)//闪避
                {
                    CurrentSP -= 25;
                    recoveryTimer = recoveryWait;
                    animator.applyRootMotion = false;   // 关闭rootmotion，因为要转向
                    animator.SetTrigger(aniAvoid);
                    katanaAnimEvent.TrailSwitch(0);//关闭col和trail
                    if (targetDirection != Vector3.zero)//如果不是向前方闪避，那么先转向
                    {
                        Rgbd.MoveRotation(Quaternion.LookRotation(targetDirection, Vector3.up));
                    }
                }
        }

        void DushInput()
        {
            if (!animator.IsInTransition(0) &&
                (stateInfo.IsName("Idle") || stateInfo.IsName("Run") || stateInfo.IsName("Att1") ||
                 stateInfo.IsName("Att2") || stateInfo.IsName("Att3") || stateInfo.IsName("Att4")))
            {
                if (Input.GetButtonDown("Dush") && CurrentSP >= 35) //闪避
                {
                    CurrentSP -= 35;
                    recoveryTimer = recoveryWait;
                    animator.applyRootMotion = false; // 关闭rootmotion，因为要转向
                    animator.SetTrigger(aniDush);
                    katanaAnimEvent.TrailSwitch(0); //关闭col和trail
                    if (targetDirection != Vector3.zero) //如果不是向前方闪避，那么先转向
                        Rgbd.MoveRotation(Quaternion.LookRotation(targetDirection, Vector3.up));
                    // 切换layer并自动切换回来
                    GameObjectInScene.layer = (int) ObjectLayer.Without;
                    CoroutineMgr.Instance.StartCoroutine(DushEnd());
                }
            }
        }

        IEnumerator DushEnd()
        {
            yield return new WaitForSeconds(0.9f);
            GameObjectInScene.layer = (int)ObjectLayer.Player;
        }

        void AllState()
		{
			if ((stateInfo.IsName("Idle") || stateInfo.IsName("Run")))
			{
				IdleOrRun();
			}
			else if (stateInfo.IsName("Avoid"))
			{
				Avoid();
			}
			else if (stateInfo.IsName("Att1"))
			{
				Att1();
			}
			else if (stateInfo.IsName("2Att1"))
			{
				_2Att1();
			}
			else if (stateInfo.IsName("3Att1"))
			{
			    _3Att1();
			}
            else if (stateInfo.IsName("Att2"))
			{
				Att2();
			}
			else if (stateInfo.IsName("2Att2"))
			{
				_2Att2();
			}
			else if (stateInfo.IsName("3Att2"))
			{
				_3Att2();
			}
			else if (stateInfo.IsName("Att3"))
			{
				Att3();
			}
			else if (stateInfo.IsName("3Att3"))
			{
				_3Att3();
			}
		}

		void IdleOrRun()
		{
			if (!animator.IsInTransition(0))
			{
				if (Input.GetButtonDown("Attack1"))
				{
					lightAttack = 1;
					heavyAttack = 0;//清除
					strongAttack = 0;
                    animator.SetTrigger(aniLAttack1);
					animator.SetInteger(aniLAttack, lightAttack);//清除之前记录的lightAttack
					animator.SetInteger(aniHAttack, heavyAttack);//清除之前记录的heavyAttack
					animator.SetInteger(aniSAttack, strongAttack);
					CanMove = false;
				    //攻击前瞬时转向
                    if (targetDirection != Vector3.zero)
				        Rgbd.MoveRotation(Quaternion.LookRotation(targetDirection, Vector3.up));
				}
				else if (CanAttack2 && Input.GetButtonDown("Attack2"))
				{
					lightAttack = 0;//清除
					heavyAttack = 1;//第一击重击设为0以区别开
					strongAttack = 0;
					animator.SetTrigger(aniHAttack1);
					animator.SetInteger(aniLAttack, lightAttack);//清除之前记录的lightAttack
					animator.SetInteger(aniHAttack, heavyAttack);//清除之前记录的lightAttack
					animator.SetInteger(aniSAttack, strongAttack);
					CanMove = false;
				    //攻击前瞬时转向
				    if (targetDirection != Vector3.zero)
				        Rgbd.MoveRotation(Quaternion.LookRotation(targetDirection, Vector3.up));
                }
				else if (CanAttack3 && Input.GetButtonDown("Attack3"))
				{
				    lightAttack = 0;//清除
				    heavyAttack = 0;
					strongAttack = 1;
				    animator.SetTrigger(aniSAttack1);
				    animator.SetInteger(aniLAttack, lightAttack);//清除之前记录的lightAttack
				    animator.SetInteger(aniHAttack, heavyAttack);//清除之前记录的lightAttack
					animator.SetInteger(aniSAttack, strongAttack);
				    CanMove = false;
				    //攻击前瞬时转向
                    if (targetDirection != Vector3.zero)
				        Rgbd.MoveRotation(Quaternion.LookRotation(targetDirection, Vector3.up));
                }
            }
            #region Transition(0)
            //else
            //{
            //    //idle->run的Transition时也要检测输入
            //    if (lightAttack == 1)
            //    {
            //        if (Input.GetButtonDown("Attack1"))
            //        {
            //            lightAttack = 2;
            //        }
            //    }
            //    else if (heavyAttack == 1)
            //    {
            //        if (Input.GetButtonDown("Attack2"))
            //        {
            //            heavyAttack = 2;
            //        }
            //    }
            //    else if (strongAttack == 1)
            //    {
            //        if (Input.GetButtonDown("Attack3"))
            //        {
            //            strongAttack = 2;
            //        }
            //    }
            //}
            #endregion
        }
		void Avoid()
		{
			//if(stateInfo.normalizedTime<0.6f)
				//Rgbd.velocity = GameObjectInScene.transform.forward*MoveSpeed*2f;
		}
		void Att1()
		{
			if (Input.GetButtonDown("Attack1"))
			{
				lightAttack = 2;
			}
			//过早的输入也只能在0.45的time时进行transition，其他输入在输入时tansition
			//如果在Att1状态且未发生状态转换
			if (!animator.IsInTransition(0))
			{
				if (stateInfo.normalizedTime >= 0.45f)
				{
					//转完方向后再移动
					animator.SetInteger(aniLAttack, lightAttack);
				}
			}
		}
		void _2Att1()
		{
			if (Input.GetButtonDown("Attack2"))
			{
				heavyAttack = 2;
			}
			if (!animator.IsInTransition(0))//一旦已经向atk2过渡就不要执行了
			{
				if (stateInfo.normalizedTime >= 0.7f)
				{
					//攻击前平滑转向
					animator.SetInteger(aniHAttack, heavyAttack);
				}
			}
		}
	    void _3Att1()
	    {
	        if (Input.GetButtonDown("Attack3"))
	        {
	            strongAttack = 2;
	        }
	        if (!animator.IsInTransition(0))//一旦已经向atk2过渡就不要执行了
	        {
	            if (stateInfo.normalizedTime >= 0.5f)
	            {
	                animator.SetInteger(aniSAttack, strongAttack);
	            }
	        }
	    }
        void Att2()
		{
			if (Input.GetButtonDown("Attack1"))
			{
				lightAttack = 3;
			}
			if (!animator.IsInTransition(0))//一旦已经向atk3过渡就不要执行了
			{
				if (stateInfo.normalizedTime >= 0.5f)
				{
					animator.SetInteger(aniLAttack, lightAttack);
				}
			}
		}
		void _2Att2()
		{
			if (Input.GetButtonDown("Attack2"))
				heavyAttack = 3;
			if (!animator.IsInTransition(0))//一旦已经向atk2过渡就不要执行了
			{
				if (stateInfo.normalizedTime >= 0.7f)
				{
					animator.SetInteger(aniHAttack, heavyAttack);
				}
			}
		}
		void _3Att2()
		{
		    if (Input.GetButtonDown("Attack3"))
		    {
		        strongAttack = 3;
		    }
		    if (!animator.IsInTransition(0))//一旦已经向atk2过渡就不要执行了
		    {
		        if (stateInfo.normalizedTime >= 0.55f)
		        {
		            animator.SetInteger(aniSAttack, strongAttack);
		        }
		    }
        }
		void Att3()
		{
			if (Input.GetButtonDown("Attack1"))
				lightAttack = 4;

			if (!animator.IsInTransition(0))//一旦已经向atk4过渡就不要执行了
			{
				if (stateInfo.normalizedTime >= 0.4f)
				{
					//转完方向后再移动
					animator.SetInteger(aniLAttack, lightAttack);
				}
			}
		}
		void _3Att3()
		{
		    if (Input.GetButtonDown("Attack3"))
		    {
		        strongAttack = 4;
		    }
		    if (!animator.IsInTransition(0))//一旦已经向atk2过渡就不要执行了
		    {
		        if (stateInfo.normalizedTime >= 0.7f)
		        {
		            animator.SetInteger(aniSAttack, strongAttack);
		        }
		    }
        }

    }
}