using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFramework;
using DG.Tweening;

namespace DreamKeeper
{
    public class CaptainMono : IEnemyMono
    {
        public SkinnedMeshRenderer meshRenderer;
        public Material fadeMaterial;
        public Material fadeWeaponMaterial;

        private EnemyCaptain enemyCaptain;

        public override void Initialize()
        {
            base.Initialize();
            enemyCaptain = EnemyMedi.Enemy as EnemyCaptain;
        }

        /// <summary>
        /// 交给EnemyCaptain调用
        /// </summary>
        public IEnumerator DeadFadeOut()
        {
            yield return new WaitForSeconds(1);
            Material[] ma = { fadeMaterial, fadeWeaponMaterial };
            meshRenderer.materials = ma;
            meshRenderer.materials[0].DOFade(0, 5);
            meshRenderer.materials[1].DOFade(0, 5);
            yield return new WaitForSeconds(5);
            gameObject.SetActive(false);
        }

        public override void PlayVoiceRandom(int index)
        {
            int random = 0;
            if(index==0)
                random = Random.Range(0, 3);
            else
                random = Random.Range(4, 6);
            if (random >= voice.Length || random < 0)
                return;
            allAudioSource[0].clip = voice[random];
            allAudioSource[0].Play();
        }

        public void ShakeCamera()
        {
            CameraCtrl.Instance.ShakeMainCamera(new Vector3(0, 1, 0), 0, 2); // shake
        }

        /// <summary>
        /// 动画事件，当Attack关闭Trigger后触发
        /// </summary>
        //public void OnAttackComplete(){}

    }
}