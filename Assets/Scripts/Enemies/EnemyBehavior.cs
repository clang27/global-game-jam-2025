using System;
using DG.Tweening;
using Managers;
using Scriptable;
using UnityEngine;

namespace Enemies {
    public abstract class EnemyBehavior : MonoBehaviour {

    #region Dependencies
        [SerializeField] protected AudioClip hurtSound;
        [SerializeField] protected float maxSpeed, acceleration;
        [SerializeField][Range(1, 10)] private int health = 1;
    #endregion

    #region Attributes
        public bool AttackCooldown { get; protected set; }
        public bool Stunned { get; private set; }
        public int Health { get; private set; }
        public Vector2 Velocity {
            get => _velocity;
            protected set {
                _velocity = value;
                _animator.SetFloat(_speed, Mathf.Sqrt(_velocity.sqrMagnitude) / 5f + 0.2f);
            }
        }
        protected Vector2 GoalVelocity { get; set; }
        public Vector2 Direction { get; protected set; }
    #endregion

    #region Components
        protected Transform _transform;
        protected Rigidbody2D _rigidbody;
        protected SpriteRenderer _spriteRenderer;
        protected Animator _animator;
        protected Collider2D _collider;
        protected AttackBehavior _attackBehavior;
    #endregion
    
    #region Data
        private Vector2 _velocity;
        protected static readonly int _speed = Animator.StringToHash("speed");
        protected static readonly int _attack = Animator.StringToHash("attack");
    #endregion

    #region Unity
        private void Awake() {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            _attackBehavior = GetComponentInChildren<AttackBehavior>();

            Health = health;
        }

        private void Start() {
            Init();
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.CompareTag("Bubble")) {
                var sourcePosition = (Vector2) other.transform.position;
                var direction = ((Vector2) _transform.position - sourcePosition).normalized;
                
                _rigidbody.AddForce(direction * 10f);
            }
        }

        private void FixedUpdate() {
            if (Stunned) { return; }
            
            Move();
            
            if (!AttackCooldown) {
                Attack();
            }
        }
    #endregion

    #region Custom

        protected abstract void Init();
        protected abstract void Move();
        protected abstract void Attack();
        
        public void Hurt(Weapon weapon, Vector2 direction) {
            if (Stunned) { return; }
            AudioManager.Instance.PlaySfx(hurtSound);
            
            Stunned = true;
            Health -= weapon.Damage;

            if (Health == 0) {
                _rigidbody.AddForce(direction * weapon.Knockback * 10f, ForceMode2D.Impulse);

                _transform.DORotate(new Vector3(0f, 0f, _transform.localRotation.eulerAngles.z + 720f), 0.5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => {
                        _spriteRenderer.DOKill();
                        EnemyManager.Instance.Poof(_transform.position);
                        Destroy(gameObject);
                    });
            } else {
                _spriteRenderer.DOFade(0.1f, 0.05f).SetLoops(-1, LoopType.Yoyo);
                _rigidbody.AddForce(direction * weapon.Knockback, ForceMode2D.Impulse);
                
                DOVirtual.DelayedCall(weapon.StunTime, () => {
                    Stunned = false;    
                    _spriteRenderer.DOKill();
                    _spriteRenderer.DOFade(1f, 0f);
                });
            }
        }

    #endregion

    }
}

