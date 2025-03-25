using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Enemies {
    public class SwordfishBehavior : EnemyBehavior {
        private Vector2 _startingPoint, _goalPoint;
        private int _playerLayerMask, _wallLayerMask, _bubbleLayerMask;
        private bool _reachedGoal = true;
        
        [SerializeField] private float zoneOfControlRadius = 1f;
        [SerializeField] private float turnSpeed = 1f;
        [SerializeField] private Transform top, bottom;

        public override void Init() {
            _reachedGoal = true;
            
            Velocity = Vector2.zero;
            Direction = Vector2.zero;
            GoalVelocity = Vector2.zero;
            
            _startingPoint = _transform.position;
            _goalPoint = _startingPoint;

            _playerLayerMask = LayerMask.GetMask("Player");
            _wallLayerMask = LayerMask.GetMask("Floor", "Bubble");
        }
        
        protected override void Move() {
            var playerHit = Physics2D.CircleCast(
                _transform.position, zoneOfControlRadius, Vector2.zero, 0f, _playerLayerMask);
            
            if (playerHit) {
                var topWallHits = Physics2D.LinecastAll(
                    top.position, playerHit.point, _wallLayerMask);
                var middleWallHits = Physics2D.LinecastAll(
                    _transform.position, playerHit.point, _wallLayerMask);
                var bottomWallHits = Physics2D.LinecastAll(
                    bottom.position, playerHit.point, _wallLayerMask);
                    
                // Wall is not in the way, so I will start swimming
                if (topWallHits.Length + middleWallHits.Length + bottomWallHits.Length == 0) {
                    Debug.Log($"{name} sees the player!");
                    _goalPoint = playerHit.point;
                    _reachedGoal = false;
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

