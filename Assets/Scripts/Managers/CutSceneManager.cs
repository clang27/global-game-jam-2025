using System.Linq;
using DG.Tweening;
using Enums;
using Managers;
using UnityEngine;

public class CutSceneManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private RectTransform topBlackBar, bottomBlackBar;
#endregion

#region Attributes
	public static CutSceneManager Instance { get; private set; }
#endregion

#region Unity
    private void Awake() {
	    Instance = this;
    }
#endregion

#region Custom
	public void GameOver() {
		ShowBlackBars();
		CameraManager.Instance.GameOver();
		
		PlayerManager.Controller.InUi = true;
		PlayerManager.Player.StopMoving();
		
		AudioManager.Instance.StopSong();

		PlayerManager.PlayerTransform.DORotate(new Vector3(0f, 0f, 360f * 5), 3f, RotateMode.FastBeyond360);
		PlayerManager.PlayerTransform.DOMove(PlayerManager.PlayerTransform.position + new Vector3(0f, -30f, 0f), 3f)
			.OnComplete(() => GameManager.Instance.ResetGame());
	}
	
	public void PlayScene(string n) {
		GameManager.Instance.GameState = GameState.CutScene;
		var scene = FindObjectsByType<CutScene>(FindObjectsSortMode.None)
			.First(scene => scene.gameObject.name.Equals(n));

		scene.StartScene();
	}

	public void ShowBlackBars() {
		topBlackBar.DOKill();
		bottomBlackBar.DOKill();
		
		topBlackBar.DOAnchorPos(Vector2.zero, 1f);
		bottomBlackBar.DOAnchorPos(Vector2.zero, 1f);
	}
	
	public void HideBlackBars() {
		topBlackBar.DOKill();
		bottomBlackBar.DOKill();
		
		topBlackBar.DOAnchorPos(new Vector2(0f, 101f), 1f);
		bottomBlackBar.DOAnchorPos(new Vector2(0f, -101f), 1f);
	}
#endregion

}

