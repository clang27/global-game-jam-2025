using System;
using Unity.Cinemachine;
using UnityEngine;
using CameraState = Enums.CameraState;

namespace Managers {
	public class CameraManager : MonoBehaviour, IManager {

	#region Dependencies
		[SerializeField] private CinemachineCamera outsideCamera, insideCamera, titleCamera;
		[SerializeField] private float zoomSpeed;
	#endregion

	#region Attributes
		public static CameraManager Instance { get; private set; }
	#endregion
		
	#region Data
		private float _defaultZoom = 10f;	
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
			insideCamera.Target.TrackingTarget = null;
		}
		
		public void SceneChange(string sceneName) {}

		public void UpdateZoom(Vector2 velocity) {
			var zoomOut = Mathf.Sqrt(velocity.sqrMagnitude) / 2f;
			
			outsideCamera.Lens.OrthographicSize = Mathf.Lerp(outsideCamera.Lens.OrthographicSize, _defaultZoom + zoomOut, 
				zoomSpeed * Time.deltaTime);
		}

		public void Switch(CameraState state, Transform bubble = null, bool track = true) {
			if (bubble) {
				insideCamera.transform.SetPositionAndRotation(new Vector3(bubble.position.x, bubble.position.y, -10f), Quaternion.identity);
				insideCamera.Target.TrackingTarget = track ? bubble : null;	
			}
			
			switch (state) {
				case CameraState.Title:
					titleCamera.Priority = 5;
					outsideCamera.Priority = 1;
					insideCamera.Priority = 1;
					break;
				case CameraState.Bubble:
					titleCamera.Priority = 1;
					outsideCamera.Priority = 1;
					insideCamera.Priority = 5;
					break;
				case CameraState.Ocean:
					titleCamera.Priority = 1;
					outsideCamera.Priority = 5;
					insideCamera.Priority = 1;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}
	#endregion

	}
}

