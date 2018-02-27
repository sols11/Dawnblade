using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskCategory("Custom")]
    [TaskDescription("用DoTween旋转自身到目标方向，而且一般我们只会旋转Y轴，所以要把XZ轴旋转到0")]
    public class AiRotateTowards : Action
    {
        [Tooltip("LookAtDir计算出来的世界坐标差")]
        public SharedVector3 worldDirection;
        [Tooltip("需要花多久")]
        public SharedFloat duration = 1;


        public override TaskStatus OnUpdate()
        {
            if (worldDirection == null || worldDirection.Value==Vector3.zero)
            {
                Debug.LogWarning("worldDirection is null or zero");
                return TaskStatus.Failure;
            }
            // 旋转到目标方向
            transform.DOLocalRotate(new Vector3(0,Quaternion.FromToRotation(Vector3.forward, worldDirection.Value).eulerAngles.y,0), duration.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            duration = 1;
        }
    }
}