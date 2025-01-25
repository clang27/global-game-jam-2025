using UnityEngine;

public class AiController : MonoBehaviour {

#region Dependencies
    [SerializeField] private LayerMask bubbleLayerMask;
    [SerializeField] private AiStyle style;
    [SerializeField] private float avoidEdgeDistance = 2f;
#endregion

#region Attributes
    public bool FlyingIn { get; set; }
    public AiStyle Type => style;
#endregion

#region Components
    private CharacterBehavior _character;
    private Transform _transform;
#endregion

#region Data
    private bool _boosted = false;
#endregion

#region Unity
    private void Awake() {
        _transform = transform;
        _character = GetComponent<CharacterBehavior>();
    }

    private void Start() {
        
    }

    private void Update() {

    }

    private void FixedUpdate() {
        if (FlyingIn) {
            var direction = (GameManager.Instance.Bubble.transform.position - _transform.position).normalized;
            _character.InputVector = direction;
        }
        else {
            var player = GameManager.Instance.Player;
            if (style == AiStyle.Swordfish) {
                var direction = (player.transform.position - _transform.position).normalized;
                var hits = Physics2D.BoxCastAll(_transform.position + (direction * avoidEdgeDistance), 
                    new Vector2(0.1f, 0.1f), 0f, Vector2.zero, 0f, bubbleLayerMask);
            
                _character.InputVector = (hits.Length == 0) ? Vector2.zero : direction;
            }
        }
    }

#endregion

#region Custom
    public void BoostStats() {
        if (_boosted) { return; }

        _boosted = true;
        _character.Acceleration *= 2f;
        _character.Deceleration *= 2f;
        _character.MaxSpeed *= 2f;
    }
    
    public void RevertStats() {
        if (!_boosted) { return; }

        _boosted = false;
        _character.Acceleration /= 2f;
        _character.Deceleration /= 2f;
        _character.MaxSpeed /= 2f;
    }

#endregion

}