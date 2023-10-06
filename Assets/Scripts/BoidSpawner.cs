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
