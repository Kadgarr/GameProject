using System.Collections;
using DarknessInside.Gameplay.Player;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace DarknessInside.Tests
{
    /// <summary>Движение игрока от экранных кнопок, прыжок, смерть и «перепутанные» кнопки уровня 14.</summary>
    public sealed class PlayerMotorTests
    {
        [UnityTest]
        public IEnumerator RightButton_MovesPlayerRight_AndRuns()
        {
            yield return Load("Level_1");
            PlayerMotor motor = Object.FindFirstObjectByType<PlayerMotor>();
            float startX = motor.transform.position.x;

            // Справа от старта уровня 1 яма, поэтому смотрим первые 0.15 с: разгон и бег.
            Press("ButtonRight");
            string trace = string.Empty;
            for (int step = 0; step < 8; step++)
            {
                yield return new WaitForFixedUpdate();
                trace += motor.GetComponent<Rigidbody2D>().linearVelocity.x.ToString("0.0") + "@" + Time.fixedTime.ToString("0.00") + "/" + motor.IsGrounded + " ";
            }

            Assert.Greater(motor.GetComponent<Rigidbody2D>().linearVelocity.x, 3f, trace);
            Assert.Greater(motor.transform.position.x, startX + 0.3f);
            Assert.AreEqual(PlayerMotor.State.Run, motor.CurrentState);

            Release("ButtonRight");
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(PlayerMotor.State.Idle, motor.CurrentState);
        }

        [UnityTest]
        public IEnumerator JumpButton_LiftsPlayer()
        {
            yield return Load("Level_1");
            PlayerMotor motor = Object.FindFirstObjectByType<PlayerMotor>();
            yield return new WaitUntil(() => motor.IsGrounded);
            Rigidbody2D body = motor.GetComponent<Rigidbody2D>();

            // На старте уровня 1 над игроком низкий выступ, поэтому проверяем импульс, а не высоту.
            Press("ButtonUp");
            yield return new WaitForFixedUpdate();

            Assert.Greater(body.linearVelocity.y, 5f);
            Assert.AreEqual(PlayerMotor.State.Jump, motor.CurrentState);
        }

        [UnityTest]
        public IEnumerator Level14_ButtonsAreSwapped()
        {
            yield return Load("Level_14");
            PlayerMotor motor = Object.FindFirstObjectByType<PlayerMotor>();
            float startX = motor.transform.position.x;

            // На уровне 14 кнопка «вправо» ведёт влево.
            Press("ButtonRight");
            yield return new WaitForSeconds(0.3f);
            Release("ButtonRight");

            Assert.Less(motor.transform.position.x, startX - 0.3f);
        }

        [UnityTest]
        public IEnumerator DeadPlayer_IgnoresInput()
        {
            yield return Load("Level_1");
            PlayerMotor motor = Object.FindFirstObjectByType<PlayerMotor>();
            motor.GetComponent<DeadPlayer>().Kill(5f);
            float startX = motor.transform.position.x;

            Press("ButtonRight");
            yield return new WaitForSeconds(0.3f);

            Assert.AreEqual(PlayerMotor.State.Dead, motor.CurrentState);
            Assert.AreEqual(startX, motor.transform.position.x, 0.01f);
        }

        [UnityTest]
        public IEnumerator MovingPlatform_CarriesPlayer()
        {
            yield return Load("Level_4");
            PlatformMove platform = null;
            foreach (PlatformMove candidate in Object.FindObjectsByType<PlatformMove>(FindObjectsSortMode.None))
            {
                if (!candidate.DirectionUp && candidate.CompareTag("PlatformMove"))
                {
                    platform = candidate;
                    break;
                }
            }

            Assert.IsNotNull(platform, "horizontal moving platform not found");
            PlayerMotor motor = Object.FindFirstObjectByType<PlayerMotor>();
            Rigidbody2D body = motor.GetComponent<Rigidbody2D>();
            Bounds top = platform.GetComponent<Collider2D>().bounds;
            body.position = new Vector2(top.center.x, top.max.y + 0.7f);

            float waited = 0f;
            while (!motor.IsGrounded && waited < 1f)
            {
                waited += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            Assert.IsTrue(motor.IsGrounded, "player did not land on the platform");
            float offset = body.position.x - platform.transform.position.x;
            float platformStart = platform.transform.position.x;

            yield return new WaitForSeconds(0.3f);

            Assert.Greater(Mathf.Abs(platform.transform.position.x - platformStart), 0.2f, "platform did not move");
            Assert.AreEqual(offset, body.position.x - platform.transform.position.x, 0.15f);
        }

        private static void Press(string button)
        {
            Find(button).OnPointerDown(new PointerEventData(EventSystem.current));
        }

        private static void Release(string button)
        {
            Find(button).OnPointerUp(new PointerEventData(EventSystem.current));
        }

        private static ControlButton Find(string name)
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
