using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Постепенно растворяет картинки (цветной слой уходит, под ним остаётся серый). Скорость задаётся в секунду,
/// поэтому не зависит от FPS. Прозрачность меняется через CanvasRenderer.SetAlpha — канвас не перестраивает
/// меш каждый кадр. Когда картинка исчезла полностью, её Image выключается и больше не рисуется (нет overdraw).
/// </summary>
public class ColorScript : MonoBehaviour
{
    public List<ColoredElement> elements = new List<ColoredElement>();

    [Tooltip("На сколько уменьшается прозрачность за секунду (0.01 — полностью за 100 с).")]
    [SerializeField] private float fadePerSecond = 0.003f;

    private readonly List<Image> _images = new List<Image>();

    private void Awake()
    {
        foreach (ColoredElement element in elements)
        {
            Image image = element.element != null ? element.element.GetComponent<Image>() : null;
            if (image != null)
            {
                _images.Add(image);
            }
        }
    }

    private void OnEnable()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        SetEnabled(true);
        float alpha = 1f;
        Apply(alpha);
        while (alpha > 0f)
        {
            yield return null;
            alpha = Mathf.Max(0f, alpha - fadePerSecond * Time.deltaTime);
            Apply(alpha);
        }

        SetEnabled(false);
    }

    private void Apply(float alpha)
    {
        for (int i = 0; i < _images.Count; i++)
        {
            _images[i].canvasRenderer.SetAlpha(alpha);
        }
    }

    private void SetEnabled(bool value)
    {
        for (int i = 0; i < _images.Count; i++)
        {
            _images[i].enabled = value;
        }
    }
}

[System.Serializable]
public class ColoredElement
{
    public GameObject element;
}
