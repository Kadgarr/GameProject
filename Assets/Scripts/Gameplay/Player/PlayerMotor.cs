using UnityEngine;

namespace DarknessInside.Gameplay.Player
{
    /// <summary>
    /// Движение игрока через Rigidbody2D (State Machine: Idle / Run / Jump / Dead).
    /// Ввод берётся из IPlayerInput, параметры — из PlayerMovementSettings.
    /// Animator меняется только при смене состояния, без вызовов каждый кадр.
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

        private const string GroundTag = "Ground";
        private const string PlatformMoveTag = "PlatformMove";

        private static readonly int IsRunHash = Animator.StringToHash("IsRun");
        private static readonly int IsJumpHash = Animator.StringToHash("IsJump");

        [SerializeField] private PlayerMovementSettings settings;
        [Tooltip("Источник ввода: ButtonsInput на канвасе управления или TiltInput. Пусто — игрок стоит.")]
        [SerializeField] private PlayerInputBase input;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        private Rigidbody2D _body;
        private DeadPlayer _death;
        private Transform _homeParent;
        private bool _grounded;

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
            _death = GetComponent<DeadPlayer>();
            _homeParent = transform.parent;
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

            float horizontal = input != null ? input.Horizontal : 0f;
            Vector2 velocity = _body.linearVelocity;
            velocity.x = horizontal * settings.moveSpeed;
            _body.linearVelocity = velocity;

            if (horizontal != 0f && spriteRenderer != null)
            {
                spriteRenderer.flipX = horizontal < 0f;
            }

            if (input != null && input.ConsumeJump() && _grounded)
            {
                _body.AddForce(Vector2.up * settings.jumpImpulse, ForceMode2D.Impulse);
                SetState(State.Jump);
                return;
            }

            if (CurrentState != State.Jump)
            {
                SetState(horizontal != 0f ? State.Run : State.Idle);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IsGroundLike(collision.gameObject))
            {
                return;
            }

            _grounded = true;
            if (CurrentState == State.Jump)
            {
                SetState(State.Idle);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            GameObject other = collision.gameObject;
            if (!IsGroundLike(other))
            {
                return;
            }

            _grounded = true;
            if (other.CompareTag(PlatformMoveTag))
            {
                AttachToPlatform(collision.transform);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            GameObject other = collision.gameObject;
            if (!IsGroundLike(other))
            {
                return;
            }

            _grounded = false;
            if (other.CompareTag(PlatformMoveTag))
            {
                DetachFromPlatform(collision.transform);
            }
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

        private static bool IsGroundLike(GameObject other)
        {
            return other.CompareTag(GroundTag) || other.CompareTag(PlatformMoveTag);
        }

        // Движущаяся платформа везёт игрока как родитель. Пока платформа выключается
        // (например, при перезагрузке сцены), Unity запрещает менять родителя — тогда ничего не делаем.
        // На этапе 4 заменится на перенос скорости платформы.
        private void AttachToPlatform(Transform platform)
        {
            if (transform.parent != platform && platform.gameObject.activeInHierarchy && gameObject.activeInHierarchy)
            {
                transform.SetParent(platform);
            }
        }

        private void DetachFromPlatform(Transform platform)
        {
            if (transform.parent == platform && platform.gameObject.activeInHierarchy && gameObject.activeInHierarchy)
            {
                transform.SetParent(_homeParent);
            }
        }
    }
}
