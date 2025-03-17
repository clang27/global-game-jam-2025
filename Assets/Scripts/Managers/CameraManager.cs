using System;
using Unity.Cinemachine;
using UnityEngine;
using CameraState = Enums.CameraState;

namespace Managers {
	public class CameraManager : MonoBehaviour, IManager {

	#region Dependencies
		[SerializeField] private CinemachineCamera outsideCamera, insideCamera, jetpackCamera, titleCamera, gameOverCamera;
	#endregion

	#region Attributes
		public static CameraManager Instance { get; private set; }
	#endregion

	#region Unity
		private void Awake() {
			Instance = this;
		}
	#endregion

	#region Custom
		public void Init() {
			var startPosition = SaveManager.Instance.HasASaveFile()
				? SaveManager.Instance.GetStartPosition()
				: new Vector2(-250f, -250f);
			
			titleCamera.transform.SetPositionAndRotation(new Vector3(startPosition.x, startPosition.y, -10f), Quaternion.identity);
			Switch(CameraState.Title, "CameraManager");
			insideCamera.Target.TrackingTarget = null;
		}
		
		public void SceneChange(string sceneName) {}

		public void GameOver() {
			var pos = PlayerManager.PlayerTransform.transform.position;
			gameOverCamera.transform.SetPositionAndRotation(new Vector3(pos.x, pos.y-5f, -10f), Quaternion.identity);
			Switch(CameraState.GameOver, "CameraManager");
		}

		public void Switch(CameraState state, string source, Transform bubble = null, bool track = true) {
			Debug.Log($"Camera is switching to {state} from {source}");
			
			if (bubble) {
				insideCamera.transform.SetPositionAndRotation(new Vector3(bubble.position.x, bubble.position.y, -10f), Quaternion.identity);
				insideCamera.Target.TrackingTarget = track ? bubble : null;	
			}
			
			titleCamera.Priority = 1;
			outsideCamera.Priority = 1;
			insideCamera.Priority = 1;
			jetpackCamera.Priority = 1;
			gameOverCamera.Priority = 1;
			
			switch (state) {
				case CameraState.Title:
					titleCamera.Priority = 5;
					break;
				case CameraState.Bubble:
					insideCamera.Priority = 5;
					break;
				case CameraState.Ocean:
					outsideCamera.Priority = 5;
					break;
				case CameraState.Jetpack:
					jetpackCamera.Priority = 5;
					break;
				case CameraState.GameOver:
					gameOverCamera.Priority = 5;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}
	#endregion

	}
}

