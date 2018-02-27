using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    /// <summary>
    /// 为Block专用的残影技能
    /// 由于如果把武器mesh作为子节点时bakemesh会将character的mesh覆盖掉，所以需要专门为katana创建一个GameObject，以及用专门的list存储它
    /// </summary>
    public class SkillAfterImage
    {
        public int AfterImageMaxNum { get; set; }
        private GameObject playerObject;
        private IPlayerMono playerMono;
        private SkinnedMeshRenderer smr;
        private SkinnedMeshRenderer weaponSmr;
        private string skillPath = @"PlayerSkills\BlockAfterImage";
        private string katanaPath = @"PlayerSkills\KatanaAfterImage";
        // 用了一个双向链表来做缓冲池（优点是方便获得首尾结点）
        private LinkedListNode<GameObject> node;
        private LinkedList<GameObject> afterImageList = new LinkedList<GameObject>();
        private LinkedList<GameObject> weaponList = new LinkedList<GameObject>();

        public SkillAfterImage(int maxNum,GameObject player)
        {
            AfterImageMaxNum = maxNum;
            playerObject = player;
            // 以下代码对go层级有严格要求，为针对Block专用
            smr = playerObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
            playerMono = playerObject.GetComponent<IPlayerMono>();
            weaponSmr = playerMono.iPlayerWeapon.gameObject.transform.GetComponent<SkinnedMeshRenderer>();
        }

        public void CreateAfterImage()
        {
            Mesh mesh = new Mesh();
            // 拷贝mesh
            smr.BakeMesh(mesh);
            // 残影到上限了就使用缓冲池的，否则就创建新的
            if (afterImageList.Count >= AfterImageMaxNum)
            {
                // 用已有的结点改变transform
                node = afterImageList.First;
                node.Value.transform.position = playerObject.transform.position;
                node.Value.transform.rotation = playerObject.transform.rotation;
                MeshFilter meshFilter = node.Value.GetComponent<MeshFilter>();
                meshFilter.mesh = mesh;
                // 将该结点移到最后一位
                afterImageList.RemoveFirst();
                afterImageList.AddLast(node);
            }
            else
            { 
                node = new LinkedListNode<GameObject>(GameMainProgram.Instance.resourcesMgr.LoadAsset(skillPath, false,
    playerObject.transform.position, playerObject.transform.rotation));
                //MeshFilter+MeshRenderer，不要使用SkinnedMeshRenderer，因为他只有共享材质
                MeshFilter meshFilter = node.Value.GetComponent<MeshFilter>();
                meshFilter.mesh = mesh;
                // 加入
                afterImageList.AddLast(node);
                // 不能显示在层级面板但是可以保存到场景，这样切场景时才会删除。
                node.Value.hideFlags= HideFlags.HideInHierarchy;
            }
        }

        public void KatanaAfterImage()
        {
            Mesh mesh = new Mesh();
            // 拷贝mesh
            weaponSmr.BakeMesh(mesh);
            // 残影到上限了就使用缓冲池的，否则就创建新的
            if (weaponList.Count >= AfterImageMaxNum)
            {
                // 用已有的结点改变transform
                node = weaponList.First;
                node.Value.transform.position = playerMono.iPlayerWeapon.transform.position;
                node.Value.transform.rotation = playerMono.iPlayerWeapon.transform.rotation;
                MeshFilter weaponFilter = node.Value.GetComponent<MeshFilter>();
                weaponFilter.mesh = mesh;
                // 将该结点移到最后一位
                weaponList.RemoveFirst();
                weaponList.AddLast(node);
            }
            else
            {
                node = new LinkedListNode<GameObject>(GameMainProgram.Instance.resourcesMgr.LoadAsset(katanaPath, false,
    playerMono.iPlayerWeapon.transform.position, playerMono.iPlayerWeapon.transform.rotation));
                MeshFilter meshFilter = node.Value.GetComponent<MeshFilter>();
                meshFilter.mesh = mesh;
                // 加入
                weaponList.AddLast(node);
                node.Value.hideFlags = HideFlags.HideInHierarchy;
            }
        }

        /// <summary>
        /// 删除第一个人物和武器残影，返回是否成功
        /// </summary>
        /// <returns></returns>
        public bool DeleteAfterImage()
        {
            if (afterImageList.Count > 0)
            {
                node = afterImageList.First;
                afterImageList.RemoveFirst();
                GameObject.Destroy(node.Value);
                if (weaponList.Count > 0)
                {
                    node = weaponList.First;
                    weaponList.RemoveFirst();
                    GameObject.Destroy(node.Value);
                    node = null;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断能否瞬移，即残影数量是否大于0
        /// </summary>
        /// <returns></returns>
        public bool CanInstantMove()
        {
            return afterImageList.Count > 0;
        }

        /// <summary>
        /// 获取最后一个结点，没有就返回null
        /// </summary>
        /// <returns></returns>
        public GameObject GetLastNode()
        {
            if(CanInstantMove())
            {
                return afterImageList.Last.Value;
            }
            return null;
        }
    }
}