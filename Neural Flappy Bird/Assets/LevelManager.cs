using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour{

    public GameObject obstaclePrefab;
    public float spawnRate = 1.0f;
    public float obstacleSpeed = 1.0f;
    public List<GameObject> obstacles = new List<GameObject>();
    public Vector3 spawnPos;

    public float timeSinceLastSpawn = 0.0f;

    public static GameObject nextObstacle;
    public static LevelManager inst;

    void Awake(){
        inst = this;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        if(nextObstacle != null) Gizmos.DrawWireSphere(nextObstacle.transform.position, 0.5f);
    }

    void Start(){
        SpawnObstacle();
    }

    void Update(){
        CheckSpawn();
        UpdateObstacles();
        UpdateNextObstacle();
    }

    void CheckSpawn(){
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn >= spawnRate){
            timeSinceLastSpawn = 0.0f;
            SpawnObstacle();
        }
    }

    void SpawnObstacle(){
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPos + (Vector3.up * Random.Range(-1.5f, 4f)), Quaternion.identity);
        obstacles.Add(obstacle);

        if(obstacles.Count == 1){
            nextObstacle = obstacle;
        }
    }

    void UpdateObstacles(){
        for(int i = 0; i < obstacles.Count; i++){
            obstacles[i].transform.position += Vector3.left * obstacleSpeed * Time.deltaTime;
        }
    }


    void UpdateNextObstacle(){
        //if the next obstacle position x is smaller than -5, pick the next obstacle
        if(nextObstacle.transform.position.x < -5f){
            nextObstacle = obstacles[obstacles.IndexOf(nextObstacle) + 1];
        }
    }

    public void ClearObstacles(){
        foreach(GameObject obstacle in obstacles){
            Destroy(obstacle);
        }
        obstacles.Clear();
        timeSinceLastSpawn = 0.0f;
        SpawnObstacle();
    }
}