using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiController : MonoBehaviour {

#region Dependencies
    [SerializeField] private AudioClip attackSound;
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
    private Animator _animator;
    private Transform _transform;
#endregion

#region Data
    private bool _boosted = false;
    private bool _reachedGoal = false;
    private bool _attackCooldown = false;
    private Vector2 _offset;
    public Vector2 Direction { get; private set; }
#endregion

#region Unity
    private void Awake() {
        _transform = transform;
        _character = GetComponent<CharacterBehavior>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        if (FlyingIn) {
            Direction = (GameManager.Instance.Bubble.transform.position - _transform.position).normalized;
            _character.InputVector = Direction;
        }
        else {
            var player = GameManager.Instance.Player;
            var bubble = GameManager.Instance.Bubble;
            
            if (style == AiStyle.Swordfish) {
                Direction = Vector2.Lerp(Direction, (player.transform.position - _transform.position).normalized, 0.015f);
                
                _character.InputVector = (NoBubbleAhead()) ? Vector2.zero : Direction;    
            } else if (style == AiStyle.Jellyfish) {
                if (NoBubbleAhead()) {
                    Direction = Vector2.Perpendicular(Direction).normalized;
                }

                _character.InputVector = Direction;
            } else if (style == AiStyle.Pufferfish) {
                if (!_reachedGoal) {
                    var position = bubble.transform.position + (Vector3) _offset;
                    var distanceToGoal = Mathf.Abs(Vector2.Distance(_transform.position, position));
                    
                    if (distanceToGoal < 0.1f) {
                        _character.InputVector = Vector2.zero;
                        _reachedGoal = true;
                    }
                    else {
                        Direction = (position - _transform.position).normalized;
                        _character.InputVector = Direction;
                    }
                } else if (!_attackCooldown) {
                    Debug.Log("Puffer Attack!");
                    
                    DOVirtual.DelayedCall(1.7f, () => AudioManager.Instance.PlaySfx(attackSound));
                    _attackCooldown = true;
                    _animator.SetTrigger("attack");
                    DOVirtual.DelayedCall(Random.Range(4f, 6f), () => _attackCooldown = false);
                }
            }
        }

        if (!_character.Spinning) {
            if (style == AiStyle.Jellyfish) {
                transform.up = Direction;    
            }
            else if (style == AiStyle.Pufferfish && FlyingIn) {
                transform.right = Direction;  
            }
            else if (style == AiStyle.Swordfish) {
                transform.right = Direction;  
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.TryGetComponent<CharacterBehavior>(out var player)) {
            if (style == AiStyle.Pufferfish) {
                _character.InputVector = Vector2.zero;
                _reachedGoal = true;
            }
        }
    }

#endregion

#region Custom
    public void Init() {
        FlyingIn = true;
        BoostStats();
        
        _reachedGoal = false;
        _attackCooldown = false;
        
        var r = Random.Range(0f, GameManager.Instance.Bubble.Radius * 0.5f);
        var d = Random.Range(0f, 2f * Mathf.PI);
        _offset = new Vector2(Mathf.Cos(d * r), Mathf.Sin(d * r));
    }

    public void Landed() {
        FlyingIn = false;
        RevertStats();
    }

    private bool NoBubbleAhead() {
        return Physics2D.BoxCastAll(
            (Vector2)_transform.position + (Direction * avoidEdgeDistance), 
            new Vector2(0.1f, 0.1f), 0f, Vector2.zero, 0f, bubbleLayerMask).Length == 0;
    }

    private void BoostStats() {
        if (_boosted) { return; }

        _boosted = true;
        _character.Acceleration = 12f;
        _character.Deceleration = 12f;
        _character.MaxSpeed = 12f;
    }
    
    private void RevertStats() {
        if (!_boosted) { return; }

        _boosted = false;
        _character.ResetStats();
    }

#endregion

}