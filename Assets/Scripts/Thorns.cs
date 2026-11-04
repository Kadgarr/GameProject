using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Шипы: при касании убивают игрока, уровень перезапускается через timer секунд.</summary>
public class Thorns : MonoBehaviour
{
    private const string PlayerTag = "Player";

    public float timer;

    [Tooltip("Необязательно: игрок определяется по столкновению автоматически.")]
    public GameObject Player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(PlayerTag))
        {
            return;
        }

        var deadPlayer = other.GetComponentInParent<DeadPlayer>();
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
