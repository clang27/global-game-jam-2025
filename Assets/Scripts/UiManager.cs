using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private TextMeshProUGUI waveCountTextMesh, coinTextMesh, bubbleTextMesh, enemiesRemainingTextMesh;
	[SerializeField] private Image oxygenBarImage, timerBarImage;
	[SerializeField] private CanvasGroup shopCanvasGroup, winScreenCanvasGroup, oxygenBarCanvasGroup, timerCanvasGroup, hudCanvasGroup, titleCanvasGroup, gameOverCanvasGroup;
	[SerializeField] private TextMeshProUGUI restartWinTextMesh, restartGameOverTextMesh, startTextMesh;
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
		ShowOxygen(false);
		ShowHud(false);

		startTextMesh.DOFade(1f, 0f);
		restartGameOverTextMesh.DOFade(1f, 0f);
		restartWinTextMesh.DOFade(1f, 0f);
		
		startTextMesh.DOFade(0f, 0.3f).SetLoops(-1, LoopType.Yoyo);
		restartGameOverTextMesh.DOFade(0f, 0.3f).SetLoops(-1, LoopType.Yoyo);
		restartWinTextMesh.DOFade(0f, 0.3f).SetLoops(-1, LoopType.Yoyo);
	}
	public void SetWaveCount(int waveCount, int maxWaveCount) {
		waveCountTextMesh.text = $"{waveCount}/{maxWaveCount}";
	}
	
	public void SetCoins(int coins) {
		coinTextMesh.text = coins.ToString();
	}

	public void SetBubble(float percentage) {
		bubbleTextMesh.text = $"{percentage*100f:F0}%";
	}

	public void SetOxygen(float oxygen) {
		oxygenBarImage.fillAmount = oxygen;
	}
	
	public void SetEnemiesRemaining(int num) {
		enemiesRemainingTextMesh.text = num.ToString();
	}
	
	public void SetTimer(float amount) {
		timerBarImage.fillAmount = amount;
	}

	public void ShowHud(bool b) {
		ShowCanvas(hudCanvasGroup, b);
	}
	
	public void ShowShopItems(bool b) {
		ShowCanvas(shopCanvasGroup, b);
	}
	
	public void ShowTitle(bool b) {
		ShowCanvas(titleCanvasGroup, b);
	}
	
	public void ShowOxygen(bool b) {
		ShowCanvas(oxygenBarCanvasGroup, b);
	}
	public void ShowTimer(bool b) {
		ShowCanvas(timerCanvasGroup, b);
	}
	
	public void ShowGameOver(bool b) {
		ShowCanvas(gameOverCanvasGroup, b);
	}
	
	public void ShowWinScreen(bool b) {
		ShowCanvas(winScreenCanvasGroup, b);
	}

	private void ShowCanvas(CanvasGroup cg, bool b) {
		if (!cg) { return; }
		cg.alpha = b ? 1f : 0f;
		cg.interactable = b;
		cg.blocksRaycasts = b;
	}
#endregion

}

