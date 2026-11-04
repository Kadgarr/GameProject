using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Кнопка быстрых нажатий: каждое нажатие добавляет к кругу заполнения, круг постепенно убывает.
/// Когда круг заполнен — рычаг включается, дверь открывается, срабатывает событие Completed.
/// </summary>
public class FastClick : MonoBehaviour
{
    public Image LoadingCircle;
    public Image ComplitedCircle;
    public Sprite ArmSpriteOn;
    public Button ClickButton;

    public GameObject ObjectDoor;
    public GameObject canvasBut;

    public SpriteRenderer ArmSprite;

    [Tooltip("Сколько добавляет одно нажатие (0..1).")]
    [SerializeField] private float fillPerClick = 0.3f;
    [Tooltip("Насколько круг убывает за шаг (каждый второй кадр физики).")]
    [SerializeField] private float decayPerStep = 0.05f;

    private bool _completed;

    /// <summary>Круг заполнен, постройка завершена.</summary>
    public event Action Completed;

    private void FixedUpdate()
    {
        if (_completed)
        {
            return;
        }

        // Только убывание. Заполнение проверяется в момент нажатия, иначе последнее нажатие
        // могло «съедаться» убыванием в том же шаге физики.
        if (LoadingCircle.fillAmount > 0f && Time.frameCount % 2 == 0)
        {
            LoadingCircle.fillAmount -= decayPerStep;
        }
    }

    public void ButtonFillClick()
    {
        if (_completed)
        {
            return;
        }

        LoadingCircle.fillAmount += fillPerClick;
        if (LoadingCircle.fillAmount >= 1f)
        {
            Complete();
        }
    }

    private void Complete()
    {
        _completed = true;

        if (ComplitedCircle != null)
        {
            ComplitedCircle.enabled = true;
        }

        if (ClickButton != null)
        {
            if (ComplitedCircle != null)
            {
                ClickButton.image = ComplitedCircle;
            }

            ClickButton.interactable = false;
        }

        if (ArmSprite != null && ArmSpriteOn != null)
        {
            ArmSprite.sprite = ArmSpriteOn;
        }

        DoorOpen door = ObjectDoor != null ? ObjectDoor.GetComponentInChildren<DoorOpen>() : null;
        if (door != null)
        {
            door.OpenedDoor(true);
        }

        // Канвас кнопки: если не задан явно — прячем ближайший родительский Canvas.
        GameObject buttonCanvas = canvasBut;
        if (buttonCanvas == null)
        {
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            buttonCanvas = parentCanvas != null ? parentCanvas.rootCanvas.gameObject : null;
        }

        if (buttonCanvas != null)
        {
            buttonCanvas.SetActive(false);
        }

        Completed?.Invoke();
    }
}
