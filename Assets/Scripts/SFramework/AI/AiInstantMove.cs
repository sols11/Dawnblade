using UnityEngine;
using SFramework;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityRigidbody
{
    [TaskCategory("Custom")]
    [TaskDescription("瞬移到Target的Distance处。先计算出一个Distance位置然后再绕其旋转")]
    public class AiInstantMove : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject moveGameObject;
        [Tooltip("The transform that the agent is moving towards")]
        public SharedTransform targetTransform;
        [Tooltip("距离Target多远")]
        public SharedFloat distance;
        [Tooltip("The minimum Angle")]
        public SharedInt randomAngleMin = 0;
        [Tooltip("The maximum Angle")]
        public SharedInt randomAngleMax = 360;
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
            if (afterImage == null)
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
            afterImage.hideFlags = HideFlags.HideInHierarchy;
        }

        public override TaskStatus OnUpdate()
        {
            if (rigidbody == null)
            {
                Debug.LogWarning("Rigidbody is null");
                return TaskStatus.Failure;
            }

            rotateAngle = Random.Range(randomAngleMin.Value, randomAngleMax.Value);
            GameMainProgram.Instance.resourcesMgr.LoadAsset(effectPath, true, transform.position, Quaternion.identity);
            CreateAfterImage();
            // 【瞬移】
            Vector3 aimPos = targetTransform.Value.position - moveGameObject.Value.transform.forward * distance.Value;
            rigidbody.MovePosition(aimPos);
            // 绕Player随机旋转一个角度
            moveGameObject.Value.transform.RotateAround(targetTransform.Value.position,Vector3.up,rotateAngle);
            // 朝向Player
            moveGameObject.Value.transform.LookAt(targetTransform.Value);


            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            moveGameObject = null;
            distance = 0;
        }
    }
}