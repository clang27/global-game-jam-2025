using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private TextMeshProUGUI waveCountTextMesh, coinTextMesh, bubbleTextMesh;
	[SerializeField] private Image oxygenBarImage;
	[SerializeField] private CanvasGroup oxygenBarCanvasGroup, hudCanvasGroup, titleCanvasGroup, gameOverCanvasGroup;
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
	public void SetWaveCount(int waveCount) {
		waveCountTextMesh.text = waveCount.ToString();
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

	public void ShowHud(bool b) {
		ShowCanvas(hudCanvasGroup, b);
	}
	
	public void ShowTitle(bool b) {
		ShowCanvas(titleCanvasGroup, b);
	}
	
	public void ShowOxygen(bool b) {
		ShowCanvas(oxygenBarCanvasGroup, b);
	}
	
	public void ShowGameOver(bool b) {
		ShowCanvas(gameOverCanvasGroup, b);
	}

	private void ShowCanvas(CanvasGroup cg, bool b) {
		if (!cg) { return; }
		cg.alpha = b ? 1f : 0f;
		cg.interactable = b;
		cg.blocksRaycasts = b;
	}
#endregion

}

