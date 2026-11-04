using DarknessInside.Gameplay.Hazards;
using UnityEngine;

/// <summary>
/// Падающая ловушка: начинает падать, когда игрок входит в триггер; убивает игрока при ударе,
/// после приземления на Ground становится безопасной.
/// </summary>
public class FallenDie : Hazard
{
    private const string GroundTag = "Ground";
    private const string PlatformMoveTag = "PlatformMove";
    private const string DieSpaceTag = "DieSpace";

    public float GravityScale;

    private bool _landed;
    private BoxCollider2D[] _childColliders;
    private Rigidbody2D[] _childBodies;

    private void Awake()
    {
        _childColliders = GetComponentsInChildren<BoxCollider2D>();
        _childBodies = GetComponentsInChildren<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        if (!_landed)
        {
            TryKill(other);
        }

        if (other.CompareTag(GroundTag))
        {
            _landed = true;
            for (int i = 0; i < _childColliders.Length; i++)
            {
                // Шипы-дети могут быть уже уничтожены (упали на движущуюся платформу).
                if (_childColliders[i] != null)
                {
                    _childColliders[i].enabled = false;
                }
            }
        }

        if (other.CompareTag(PlatformMoveTag))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PlayerTag))
        {
            for (int i = 0; i < _childBodies.Length; i++)
            {
                if (_childBodies[i] != null)
                {
                    _childBodies[i].gravityScale = GravityScale;
                }
            }
        }

        if (other.CompareTag(DieSpaceTag))
        {
            Destroy(gameObject);
        }
    }
}
