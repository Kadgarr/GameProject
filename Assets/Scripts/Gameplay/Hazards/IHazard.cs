namespace DarknessInside.Gameplay.Hazards
{
    /// <summary>Всё, что убивает игрока при касании.</summary>
    public interface IHazard
    {
        /// <summary>Сколько секунд показывать смерть до перезапуска уровня.</summary>
        float RestartDelay { get; }
    }
}
