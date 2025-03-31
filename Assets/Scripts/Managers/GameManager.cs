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
		public GameState GameState { get; set; } = GameState.Start;
		public string CurrentSceneName => _sceneNameLoaded;

	#endregion

	#region Data
		private string _sceneNameLoaded;
		private VidPlayer _vidPlayer;
	#endregion

	#region Unity
		private void Awake() {
			_vidPlayer = FindAnyObjectByType<VidPlayer>();
			
			Instance = this;

			DOTween.Init(false, false, LogBehaviour. Default)
				.SetCapacity(1000, 200);
		}

		private void Start() {
			var startScene = SaveManager.Instance.GetStartScene();

			if (startScene.Equals("Intro")) {
				_vidPlayer.Play();
			} else {
				_vidPlayer.gameObject.SetActive(false);
				LoadBeginning(startScene);
			}
		}
    
		private void OnDestroy() {
			DOTween.KillAll();
		}
	#endregion

	#region Custom
		public void LoadBeginning(string scene) {
			Init();
			StartCoroutine(ChangeScene(scene));
		}
		
		private void Init() {
			GameState = GameState.Start;
		
			foreach (var manager in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IManager>()) {
				manager.Init();
			}
		}

		public void StartLargeBubbleTransition(string sceneName) {
			GameState = GameState.BubbleTransition;
			BubbleManager.Instance.PreBubbleRide(_sceneNameLoaded);

			DOVirtual.DelayedCall(0.5f, () => {
				UiManager.Instance.ShowLoading(true, () => StartCoroutine(ChangeScene(sceneName)));
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
			DOTween.KillAll();
			
			AsyncOperation unloadSceneAsync = null;
		
			if (_sceneNameLoaded != null) {
				Debug.Log($"Unloading scene: {_sceneNameLoaded}");
				unloadSceneAsync = SceneManager.UnloadSceneAsync(_sceneNameLoaded);
			}
			
			while (unloadSceneAsync is { isDone: false }) { yield return null; }
			Debug.Log($"Finished unloading {_sceneNameLoaded}");

			Debug.Log($"Loading scene: {sceneName}");
			var loadSceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		
			while (loadSceneAsync is { isDone: false }) { yield return null; }
			Debug.Log($"Finished loading {sceneName}");
			
			_sceneNameLoaded = sceneName;
			
			foreach (var manager in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IManager>()) {
				manager.SceneChange(_sceneNameLoaded);
			}
		}

		public void ResetGame() {
			UiManager.Instance.ShowLoading(true, () => {
				Init();
				StartCoroutine(ChangeScene("Ship"));

				CutSceneManager.Instance.HideBlackBars();
				OxygenManager.Instance.ResetVolume();
				UiManager.Instance.ShowLoading(false, () => { });
			});
		}

		public void StartGame() {
			PlayerManager.Controller.InUi = false;
			BubbleManager.Instance.enabled = true;
			CoinManager.Instance.enabled = true;
			OxygenManager.Instance.enabled = true;
			
			UiManager.Instance.ShowTitle(false);
			UiManager.Instance.ShowHud(true);
			OxygenManager.Instance.ShrinkEffects();
			
			if (SaveManager.Instance.HasASaveFile()) {
				GameState = GameState.Playing;
				
				var bubbleName = SaveManager.Instance.GetStartBubbleName();
				var bubble = GameObject.Find(bubbleName);
				
				BubbleManager.Instance.SmallBubblePlayerIsOn = bubble.GetComponent<SaveBubbleBehavior>();
				OxygenManager.Instance.InBubble();
				CameraManager.Instance.Switch(CameraState.Bubble, name, bubble.transform);
			} else {
				CameraManager.Instance.Switch(CameraState.Title, name);
				CutSceneManager.Instance.PlayScene("Intro");
			}
		}

		public void GameOver() {
			GameState =  GameState.GameOver;
			
			BubbleManager.Instance.enabled = false;
			CoinManager.Instance.enabled = false;
			OxygenManager.Instance.enabled = false;
			
			UiManager.Instance.ShowTitle(false);
			UiManager.Instance.ShowHud(false);
			
			CutSceneManager.Instance.GameOver();
		}

		public void Pause() {
			GameState = GameState.Paused;
			
			PlayerManager.Controller.InUi = true;
			PlayerManager.Player.StopMoving();
			
			CoinManager.Instance.enabled = false;
			OxygenManager.Instance.enabled = false;
		
			SettingsAndPauseManager.Instance.OpenPause();
			UiManager.Instance.ShowHud(false);
		}
	
		public void Unpause() {
			GameState = GameState.Playing;
		
			PlayerManager.Controller.InUi = false;
			
			CoinManager.Instance.enabled = true;
			OxygenManager.Instance.enabled = true;
		
			SettingsAndPauseManager.Instance.ClosePauseAndSettings();
			UiManager.Instance.ShowHud(GameState != GameState.Win);
		}

		public void Quit() {
			Application.Quit();
		}
	#endregion
	}
}

