using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
  static public List<Obstacle> OBSTACLES;

  [Header("Inscribed: Settings for Spawning Obstacles")]
  public GameObject obstaclePrefab;
  public Transform obstacleAnchor;

  public int maxNumObstacles = 20;
  public float spawnRadius = 75f;
  public float spawnDelay = 2f;

  void Awake() {
    OBSTACLES = new List<Obstacle>();
    InstantiateObstacle();
  }

  public void InstantiateObstacle() {
    GameObject obstacleObject = Instantiate<GameObject>(obstaclePrefab);
    obstacleObject.transform.position = Random.insideUnitSphere * spawnRadius;
    
    Obstacle obstacle = obstacleObject.GetComponent<Obstacle>();
    obstacle.transform.SetParent(obstacleAnchor);
    
    OBSTACLES.Add(obstacle);
    if(OBSTACLES.Count < maxNumObstacles) {
      Invoke("InstantiateObstacle", spawnDelay);
    }

  }
}
