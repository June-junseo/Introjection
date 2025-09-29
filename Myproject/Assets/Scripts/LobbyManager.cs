using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject characterPrefab;
    public Transform spawnPoint;      
    public Vector3 characterScale = Vector3.one * 5f; 

    void Start()
    {
        if (characterPrefab != null && spawnPoint != null)
        {
            GameObject character = Instantiate(characterPrefab, spawnPoint.position, spawnPoint.rotation);
            character.transform.localScale = characterScale;
        }
    }
}
