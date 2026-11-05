using UnityEngine;

namespace DarknessInside.UI
{
    /// <summary>
    /// Выключает объект на указанных платформах. Например, кнопку «Выход» в браузере:
    /// Application.Quit там ничего не делает, и кнопка выглядела бы сломанной.
    /// </summary>
    public sealed class HideOnPlatform : MonoBehaviour
    {
        [SerializeField] private RuntimePlatform[] platforms = { RuntimePlatform.WebGLPlayer };

        [Tooltip("Соседние элементы, которые сдвигаются, чтобы закрыть место скрытого объекта.")]
        [SerializeField] private RectTransform[] shiftWhenHidden = new RectTransform[0];
        [SerializeField] private Vector2 shift;

        private void Awake()
        {
            RuntimePlatform current = Application.platform;
            for (int i = 0; i < platforms.Length; i++)
            {
                if (platforms[i] == current)
                {
                    for (int j = 0; j < shiftWhenHidden.Length; j++)
                    {
                        if (shiftWhenHidden[j] != null)
                        {
                            shiftWhenHidden[j].anchoredPosition += shift;
                        }
                    }

                    gameObject.SetActive(false);
                    return;
                }
            }
        }
    }
}
