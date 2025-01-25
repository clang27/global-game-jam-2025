using DG.Tweening;
using UnityEngine;

public class AiController : MonoBehaviour {

#region Dependencies
    [SerializeField] private LayerMask bubbleLayerMask;
    [SerializeField] private AiStyle style;
    [SerializeField] private float avoidEdgeDistance = 2f;
#endregion

#region Attributes
    public bool FlyingIn { get; set; }
#endregion

#region Components
    private CharacterBehavior _character;
    private Transform _transform;
    private CharacterBehavior _playerCharacter;
#endregion

#region Data
#endregion

#region Unity
    private void Awake() {
        _transform = transform;
        _character = GetComponent<CharacterBehavior>();
    }

    private void Start() {
        _playerCharacter = FindFirstObjectByType<PlayerController>().GetComponent<CharacterBehavior>();
    }

    private void Update() {

    }

    private void FixedUpdate() {
        if (FlyingIn) { return; }
        if (style == AiStyle.Aggressive) {
            var direction = (_playerCharacter.transform.position - _transform.position).normalized;
            var hits = Physics2D.BoxCastAll(_transform.position + (direction * avoidEdgeDistance), 
                new Vector2(0.1f, 0.1f), 0f, Vector2.zero, 0f, bubbleLayerMask);
            
            _character.InputVector = (hits.Length == 0) ? Vector2.zero : direction;
        } else if (style == AiStyle.KeepAway) {
            var direction = (_transform.position - _playerCharacter.transform.position).normalized;
            var hits = Physics2D.BoxCastAll(_transform.position + (direction * avoidEdgeDistance), 
                new Vector2(0.1f, 0.1f), 0f, Vector2.zero, 0f, bubbleLayerMask);
            
            _character.InputVector = (hits.Length == 0) ? Vector2.zero : direction;
        }
    }

#endregion

#region Custom

#endregion

}