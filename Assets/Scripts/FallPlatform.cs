using System.Collections;
using UnityEngine;

/// <summary>Платформа, которая падает через timer секунд после того, как на неё зашёл игрок.</summary>
public class FallPlatform : MonoBehaviour
{
    private const string PlayerTag = "Player";

    [Tooltip("Через сколько секунд после касания игрока платформа падает.")]
    public float timer;

    [SerializeField] private float fallGravityScale = 2f;

    private Rigidbody2D _body;
    private BoxCollider2D _collider;
    private bool _triggered;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggered || !other.CompareTag(PlayerTag))
        {
            return;
        }

        _triggered = true;
        StartCoroutine(FallAfterDelay());
    }

    private IEnumerator FallAfterDelay()
    {
        if (timer > 0f)
        {
            yield return new WaitForSeconds(timer);
        }

        if (_body != null)
        {
            _body.bodyType = RigidbodyType2D.Dynamic;
            _body.gravityScale = fallGravityScale;
        }

        if (_collider != null)
        {
            _collider.enabled = false;
        }
    }
}
