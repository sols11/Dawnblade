using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("计算两个Transform之间的距离是否在某个范围内")]
    [TaskCategory("Custom")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=3")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class AiJudgeDistance : Action
    {
        public SharedTransform transform1;
        public SharedTransform transform2;

        public SharedFloat returnDistance;

        public SharedFloat min;
        public SharedFloat max;

        public override TaskStatus OnUpdate()
        {
            if (transform1 != null && transform2 != null)
            {
                returnDistance.Value = Vector3.Distance(transform1.Value.position, transform2.Value.position);
                if (returnDistance.Value < min.Value || returnDistance.Value > max.Value)
                    return TaskStatus.Failure;
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }

        // Reset the public variables
        public override void OnReset()
        {
            transform1 = null;
            transform2 = null;
        }
    }
}