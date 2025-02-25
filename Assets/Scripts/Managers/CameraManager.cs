using System;
using Unity.Cinemachine;
using UnityEngine;
using CameraState = Enums.CameraState;

namespace Managers {
	public class CameraManager : MonoBehaviour, IManager {

	#region Dependencies
		[SerializeField] private CinemachineCamera outsideCamera, insideCamera, titleCamera;
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
				: new Vector2(-167.8f, -259.92f);
			
			titleCamera.transform.SetPositionAndRotation(new Vector3(startPosition.x, startPosition.y, -10f), Quaternion.identity);
			titleCamera.Priority = 5;
			outsideCamera.Priority = 1;
			insideCamera.Priority = 1;
			insideCamera.Target.TrackingTarget = null;
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

