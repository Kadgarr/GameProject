using DarknessInside.Core;
using UnityEngine;

namespace DarknessInside.Gameplay.Player
{
    /// <summary>
    /// Общая часть источников ввода: буфер прыжка и приём нажатий от экранных кнопок (ControlButton).
    /// Нажатие прыжка запоминается до ближайшего шага физики, поэтому не теряется между кадрами.
    /// На паузе ввод сбрасывается, чтобы игрок не продолжил идти после неё.
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
            if (isActiveAndEnabled && !GamePause.IsPaused)
            {
                _jumpRequested = true;
            }
        }

        /// <summary>Кнопка движения нажата или отпущена. direction: -1 влево, 1 вправо.</summary>
        public virtual void SetMovePressed(int direction, bool pressed)
        {
        }

        protected virtual void OnEnable()
        {
            GamePause.Changed += OnPauseChanged;
        }

        protected virtual void OnDisable()
        {
            GamePause.Changed -= OnPauseChanged;
            ResetInput();
        }

        /// <summary>Отпустить всё: прыжок и зажатые кнопки.</summary>
        protected virtual void ResetInput()
        {
            _jumpRequested = false;
        }

        private void OnPauseChanged(bool paused)
        {
            if (paused)
            {
                ResetInput();
            }
        }
    }
}
