using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private CinemachineCamera outBubbleCamera, titleCamera;
#endregion

#region Attributes
	public static GameManager Instance { get; private set; }
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
		AudioManager.Instance.PlayGameTheme();
		
		GameState = GameState.Start;
		_gameOverCooldown = false;
		
		CoinManager.Instance.enabled = false;
		OxygenManager.Instance.enabled = false;
		IslandManager.Instance.enabled = false;
		
		TitleCamera();
		outBubbleCamera.Follow = Player.transform;
		
		Player.Init();
		
		BubbleManager.Instance.Init();
		TimeManager.Instance.Init();
		CoinManager.Instance.Init();
		OxygenManager.Instance.Init();
		UiManager.Instance.Init();
		IslandManager.Instance.Init();
	}

	public void ResetGame() {
		if (!_gameOverCooldown) {
			DOTween.KillAll();
			Init();
		}
	}

	public void StartGame() {
		GameState = GameState.Wave;
		
		BubbleManager.Instance.StartGame();
		TimeManager.Instance.StartGame();
		
		CoinManager.Instance.enabled = true;
		OxygenManager.Instance.enabled = true;
		
		UiManager.Instance.ShowWinScreen(false);
		UiManager.Instance.ShowGameOver(false);
		UiManager.Instance.ShowTitle(false);
		UiManager.Instance.ShowOxygen(true);
		UiManager.Instance.ShowHud(true);
		
		Player.Controllable = true;
		PlayerOutOfBubble();
	}

	// public void WaveDone() {
	// 	Debug.Log("Wave done!");
	// 	
	// 	if (GameState == GameState.GameOver) { return; }
	// 	
	// 	//WaveManager.Instance.enabled = false;
	// 	if (WaveManager.Instance.CompletedWaves) {
	// 		GameOver(true);
	// 	} else {
	// 		GameState = GameState.Shop;
	// 		AudioManager.Instance.PlayShopTheme();
	//
	// 		ShopManager.Instance.enabled = true;
	// 		ShopManager.Instance.StartShop();
	// 	}
	// }
	//
	// public void ShopDone() {
	// 	Debug.Log("Shop done!");
	// 	GameState = GameState.Wave;
	// 	AudioManager.Instance.PlayGameTheme();
	// 	
	// 	ShopManager.Instance.enabled = false;
	// 	ShopManager.Instance.StopShop();
	// 	
	// 	WaveManager.Instance.enabled = true;
	// 	WaveManager.Instance.StartWave();
	// }

	public void GameOver(bool won) {
		GameState = won ? GameState.Win : GameState.GameOver;
		
		_gameOverCooldown = true;
		DOVirtual.DelayedCall(1f, () => _gameOverCooldown = false);

		if (won) {
			UiManager.Instance.SetWinCoins(CoinManager.Instance.Coins);
		}
		
		Player.Controllable = false;
		Player.JetpackOff();

		TimeManager.Instance.enabled = false;
		CoinManager.Instance.enabled = false;
		OxygenManager.Instance.enabled = false;
		
		PlayerOutOfBubble();
		outBubbleCamera.Follow = null;
		
		UiManager.Instance.ShowGameOver(!won);
		UiManager.Instance.ShowWinScreen(won);
		UiManager.Instance.ShowTitle(false);
		UiManager.Instance.ShowOxygen(false);
		UiManager.Instance.ShowTimer(false);
		UiManager.Instance.ShowHud(false);
	}

	public void TitleCamera() {
		titleCamera.Priority = 3;
		outBubbleCamera.Priority = 1;
	}

	public void PlayerOutOfBubble() {
		OxygenManager.Instance.OutOfBubble();
		titleCamera.Priority = 1;
		outBubbleCamera.Priority = 3;
	}
	
	public void PlayerInBubble(CinemachineCamera bubbleCam) {
		OxygenManager.Instance.InBubble();
		titleCamera.Priority = 1;
		bubbleCam.Priority = 3;
		outBubbleCamera.Priority = 2;
	}
#endregion

}

