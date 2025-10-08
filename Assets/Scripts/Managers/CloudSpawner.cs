using System.Collections;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject cloudPrefab;
    public float minY = -2f, maxY = 2f;
    public float minSpawnTime = 1f, maxSpawnTime = 3f;
    public float minScale = 0.5f, maxScale = 1.5f;
    public float minAlpha = 0.3f, maxAlpha = 1f;
    public int minLayer = -2, maxLayer = -82;
    public float spawnX = -10f; // ponto de spawn (fora da tela à esquerda)

    public bool isCloud = true;

    void Start()
    {
        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

            Vector3 spawnPosition = new Vector3(spawnX, Random.Range(minY, maxY), 0f);
            GameObject cloud = Instantiate(cloudPrefab, spawnPosition, Quaternion.identity);
            if(isCloud)
            cloud.GetComponent<CloudMovement>().speed = Random.Range(1, 5);

            // Escala aleatória
            float scale = Random.Range(minScale, maxScale);
            cloud.transform.localScale = new Vector3(scale, scale, 1f);

            // Opacidade aleatória
            SpriteRenderer sr = cloud.GetComponent<SpriteRenderer>();
            sr.sortingOrder = Random.Range(minLayer,maxLayer);
            Color color = sr.color;
            color.a = Random.Range(minAlpha, maxAlpha);
            sr.color = color;
        }
    }
}
