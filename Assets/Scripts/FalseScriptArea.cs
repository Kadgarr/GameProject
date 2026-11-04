using DarknessInside.Gameplay.Hazards;
using UnityEngine;

/// <summary>«Ложный» проход (уровень 8): игрок, вошедший в зону, погибает и уровень начинается заново.</summary>
public class FalseScriptArea : Hazard
{
    protected override string DefaultCause => "false_passage";

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryKill(other.gameObject);
    }
}
