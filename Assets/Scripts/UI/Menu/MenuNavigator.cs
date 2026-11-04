using System;
using UnityEngine;

namespace DarknessInside.UI.Menu
{
    /// <summary>
    /// Переключает экраны главного меню (State: в каждый момент виден один экран + при необходимости окно Info).
    /// Кнопка «Назад» Android обрабатывается только пока открыт не главный экран — в остальное время Update выключен.
    /// </summary>
    public sealed class MenuNavigator : MonoBehaviour
    {
        [Tooltip("Все экраны меню, включая Info.")]
        [SerializeField] private MenuScreen[] screens = new MenuScreen[0];

        [SerializeField] private MenuScreenId startScreen = MenuScreenId.Main;

        private MenuScreenId _current;
        private bool _infoOpen;

        public MenuScreenId Current => _current;

        public bool InfoOpen => _infoOpen;

        public event Action<MenuScreenId> ScreenChanged;

        private void Awake()
        {
            Show(startScreen);
        }

        /// <summary>Показать экран (Main, Levels, Options). Окно Info при этом закрывается.</summary>
        public void Show(MenuScreenId id)
        {
            if (id == MenuScreenId.Info)
            {
                SetInfoOpen(true);
                return;
            }

            for (int i = 0; i < screens.Length; i++)
            {
                MenuScreen screen = screens[i];
                if (screen != null && screen.Id != MenuScreenId.Info)
                {
                    screen.SetVisible(screen.Id == id);
                }
            }

            _current = id;
            SetInfoOpen(false);
            ScreenChanged?.Invoke(id);
        }

        public void SetInfoOpen(bool open)
        {
            for (int i = 0; i < screens.Length; i++)
            {
                if (screens[i] != null && screens[i].Id == MenuScreenId.Info)
                {
                    screens[i].SetVisible(open);
                }
            }

            _infoOpen = open;
            enabled = _infoOpen || _current != MenuScreenId.Main;
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape))
            {
                return;
            }

            if (_infoOpen)
            {
                SetInfoOpen(false);
            }
            else
            {
                Show(MenuScreenId.Main);
            }
        }
    }
}
