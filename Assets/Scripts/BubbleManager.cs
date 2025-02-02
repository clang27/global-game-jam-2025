using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BubbleManager : MonoBehaviour {

#region Dependencies
	// [SerializeField] private GameObject[] Entries;
#endregion

#region Attributes
	public static BubbleManager Instance { get; private set; }
	public BubbleBehavior Bubble { get; set; }
#endregion

#region Components
	private Transform _transform;
#endregion

#region Data
	private List<BubbleBehavior> _bubbles;
#endregion

#region Unity
    private void Awake() {
	    Instance = this;
		_transform = transform;
		
		_bubbles = FindObjectsByType<BubbleBehavior>(FindObjectsSortMode.None).ToList();
    }

    private void Start() {
	    
    }

    private void Update() {
        
    }
	
	private void FixedUpdate() {
        
    }
#endregion

#region Custom
	public void Init() {
		foreach (var bubble in _bubbles) {
			bubble.Init();
		}
	}
	
	public void StartGame() {
		foreach (var bubble in _bubbles) {
			bubble.enabled = true;
			bubble.StartMoving();
		}
	}
#endregion

}

