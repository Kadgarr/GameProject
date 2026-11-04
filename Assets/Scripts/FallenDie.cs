using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Падающая ловушка: начинает падать, когда игрок входит в триггер; убивает игрока при ударе,
/// после приземления на Ground становится безопасной.
/// </summary>
public class FallenDie : MonoBehaviour
{
    private const string PlayerTag = "Player";
    private const string GroundTag = "Ground";
    private const string PlatformMoveTag = "PlatformMove";
    private const string DieSpaceTag = "DieSpace";

    public float GravityScale;

    [Tooltip("Через сколько секунд после смерти перезапустится уровень.")]
    public float timer;

    [Tooltip("Необязательно: игрок определяется по столкновению автоматически.")]
    public GameObject Player;

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

        if (!_landed && other.CompareTag(PlayerTag))
        {
            KillPlayer(other);
        }

        if (other.CompareTag(GroundTag))
        {
            _landed = true;
            for (int i = 0; i < _childColliders.Length; i++)
            {
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

    private void KillPlayer(GameObject playerObject)
    {
        var deadPlayer = playerObject.GetComponentInParent<DeadPlayer>();
        if (deadPlayer == null && Player != null)
        {
            deadPlayer = Player.GetComponentInChildren<DeadPlayer>();
        }

        if (deadPlayer != null)
        {
            deadPlayer.Kill(timer);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
