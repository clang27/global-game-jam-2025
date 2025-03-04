using UnityEngine;

namespace Managers {
    public class PlayerManager : MonoBehaviour, IManager {

    #region Attributes
        public static PlayerBehavior Player { get; private set; }
        public static Transform PlayerTransform { get; private set; }
        public static PlayerController Controller { get; private set; }
        public static LayerMask PlayerMask { get; private set; }
    #endregion
    

    #region Unity
        private void Awake() {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            Controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            PlayerMask = LayerMask.NameToLayer("Player");
        }

    #endregion

    #region Custom
        public void Init() {
            if (SaveManager.Instance.HasASaveFile()) {
                var startPosition = SaveManager.Instance.GetStartPosition();
                Player.InitWithBubble(startPosition);
            } else {
                Player.Init();
            }
            
            Controller.InUi = true;
            Controller.Enabled = true;
        }
        
        public void SceneChange(string sceneName) {}
    #endregion
    }
}