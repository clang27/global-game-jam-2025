using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using CameraState = Enums.CameraState;

public class CameraManager : MonoBehaviour {

#region Dependencies
	[SerializeField] private CinemachineCamera outsideCamera, titleCamera;
#endregion

#region Attributes
	public static CameraManager Instance { get; private set; }
#endregion

#region Components
	private Transform _transform;
#endregion

#region Data
	private List<CinemachineCamera> _bubbleCameras;
#endregion

#region Unity
    private void Awake() {
		_transform = transform;
		Instance = this;
    }
#endregion

#region Custom
	public void Init() {
		_bubbleCameras = FindObjectsByType<BubbleBehavior>(FindObjectsInactive.Include, FindObjectsSortMode.None)
			.Select(obj => obj.GetComponentInChildren<CinemachineCamera>())
			.ToList();

		Switch(CameraState.Title);
	}

	private void ChangeAllBubblePriorities(int priority) {
		foreach (var cam in _bubbleCameras) {
			cam.Priority = priority;
		}
	}
	
	private void ChangeBubblePriority(BubbleBehavior bubble, int priority) {
		var cam = _bubbleCameras
			.Find(b => bubble.GetComponentInChildren<CinemachineCamera>().Equals(b));

		cam.Priority = priority;
	}

	public void Switch(CameraState state, BubbleBehavior bubble = null) {
		switch (state) {
			case CameraState.Title:
				titleCamera.Priority = 5;
				outsideCamera.Priority = 1;
				ChangeAllBubblePriorities(1);
				break;
			case CameraState.Bubble:
				titleCamera.Priority = 1;
				outsideCamera.Priority = 1;
				ChangeBubblePriority(bubble, 5);
				break;
			case CameraState.Ocean:
				titleCamera.Priority = 1;
				outsideCamera.Priority = 5;
				ChangeAllBubblePriorities(1);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(state), state, null);
		}
	}
#endregion

}

