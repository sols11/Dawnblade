using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    public class PlayerPeace : IPlayer
    {
        public override bool CanMove
        {
            get { return m_CanMove; }
            set
            {
                h = 0; v = 0;
                m_CanMove = value;
                Speed = 0;
                if (!m_CanMove)
                    animator.SetFloat(aniSpeed, 0);
            }
        }

        private string aniSpeed = "Speed";
        private Vector3 targetDirection;//输入的方向
        private Vector3 forwardDirection;//存储输入后的朝向
        //获取Mono的引用

        /// <summary>
        /// 创建时的初始化
        /// </summary>
        /// <param name="gameObject"></param>
        public PlayerPeace(GameObject gameObject):base(gameObject)
        {
            m_CanMove = true;
            CanRotate = true;
        }

        public override void Initialize()
        {
            GameMainProgram.Instance.inputMgr.CanMoveInput = true;
            GameMainProgram.Instance.inputMgr.OnDirectionAxis.AddListener(MoveInput);
        }

        public override void Release()
        {
            base.Release();
            GameMainProgram.Instance.inputMgr.OnDirectionAxis.RemoveListener(MoveInput);
            GameMainProgram.Instance.inputMgr.CanMoveInput = false;
        }

        public override void Update()
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            //MoveInput();
        }

        public override void FixedUpdate()
        {
            if (CanMove)
                if (stateInfo.IsName("Idle") || stateInfo.IsName("Run"))//使用MovePosition调整速度的状态
                    GroundMove(h, v);
        }

        void MoveInput()
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            Vector3 inputDir = new Vector3(h, 0, v).normalized;
            // 用四元数绕Y轴旋转向量，使其和相机y朝向一致
            targetDirection = Quaternion.AngleAxis(-95, Vector3.up) * inputDir;

            //移动状态时使用平滑旋转
            if (stateInfo.IsName("Idle") || stateInfo.IsName("Run"))
            {
                if (CanRotate)
                    Rotating();
            }

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
        void Rotating()
        {
            //计算出旋转
            if (targetDirection != Vector3.zero)
            {
                //目标方向的旋转角度
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                //平滑插值
                // 村子里旋转速度慢
                Quaternion newRotation = Quaternion.Slerp(Rgbd.rotation, targetRotation, 5 * Time.deltaTime);
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
            //Speed = 4 村子里移动速度慢
            if (Speed != 0)
                Rgbd.MovePosition(GameObjectInScene.transform.position + targetDirection * 4 * Time.deltaTime);
        }

    }
}
