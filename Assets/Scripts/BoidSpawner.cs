using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoidSettings {
  public int velocity = 32;
  public int boidColliderRadius = 8;
  public int nearBoidDistance = 5;
  public int nearObstacleDistance = 15;
  public int attractPushDist = 5;

  [Header("These \"influences\" are floats, usually from [0...4]")]
  public float velocityMatching = 8f;
  public float flockCentering = 0.2f;
  public float boidAvoid = 4f;
  public float obstacleAvoid = 200f;
  public float attractPull = 8f;
  public float attractPush = 2f;

  [Header("This determines how quickly Boids can turn and is [0...1]")]
  public float velocityEasing = 0.06f;
  
}

public class BoidSpawner : MonoBehaviour
{
  static public BoidSettings SETTINGS;
  static public List<Boid> BOIDS;

  [Header("Inscribed: Settings for Spawning Boids")]
  public GameObject boidPrefab;
  public Transform boidAnchor;
  public int maxNumBoids = 100;
  public float spawnRadius = 100f;
  public float spawnDelay = 0.1f;

  [Header("Inscribed: Settings for Spawning Boids")]
  public BoidSettings boidSettings;

  void Awake() {
    BoidSpawner.SETTINGS = boidSettings;
    BOIDS = new List<Boid>();
    InstantiateBoid();
  }

  void Update() {

    if (Input.anyKey)
    {
     Event e = Event.current;
      switch (e.keyCode)
      {
        case KeyCode.S:
          //Change Speed (velocity)
          if (Input.GetKeyDown(KeyCode.UpArrow)) {
            boidSettings.velocity += 1;
          }
          if (Input.GetKeyDown(KeyCode.DownArrow)) {
            boidSettings.velocity -= 1;
          }
          break;
        case KeyCode.A:
          //Change Agility (velocityEasing)
          if (Input.GetKeyDown(KeyCode.UpArrow)) {
            boidSettings.velocityEasing += 0.01f;
          }
          if (Input.GetKeyDown(KeyCode.DownArrow)) {
            boidSettings.velocityEasing -= 0.01f;
          }
          break;
        case KeyCode.G:
          //Change Grouping (flockCentering and velocityMatching)
          if (Input.GetKeyDown(KeyCode.UpArrow)) {
            boidSettings.flockCentering -= 0.1f;
          }
          if (Input.GetKeyDown(KeyCode.DownArrow)) {
            boidSettings.flockCentering += 0.1f;
          }
          // ChangeFloatSetting(ref boidSettings.velocityMatching, 0.5f);
          // if (Input.GetKeyDown(KeyCode.UpArrow)) {
          //   setting += delta;
          // }
          // if (Input.GetKeyDown(KeyCode.DownArrow)) {
          //   setting -= delta;
          // }
          break;
        case KeyCode.D: 
          //Change Flock Density (boidAvoid and nearBoidDistance)
          if (Input.GetKeyDown(KeyCode.UpArrow)) {
            boidSettings.boidAvoid -= 1f;
          }
          if (Input.GetKeyDown(KeyCode.DownArrow)) {
            boidSettings.boidAvoid += 1f;
          }
          // ChangeIntSetting(ref boidSettings.nearBoidDistance, -1);
          // if (Input.GetKeyDown(KeyCode.UpArrow)) {
          //   setting += delta;
          // }
          // if (Input.GetKeyDown(KeyCode.DownArrow)) {
          //   setting -= delta;
          // }
          break;
        default:
          break;
      }
    }
  }

  public void InstantiateBoid() {
    GameObject boidObject = Instantiate<GameObject>(boidPrefab);
    boidObject.transform.position = Random.insideUnitSphere * spawnRadius;
    
    Boid boid = boidObject.GetComponent<Boid>();
    boid.transform.SetParent(boidAnchor);
    
    BOIDS.Add(boid);
    if(BOIDS.Count < maxNumBoids) {
      Invoke("InstantiateBoid", spawnDelay);
    }

  }
}
