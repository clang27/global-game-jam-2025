using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Enemies {
    public class JellyfishBehavior : EnemyBehavior {
        private int _wallLayerMask;
        
        [SerializeField] private float raycastDistance;

        public override void Init() {
            Velocity = Vector2.zero;
            Direction = _transform.rotation * Vector2.up;

            _wallLayerMask = LayerMask.GetMask("Floor", "Bubble");
            
            GoalVelocity = Direction * maxSpeed;
        }
        
        protected override void Move() {
            if (WallAhead()) {
                Direction = -Direction;
                _transform.Rotate(new Vector3(0f, 0f, 180f));
                GoalVelocity = -GoalVelocity;
            }

            Velocity = Vector2.MoveTowards(Velocity, GoalVelocity, acceleration * Time.fixedDeltaTime);
            //Debug.Log($"Jellyfish moving {Velocity}");
            _rigidbody.linearVelocity = Velocity;
            
            _animator.SetFloat(_speed, Mathf.Clamp(Mathf.Sqrt(Velocity.sqrMagnitude)/10, 0.2f, 1f));
        }
        
        protected override void Attack() {
            
        }

        private bool WallAhead() {
            // 45 degree angles need a longer raycast to detect wall
            var tan = Mathf.Abs(Mathf.Tan(_transform.rotation.eulerAngles.z * Mathf.Deg2Rad)) / 5f;
            tan = Mathf.Clamp(tan, -0.2f, 0.2f);
            var hit = Physics2D.Raycast(_transform.position, Direction, raycastDistance + tan, _wallLayerMask);
            if (hit) {
                //Debug.Log($"{name} detects {hit.collider.gameObject.transform.parent.name}");    
            }
            
            return hit;
        }
    }
}

