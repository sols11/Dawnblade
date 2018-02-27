using DG.Tweening;
using UnityEngine;

namespace SFramework
{
    /// <summary>
    /// NPC基类，NPC都会说话，无声音，不移动，动态添加到场景，以Prefab形式保存
    /// </summary>
    public abstract class INPC:MonoBehaviour
    {
        public Animator AnimatorComponent { get; protected set; }

        protected string dialogKey = string.Empty;
        protected float maxDistance = 2;
        protected IPlayer player;
        protected Transform playerTransform;
        protected Transform triangleTransform;
        /// <summary>
        /// 初始朝向
        /// </summary>
        protected Vector3 initDir;
        protected GameMainProgram gameMainProgram;

        /// <summary>
        /// 初始化和释放由NpcMgr调用
        /// </summary>
        public virtual void Initialize()
        {
            gameMainProgram=GameMainProgram.Instance;
            AnimatorComponent = gameObject.GetComponent<Animator>();
            triangleTransform = transform.GetChild(0);
            initDir = transform.rotation.eulerAngles;
            // Npc的生成应该在Player之后
            player = GameMainProgram.Instance.playerMgr.CurrentPlayer;
            if (player != null)
                playerTransform = player.GameObjectInScene.transform;
            else
                Debug.LogError("未能获取到Player");
        }

        public virtual void Release()
        {
            Destroy(gameObject);
        }

        public virtual void OnUpdate()
        {
            // 检查Player存在
            if (player == null)
            {
                player = gameMainProgram.playerMgr.CurrentPlayer;
                if (player != null)
                    playerTransform = player.GameObjectInScene.transform;
            }
            if (!UIDialog.IsTalking)
            {
                if (playerTransform == null)
                    return;
                if (Vector3.Distance(playerTransform.position, transform.position) > maxDistance)
                {
                    if(triangleTransform.gameObject.activeSelf)
                        triangleTransform.gameObject.SetActive(false);
                    return;
                }
                // 如果暂停菜单打开着，那么不对话
                if (gameMainProgram.courseMgr.normalMenuOpen)   
                    return;
                // 在半径为maxDistance的圆范围内时检测输入
                if(!triangleTransform.gameObject.activeSelf)
                    triangleTransform.gameObject.SetActive(true);
                if (Input.GetButtonDown("Attack1"))
                {
                    // 对话并禁止移动
                    UIDialog.IsTalking = true;
                    gameMainProgram.playerMgr.CurrentPlayer.CanMove = false;
                    gameMainProgram.inputMgr.CanMoveInput = false;
                    triangleTransform.gameObject.SetActive(false);
                    // 转向 从Z轴向朝向Player的方向创造一个旋转，其欧拉角就是该向量在世界空间下的旋转角度
                    transform.DOLocalRotate(Quaternion.FromToRotation(Vector3.forward, playerTransform.position - transform.position).eulerAngles, 1);
                    CameraCtrl.Instance.DialogCamera(true);
                    gameMainProgram.dialogMgr.StartDialog(dialogKey, OnDialogComplete);
                }
            }
        }

        /// <summary>
        /// 对话结束时的事件
        /// </summary>
        protected virtual void OnDialogComplete()
        {
            transform.DOLocalRotate(initDir, 1);
            CameraCtrl.Instance.DialogCamera(false);
            UIDialog.IsTalking = false;
            gameMainProgram.inputMgr.CanMoveInput = true;
        }

    }
}
