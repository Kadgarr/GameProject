using UnityEngine;
using UnityEngine.UI;

/// <summary>Шкала, которая постепенно убывает; когда опустеет — игрок погибает.</summary>
public class Scale : MonoBehaviour
{
    public Image fillingScale;

    public GameObject diePlayer;

    // TODO(этап 3): убывание за кадр зависит от FPS — перевести на «в секунду» вместе с переходом на 60 FPS.
    public float filling = 0.0001f;

    private bool _expired;

    private void Update()
    {
        if (_expired)
        {
            return;
        }

        fillingScale.fillAmount -= filling;
        if (fillingScale.fillAmount > 0f)
        {
            return;
        }

        _expired = true;
        DeadPlayer deadPlayer = diePlayer != null ? diePlayer.GetComponent<DeadPlayer>() : null;
        var die = GetComponent<Die>();
        if (die != null)
        {
            die.KillPlayer(deadPlayer);
        }
        else if (deadPlayer != null)
        {
            deadPlayer.Kill(1f);
        }
    }
}
