using UnityEngine;
using UnityEngine.Events;

namespace DarknessInside.Gameplay.Puzzles
{
    /// <summary>
    /// Кнопка на полу, на которую наступает игрок: меняет спрайт и сообщает о нажатии.
    /// Реакцию можно задать событиями в инспекторе (pressed / released) или в наследнике.
    /// </summary>
    public abstract class PressurePlate : MonoBehaviour
    {
        protected const string PlayerTag = "Player";

        public Sprite switchOnBut;
        public Sprite switchOffBut;

        [SerializeField] private UnityEvent pressed = new UnityEvent();
        [SerializeField] private UnityEvent released = new UnityEvent();

        private SpriteRenderer _renderer;

        protected virtual void Awake()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag(PlayerTag))
            {
                return;
            }

            SetSprite(switchOnBut);
            OnPressed();
            pressed.Invoke();
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag(PlayerTag))
            {
                return;
            }

            SetSprite(switchOffBut);
            OnReleased();
            released.Invoke();
        }

        protected virtual void OnPressed()
        {
        }

        protected virtual void OnReleased()
        {
        }

        private void SetSprite(Sprite sprite)
        {
            if (_renderer != null && sprite != null)
            {
                _renderer.sprite = sprite;
            }
        }
    }
}
