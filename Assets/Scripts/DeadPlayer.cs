using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Смерть игрока: «мёртвый» спрайт и событие Died. На событие реагируют PlayerMotor (перестаёт
/// слушать управление) и LevelFlow (перезапускает уровень). Если LevelFlow в сцене нет,
/// уровень перезапускается сам.
/// </summary>
public class DeadPlayer : MonoBehaviour
{
    public Sprite SpriteDead;

    private bool _isDead;

    /// <summary>Игрок погиб. Аргумент — через сколько секунд перезапустить уровень.</summary>
    public event Action<float> Died;

    public bool IsDead => _isDead;

    /// <summary>Что убило игрока (для аналитики): thorns, fall, falling_object, timer…</summary>
    public string Cause { get; private set; } = UnknownCause;

    public const string UnknownCause = "unknown";

    public void Kill(float restartDelay)
    {
        Kill(restartDelay, UnknownCause);
    }

    public void Kill(float restartDelay, string cause)
    {
        if (_isDead)
        {
            return;
        }

        _isDead = true;
        Cause = string.IsNullOrEmpty(cause) ? UnknownCause : cause;
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
