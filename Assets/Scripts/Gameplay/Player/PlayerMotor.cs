using UnityEngine;

namespace DarknessInside.Gameplay.Player
{
    /// <summary>
    /// Движение игрока через Rigidbody2D (State Machine: Idle / Run / Jump / Dead).
    /// Земля проверяется зоной под ногами по слоям (без тегов и событий столкновений);
    /// есть coyote time и буфер прыжка. Движущаяся платформа под ногами передаёт игроку свою скорость.
    /// Ввод — из IPlayerInput, параметры — из PlayerMovementSettings. Без аллокаций в шаге физики.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Run,
            Jump,
            Dead
        }

        private static readonly int IsRunHash = Animator.StringToHash("IsRun");
        private static readonly int IsJumpHash = Animator.StringToHash("IsJump");

        [SerializeField] private PlayerMovementSettings settings;
        [Tooltip("Источник ввода: ButtonsInput на канвасе управления или TiltInput. Пусто — игрок стоит.")]
        [SerializeField] private PlayerInputBase input;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        private readonly Collider2D[] _groundHits = new Collider2D[4];

        private Rigidbody2D _body;
        private Collider2D _collider;
        private DeadPlayer _death;
        private ContactFilter2D _groundFilter;
        private bool _grounded;
        private Vector2 _groundVelocity;
        private float _lastGroundedTime = float.NegativeInfinity;
        private float _jumpRequestTime = float.NegativeInfinity;

        public State CurrentState { get; private set; }

        public bool IsGrounded => _grounded;

        private void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _death = GetComponent<DeadPlayer>();
            _groundFilter = new ContactFilter2D { useTriggers = false, useLayerMask = true };
        }

        private void OnEnable()
        {
            if (_death != null)
            {
                _death.Died += OnDied;
            }
        }

        private void OnDisable()
        {
            if (_death != null)
            {
                _death.Died -= OnDied;
            }
        }

        private void FixedUpdate()
        {
            if (CurrentState == State.Dead || settings == null)
            {
                return;
            }

            float now = Time.time;
            float delta = Time.fixedDeltaTime;
            UpdateGround(now);

            float horizontal = input != null ? input.Horizontal : 0f;
            if (input != null && input.ConsumeJump())
            {
                _jumpRequestTime = now;
            }

            Vector2 velocity = _body.linearVelocity;

            // Горизонталь считаем относительно опоры: на движущейся платформе игрок едет вместе с ней.
            float relative = velocity.x - _groundVelocity.x;
            float target = horizontal * settings.moveSpeed;
            float rate = _grounded
                ? (Mathf.Abs(target) > 0.01f ? settings.groundAcceleration : settings.groundDeceleration)
                : settings.airAcceleration;
            relative = Mathf.MoveTowards(relative, target, rate * delta);
            velocity.x = relative + _groundVelocity.x;

            bool canJump = now - _lastGroundedTime <= settings.coyoteTime;
            bool wantsJump = now - _jumpRequestTime <= settings.jumpBufferTime;
            if (canJump && wantsJump)
            {
                velocity.y = 0f;
                _body.linearVelocity = velocity;
                _body.AddForce(Vector2.up * settings.jumpImpulse, ForceMode2D.Impulse);
                _jumpRequestTime = float.NegativeInfinity;
                _lastGroundedTime = float.NegativeInfinity;
                _grounded = false;
                SetState(State.Jump);
            }
            else
            {
                _body.linearVelocity = velocity;
                UpdateState(horizontal);
            }

            if (horizontal != 0f && spriteRenderer != null)
            {
                spriteRenderer.flipX = horizontal < 0f;
            }
        }

        private void UpdateGround(float now)
        {
            Bounds bounds = _collider.bounds;
            float depth = settings.groundCheckDepth;
            var center = new Vector2(bounds.center.x, bounds.min.y - depth * 0.5f + 0.01f);
            var size = new Vector2(bounds.size.x * settings.groundCheckWidth, depth);

            _groundFilter.layerMask = settings.groundLayers;
            int count = Physics2D.OverlapBox(center, size, 0f, _groundFilter, _groundHits);

            // Пока игрок летит вверх после прыжка, касание края платформы землёй не считается.
            _grounded = count > 0 && (CurrentState != State.Jump || _body.linearVelocity.y <= 0.01f);
            _groundVelocity = Vector2.zero;
            if (!_grounded)
            {
                return;
            }

            _lastGroundedTime = now;
            Rigidbody2D support = _groundHits[0].attachedRigidbody;
            if (support != null && support.bodyType == RigidbodyType2D.Kinematic)
            {
                _groundVelocity = support.linearVelocity;
            }
        }

        private void UpdateState(float horizontal)
        {
            if (CurrentState == State.Jump && !_grounded)
            {
                return;
            }

            SetState(horizontal != 0f ? State.Run : State.Idle);
        }

        private void OnDied(float restartDelay)
        {
            Vector2 velocity = _body.linearVelocity;
            velocity.x = 0f;
            _body.linearVelocity = velocity;
            SetState(State.Dead);
        }

        private void SetState(State next)
        {
            if (next == CurrentState)
            {
                return;
            }

            CurrentState = next;
            if (animator == null || !animator.isActiveAndEnabled)
            {
                return;
            }

            animator.SetBool(IsRunHash, next == State.Run);
            animator.SetBool(IsJumpHash, next == State.Jump);
        }

        private void OnDrawGizmosSelected()
        {
            var collider = GetComponent<Collider2D>();
            if (collider == null || settings == null)
            {
                return;
            }

            Bounds bounds = collider.bounds;
            float depth = settings.groundCheckDepth;
            Gizmos.color = _grounded ? Color.green : Color.yellow;
            Gizmos.DrawWireCube(
                new Vector3(bounds.center.x, bounds.min.y - depth * 0.5f + 0.01f, 0f),
                new Vector3(bounds.size.x * settings.groundCheckWidth, depth, 0f));
        }
    }
}
