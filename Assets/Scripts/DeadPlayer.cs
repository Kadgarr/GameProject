using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Смерть игрока: «мёртвый» спрайт и отключение управления. О смерти сообщает событием Died —
/// перезапуск уровня делает LevelFlow. Если LevelFlow в сцене нет, уровень перезапускается сам.
/// </summary>
public class DeadPlayer : MonoBehaviour
{
    public Sprite SpriteDead;

    [Tooltip("Необязательно: кнопки управления, которые выключаются после смерти.")]
    public GameObject ButRight;
    public GameObject ButLeft;
    public GameObject ButUp;

    private bool _isDead;

    /// <summary>Игрок погиб. Аргумент — через сколько секунд перезапустить уровень.</summary>
    public event Action<float> Died;

    public bool IsDead => _isDead;

    public void Kill(float restartDelay)
    {
        if (_isDead)
        {
            return;
        }

        _isDead = true;
        ShowDeath();

        if (Died != null)
        {
            Died(restartDelay);
        }
        else
        {
            StartCoroutine(ReloadAfter(restartDelay));
        }
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

    private static IEnumerator ReloadAfter(float delay)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
