using UnityEngine;

namespace DarknessInside.Gameplay.Player
{
    /// <summary>
    /// Общая часть источников ввода: буфер прыжка и приём нажатий от экранных кнопок (ControlButton).
    /// Нажатие прыжка запоминается до ближайшего шага физики, поэтому не теряется между кадрами.
    /// </summary>
    public abstract class PlayerInputBase : MonoBehaviour, IPlayerInput
    {
        private bool _jumpRequested;

        public abstract float Horizontal { get; }

        public bool ConsumeJump()
        {
            bool requested = _jumpRequested;
            _jumpRequested = false;
            return requested;
        }

        public void PressJump()
        {
            if (isActiveAndEnabled)
            {
                _jumpRequested = true;
            }
        }

        /// <summary>Кнопка движения нажата или отпущена. direction: -1 влево, 1 вправо.</summary>
        public virtual void SetMovePressed(int direction, bool pressed)
        {
        }

        protected virtual void OnDisable()
        {
            _jumpRequested = false;
        }
    }
}
