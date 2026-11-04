using System.Collections;
using UnityEngine;

/// <summary>
/// Платформа (или шипы), которая ездит туда-обратно. Движется через кинематический Rigidbody2D
/// со скоростью — физика перемещает её сама, а игрок на ней получает её скорость (PlayerMotor).
/// Разворот — корутиной раз в time секунд, без Update.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlatformMove : MonoBehaviour
{
    [Tooltip("Сдвиг фазы: сколько секунд из первого отрезка уже «прошло» на старте.")]
    public float timer;

    [Tooltip("Скорость, единиц в секунду.")]
    public float SpeedOfPlatform = 3f;

    [Tooltip("Направление: 1 — вправо/вверх, -1 — влево/вниз (относительно поворота объекта).")]
    public int IntOfDirection = 1;

    [Tooltip("Сколько секунд в одну сторону.")]
    public float time = 1f;

    [Tooltip("Ездить вверх-вниз вместо влево-вправо.")]
    public bool DirectionUp;

    private Rigidbody2D _body;
    private Vector2 _velocity;
    private WaitForSeconds _legDuration;

    public Vector2 Velocity => _velocity;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.bodyType = RigidbodyType2D.Kinematic;
        Vector3 axis = DirectionUp ? transform.up : transform.right;
        _velocity = (Vector2)axis.normalized * (IntOfDirection * SpeedOfPlatform);
        _legDuration = new WaitForSeconds(Mathf.Max(0.01f, time));
    }

    private void OnEnable()
    {
        StartCoroutine(Patrol());
    }

    private void OnDisable()
    {
        if (_body != null)
        {
            _body.linearVelocity = Vector2.zero;
        }
    }

    private IEnumerator Patrol()
    {
        _body.linearVelocity = _velocity;
        float firstLeg = time - timer;
        if (firstLeg > 0f)
        {
            yield return new WaitForSeconds(firstLeg);
        }

        while (true)
        {
            _velocity = -_velocity;
            _body.linearVelocity = _velocity;
            yield return _legDuration;
        }
    }
}
