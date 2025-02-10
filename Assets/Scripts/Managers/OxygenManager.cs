using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour {

#region Dependencies
    [Header("Variables")]
    [SerializeField] private float decayRate = 0.005f;
    [SerializeField] private float growRate = 0.005f;

    [Header("UI")] 
    [SerializeField] private Image barUnlockImage;
    [SerializeField] private Image oxygenImage, quadrantsImage;
#endregion

#region Attributes
    public static OxygenManager Instance { get; private set; }

    public float OxygenPercent {
        get => _oxygenPercent;
        set {
            _oxygenPercent = value;
            
            var maxAmount = _upgrades switch {
                0 => 0.25f,
                1 => 0.5f,
                2 => 0.75f,
                3 => 1f,
                _ => 1f
            };

            oxygenImage.fillAmount = maxAmount * _oxygenPercent;
        }
    }
    
    public int Upgrades {
        get => _upgrades;
        set {
            _upgrades = value;

            quadrantsImage.fillAmount = _upgrades switch {
                0 => 1f,
                1 => 0.7f,
                2 => 0.4f,
                3 => 0.1f,
                _ => 0.1f
            };
            
            barUnlockImage.fillAmount = _upgrades switch {
                0 => 0.25f,
                1 => 0.5f,
                2 => 0.75f,
                3 => 1f,
                _ => 1f
            };
        }
    }
#endregion

#region Components
    private Transform _transform;
#endregion

#region Data
    private float _oxygenPercent = 1f;
    private int _upgrades;
    private bool _outOfBubble;
#endregion

#region Unity
    private void Awake() {
        Instance = this;
        
        _transform = transform;
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
        var trueRate = amount > 0f ? amount * (_upgrades + 1) : amount / (_upgrades + 1);
        
        OxygenPercent += trueRate;

        if (OxygenPercent <= 0f) {
            OxygenPercent = 0f;
            GameManager.Instance.GameOver(false);
        } else if (OxygenPercent >= 1f) {
            OxygenPercent = 1f;
        }
    }

    public void Init() {
        Upgrades = 1;
        OxygenPercent = 1f;
        OutOfBubble();
    }

    public void OutOfBubble() {
        _outOfBubble = true;
    }
    
    public void InBubble() {
        _outOfBubble = false;
    }
    
#endregion

}