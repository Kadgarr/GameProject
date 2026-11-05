using UnityEngine;

namespace DarknessInside.Core
{
    /// <summary>
    /// Внешние ссылки игры (политика конфиденциальности и т. п.) в одном ассете:
    /// адрес меняется в инспекторе, без правок кода и сцен.
    /// </summary>
    [CreateAssetMenu(fileName = "ExternalLinks", menuName = "Darkness Inside/External Links")]
    public sealed class ExternalLinks : ScriptableObject
    {
        [Tooltip("Публичная страница политики конфиденциальности (та же, что в Play Console).")]
        [SerializeField] private string privacyPolicyUrl = "https://kadgarr.github.io/GameProject/privacy-policy.html";

        public string PrivacyPolicyUrl => privacyPolicyUrl;
    }
}
