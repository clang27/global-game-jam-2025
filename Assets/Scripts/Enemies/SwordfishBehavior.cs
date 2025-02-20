using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Enemies {
    public class SwordfishBehavior : EnemyBehavior {
        private Vector2 _startingPoint, _goalPoint;
        private int _playerLayerMask, _wallLayerMask, _bubbleLayerMask;
        private bool _reachedGoal = true;
        
        [SerializeField] private float zoneOfControlRadius = 1f;
        [SerializeField] private float turnSpeed = 1f;

        protected override void Init() {
            Velocity = Vector2.zero;
            Direction = Vector2.zero;
            GoalVelocity = Vector2.zero;
            
            _startingPoint = _transform.position;
            _goalPoint = _startingPoint;

            _playerLayerMask = LayerMask.GetMask("Player");
            _wallLayerMask = LayerMask.GetMask("Floor", "Bubble");
            
            _attackBehavior.TurnOnHitBox();
        }
        
        protected override void Move() {
            var playerHit = Physics2D.CircleCast(
                _transform.position, zoneOfControlRadius, Vector2.zero, 0f, _playerLayerMask);
            
            if (playerHit) {
                Debug.Log($"{name} sees the player!");
                var wallHits = Physics2D.LinecastAll(
                    _transform.position, playerHit.point, _wallLayerMask);
                    
                // Wall is not in the way, so I will start swimming
                if (wallHits.Length == 0) {
                    _goalPoint = playerHit.point;
                    _reachedGoal = false;
                } else {
                    Debug.Log($"But there is a wall in the way!");
                }
            } else {
                _reachedGoal = Vector2.Distance(_transform.position, _goalPoint) < 2f;
            }

            if (!_reachedGoal) {
                Direction = Vector2.MoveTowards(Direction,
                    (_goalPoint - (Vector2)_transform.position).normalized,
                    turnSpeed * Time.fixedDeltaTime);
                
                _transform.right = Direction;
                GoalVelocity = maxSpeed * Direction;
            }
            else {
                GoalVelocity = Vector2.zero;
            }
            
            Velocity = Vector2.MoveTowards(Velocity, GoalVelocity, acceleration * Time.fixedDeltaTime);
            _rigidbody.linearVelocity = Velocity;
            _animator.SetFloat(_speed, Mathf.Clamp(Mathf.Sqrt(Velocity.sqrMagnitude)/10, 0.2f, 1f));
        }
        
        protected override void Attack() {
            
        }
    }
}

