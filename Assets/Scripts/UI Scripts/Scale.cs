using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Шкала, которая постепенно убывает; когда опустеет — игрок погибает.
/// Скорость задаётся в секунду, поэтому не зависит от FPS. На паузе шкала стоит.
/// </summary>
public class Scale : MonoBehaviour
{
    public Image fillingScale;
    public GameObject diePlayer;

    [Tooltip("Какая доля шкалы убывает за секунду (0.01 — вся шкала за 100 с).")]
    [SerializeField] private float drainPerSecond = 0.003f;

    private DeadPlayer _player;
    private Die _die;

    private void Awake()
    {
        _player = diePlayer != null ? diePlayer.GetComponent<DeadPlayer>() : null;
        _die = GetComponent<Die>();
    }

    private void OnEnable()
    {
        if (fillingScale != null)
        {
            StartCoroutine(Drain());
        }
    }

    private IEnumerator Drain()
    {
        while (fillingScale.fillAmount > 0f)
        {
            fillingScale.fillAmount -= drainPerSecond * Time.deltaTime;
            yield return null;
        }

        if (_die != null)
        {
            _die.Kill(_player);
        }
        else if (_player != null)
        {
            _player.Kill(1f);
        }
    }
}
