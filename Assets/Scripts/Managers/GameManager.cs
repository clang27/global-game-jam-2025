using System.Collections;
using System.Linq;
using DG.Tweening;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers {
	public class GameManager : MonoBehaviour {

		#region Attributes
		public static GameManager Instance { get; private set; }
		public GameState GameState { get; private set; } = GameState.Start;

		#endregion

		#region Data
		private bool _gameOverCooldown;
		private string _sceneNameLoaded;
		#endregion

		#region Unity
		private void Awake() {
			Instance = this;

			DOTween.Init(false, false, LogBehaviour. Default)
				.SetCapacity(1000, 200);
		}

		private void Start() {
			Init();
			StartCoroutine(ChangeScene("Ship"));
		}
    
		private void OnDestroy() {
			DOTween.KillAll();
		}
		#endregion

		#region Custom
		private void Init() {
			GameState = GameState.Start;
			_gameOverCooldown = false;
		
			foreach (var manager in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IManager>()) {
				manager.Init();
			}
		}

		public void StartLargeBubbleTransition(string sceneName) {
			GameState = GameState.BubbleTransition;
			BubbleManager.Instance.PreBubbleRide(_sceneNameLoaded);

			DOVirtual.DelayedCall(0.5f, () => {
				UiManager.Instance.ShowLoading(true,
					() => StartCoroutine(ChangeScene(sceneName)));
			});
		}
		
		public void EndLargeBubbleTransition() {
			UiManager.Instance.ShowLoading(false,
				() => {
					GameState = GameState.Playing;
					BubbleManager.Instance.PostBubbleRide(_sceneNameLoaded);
				});
		}

		private IEnumerator ChangeScene(string sceneName) {
			AsyncOperation unloadSceneAsync = null;
		
			if (_sceneNameLoaded != null) {
				Debug.Log($"Unloading scene: {_sceneNameLoaded}");
				unloadSceneAsync = SceneManager.UnloadSceneAsync(_sceneNameLoaded);
			}

			Debug.Log($"Loading scene: {sceneName}");
			var loadSceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		
			while (loadSceneAsync is { isDone: false }) { yield return null; }
			Debug.Log($"Finished loading {sceneName}");
			while (unloadSceneAsync is { isDone: false }) { yield return null; }
			Debug.Log($"Finished unloading {_sceneNameLoaded}");
			
			_sceneNameLoaded = sceneName;
			BubbleManager.Instance.PostSceneLoad(_sceneNameLoaded);
		}

		public void ResetGame() {
			if (!_gameOverCooldown) {
				DOTween.KillAll();
				Init();
			}
		}

		public void StartGame() {
			GameState = GameState.Playing;
		
			CameraManager.Instance.Switch(CameraState.Ocean);
			
			PlayerManager.Instance.enabled = true;
			BubbleManager.Instance.enabled = true;
			CoinManager.Instance.enabled = true;
			OxygenManager.Instance.enabled = true;
			
			UiManager.Instance.ShowGameOver(false);
			UiManager.Instance.ShowTitle(false);
			UiManager.Instance.ShowHud(true);
		}

		public void GameOver(bool won) {
			GameState = won ? GameState.Win : GameState.GameOver;
		
			_gameOverCooldown = true;
			DOVirtual.DelayedCall(1f, () => _gameOverCooldown = false);

			BubbleManager.Instance.enabled = false;
			CoinManager.Instance.enabled = false;
			OxygenManager.Instance.enabled = false;
			PlayerManager.Instance.enabled = false;
			PlayerManager.Instance.StopPlayer();
		
			UiManager.Instance.ShowGameOver(!won);
			UiManager.Instance.ShowTitle(false);
			UiManager.Instance.ShowHud(false);
		}

		public void Pause() {
			GameState = GameState.Paused;
		
			PlayerManager.Instance.StopPlayer();
			PlayerManager.Instance.enabled = false;
			CoinManager.Instance.enabled = false;
			OxygenManager.Instance.enabled = false;
		
			UiManager.Instance.ShowPause(true);
			UiManager.Instance.ShowHud(false);
		}
	
		public void Unpause() {
			GameState = GameState.Playing;
		
			PlayerManager.Instance.enabled = true;
			CoinManager.Instance.enabled = true;
			OxygenManager.Instance.enabled = true;
		
			UiManager.Instance.ShowPause(false);
			UiManager.Instance.ShowHud(true);
		}
		#endregion

	}
}

