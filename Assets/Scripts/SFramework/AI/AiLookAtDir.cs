using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("得到源位置朝向目标位置的方向，用于瞄准。得到的是一个xyz三轴normalized后的值")]
    [TaskCategory("Custom")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=3")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class AiLookAtDir : Action
    {
        [Tooltip("源位置")]
        public SharedTransform sourceTransform;

        [Tooltip("目标位置")]
        public SharedTransform targetTransform;

        [Tooltip("返回的方向")]
        public SharedVector3 returnDirection;


        public override TaskStatus OnUpdate()
        {
            if (sourceTransform!=null && targetTransform!=null)
            {
                returnDirection.Value = (targetTransform.Value.position - sourceTransform.Value.position).normalized;

                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        // Reset the public variables
        public override void OnReset()
        {
            returnDirection = null;
        }
    }
}