using UnityEngine;
using System.Collections;

public class LavaBubbleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private int maxBubbles = 20;
    [SerializeField] private float bubbleLifetime = 3f;

    private BoxCollider2D lavaCollider;

    private void Start()
    {
        lavaCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(SpawnBubbles());
    }

    private IEnumerator SpawnBubbles()
    {
        while (true)
        {
            // pick a random point inside lava box
            Vector2 randomPos = new Vector2(
                Random.Range(lavaCollider.bounds.min.x, lavaCollider.bounds.max.x),
                Random.Range(lavaCollider.bounds.min.y, lavaCollider.bounds.max.y)
            );

            // count current bubbles
            if (GameObject.FindGameObjectsWithTag("LavaBubble").Length < maxBubbles)
            {
                GameObject bubble = Instantiate(bubblePrefab, randomPos, Quaternion.identity);
                bubble.tag = "LavaBubble";
                Destroy(bubble, bubbleLifetime);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
