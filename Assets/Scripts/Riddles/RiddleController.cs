using UnityEngine;

/// <summary>Кодовый замок уровня 6: номер текущей ячейки и проверка введённого кода.</summary>
public class RiddleController : MonoBehaviour
{
    private const string Solution = "7777";

    public int count = 1;
    public GameObject Door;

    private string _entered = string.Empty;
    private DoorOpen _door;

    private void Awake()
    {
        _door = Door != null ? Door.GetComponentInChildren<DoorOpen>() : null;
    }

    /// <summary>Переход к следующей ячейке (кнопку отпустили).</summary>
    public void ButtonController1()
    {
        count++;
    }

    /// <summary>Введена цифра.</summary>
    public void ButtonController2(string digit)
    {
        _entered += digit;
        if (_entered.Length == Solution.Length)
        {
            DarknessInside.Gameplay.Levels.LevelFlow.ReportRiddleAttempt(_entered == Solution);
        }

        if (_entered == Solution && _door != null)
        {
            _door.OpenedDoor(true);
        }
    }
}
