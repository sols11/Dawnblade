using UnityEngine;

namespace SFramework
{
	/// <summary>
	/// 游戏主循环
	/// </summary>
	public class GameLoop : MonoBehaviour
	{
        private static GameLoop _instance;

        public static GameLoop Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameLoop>();

                    if (_instance == null)
                        _instance = new GameObject("GameLoop").AddComponent<GameLoop>();
                }
                return _instance;
            }
        }

        public SceneStateController sceneStateController = new SceneStateController();

        [SerializeField]
        private SceneState sceneState;

        void Awake()
        {
            if (_instance == null)
                _instance = GetComponent<GameLoop>();
            // 删除新增的实例
            else if (_instance != GetComponent<GameLoop>())
            {
                Debug.LogWarningFormat("There is more than one {0} in the scene，auto destroy the copy one.", typeof(GameLoop).ToString());
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

		void Start()
		{
			// 要测试的场景，只需要在Inspector中设置就行了
			sceneStateController.SetState(sceneState, false);
		}

		void Update()
		{
			sceneStateController.StateUpdate();
		}

		void FixedUpdate()
		{
			//物理相关的处理
			sceneStateController.FixedUpdate();
		}
	}
}
