using UnityEngine;

namespace DarknessInside.Gameplay.Player
{
    /// <summary>
    /// Клавиши для экранной кнопки — для игры на компьютере (WebGL в браузере, редактор).
    /// Нажимает ту же ControlButton, что и палец. На телефоне (в том числе в мобильном браузере)
    /// компонент выключается и не опрашивает клавиатуру. В браузере на компьютере экранная кнопка
    /// становится невидимой, но продолжает работать от клавиш.
    /// </summary>
    [RequireComponent(typeof(ControlButton))]
    public sealed class KeyboardButtonBinding : MonoBehaviour
    {
        [SerializeField] private KeyCode[] keys = new KeyCode[0];

        [Tooltip("Скрыть экранную кнопку в браузере на компьютере (управление — клавишами).")]
        [SerializeField] private bool hideOnDesktopWeb = true;

        private ControlButton _button;

        private void Awake()
        {
            _button = GetComponent<ControlButton>();
            if (Application.isMobilePlatform)
            {
                enabled = false;
                return;
            }

            if (hideOnDesktopWeb && Application.platform == RuntimePlatform.WebGLPlayer)
            {
                CanvasGroup group = GetComponent<CanvasGroup>();
                if (group == null)
                {
                    group = gameObject.AddComponent<CanvasGroup>();
                }

                group.alpha = 0f;
                group.blocksRaycasts = false;
            }
        }

        private void Update()
        {
            bool held = false;
            for (int i = 0; i < keys.Length; i++)
            {
                if (Input.GetKey(keys[i]))
                {
                    held = true;
                    break;
                }
            }

            _button.SetKeyHeld(held);
        }

        private void OnDisable()
        {
            if (_button != null)
            {
                _button.SetKeyHeld(false);
            }
        }
    }
}
