using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Enemies {
    public class JellyfishBehavior : EnemyBehavior {
        private Vector2 _goalVelocity;
        private int _floorLayerMask;
        
        [SerializeField] private float raycastDistance;

        protected override void Init() {
            Velocity = Vector2.zero;
            Direction = _transform.rotation * Vector2.up;

            _floorLayerMask = LayerMask.GetMask("Floor");
            _goalVelocity = Direction * maxSpeed;
            _attackBehavior.TurnOnHitBox();
        }
        
        protected override void Move() {
            if (WallAhead()) {
                Direction = -Direction;
                _transform.Rotate(new Vector3(0f, 0f, 180f));
                _goalVelocity = -_goalVelocity;
            }

            Velocity = Vector2.Lerp(Velocity, _goalVelocity, acceleration);
            Vector2.MoveTowards(Velocity, _goalVelocity, 1f / acceleration);
            //Debug.Log($"Jellyfish moving {Velocity}");
            _rigidbody.linearVelocity = Velocity;
        }
        
        protected override void Attack() {
            
        }

        private bool WallAhead() {
            // 45 degree angles need a longer raycast to detect wall
            var tan = Mathf.Abs(Mathf.Tan(_transform.rotation.eulerAngles.z * Mathf.Deg2Rad)) / 5f;
            var hit = Physics2D.Raycast(_transform.position, Direction, raycastDistance + tan, _floorLayerMask);
            
            return hit;
        }
    }
}

