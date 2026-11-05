using DarknessInside.Core;
using UnityEngine;

namespace DarknessInside.UI.Menu
{
    /// <summary>Кнопка или текст-ссылка «Политика конфиденциальности»: открывает страницу в браузере.</summary>
    public sealed class PrivacyPolicyLink : MonoBehaviour
    {
        [SerializeField] private ExternalLinks links;

        /// <summary>Вызывается кнопкой.</summary>
        public void Open()
        {
            if (links == null || string.IsNullOrEmpty(links.PrivacyPolicyUrl))
            {
                Debug.LogWarning("PrivacyPolicyLink: не задан адрес политики в ExternalLinks.", this);
                return;
            }

            Application.OpenURL(links.PrivacyPolicyUrl);
        }
    }
}
