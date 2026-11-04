namespace DarknessInside.Gameplay.Player
{
    /// <summary>
    /// Источник управления игроком (Strategy). PlayerMotor не знает, откуда ввод:
    /// кнопки на экране, наклон телефона или что-то ещё.
    /// </summary>
    public interface IPlayerInput
    {
        /// <summary>Горизонтальное направление от -1 (влево) до 1 (вправо).</summary>
        float Horizontal { get; }

        /// <summary>Было ли нажатие прыжка с прошлого вызова (флаг сбрасывается при чтении).</summary>
        bool ConsumeJump();
    }
}
