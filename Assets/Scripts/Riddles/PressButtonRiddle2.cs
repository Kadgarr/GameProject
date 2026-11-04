using DarknessInside.Gameplay.Puzzles;

/// <summary>Кнопка загадки уровня 12: при нажатии прибавляет/убирает своё число (Riddle2Code).</summary>
public class PressButtonRiddle2 : PressurePlate
{
    private Riddle2Code _riddle;

    protected override void Awake()
    {
        base.Awake();
        _riddle = GetComponent<Riddle2Code>();
    }

    protected override void OnPressed()
    {
        if (_riddle != null)
        {
            _riddle.RiddleClick();
        }
    }
}
