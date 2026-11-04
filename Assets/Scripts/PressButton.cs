using DarknessInside.Gameplay.Puzzles;
using UnityEngine;

/// <summary>Кнопка на полу, которая открывает дверь.</summary>
public class PressButton : PressurePlate
{
    [Tooltip("Дверь (объект с DoorOpen в дочерних).")]
    public GameObject objectd;

    private DoorOpen _door;

    protected override void Awake()
    {
        base.Awake();
        _door = objectd != null ? objectd.GetComponentInChildren<DoorOpen>() : null;
    }

    protected override void OnPressed()
    {
        if (_door != null)
        {
            _door.OpenedDoor(true);
        }
    }
}
