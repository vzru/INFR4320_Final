using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;

    public Transform SpawnPoint;
    public float timeBetweenWaves = 5f;
    private float countdown = 2f;
    private int WaveIndex = 1;
    public Text WaveCountDownText;
    void Update()
    {
        if (countdown<= 0f)
        {

            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        countdown -= Time.deltaTime;

        WaveCountDownText.text = Mathf.Round(countdown).ToString();
    }



    IEnumerator SpawnWave()
    {
        WaveIndex++;
        for (int i = 0; i < WaveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }


    }
     
    void SpawnEnemy()
    {
        if (SpawnPoint != null)
        {
            Instantiate(enemyPrefab, SpawnPoint.position, SpawnPoint.rotation);
        }
    }
}
