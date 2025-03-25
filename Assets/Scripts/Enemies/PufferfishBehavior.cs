using DG.Tweening;
using Managers;
using UnityEngine;

namespace Enemies {
    public class PufferfishBehavior : EnemyBehavior {
        public override void Init() {
            AttackCooldown = false;
            
            Velocity = Vector2.zero;
            GoalVelocity = Vector2.zero;
            Direction = Vector2.zero;
        }
        
        protected override void Move() {
            if (PlayerManager.PlayerTransform) {
                var playerIsOnLeft = PlayerManager.PlayerTransform.position.x < _transform.position.x;
                _spriteRenderer.flipX = playerIsOnLeft;
            }
            
            Velocity = Vector2.MoveTowards(Velocity, GoalVelocity, acceleration * Time.fixedDeltaTime);
            _rigidbody.linearVelocity = Velocity;
            _animator.SetFloat(_speed, Mathf.Clamp(Mathf.Sqrt(Velocity.sqrMagnitude)/10, 0.2f, 1f));
        }
        
        protected override void Attack() {
            DOVirtual.DelayedCall(1.7f, () => AudioManager.Instance.PlaySfx(_attackBehavior.EquippedWeapon.Sound));
            AttackCooldown = true;
            _animator.SetTrigger(_attack);

            var x = _attackBehavior.EquippedWeapon.AttackSpeed;
            DOVirtual.DelayedCall(Random.Range(x, x * 1.25f), () => AttackCooldown = false);
        }
    }
}

