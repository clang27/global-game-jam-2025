using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private CinemachineCamera inBubbleCamera, outBubbleCamera, titleCamera;
#endregion

#region Attributes
	public static GameManager Instance { get; private set; }
	public BubbleBehavior Bubble { get; private set; }
	public CharacterBehavior Player { get; private set; }
	public GameState GameState { get; private set; } = GameState.Start;
#endregion

#region Components
	//private BubbleBehavior _bubbleBehavior;
	//private CharacterBehavior _playerBehavior;
#endregion

#region Data
	private bool _gameOverCooldown = false;
#endregion

#region Unity
    private void Awake() {
	    Instance = this;

	    DOTween.Init(false, false, LogBehaviour. Default)
		    .SetCapacity(100, 20);
        
	    Bubble = GameObject.FindGameObjectWithTag("Bubble").GetComponent<BubbleBehavior>();
	    Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehavior>();
    }

    private void Start() {
	    Init();
    }

    private void Update() {
	    if (Input.GetKeyDown(KeyCode.Escape)) {
		    Application.Quit();
	    }
    }
#endregion

#region Custom
	private void Init() {
		GameState = GameState.Start;
		_gameOverCooldown = false;
		
		Bubble.enabled = false;

		WaveManager.Instance.enabled = false;
		CoinManager.Instance.enabled = false;
		OxygenManager.Instance.enabled = false;
		ShopManager.Instance.enabled = false;
		
		TitleCamera();
		
		Bubble.Init();
		Player.Init();
		
		WaveManager.Instance.Init();
		EnemyManager.Instance.Init();
		CoinManager.Instance.Init();
		OxygenManager.Instance.Init();
		
		UiManager.Instance.ShowWinScreen(false);
		UiManager.Instance.ShowGameOver(false);
		UiManager.Instance.ShowTitle(true);
		UiManager.Instance.ShowTimer(false);
		UiManager.Instance.ShowOxygen(false);
		UiManager.Instance.ShowHud(false);
	}

	public void ResetGame() {
		if (!_gameOverCooldown) {
			DOTween.KillAll();
			Init();
		}
	}

	public void StartGame() {
		GameState = GameState.Wave;
		
		Bubble.enabled = true;
		Player.Controllable = true;
		
		PlayerInBubble();
		
		Bubble.StartMoving();
		WaveManager.Instance.enabled = true;
		CoinManager.Instance.enabled = true;
		OxygenManager.Instance.enabled = true;
		
		WaveManager.Instance.StartWave();
		
		UiManager.Instance.ShowWinScreen(false);
		UiManager.Instance.ShowGameOver(false);
		UiManager.Instance.ShowTimer(false);
		UiManager.Instance.ShowTitle(false);
		UiManager.Instance.ShowOxygen(false);
		UiManager.Instance.ShowHud(true);
	}

	public void WaveDone() {
		Debug.Log("Wave done!");
		
		WaveManager.Instance.enabled = false;
		if (WaveManager.Instance.CompletedWaves) {
			GameOver(true);
		} else {
			GameState = GameState.Shop;

			ShopManager.Instance.enabled = true;
			ShopManager.Instance.StartShop();
		}
	}

	public void ShopDone() {
		Debug.Log("Shop done!");
		GameState = GameState.Wave;
		ShopManager.Instance.enabled = false;
		ShopManager.Instance.StopShop();
		
		WaveManager.Instance.enabled = true;
		WaveManager.Instance.StartWave();
	}

	public void GameOver(bool won) {
		GameState = GameState.GameOver;
		_gameOverCooldown = true;
		DOVirtual.DelayedCall(1f, () => _gameOverCooldown = false);

		Player.Controllable = false;
		Bubble.enabled = false;

		WaveManager.Instance.enabled = false;
		CoinManager.Instance.enabled = false;
		OxygenManager.Instance.enabled = false;
		
		PlayerOutOfBubble();
		
		UiManager.Instance.ShowGameOver(!won);
		UiManager.Instance.ShowWinScreen(won);
		UiManager.Instance.ShowTitle(false);
		UiManager.Instance.ShowOxygen(false);
		UiManager.Instance.ShowTimer(false);
		UiManager.Instance.ShowHud(false);
	}

	public void TitleCamera() {
		titleCamera.Priority = 3;
		inBubbleCamera.Priority = 2;
		outBubbleCamera.Priority = 1;
	}

	public void PlayerOutOfBubble() {
		OxygenManager.Instance.OutOfBubble();
		titleCamera.Priority = 1;
		inBubbleCamera.Priority = 2;
		outBubbleCamera.Priority = 3;
	}
	
	public void PlayerInBubble() {
		OxygenManager.Instance.InBubble();
		titleCamera.Priority = 1;
		inBubbleCamera.Priority = 3;
		outBubbleCamera.Priority = 2;
	}
#endregion

}

