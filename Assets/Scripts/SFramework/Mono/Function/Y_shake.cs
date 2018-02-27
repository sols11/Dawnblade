using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SFramework
{
    /// <summary>
    /// Y_Shake1.0
    /// 震动物体，主要用于Camera震屏
    /// </summary>
    public class Y_shake : MonoBehaviour
    {
        //enable设为true后，在startTime后震屏一次
        public float StartTime = 1.0f;  // 开始震屏的时间
        public Vector3 DirectionStrength = new Vector3(0, 1, 0);  //方向和力度
        public float Duration = 1;  // 震动时长
        public float Speed = 1;  // 震动速度
        public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.13f, 0.4f), new Keyframe(0.33f, -0.33f), new Keyframe(0.5f, 0.17f), new Keyframe(0.71f, -0.12f), new Keyframe(1, 0));
        // 动画曲线默认持续1s，实际震动时长=1/speed

        float timer;
        float keepTimer;
        Vector3 thisPosition; // currentPos-lastShakePos
        Vector3 shakePosition;// culculatePos

        /// <summary>
        /// 每次激活时震屏
        /// </summary>
        void FixedUpdate()
        {
            keepTimer += Time.deltaTime;
            thisPosition = transform.position - shakePosition;
            // 获得对应时间下的动画曲线
            float t = curve.Evaluate((keepTimer - timer) * Speed);
            shakePosition = new Vector3(t * DirectionStrength.x, t * DirectionStrength.y, t * DirectionStrength.z);
            if (timer >= StartTime)
            {
                // 更新物体的position
                transform.position = shakePosition + thisPosition;
                // 震屏结束后
                if(keepTimer>StartTime+Duration)
                {
                    keepTimer = timer = 0;
                    enabled = false;
                }
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

    }
}