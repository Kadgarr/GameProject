using UnityEngine;

public class LevelArm : MonoBehaviour
{
    public GameObject CanvasButton;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CanvasButton.SetActive(true);
        }
    }
}
