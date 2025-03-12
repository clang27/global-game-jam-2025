using DG.Tweening;
using Enums;
using Managers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutScene : MonoBehaviour {

#region Dependencies
	[SerializeField] private float startTime;
	[SerializeField] private GameObject actor;
#endregion

#region Components
	private Transform _startPoint, _endPoint;
	private PlayableDirector _playableDirector;
#endregion

#region Unity
    private void Awake() {
	    _playableDirector = GetComponent<PlayableDirector>();
	    _startPoint = transform.GetChild(0);
	    _endPoint = transform.GetChild(1);
    }

    private void Start() {
	    // Have to dynamically add SFX audio source
	    var timelineAsset = (TimelineAsset) _playableDirector.playableAsset;
	    foreach (var asset in timelineAsset.outputs) {
		    var track = (TrackAsset)asset.sourceObject;
		    if (track.name.Equals("Audio Track")) {
			    _playableDirector.SetGenericBinding(track, AudioManager.Instance.SfxSource);    
		    }
	    }
    }
#endregion

#region Custom
	public void StartScene() {
		actor.transform.SetPositionAndRotation(_startPoint.position, Quaternion.identity);
		PlayerManager.Player.transform.SetPositionAndRotation(new Vector3(-100000f, -100000f), Quaternion.identity);
		
		UiManager.Instance.ShowHud(false);
		PlayerManager.Controller.Enabled = false;
		PlayerManager.Player.CutScene(startTime + 0.1f); // Munny continues previous animation until tween is done
		CutSceneManager.Instance.ShowBlackBars();
		
		PlayerManager.PlayerTransform.DOMove(_startPoint.position, startTime)
			.OnComplete(() => {
				PlayerManager.Player.gameObject.SetActive(false);
				actor.SetActive(true);
				
				_playableDirector.Play();
				_playableDirector.stopped += EndScene;
			});
	}
	
	private void EndScene(PlayableDirector pd) {
		GameManager.Instance.GameState = GameState.Playing;
		UiManager.Instance.ShowHud(true);
		_playableDirector.stopped -= EndScene;
		
		PlayerManager.Player.gameObject.SetActive(true);
		Debug.Log($"Settings position to {_endPoint.position}");
		PlayerManager.PlayerTransform.SetPositionAndRotation(_endPoint.position, quaternion.identity);
		CutSceneManager.Instance.HideBlackBars();
		
		actor.SetActive(false);
		PlayerManager.Controller.Enabled = true;
	}
#endregion

}

