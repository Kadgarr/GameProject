using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Смерть игрока: «мёртвый» спрайт, отключение управления и перезапуск уровня.
/// Перезапуск запускается на самом игроке, поэтому срабатывает, даже если ловушка,
/// которая убила игрока, после этого исчезла.
/// </summary>
public class DeadPlayer : MonoBehaviour
{
    public Sprite SpriteDead;

    [Tooltip("Необязательно: кнопки управления, которые выключаются после смерти.")]
    public GameObject ButRight;
    public GameObject ButLeft;
    public GameObject ButUp;

    private bool _isDead;

    public bool IsDead => _isDead;

    /// <summary>Убить игрока и перезапустить уровень через reloadDelay секунд.</summary>
    /// <param name="sceneBuildIndex">Какую сцену загрузить; меньше 0 — текущую.</param>
    public void Kill(float reloadDelay, int sceneBuildIndex = -1)
    {
        if (_isDead)
        {
            return;
        }

        _isDead = true;
        ShowDeath();

        int scene = sceneBuildIndex >= 0 ? sceneBuildIndex : SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(ReloadAfter(reloadDelay, scene));
    }

    private void ShowDeath()
    {
        var animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }

        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null && SpriteDead != null)
        {
            spriteRenderer.sprite = SpriteDead;
        }

        if (ButUp != null)
        {
            var jumpTrigger = ButUp.GetComponent<EventTrigger>();
            if (jumpTrigger != null)
            {
                jumpTrigger.enabled = false;
            }
        }

        var accelerometerControl = GetComponent<AccelerContr>();
        if (accelerometerControl != null)
        {
            accelerometerControl.enabled = false;
        }

        var playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    private static IEnumerator ReloadAfter(float delay, int sceneBuildIndex)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        SceneManager.LoadScene(sceneBuildIndex);
    }
}
