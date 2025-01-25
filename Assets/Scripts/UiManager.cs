using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private TextMeshProUGUI waveCountTextMesh, coinTextMesh;
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
#endregion

}

