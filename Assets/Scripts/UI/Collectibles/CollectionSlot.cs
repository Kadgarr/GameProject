using System.Collections;
using DarknessInside.Gameplay.Collectibles;
using UnityEngine;
using UnityEngine.UI;

namespace DarknessInside.UI.Collectibles
{
    /// <summary>Слот HUD: иконка предмета + галочка, когда предмет собран.</summary>
    public sealed class CollectionSlot : MonoBehaviour
    {
        [SerializeField] private CollectibleDefinition item;

        [Header("Ссылки")]
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private Image checkmark;

        [Header("Вид")]
        [SerializeField] private Color pendingIconColor = new Color(1f, 1f, 1f, 0.35f);
        [SerializeField] private Color collectedIconColor = Color.white;
        [SerializeField] private Color missingFlashColor = new Color(1f, 0.25f, 0.25f, 1f);
        [SerializeField] private float popScale = 1.25f;
        [SerializeField] private float animationDuration = 0.25f;

        private Color _backgroundColor = Color.white;
        private Coroutine _animation;

        public CollectibleDefinition Item => item;

        private void Awake()
        {
            if (background != null)
            {
                _backgroundColor = background.color;
            }

            ApplyIcon();
        }

        private void OnDisable()
        {
            _animation = null;
            transform.localScale = Vector3.one;
            if (background != null)
            {
                background.color = _backgroundColor;
            }
        }

        public void SetCollected(bool collected, bool animate)
        {
            if (checkmark != null)
            {
                checkmark.enabled = collected;
            }

            if (icon != null)
            {
                icon.color = collected ? collectedIconColor : pendingIconColor;
            }

            if (collected && animate)
            {
                Play(Pop());
            }
        }

        public void FlashMissing() => Play(Flash());

        private void Play(IEnumerator routine)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            if (_animation != null)
            {
                StopCoroutine(_animation);
            }

            _animation = StartCoroutine(routine);
        }

        private IEnumerator Pop()
        {
            Transform target = transform;
            for (float t = 0f; t < animationDuration; t += Time.unscaledDeltaTime)
            {
                float scale = 1f + (popScale - 1f) * Mathf.Sin(t / animationDuration * Mathf.PI);
                target.localScale = new Vector3(scale, scale, 1f);
                yield return null;
            }

            target.localScale = Vector3.one;
            _animation = null;
        }

        private IEnumerator Flash()
        {
            if (background == null)
            {
                yield break;
            }

            for (int i = 0; i < 2; i++)
            {
                for (float t = 0f; t < animationDuration; t += Time.unscaledDeltaTime)
                {
                    background.color = Color.Lerp(missingFlashColor, _backgroundColor, t / animationDuration);
                    yield return null;
                }
            }

            background.color = _backgroundColor;
            _animation = null;
        }

        private void ApplyIcon()
        {
            if (icon != null && item != null)
            {
                icon.sprite = item.Icon;
            }
        }

#if UNITY_EDITOR
        // Иконка сразу видна в редакторе при выборе предмета в инспекторе.
        private void OnValidate() => ApplyIcon();
#endif
    }
}
