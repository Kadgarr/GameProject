using DarknessInside.Gameplay.Levels;
using UnityEngine;

/// <summary>Выход с уровня: сообщает LevelFlow, что уровень пройден.</summary>
public class NextLevelScript : MonoBehaviour
{
    private const string PlayerTag = "Player";

    [SerializeField] private LevelFlow levelFlow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (levelFlow != null && other.CompareTag(PlayerTag))
        {
            levelFlow.CompleteLevel();
        }
    }
}
