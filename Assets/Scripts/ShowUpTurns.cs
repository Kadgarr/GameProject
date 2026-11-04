using System.Collections;
using UnityEngine;

/// <summary>
/// Шипы, которые по циклу выдвигаются и прячутся. Движение начинается в секунды уровня, кратные
/// периоду, поэтому все такие шипы на уровне работают синхронно (раньше — по системным часам).
/// Работает корутиной по шагам физики, без Update.
/// </summary>
public class ShowUpTurns : MonoBehaviour
{
    private static readonly WaitForFixedUpdate FixedStep = new WaitForFixedUpdate();

    [Tooltip("Доля пути за шаг физики: 0.02 — ход длится 50 шагов (1 секунда).")]
    public float step = 0.02f;

    [Tooltip("Период в секундах: ход начинается в секунды уровня, кратные этому числу.")]
    public float second = 3f;

    public Vector3 posYUp;
    public Vector3 posYDown;

    private void OnEnable()
    {
        StartCoroutine(Cycle());
    }

    // Один цикл без вложенных корутин: вложенные IEnumerator создавали мусор на каждом ходе.
    private IEnumerator Cycle()
    {
        int period = Mathf.Max(1, Mathf.RoundToInt(second));
        float safeStep = Mathf.Max(step, 0.001f);
        bool atTop = (transform.position - posYDown).sqrMagnitude >= (transform.position - posYUp).sqrMagnitude;

        while (true)
        {
            while (Mathf.FloorToInt(Time.timeSinceLevelLoad) % period != 0)
            {
                yield return FixedStep;
            }

            Vector3 from = atTop ? posYUp : posYDown;
            Vector3 to = atTop ? posYDown : posYUp;
            for (float progress = 0f; progress < 1f; progress += safeStep)
            {
                transform.position = Vector3.Lerp(from, to, progress);
                yield return FixedStep;
            }

            transform.position = to;
            atTop = !atTop;
        }
    }
}
