using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour {

#region Dependencies
	// [SerializeField] private GameObject[] Entries;
#endregion

#region Attributes
	public static GameManager Instance { get; private set; }
#endregion

#region Components
	private Transform _transform;
#endregion

#region Data
	// private Coroutine _marchOverCoroutine;
#endregion

#region Unity
    private void Awake() {
	    Instance = this;
	    
		_transform = transform;
    }

    private void Start() {
	    DOTween.Init(false, false, LogBehaviour. Default)
		    .SetCapacity(100, 20);
    }

    private void Update() {
        
    }
	
	private void FixedUpdate() {
        
    }
#endregion

#region Custom
	private void DoFunction() {
		
	}
#endregion

}

