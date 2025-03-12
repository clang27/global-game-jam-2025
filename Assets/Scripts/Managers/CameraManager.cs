using System;
using Unity.Cinemachine;
using UnityEngine;
using CameraState = Enums.CameraState;

namespace Managers {
	public class CameraManager : MonoBehaviour, IManager {

	#region Dependencies
		[SerializeField] private CinemachineCamera outsideCamera, insideCamera, jetpackCamera, titleCamera;
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
			titleCamera.Priority = 5;
			outsideCamera.Priority = 1;
			insideCamera.Priority = 1;
			jetpackCamera.Priority = 1;
			insideCamera.Target.TrackingTarget = null;
		}
		
		public void SceneChange(string sceneName) {}

		public void Switch(CameraState state, string source, Transform bubble = null, bool track = true) {
			Debug.Log($"Camera is switching to {state} from {source}");
			
			if (bubble) {
				insideCamera.transform.SetPositionAndRotation(new Vector3(bubble.position.x, bubble.position.y, -10f), Quaternion.identity);
				insideCamera.Target.TrackingTarget = track ? bubble : null;	
			}
			
			switch (state) {
				case CameraState.Title:
					titleCamera.Priority = 5;
					outsideCamera.Priority = 1;
					insideCamera.Priority = 1;
					jetpackCamera.Priority = 1;
					break;
				case CameraState.Bubble:
					titleCamera.Priority = 1;
					outsideCamera.Priority = 1;
					insideCamera.Priority = 5;
					jetpackCamera.Priority = 1;
					break;
				case CameraState.Ocean:
					titleCamera.Priority = 1;
					outsideCamera.Priority = 5;
					insideCamera.Priority = 1;
					jetpackCamera.Priority = 1;
					break;
				case CameraState.Jetpack:
					titleCamera.Priority = 1;
					outsideCamera.Priority = 1;
					insideCamera.Priority = 1;
					jetpackCamera.Priority = 5;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}
	#endregion

	}
}

