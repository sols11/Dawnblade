using UnityEngine;
using SFramework;

namespace DreamKeeper
{
    public class SetStateTrigger:MonoBehaviour
    {
        [SerializeField]
        private SceneState sceneState=SceneState.VillageScene;

        private void OnTriggerEnter(Collider other)
        {
            GameMainProgram.Instance.uiManager.ShowUIForms("FadeIn");
            Invoke("ChangeScene", 2);
        }

        void ChangeScene()
        {
            // 切换Scene
            GameLoop.Instance.sceneStateController.SetState(sceneState);
        }
    }
}
