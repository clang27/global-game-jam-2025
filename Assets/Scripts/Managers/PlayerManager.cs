using UnityEngine;

namespace Managers {
    public class PlayerManager : MonoBehaviour, IManager {

    #region Attributes
        public static PlayerBehavior Player { get; private set; }
        public static Transform PlayerTransform { get; private set; }
        public static PlayerController Controller { get; private set; }
    #endregion
    

    #region Unity
        private void Awake() {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
            PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            Controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

    #endregion

    #region Custom
        public void Init() {
            Player.Init();
            Controller.InUi = true;
            Controller.Enabled = true;
        }
    #endregion
    }
}