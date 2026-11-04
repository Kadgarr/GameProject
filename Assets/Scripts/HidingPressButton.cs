using DarknessInside.Gameplay.Puzzles;
using UnityEngine;

/// <summary>Кнопка на полу: при нажатии игроком показывает стены и меняет коллайдеры.</summary>
public class HidingPressButton : PressurePlate
{
    [Tooltip("Стены, которые появляются при нажатии. Пустые поля пропускаются.")]
    public GameObject Wall1;
    public GameObject Wall2;
    public GameObject Wall3;
    public GameObject Wall4;

    public GameObject ColliderWall;
    public GameObject ColliderGround;

    protected override void OnPressed()
    {
        SetColliderEnabled(ColliderWall, false);
        SetColliderEnabled(ColliderGround, true);
        ShowWall(Wall1);
        ShowWall(Wall2);
        ShowWall(Wall3);
        ShowWall(Wall4);
    }

    private static void SetColliderEnabled(GameObject target, bool enabled)
    {
        if (target == null)
        {
            return;
        }

        var collider = target.GetComponentInChildren<BoxCollider2D>();
        if (collider != null)
        {
            collider.enabled = enabled;
        }
    }

    private static void ShowWall(GameObject wall)
    {
        if (wall == null)
        {
            return;
        }

        var wallRenderer = wall.GetComponentInChildren<SpriteRenderer>();
        if (wallRenderer != null)
        {
            wallRenderer.enabled = true;
        }
    }
}
