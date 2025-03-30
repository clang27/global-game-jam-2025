using System.IO;
using Managers;
using UnityEngine;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour {

#region Dependencies
	[SerializeField] private string videoFileName;
#endregion

#region Components
	private VideoPlayer _videoPlayer;
	private float _timer;
#endregion

#region Unity
    private void Awake() {
	    _videoPlayer = GetComponent<VideoPlayer>();
	    
	    var path = Path.Combine(Application.streamingAssetsPath, videoFileName);
	    _videoPlayer.url = path;
	    _videoPlayer.loopPointReached += Stop;
    }

    private void Update() {
	    _timer += Time.deltaTime;

	    if (_timer > 2f) {
		    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X)) {
			    Stop(_videoPlayer);
		    }
	    }
    }
#endregion

#region Custom
	public void Play() {
		if (!_videoPlayer) { Debug.LogError("No Video Player found!"); return; }
		
		Debug.Log($"Playing {_videoPlayer.url}");
		_timer = 0f;
		_videoPlayer.Play();
	}

	private static void Stop(VideoPlayer videoPlayer) {
		videoPlayer.Stop();
		videoPlayer.gameObject.SetActive(false);
		
		GameManager.Instance.LoadBeginning("Ship");
	}
#endregion

}

