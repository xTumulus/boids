using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
  static public List<Obstacle> OBSTACLES;
  static private List<GameObject> obstacleObjects;

  [Header("Inscribed: Settings for Spawning Obstacles")]
  public GameObject obstaclePrefab;
  public Transform obstacleAnchor;

  public int maxNumObstacles = 20;
  public float spawnRadius = 75f;
  public float spawnDelay = 2f;

  void Awake() {
    OBSTACLES = new List<Obstacle>();
    obstacleObjects = new List<GameObject>();
  }

  void Update() {
    if (Input.GetKey(KeyCode.O)) {
      if(Input.GetKeyDown(KeyCode.UpArrow)) {
        InstantiateObstacle();
      } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
        destroyObstacle();
      }
    }
  }

  public void InstantiateObstacle() {
    GameObject obstacleObject = Instantiate<GameObject>(obstaclePrefab);
    obstacleObjects.Add(obstacleObject);

    obstacleObject.transform.position = Random.insideUnitSphere * spawnRadius;
    
    Obstacle obstacle = obstacleObject.GetComponent<Obstacle>();
    obstacle.transform.SetParent(obstacleAnchor);
    
    OBSTACLES.Add(obstacle);
    if(OBSTACLES.Count > maxNumObstacles) {
      destroyObstacle();
    }

  }

  private void destroyObstacle() {
    GameObject obstacleObject = obstacleObjects[0];
    Obstacle obstacle = obstacleObject.GetComponent<Obstacle>();

    foreach (Boid boid in BoidSpawner.BOIDS) {
      boid.RemoveObstacleReferences(obstacle);
    }
    OBSTACLES.RemoveAt(0);
    obstacleObjects.RemoveAt(0);
    Destroy(obstacleObject);
  }
}
