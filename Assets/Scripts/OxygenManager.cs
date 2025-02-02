using UnityEngine;

public class OxygenManager : MonoBehaviour {

#region Dependencies
    [SerializeField] private float decayRate = 0.005f;
    [SerializeField] private float growRate = 0.005f;
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
            AddOxygen(-decayRate);
        }
        else {
            AddOxygen(growRate);
        }
    }
#endregion

#region Custom
    public void AddOxygen(float amount) {
        _oxygen += amount;
        UiManager.Instance.SetOxygen(_oxygen);

        if (_oxygen <= 0f) {
            _oxygen = 0f;
            GameManager.Instance.GameOver(false);
        } else if (_oxygen >= 1f) {
            _oxygen = 1f;
        }
    }
    public void Init() {
        _oxygen = 1f;
        UiManager.Instance.SetOxygen(_oxygen);
        UiManager.Instance.ShowOxygen(false);
    }

    public void OutOfBubble() {
        _outOfBubble = true;
    }
    
    public void InBubble() {
        _outOfBubble = false;
    }
    
#endregion

}