using DG.Tweening;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private TextMeshProUGUI winCoinTextMesh;
	[SerializeField] private CanvasGroup winScreenCanvasGroup, pauseCanvasGroup, hudCanvasGroup, titleCanvasGroup, gameOverCanvasGroup;
	[SerializeField] private TextMeshProUGUI restartGameOverTextMesh, startTextMesh;
	[SerializeField] private RectTransform _munnyWinSprite; 
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
		ShowWinScreen(false);
		ShowGameOver(false);
		ShowTitle(true);
		ShowHud(false);

		startTextMesh.DOFade(1f, 0f);
		restartGameOverTextMesh.DOFade(1f, 0f);
		winCoinTextMesh.transform.DOScale(1f, 0f);
		
		startTextMesh.DOFade(0f, 0.3f).SetLoops(-1, LoopType.Yoyo);
		restartGameOverTextMesh.DOFade(0f, 0.3f).SetLoops(-1, LoopType.Yoyo);
		winCoinTextMesh.transform.DOScale(1.2f, 0.8f).SetLoops(-1, LoopType.Yoyo);
	}

	
	public void SetWinCoins(int coins) {
		winCoinTextMesh.text = $"Made it out with {coins} coins!";
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
	
	public void ShowWinScreen(bool b) {
		if (!b) {
			ShowCanvas(winScreenCanvasGroup, false);
		}
		else {
			DOVirtual.DelayedCall(8f, () => {
				_munnyWinSprite.DOMoveY(560f, 1f);
				ShowCanvas(winScreenCanvasGroup, true);
			});
		}
	}

	private void ShowCanvas(CanvasGroup cg, bool b) {
		if (!cg) { return; }
		cg.alpha = b ? 1f : 0f;
		cg.interactable = b;
		cg.blocksRaycasts = b;
	}
#endregion

}

