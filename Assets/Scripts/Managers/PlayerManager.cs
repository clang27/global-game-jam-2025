using Enums;
using UnityEngine;

namespace Managers {
    public class PlayerManager : MonoBehaviour, IManager {

        #region Attributes
        public static PlayerManager Instance { get; private set; }
	
        #endregion

        #region Components
        private PlayerBehavior _player;
        private PlayerController _controller;
        #endregion

        #region Unity
        private void Awake() {
            Instance = this;
        
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();
            _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        private void OnEnable() {
            if (!_controller) return;
        
            _controller.GoToPlayerControls();
        }
    
        private void OnDisable() {
            if (!_controller) return;
        
            _controller.GoToUiControls();
        }

        #endregion

        #region Custom
        public void Init() {
            _player.Init();
            enabled = false;
        }

        public void StopPlayer() {
            _player.StopMoving();
        }

        public void HopLargeBubbles(LargeBubbleBehavior bubble) {
            StopPlayer();
            _player.MoveToBubble(bubble.transform.position);
        }

        public void GoToCenterOfBubble(LargeBubbleBehavior bubbleBehavior) {
            var direction = (bubbleBehavior.transform.position - _player.transform.position).normalized / 2f;
            _player.InputVector = direction;
        }
	
        #endregion

    }
}