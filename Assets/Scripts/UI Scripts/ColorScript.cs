using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Постепенно растворяет картинки (затемнение уровня). Скорость задаётся в секунду, поэтому
/// не зависит от FPS; когда картинки исчезли, компонент больше ничего не делает.
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
        float alpha = 1f;
        Apply(alpha);
        while (alpha > 0f)
        {
            alpha = Mathf.Max(0f, alpha - fadePerSecond * Time.deltaTime);
            Apply(alpha);
            yield return null;
        }
    }

    private void Apply(float alpha)
    {
        var color = new Color(1f, 1f, 1f, alpha);
        for (int i = 0; i < _images.Count; i++)
        {
            _images[i].color = color;
        }
    }
}

[System.Serializable]
public class ColoredElement
{
    public GameObject element;
}
