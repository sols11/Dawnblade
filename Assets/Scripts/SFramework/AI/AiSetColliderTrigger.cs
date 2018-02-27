using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityBoxCollider
{
    [TaskCategory("Custom")]
    [TaskDescription("设置Collider的Trigger属性")]
    public class AiSetColliderTrigger : Action
    {
        public Collider collider;
        public SharedBool boolValue;

        public override TaskStatus OnUpdate()
        {
            collider.isTrigger=boolValue.Value;

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            collider = null;
            boolValue = false;
        }
    }
}