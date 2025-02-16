using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Managers {
	public class UiManager : MonoBehaviour, IManager {

	#region Dependencies
		[SerializeField] private CanvasGroup pauseCanvasGroup, hudCanvasGroup, titleCanvasGroup, gameOverCanvasGroup;
		[SerializeField] private TextMeshProUGUI restartGameOverTextMesh;
		[SerializeField] private RectTransform _loadingScreen; 
	#endregion

	#region Attributes
		public static UiManager Instance { get; private set; }
	#endregion

	#region Components
		private Transform _transform;
	#endregion

	#region Data
		// private Coroutine _marchOverCoroutine;
	#endregion

	#region Unity
		private void Awake() {
			Instance = this;
			_transform = transform;
		}
	#endregion

	#region Custom
		public void Init() {
			ShowGameOver(false);
			ShowTitle(true);
			ShowHud(false);
			
			restartGameOverTextMesh.DOFade(1f, 0f);
			restartGameOverTextMesh.DOFade(0f, 0.3f).SetLoops(-1, LoopType.Yoyo);
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
	
		public void ShowTitle(bool b) {
			ShowCanvas(titleCanvasGroup, b);
		}
	
		public void ShowGameOver(bool b) {
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

