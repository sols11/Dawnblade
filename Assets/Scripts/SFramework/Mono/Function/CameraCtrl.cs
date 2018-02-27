using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SFramework {
    /// <summary>
    /// Camera控制器，建议直接挂在MainCamera上
    /// 继承了Mono，用Awake管理生命周期，不继承或许也行
    /// </summary>
    public class CameraCtrl : MonoBehaviour {
        private static CameraCtrl _instance;
        private Camera mainCamera;
        private Y_shake shakeComponent;
        private AutoCam autoCam;

        public static CameraCtrl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CameraCtrl>();

                    if (_instance == null)
                        _instance = new GameObject("CameraCtrl").AddComponent<CameraCtrl>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = GetComponent<CameraCtrl>();
            else if (_instance != GetComponent<CameraCtrl>())
            {
                Debug.LogWarningFormat("There is more than one {0} in the scene，auto inactive the copy one.", typeof(CameraCtrl).ToString());
                gameObject.SetActive(false);
                return;
            }
            mainCamera = Camera.main;
            if (mainCamera)
            {
                shakeComponent = mainCamera.GetComponent<Y_shake>();
                autoCam = mainCamera.GetComponentInParent<AutoCam>();
            }
        }

        public void ShakeMainCamera(Vector3 directionStregth,float startTime=0,float speed=1,float duration=0)
        {
            shakeComponent.DirectionStrength = directionStregth;
            shakeComponent.StartTime = startTime;
            shakeComponent.Speed = speed;
            if (duration <= 0)  // 默认的话就根据1/speed计算时间
                shakeComponent.Duration = 1 / speed;
            else
                shakeComponent.Duration = duration;
            shakeComponent.enabled = true;
        }

        /// <summary>
        /// 拉近视野
        /// </summary>
        /// <param name="close"></param>
        public void DialogCamera(bool close=true)
        {
            if(close)
                mainCamera.DOFieldOfView(30, 0.7f);
            else
                mainCamera.DOFieldOfView(50, 1);
        }

        /// <summary>
        /// 边界控制，限制AutoCam的移动范围
        /// </summary>
        /// <param name="x"></param>
        /// <param name="x_"></param>
        /// <param name="z"></param>
        /// <param name="z_"></param>
        public void SetAreaLimit(float x, float x_, float z, float z_)
        {
            autoCam.SetAreaLimit(x,x_,z,z_);
        }

        /// <summary>
        /// 设置是否启用AutoCam
        /// </summary>
        /// <param name="enable"></param>
        public void EnableAutoCam(bool enable)
        {
            autoCam.enabled = enable;
        }
    }
}