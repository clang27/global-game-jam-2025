using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AiController : MonoBehaviour {

#region Dependencies
    [SerializeField] private LayerMask bubbleLayerMask;
    [SerializeField] private AiStyle style;
    [SerializeField] private float avoidEdgeDistance = 2f;
    [SerializeField] private float knockback = 2f;
    [SerializeField] private float stunTime = 0.25f;
#endregion

#region Attributes
    public bool FlyingIn { get; set; }
    public AiStyle Type => style;
    public float Knockback => knockback;
    public float StunTime => stunTime;
#endregion

#region Components
    private CharacterBehavior _character;
    private Transform _transform;
#endregion

#region Data
    private bool _boosted = false;
    public Vector2 Direction { get; private set; }
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
            Direction = (GameManager.Instance.Bubble.transform.position - _transform.position).normalized;
            _character.InputVector = Direction;
        }
        else {
            var player = GameManager.Instance.Player;
            if (style == AiStyle.Swordfish) {
                Direction = (player.transform.position - _transform.position).normalized;
            
                _character.InputVector = (NoBubbleAhead()) ? Vector2.zero : Direction;
            } else if (style == AiStyle.Jellyfish) {
                if (NoBubbleAhead()) {
                    Direction = Vector2.Perpendicular(Direction).normalized;
                }

                _character.InputVector = Direction;
            }
        }

        if (!_character.Spinning) {
            transform.up = Direction;    
        }
    }

#endregion

#region Custom

    private bool NoBubbleAhead() {
        return Physics2D.BoxCastAll(
            (Vector2)_transform.position + (Direction * avoidEdgeDistance), 
            new Vector2(0.1f, 0.1f), 0f, Vector2.zero, 0f, bubbleLayerMask).Length == 0;
    }
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