using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Managers {
	public class UiManager : MonoBehaviour, IManager {

	#region Dependencies
		[SerializeField] private CanvasGroup settingsCanvasGroup, pauseCanvasGroup, hudCanvasGroup, titleCanvasGroup, gameOverCanvasGroup;
		[SerializeField] private RectTransform _loadingScreen;
		[SerializeField] private Button startGameButton, resetGameButton;
	#endregion

	#region Attributes
		public static UiManager Instance { get; private set; }
	#endregion

	#region Components
		private Transform _transform;
		private EventSystem _eventSystem;
	#endregion

	#region Data
		// private Coroutine _marchOverCoroutine;
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
			ShowGameOver(false);
			ShowTitle(true);
			ShowHud(false);
		}
		
		public void ShowLoading(bool b, Action a) {
			_loadingScreen.DOKill();
			_loadingScreen.DOLocalMoveY(b ? -3080f : 0f, 0f);
			_loadingScreen.DOLocalMoveY(b ? 0f : 3080f, 1.5f).SetEase(Ease.Linear)
				.OnComplete(() => a());
		}
	
		public void ShowPause(bool b) {
			ShowCanvas(pauseCanvasGroup, b);
		}

		public void ShowHud(bool b) {
			ShowCanvas(hudCanvasGroup, b);
		}
		
		public void ShowSettings(bool b, Slider firstSlider) {
			if (b) {
				_eventSystem.SetSelectedGameObject(firstSlider.gameObject);	
			}
			
			ShowCanvas(settingsCanvasGroup, b);
		}
	
		public void ShowTitle(bool b) {
			if (b) {
				_eventSystem.SetSelectedGameObject(startGameButton.gameObject);	
			}
			
			ShowCanvas(titleCanvasGroup, b);
		}
	
		public void ShowGameOver(bool b) {
			if (b) {
				_eventSystem.SetSelectedGameObject(resetGameButton.gameObject);	
			}
			
			ShowCanvas(gameOverCanvasGroup, b);
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

