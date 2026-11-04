using UnityEngine;
using UnityEngine.EventSystems;

namespace DarknessInside.Gameplay.Player
{
    /// <summary>
    /// Экранная кнопка управления. Что она делает, задаётся в инспекторе — так на уровне 14
    /// кнопки «перепутаны» простой настройкой, без отдельного кода.
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

        private bool _held;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (input == null)
            {
                return;
            }

            if (action == ControlAction.Jump)
            {
                input.PressJump();
                return;
            }

            _held = true;
            input.SetMovePressed(Direction, true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Release();
        }

        // Пауза выключает кнопки: отпускаем, чтобы игрок не продолжил идти после паузы.
        private void OnDisable()
        {
            Release();
        }

        private int Direction => action == ControlAction.MoveLeft ? -1 : 1;

        private void Release()
        {
            if (!_held || input == null)
            {
                return;
            }

            _held = false;
            input.SetMovePressed(Direction, false);
        }
    }
}
