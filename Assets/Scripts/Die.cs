using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Смертельная зона/объект: при касании убивает игрока, уровень перезапускается через timer секунд.</summary>
public class Die : MonoBehaviour
{
    private const string PlayerTag = "Player";

    [Tooltip("Через сколько секунд после смерти перезапустится уровень.")]
    public float timer;

    [Tooltip("Необязательно: игрок определяется по столкновению автоматически.")]
    public GameObject Player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PlayerTag))
        {
            KillPlayer(FindDeadPlayer(other.gameObject));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(PlayerTag))
        {
            KillPlayer(FindDeadPlayer(collision.gameObject));
        }
    }

    /// <summary>Убить игрока с задержкой перезапуска этой зоны (используется и шкалой Scale).</summary>
    public void KillPlayer(DeadPlayer player)
    {
        if (player != null)
        {
            player.Kill(timer);
        }
        else
        {
            // Игрок без DeadPlayer — просто перезапускаем уровень, как раньше.
            StartCoroutine(ReloadAfter(timer));
        }
    }

    private DeadPlayer FindDeadPlayer(GameObject source)
    {
        var deadPlayer = source.GetComponentInParent<DeadPlayer>();
        if (deadPlayer == null && Player != null)
        {
            deadPlayer = Player.GetComponentInChildren<DeadPlayer>();
        }

        return deadPlayer;
    }

    private static IEnumerator ReloadAfter(float delay)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
