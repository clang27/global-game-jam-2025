using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Managers {
	public class UiManager : MonoBehaviour, IManager {

	#region Dependencies
		[SerializeField] private CanvasGroup hudCanvasGroup, titleCanvasGroup;
		[SerializeField] private RectTransform _loadingScreen;
		[SerializeField] private Button startGameButton;
		[SerializeField] private RectTransform titleImage;
		[SerializeField] private CanvasGroup pauseAndSettingsCanvasGroup, pauseCanvasGroup, settingsCanvasGroup;
	#endregion

	#region Attributes
		public static UiManager Instance { get; private set; }
	#endregion

	#region Components
		private Transform _transform;
		private EventSystem _eventSystem;
	#endregion

	#region Unity
		private void Awake() {
			Instance = this;
			_transform = transform;
			_eventSystem = FindFirstObjectByType<EventSystem>();
		}
	#endregion

	#region Custom
		public void Init() {
			ShowTitle(true);
			ShowHud(false);

			titleImage.DOKill();
			titleImage.DOScale(Vector3.one, 0f);
			titleImage.DOScale(Vector3.one * 1.1f, 2f)
				.SetEase(Ease.InOutSine)
				.SetLoops(-1, LoopType.Yoyo)
				.OnKill(() => {
					titleImage.DOScale(Vector3.one, 0f);
				});
		}

		public void SceneChange(string sceneName) {
			foreach (var kp in FindObjectsByType<KeyPrompt>(FindObjectsSortMode.None)) {
				kp.Hide();
				kp.Bounce();
			}
		}
		
		public void ShowLoading(bool b, Action a) {
			_loadingScreen.DOKill();
			_loadingScreen.DOLocalMoveY(b ? -2080f : 0f, 0f);
			_loadingScreen.DOLocalMoveY(b ? 0f : 2080f, 1f).SetEase(Ease.Linear)
				.OnComplete(() => a());
		}
		
		public void ShowSettingsGroup(GameObject go, bool b) {
			if (b) {
				_eventSystem.SetSelectedGameObject(go);
			}

			ShowCanvas(settingsCanvasGroup, b);
		}
		
		public void ShowPauseGroup(GameObject go, bool b) {
			if (b) {
				_eventSystem.SetSelectedGameObject(go);	
			}
			
			ShowCanvas(pauseCanvasGroup, b);
		}

		public void ShowPauseAndSettings(bool b) {
			ShowCanvas(pauseAndSettingsCanvasGroup, b);
		}

		public void ShowHud(bool b) {
			ShowCanvas(hudCanvasGroup, b);
		}
	
		public void ShowTitle(bool b) {
			if (b) {
				_eventSystem.SetSelectedGameObject(startGameButton.gameObject);
				
				startGameButton.GetComponentInChildren<TextMeshProUGUI>().text = SaveManager.Instance.HasASaveFile() ?
					"Continue" :
					"New Game";
			}
			
			ShowCanvas(titleCanvasGroup, b);
		}

		public void ChangeAllKeyPrompts(PlayerInput playerInput) {
			foreach (var kp in FindObjectsByType<KeyPrompt>(FindObjectsSortMode.None)) {
				if (playerInput.currentControlScheme.Contains("Gamepad")) {
					kp.ChangeToXbox();
				} else {
					kp.ChangeToPc();
				}
			}
		}

		private void ShowCanvas(CanvasGroup cg, bool b) {
			if (!cg) { return; }
			cg.alpha = b ? 1f : 0f;
			cg.interactable = b;
			cg.blocksRaycasts = false; // No mouse inputs
		}
	#endregion

	}
}

