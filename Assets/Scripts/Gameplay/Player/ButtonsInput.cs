namespace DarknessInside.Gameplay.Player
{
    /// <summary>Управление экранными кнопками «влево», «вправо», «прыжок».</summary>
    public sealed class ButtonsInput : PlayerInputBase
    {
        private bool _left;
        private bool _right;

        public override float Horizontal => (_right ? 1f : 0f) - (_left ? 1f : 0f);

        public override void SetMovePressed(int direction, bool pressed)
        {
            if (direction < 0)
            {
                _left = pressed;
            }
            else if (direction > 0)
            {
                _right = pressed;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _left = false;
            _right = false;
        }
    }
}
