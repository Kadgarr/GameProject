using UnityEngine;

/// <summary>Ячейка кодового замка уровня 6: показывает введённую цифру.</summary>
public class RiddleCode : MonoBehaviour
{
    public bool stateOfDoor;
    public GameObject Door;

    public Sprite Cell3;
    public Sprite Cell4;
    public Sprite Cell5;
    public Sprite Cell7;

    private SpriteRenderer _renderer;
    private string _entered;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Code(string digit)
    {
        if (_entered == digit)
        {
            return;
        }

        Sprite sprite = SpriteFor(digit);
        if (sprite == null)
        {
            return;
        }

        if (_renderer != null)
        {
            _renderer.sprite = sprite;
        }

        _entered += digit;
    }

    private Sprite SpriteFor(string digit)
    {
        switch (digit)
        {
            case "3":
                return Cell3;
            case "4":
                return Cell4;
            case "5":
                return Cell5;
            case "7":
                return Cell7;
            default:
                return null;
        }
    }
}
