using DarknessInside.Gameplay.Puzzles;
using UnityEngine;

/// <summary>
/// Кнопка кодового замка уровня 6: при нажатии записывает свою цифру в текущую ячейку,
/// при отпускании замок переходит к следующей ячейке.
/// </summary>
public class RidllePressButton : PressurePlate
{
    public int NumberForRiddle;

    public GameObject Cell1;
    public GameObject Cell2;
    public GameObject Cell3;
    public GameObject Cell4;
    public GameObject Controller;

    private RiddleController _controller;
    private readonly RiddleCode[] _cells = new RiddleCode[4];
    private string _digit;

    protected override void Awake()
    {
        base.Awake();
        _digit = NumberForRiddle.ToString();
        _controller = Controller != null ? Controller.GetComponentInChildren<RiddleController>() : null;
        _cells[0] = FindCell(Cell1);
        _cells[1] = FindCell(Cell2);
        _cells[2] = FindCell(Cell3);
        _cells[3] = FindCell(Cell4);
    }

    protected override void OnPressed()
    {
        if (_controller == null)
        {
            return;
        }

        int index = _controller.count - 1;
        if (index < 0 || index >= _cells.Length)
        {
            return;
        }

        if (_cells[index] != null)
        {
            _cells[index].Code(_digit);
        }

        _controller.ButtonController2(_digit);
    }

    protected override void OnReleased()
    {
        if (_controller != null)
        {
            _controller.ButtonController1();
        }
    }

    private static RiddleCode FindCell(GameObject cell)
    {
        return cell != null ? cell.GetComponentInChildren<RiddleCode>() : null;
    }
}
