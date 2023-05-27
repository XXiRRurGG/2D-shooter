using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;
using TMPro;

public class GameControls : MonoBehaviour
{
    public GameObject player;
    public Canvas canvas;
    public Canvas ui;
    public int score = 0;
    public List<Transform> obstacles = new List<Transform>();
    public GameObject obstacle;
    public List<Transform> pickUps = new List<Transform>();
    public GameObject[] pickUpPrefabs;
    public List<Transform> enemyPositions = new List<Transform>();
    public GameObject[] enemies;

    private List<Transform> occupiedSpawnPoints = new List<Transform>();

    private NewPhysicMovement playerData;
    private void OnEnable()
    {
        playerData = player.GetComponent<NewPhysicMovement>();
        playerData.OnDeath += GameEnd;
    }
    void Start()
    {
        StartCoroutine(SpawnObstacleWithTimer());
        StartCoroutine(SpawnEnemyWithTimer());
        StartCoroutine(SpawnPickUpWithTimer());
        StartCoroutine(CountScore());
    }
    private void Update()
    {
        var hpText = GameObject.Find("hpText").GetComponent<TMP_Text>();
        var scoreText = GameObject.Find("scoreText").GetComponent<TMP_Text>();
        var damageText = GameObject.Find("damageText").GetComponent<TMP_Text>();

        hpText.text = "Hp:" + playerData.health;
        damageText.text = "Dmg:" + playerData.damage;
        scoreText.text = "Score:" + score;
    }
    IEnumerator CountScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            score += 10;
        }
    }
    void GameEnd()
    {
        Debug.Log("Death called");
        canvas.gameObject.SetActive(true);
    }
    private void SpawnPickUp()
    {
        Transform randomSpawnPoint = GetRandomSpawnPoint();

        if (randomSpawnPoint != null)
        {

            System.Random rand = new System.Random();
            var obj = pickUpPrefabs[rand.Next(pickUpPrefabs.Length)];
            GameObject spawnedObject = Instantiate(obj, randomSpawnPoint.position, randomSpawnPoint.rotation);

            occupiedSpawnPoints.Add(randomSpawnPoint);

            StrengthPickUp strengthPickUp;
            if (spawnedObject.TryGetComponent<StrengthPickUp>(out strengthPickUp))
            {
                spawnedObject.GetComponent<StrengthPickUp>().OnPickUp += () => ReleaseSpawnPoint(randomSpawnPoint);
            }
            else
            {
                spawnedObject.GetComponent<HealthPickUp>().OnPickUp += () => ReleaseSpawnPoint(randomSpawnPoint);
            }
        }
    }
    private Transform GetRandomSpawnPoint()
    {
        List<Transform> availableSpawnPoints = new List<Transform>();

       
        foreach (Transform spawnPoint in pickUps)
        {
            if (!occupiedSpawnPoints.Contains(spawnPoint))
            {
                availableSpawnPoints.Add(spawnPoint);
            }
        }

        if (availableSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            return availableSpawnPoints[randomIndex];
        }

        return null;
    }
    private void ReleaseSpawnPoint(Transform spawnPoint)
    {
        occupiedSpawnPoints.Remove(spawnPoint);
        score += 100;
    }
    private void SpawnEnemy()
    {
        System.Random rand = new System.Random();
        var enemy = Instantiate(enemies[rand.Next(enemies.Length)], enemyPositions[rand.Next(enemyPositions.Count)].position, Quaternion.identity);
        var script = NewPhysicMovement.GetEnemyData(enemy);
        script.hp = 15;
        AIDestinationSetter pathScript;
        if(enemy.TryGetComponent<AIDestinationSetter>(out pathScript))
        {
            pathScript.target = GameObject.Find("Player").gameObject.transform;
        }
        else
        {
            var anotherScript = enemy.GetComponent<AdvancedEnemy>();
            anotherScript.target = GameObject.Find("Player").gameObject.transform;
        }
    }
    private void SpawnObstacle()
    {
        System.Random rand = new System.Random();
        var position = obstacles[rand.Next(obstacles.Count)];
        Instantiate(obstacle, position.position, Quaternion.identity);
        obstacles.Remove(position);
    }
    private IEnumerator SpawnObstacleWithTimer()
    {
        while (obstacles.Count > 0)
        {
            SpawnObstacle();
            yield return new WaitForSeconds(15f);
        }
    }
    private IEnumerator SpawnEnemyWithTimer()
    {
        while(true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(10f);
        }
    }
    private IEnumerator SpawnPickUpWithTimer()
    {
        while(true)
        {
            SpawnPickUp();
            yield return new WaitForSeconds(10f);
        }
    }
}
