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

