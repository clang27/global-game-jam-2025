using UnityEngine;

public class OxygenManager : MonoBehaviour {

#region Dependencies
    [SerializeField] private float decayRate = 0.005f;
#endregion

#region Attributes
    public static OxygenManager Instance { get; private set; }
    
#endregion

#region Components
    private Transform _transform;
#endregion

#region Data
    private float _oxygen = 1f;
    private bool _outOfBubble;
#endregion

#region Unity
    private void Awake() {
        Instance = this;
        
        _transform = transform;
    }

    private void Update() {
        
    }
	
    private void FixedUpdate() {
        if (_outOfBubble) {
            _oxygen -= decayRate;
            UiManager.Instance.SetOxygen(_oxygen);

            if (_oxygen <= 0f) {
                GameManager.Instance.GameOver();
            }
        }
        else {
            _oxygen += decayRate * 2f;
            UiManager.Instance.SetOxygen(_oxygen);

            if (_oxygen >= 1f) {
                _oxygen = 1f;
            }
        }
    }
#endregion

#region Custom
    public void Init() {
        _oxygen = 1f;
        UiManager.Instance.SetOxygen(_oxygen);
    }

    public void OutOfBubble() {
        _outOfBubble = true;
        UiManager.Instance.ShowOxygen(true);
    }
    
    public void InBubble() {
        _outOfBubble = false;
        UiManager.Instance.ShowOxygen(false);
    }
    
#endregion

}