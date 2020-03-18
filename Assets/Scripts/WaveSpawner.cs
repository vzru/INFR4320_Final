//Regan Tran 100622360
//Mathooshan Thevakumaran 100553777
//Victor Zhang 100421055

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

    //countdown timer inbetween each wave
    void Update()
    {
        if (countdown<= 0f)
        {

            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        countdown -= Time.deltaTime;
        //text UI for game screen
        WaveCountDownText.text = Mathf.Round(countdown).ToString();
    }


    //how often the wave spawns
    IEnumerator SpawnWave()
    {
        WaveIndex++;
        for (int i = 0; i < WaveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }


    }
    //instantiate enemy
    void SpawnEnemy()
    {
        if (SpawnPoint != null)
        {
            Instantiate(enemyPrefab, SpawnPoint.position, SpawnPoint.rotation);
        }
    }
}
