using UnityEngine;
using TMPro;

/// <summary>
/// Загадка уровня 12: две группы кнопок, каждая прибавляет или убирает своё число к сумме группы.
/// Дверь открывается, когда разность сумм равна 8.
/// </summary>
public class Riddle2Code : MonoBehaviour
{
    private const int TargetDifference = 8;

    public int number;
    public int numberOfGroupButtons;
    public GameObject objCanv;

    public GameObject cell1;
    public GameObject cell2;
    public GameObject cell3;
    public GameObject doorObj;

    private bool _activeInGroup;
    private DoorOpen _door;
    private SummRiddle2 _sum;
    private TMP_Text _cell1;
    private TMP_Text _cell2;
    private TMP_Text _cell3;

    private void Awake()
    {
        _cell1 = cell1 != null ? cell1.GetComponent<TMP_Text>() : null;
        _cell2 = cell2 != null ? cell2.GetComponent<TMP_Text>() : null;
        _cell3 = cell3 != null ? cell3.GetComponent<TMP_Text>() : null;
        _door = doorObj != null ? doorObj.GetComponent<DoorOpen>() : null;
        _sum = objCanv != null ? objCanv.GetComponent<SummRiddle2>() : null;
    }

    public void RiddleClick()
    {
        if (_sum == null)
        {
            return;
        }

        int delta = _activeInGroup ? -number : number;
        _activeInGroup = !_activeInGroup;

        if (numberOfGroupButtons == 1)
        {
            _sum.summ1 += delta;
            SetText(_cell1, _sum.summ1);
        }
        else if (numberOfGroupButtons == 2)
        {
            _sum.summ2 += delta;
            SetText(_cell2, _sum.summ2);
        }
        else
        {
            return;
        }

        int difference = _sum.summ1 - _sum.summ2;
        SetText(_cell3, difference);
        if (difference == TargetDifference && _door != null)
        {
            _door.OpenedDoor(true);
            if (!_sum.solved)
            {
                _sum.solved = true;
                DarknessInside.Gameplay.Levels.LevelFlow.ReportRiddleAttempt(true);
            }
        }
    }

    private static void SetText(TMP_Text text, int value)
    {
        if (text != null)
        {
            text.text = value.ToString();
        }
    }
}
