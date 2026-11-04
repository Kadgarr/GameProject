using System.Collections;
using DarknessInside.Core;
using DarknessInside.Gameplay.Player;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DarknessInside.Tests
{
    /// <summary>Пауза, шкала уровня, падающая платформа и выдвижные шипы.</summary>
    public sealed class PuzzlesAndPauseTests
    {
        [TearDown]
        public void Unpause()
        {
            GamePause.Set(false);
        }

        [UnityTest]
        public IEnumerator Pause_StopsTime_AndReleasesControls()
        {
            yield return Load("Level_1");
            ControlButton right = FindButton("ButtonRight");
            right.OnPointerDown(new PointerEventData(EventSystem.current));
            ButtonsInput input = Object.FindFirstObjectByType<ButtonsInput>();
            Assert.AreEqual(1f, input.Horizontal);

            EscapeMenu menu = Object.FindFirstObjectByType<EscapeMenu>();
            menu.Pause();

            Assert.AreEqual(0f, Time.timeScale);
            Assert.AreEqual(0f, input.Horizontal, "held button must be released on pause");
            Assert.IsFalse(right.GetComponentInParent<CanvasGroup>().blocksRaycasts);

            menu.Continue();
            Assert.AreEqual(1f, Time.timeScale);
            Assert.IsTrue(right.GetComponentInParent<CanvasGroup>().blocksRaycasts);
        }

        [UnityTest]
        public IEnumerator Scale_DrainsPerSecond_AndStopsOnPause()
        {
            yield return Load("Level_14");
            Scale scale = Object.FindFirstObjectByType<Scale>();
            float start = scale.fillingScale.fillAmount;

            yield return new WaitForSeconds(1f);
            float afterSecond = scale.fillingScale.fillAmount;
            Assert.AreEqual(0.03f, start - afterSecond, 0.01f);

            GamePause.Set(true);
            yield return new WaitForSecondsRealtime(0.3f);
            Assert.AreEqual(afterSecond, scale.fillingScale.fillAmount, 0.002f);
        }

        [UnityTest]
        public IEnumerator FallPlatform_FallsAfterDelay()
        {
            yield return Load("Level_8");
            FallPlatform platform = Object.FindFirstObjectByType<FallPlatform>();
            Collider2D player = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
            Rigidbody2D body = platform.GetComponent<Rigidbody2D>();

            platform.SendMessage("OnTriggerEnter2D", player);
            yield return new WaitForSeconds(platform.timer * 0.5f);
            Assert.AreNotEqual(RigidbodyType2D.Dynamic, body.bodyType);

            yield return new WaitForSeconds(platform.timer * 0.5f + 0.1f);
            Assert.AreEqual(RigidbodyType2D.Dynamic, body.bodyType);
        }

        [UnityTest]
        public IEnumerator ShowUpTurns_MovesOnBeat()
        {
            yield return Load("Level_15");
            ShowUpTurns spikes = Object.FindFirstObjectByType<ShowUpTurns>();
            Vector3 start = spikes.transform.position;

            float maxDistance = 0f;
            float elapsed = 0f;
            while (elapsed < 4.5f)
            {
                maxDistance = Mathf.Max(maxDistance, Vector3.Distance(start, spikes.transform.position));
                elapsed += Time.deltaTime;
                yield return null;
            }

            Assert.Greater(maxDistance, 0.3f, "spikes did not move within one period");
        }

        private static ControlButton FindButton(string name)
        {
            foreach (ControlButton button in Object.FindObjectsByType<ControlButton>(FindObjectsSortMode.None))
            {
                if (button.name == name)
                {
                    return button;
                }
            }

            Assert.Fail("ControlButton " + name + " not found");
            return null;
        }

        private static IEnumerator Load(string scene)
        {
            yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            yield return null;
        }
    }
}
