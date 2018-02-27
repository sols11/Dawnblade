using UnityEngine;
using SFramework;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityRigidbody
{
    [TaskCategory("Custom")]
    [TaskDescription("瞬移到moveGameObject身后的Distance处")]
    public class AiInstantAvoid : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject moveGameObject;
        [Tooltip("距离多远")]
        public SharedFloat distance;
        [Tooltip("瞬移特效的路径")]
        public string effectPath = @"EnemySkills\InstantMoveEnemy";

        // cache the rigidbody component
        private Rigidbody rigidbody;
        private GameObject prevGameObject;
        private int rotateAngle = 0;
        // 残影
        private SkinnedMeshRenderer smr;
        private GameObject afterImage;
        private string skillPath = @"EnemySkills\CaptainAfterImage";

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(moveGameObject.Value);
            if (currentGameObject != prevGameObject)
            {
                rigidbody = currentGameObject.GetComponent<Rigidbody>();
                // 注意层级
                smr = currentGameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
                prevGameObject = currentGameObject;
            }
        }

        private void CreateAfterImage()
        {
            Mesh mesh = new Mesh();
            // 拷贝mesh
            smr.BakeMesh(mesh);
            // 就一个残影，用prefab做比较方便
            if(afterImage==null)
            {
                afterImage = GameMainProgram.Instance.resourcesMgr.LoadAsset(skillPath, false,
    prevGameObject.transform.position, prevGameObject.transform.rotation);
            }
            else
            {
                afterImage.transform.position = prevGameObject.transform.position;
                afterImage.transform.rotation = prevGameObject.transform.rotation;
            }
            MeshFilter meshFilter = afterImage.GetComponentInChildren<MeshFilter>();
            meshFilter.mesh = mesh;
            afterImage.SetActive(true);
            afterImage.hideFlags = HideFlags.HideAndDontSave;
        }

        public override TaskStatus OnUpdate()
        {
            if (rigidbody == null)
            {
                Debug.LogWarning("Rigidbody is null");
                return TaskStatus.Failure;
            }

            GameMainProgram.Instance.resourcesMgr.LoadAsset(effectPath, true, transform.position, Quaternion.identity);
            CreateAfterImage();
            // 【瞬移】
            Vector3 aimPos = moveGameObject.Value.transform.position - moveGameObject.Value.transform.forward * distance.Value;

            rigidbody.MovePosition(aimPos);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            moveGameObject = null;
            distance = 0;
        }
    }
}