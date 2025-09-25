using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpened)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            isOpened = true;
            OpenChest(collision.GetComponent<Player>());
        }
    }

    private void OpenChest(Player player)
    {
        Destroy(gameObject); 
    }
}
