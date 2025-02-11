using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using UnityEngine;

public class GameManager : MonoBehaviour {

#region Attributes
	public static GameManager Instance { get; private set; }
	public List<BubbleBehavior> Bubbles { get; private set; }
	public CharacterBehavior Player { get; private set; }
	public GameState GameState { get; private set; } = GameState.Start;
	public bool PlayerOnBubble => Bubbles.Any(bubble => bubble.OnBubble(Player));
	public BubbleBehavior BubblePlayerIsOn => Bubbles.First(bubble => bubble.OnBubble(Player));
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
		    .SetCapacity(1000, 200);

	    Bubbles = FindObjectsByType<BubbleBehavior>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
	    Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehavior>();
    }

    private void Start() {
	    Init();
    }
    
    private void OnDestroy() {
	    DOTween.KillAll();
    }
#endregion

#region Custom
	private void Init() {
		AudioManager.Instance.PlayGameTheme();
		
		GameState = GameState.Start;
		_gameOverCooldown = false;
		
		CoinManager.Instance.enabled = false;
		OxygenManager.Instance.enabled = false;
		
		Player.Init();
		Player.GetComponent<PlayerController>().GoToUiControls();
		
		foreach (var bubble in Bubbles) {
			bubble.enabled = false;
			bubble.Init();
		}

		CoinManager.Instance.Init();
		OxygenManager.Instance.Init();
		UiManager.Instance.Init();
		CameraManager.Instance.Init();
	}

	public void ResetGame() {
		if (!_gameOverCooldown) {
			DOTween.KillAll();
			Init();
		}
	}

	public void StartGame() {
		GameState = GameState.Playing;
		
		CameraManager.Instance.Switch(CameraState.Ocean);
		
		foreach (var bubble in Bubbles) {
			bubble.enabled = true;
		}
		Player.GetComponent<PlayerController>().GoToPlayerControls();
		
		CoinManager.Instance.enabled = true;
		OxygenManager.Instance.enabled = true;
		
		UiManager.Instance.ShowWinScreen(false);
		UiManager.Instance.ShowGameOver(false);
		UiManager.Instance.ShowTitle(false);
		UiManager.Instance.ShowHud(true);
	}

	public void GameOver(bool won) {
		GameState = won ? GameState.Win : GameState.GameOver;
		
		_gameOverCooldown = true;
		DOVirtual.DelayedCall(1f, () => _gameOverCooldown = false);

		if (won) {
			UiManager.Instance.SetWinCoins(CoinManager.Instance.Coins);
		}
		
		Player.GetComponent<PlayerController>().GoToUiControls();
		foreach (var bubble in Bubbles) {
			bubble.enabled = false;
		}
		
		CoinManager.Instance.enabled = false;
		OxygenManager.Instance.enabled = false;
		
		UiManager.Instance.ShowGameOver(!won);
		UiManager.Instance.ShowWinScreen(won);
		UiManager.Instance.ShowTitle(false);
		UiManager.Instance.ShowHud(false);
	}
	public void Pause() {
		GameState = GameState.Paused;
		Player.GetComponent<PlayerController>().GoToUiControls();
		
		CoinManager.Instance.enabled = false;
		OxygenManager.Instance.enabled = false;
		
		UiManager.Instance.ShowPause(true);
		UiManager.Instance.ShowHud(false);
	}
	
	public void Unpause() {
		GameState = GameState.Playing;
		Player.GetComponent<PlayerController>().GoToPlayerControls();
		
		CoinManager.Instance.enabled = true;
		OxygenManager.Instance.enabled = true;
		
		UiManager.Instance.ShowPause(false);
		UiManager.Instance.ShowHud(true);
	}
#endregion

}

