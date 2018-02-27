using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("判断TargetTransform.forward与另一个方向得到的角度是否在某个范围内，不在范围则返回失败，建议用AiLookAtDir得到方向。")]
    [TaskCategory("Custom")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=3")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class AiJudgeAngle : Action
    {
        public SharedTransform targetTransform;

        public SharedVector3 judgeDirection;

        [Tooltip("计算后得到的欧拉角最小不小于的范围")]
        public SharedVector3 minAngle;

        [Tooltip("计算后得到的欧拉角最大不超过的范围")]
        public SharedVector3 maxAngle;

        public override TaskStatus OnUpdate()
        {
            if (targetTransform != null && judgeDirection != null)
            {
                Vector3 angle = Quaternion.FromToRotation(targetTransform.Value.forward, judgeDirection.Value).eulerAngles;
                if (angle.x < minAngle.Value.x || angle.x > maxAngle.Value.x)
                    return TaskStatus.Failure;
                if (angle.y < minAngle.Value.y || angle.y > maxAngle.Value.y)
                    return TaskStatus.Failure;
                if (angle.z < minAngle.Value.z || angle.z > maxAngle.Value.z)
                    return TaskStatus.Failure;
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }

        // Reset the public variables
        public override void OnReset()
        {
            targetTransform = null;
            judgeDirection = null;
        }
    }
}