using UnityEngine;
using UnityEngine.EventSystems;

namespace DarknessInside.Gameplay.Player
{
    /// <summary>
    /// Экранная кнопка управления. Что она делает, задаётся в инспекторе — так на уровне 14
    /// кнопки «перепутаны» простой настройкой, без отдельного кода. Кнопку можно нажать пальцем
    /// или клавишей (KeyboardButtonBinding): клавиша наследует ту же настройку, поэтому путаница уровня 14
    /// работает и на клавиатуре.
    /// </summary>
    public sealed class ControlButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public enum ControlAction
        {
            MoveLeft,
            MoveRight,
            Jump
        }

        [SerializeField] private PlayerInputBase input;
        [SerializeField] private ControlAction action = ControlAction.MoveRight;

        private bool _pointerHeld;
        private bool _keyHeld;

        public void OnPointerDown(PointerEventData eventData)
        {
            SetPointerHeld(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetPointerHeld(false);
        }

        /// <summary>Клавиша, привязанная к кнопке, нажата или отпущена.</summary>
        public void SetKeyHeld(bool held)
        {
            if (_keyHeld == held)
            {
                return;
            }

            bool wasHeld = IsHeld;
            _keyHeld = held;
            Apply(wasHeld);
        }

        // Пауза выключает кнопки: отпускаем, чтобы игрок не продолжил идти после паузы.
        private void OnDisable()
        {
            bool wasHeld = IsHeld;
            _pointerHeld = false;
            _keyHeld = false;
            Apply(wasHeld);
        }

        private bool IsHeld => _pointerHeld || _keyHeld;

        private int Direction => action == ControlAction.MoveLeft ? -1 : 1;

        private void SetPointerHeld(bool held)
        {
            if (_pointerHeld == held)
            {
                return;
            }

            bool wasHeld = IsHeld;
            _pointerHeld = held;
            Apply(wasHeld);
        }

        /// <summary>Сообщить вводу о переходе «отпущена ↔ нажата» (палец и клавиша вместе — одно нажатие).</summary>
        private void Apply(bool wasHeld)
        {
            bool held = IsHeld;
            if (input == null || held == wasHeld)
            {
                return;
            }

            if (action == ControlAction.Jump)
            {
                if (held)
                {
                    input.PressJump();
                }

                return;
            }

            input.SetMovePressed(Direction, held);
        }
    }
}
