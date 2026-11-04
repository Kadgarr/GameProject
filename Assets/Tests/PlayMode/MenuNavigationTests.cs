using System.Collections;
using System.Collections.Generic;
using DarknessInside.UI.Menu;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DarknessInside.Tests
{
    /// <summary>
    /// Главное меню: в каждый момент виден один экран, окно Info перекрывает нажатия под собой,
    /// «Назад» возвращает на главный экран.
    /// </summary>
    public class MenuNavigationTests
    {
        private MenuNavigator _navigator;
        private MenuScreen[] _screens;

        [UnitySetUp]
        public IEnumerator LoadMenu()
        {
            yield return SceneManager.LoadSceneAsync("SceneMenu", LoadSceneMode.Single);
            yield return null;
            _navigator = Object.FindFirstObjectByType<MenuNavigator>();
            _screens = Object.FindObjectsByType<MenuScreen>(FindObjectsSortMode.None);
            Assert.That(_navigator, Is.Not.Null, "В SceneMenu нет MenuNavigator.");
            Assert.That(_screens.Length, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator Show_MakesOnlyThatScreenVisible()
        {
            Assert.That(VisibleScreens(), Is.EquivalentTo(new[] { MenuScreenId.Main }));

            _navigator.Show(MenuScreenId.Levels);
            yield return null;
            Assert.That(VisibleScreens(), Is.EquivalentTo(new[] { MenuScreenId.Levels }));

            _navigator.Show(MenuScreenId.Options);
            yield return null;
            Assert.That(VisibleScreens(), Is.EquivalentTo(new[] { MenuScreenId.Options }));

            _navigator.Show(MenuScreenId.Main);
            yield return null;
            Assert.That(VisibleScreens(), Is.EquivalentTo(new[] { MenuScreenId.Main }));
            Assert.That(_navigator.enabled, Is.False, "На главном экране Update навигатора должен быть выключен.");
        }

        [UnityTest]
        public IEnumerator Info_BlocksButtonsBelow()
        {
            var center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            _navigator.SetInfoOpen(true);
            yield return null;

            GameObject top = TopHit(center);
            Assert.That(top, Is.Not.Null);
            MenuScreen owner = top.GetComponentInParent<MenuScreen>();
            Assert.That(owner != null && owner.Id == MenuScreenId.Info, "Верхний объект под пальцем — не окно Info: " + top.name);

            _navigator.SetInfoOpen(false);
            yield return null;
            Assert.That(VisibleScreens(), Is.EquivalentTo(new[] { MenuScreenId.Main }));
        }

        private List<MenuScreenId> VisibleScreens()
        {
            var list = new List<MenuScreenId>();
            foreach (MenuScreen screen in _screens)
            {
                if (screen.IsVisible)
                {
                    list.Add(screen.Id);
                }
            }

            return list;
        }

        private static GameObject TopHit(Vector2 position)
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = position }, results);
            return results.Count > 0 ? results[0].gameObject : null;
        }
    }
}
