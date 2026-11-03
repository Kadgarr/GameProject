using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Кнопка движения влево/вправо для мобильного управления.
/// Ссылка на игрока задаётся в инспекторе (если не задана — ищется один раз в Awake).
/// </summary>
public class ButtonClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum MoveDirection
    {
        Left = -1,
        Right = 1
    }

    [SerializeField] private PlayerController player;
    [SerializeField] private MoveDirection direction = MoveDirection.Right;

    private void Awake()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerController>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (player != null)
        {
            player.horizontalspeed = (int)direction;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (player != null)
        {
            player.horizontalspeed = 0;
        }
    }
}
