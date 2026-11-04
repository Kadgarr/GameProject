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

    private IEnumerator Cycle()
    {
        bool startsDown = (transform.position - posYDown).sqrMagnitude < (transform.position - posYUp).sqrMagnitude;
        if (startsDown)
        {
            yield return WaitForBeat();
            yield return Move(posYDown, posYUp);
        }

        while (true)
        {
            yield return WaitForBeat();
            yield return Move(posYUp, posYDown);
            yield return WaitForBeat();
            yield return Move(posYDown, posYUp);
        }
    }

    private IEnumerator WaitForBeat()
    {
        int period = Mathf.Max(1, Mathf.RoundToInt(second));
        while (Mathf.FloorToInt(Time.timeSinceLevelLoad) % period != 0)
        {
            yield return FixedStep;
        }
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float safeStep = Mathf.Max(step, 0.001f);
        for (float progress = 0f; progress < 1f; progress += safeStep)
        {
            transform.position = Vector3.Lerp(from, to, progress);
            yield return FixedStep;
        }

        transform.position = to;
    }
}
