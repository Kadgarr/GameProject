using DarknessInside.Gameplay.Hazards;
using UnityEngine;

/// <summary>Смертельная зона или объект: убивает игрока при касании (триггер или столкновение).</summary>
public class Die : Hazard
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        TryKill(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryKill(collision.gameObject);
    }
}
