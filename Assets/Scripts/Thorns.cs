using DarknessInside.Gameplay.Hazards;
using UnityEngine;

/// <summary>Шипы: убивают игрока, вошедшего в триггер.</summary>
public class Thorns : Hazard
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        TryKill(other.gameObject);
    }
}
