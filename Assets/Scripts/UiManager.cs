using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private TextMeshProUGUI waveCountTextMesh, coinTextMesh, bubbleTextMesh, enemiesRemainingTextMesh, winCoinTextMesh;
	[SerializeField] private Image oxygenBarImage, timerBarImage;
	[SerializeField] private CanvasGroup shopCanvasGroup, winScreenCanvasGroup, oxygenBarCanvasGroup, timerCanvasGroup, hudCanvasGroup, titleCanvasGroup, gameOverCanvasGroup;
	[SerializeField] private TextMeshProUGUI restartGameOverTextMesh, startTextMesh;
	[SerializeField] private RectTransform _munnyWinSprite, depthArrow; 
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
	public void SetWaveCount(int waveCount, int maxWaveCount) {
		waveCountTextMesh.text = $"{waveCount}/{maxWaveCount}";
	}
	
	public void SetCoins(int coins) {
		coinTextMesh.text = coins.ToString();
	}
	
	public void SetWinCoins(int coins) {
		winCoinTextMesh.text = $"Made it out with {coins} coins!";
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

	public void SetDepth(float y, float maxY) {
		const float minY = -210f;
		const float arrowMinY = -300f;
		const float arrowMaxY = 300f;
		
		y = Mathf.Clamp(y, minY, maxY);
		var percent = (y - maxY) / (minY - maxY);
		var goalY = arrowMaxY - ((arrowMaxY - arrowMinY) * percent);

		depthArrow.anchoredPosition = new Vector2(depthArrow.anchoredPosition.x, goalY);
	}
	
	public void ShowWinScreen(bool b) {
		if (!b) {
			ShowCanvas(winScreenCanvasGroup, false);
		}
		else {
			DOVirtual.DelayedCall(2f, () => {
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

