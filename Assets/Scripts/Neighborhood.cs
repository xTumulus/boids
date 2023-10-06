using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighborhood : MonoBehaviour
{
  [Header("Dynamic")]
  public List<Boid> neighbors;
  public List<Obstacle> obstacles;
  private SphereCollider collider;

  // Start is called before the first frame update
  void Start()
  {
    neighbors = new List<Boid>();
    obstacles = new List<Obstacle>();

    collider = GetComponent<SphereCollider>();
    collider.radius = BoidSpawner.SETTINGS.boidColliderRadius;
  }

  void FixedUpdate() {
    if (collider.radius != BoidSpawner.SETTINGS.boidColliderRadius) {
      collider.radius = BoidSpawner.SETTINGS.boidColliderRadius;
    }
  }

  void OnTriggerEnter(Collider other) {
    GameObject collidedWith = other.gameObject;
    if (collidedWith.CompareTag("Boid")) {
      Boid otherBoid = other.GetComponent<Boid>();      
      if (!neighbors.Contains(otherBoid)) {
        neighbors.Add(otherBoid);
      }
    }
    else {
      Obstacle obstacle = other.GetComponent<Obstacle>();
      if (obstacle != null) {
        if (!obstacles.Contains(obstacle)) {
          obstacles.Add(obstacle);
        }
      }
    }
  }

  void OnTriggerExit(Collider other) {
    GameObject collidedWith = other.gameObject;
    if (collidedWith.CompareTag("Boid")) {
      Boid otherBoid = other.GetComponent<Boid>();
      if (!neighbors.Contains(otherBoid)) {
        neighbors.Remove(otherBoid);
      }
    }
    else {
      Obstacle obstacle = other.GetComponent<Obstacle>();
      if (obstacle != null) {
        if (!obstacles.Contains(obstacle)) {
          obstacles.Remove(obstacle);
        }
      }
    }
  }

  public Vector3 averageNeighborPosition {
    get {
      Vector3 average = Vector3.zero;
      
      if (neighbors.Count == 0) {
        return average;
      }

      for (int i=0; i<neighbors.Count; i++) {
        average += neighbors[i].position;
      }
      average /= neighbors.Count;
      
      return average;
    }
  }

  public Vector3 averageNeighborVelocity {
    get {
      Vector3 average = Vector3.zero;
      
      if (neighbors.Count == 0) {
        return average;
      }

      for (int i=0; i<neighbors.Count; i++) {
        average += neighbors[i].velocity;
      }
      average /= neighbors.Count;
      
      return average;
    }
  }
  
  public Vector3 averageNearNeighborPosition {
    get {
      Vector3 average = Vector3.zero;
      Vector3 delta;
      int nearCount = 0;

      for (int i=0; i<neighbors.Count; i++) {
        delta = neighbors[i].position - transform.position;
        if (delta.magnitude <= BoidSpawner.SETTINGS.nearBoidDistance) {
          average += neighbors[i].position;
          nearCount++;
        }
      }

      if (nearCount == 0) {
        return Vector3.zero;
      }
      else {
        average /= nearCount;
        return average;
      }
    }
  }

  public bool CloseToObstacle() {
    if (obstacles.Count != 0) {
      return true;
    } else {
      return false;
    }
  }

  public Vector3 averageNearObstaclePosition {
    get {
      Vector3 average = Vector3.zero;
      Vector3 delta;
      int nearObstacleCount = 0;

      for (int i = 0; i < obstacles.Count; i++){
        delta = obstacles[i].position - transform.position;
        if (delta.magnitude <= BoidSpawner.SETTINGS.nearObstacleDistance) {
          average += obstacles[i].position;
          nearObstacleCount++;
        }
      }

      if (nearObstacleCount == 0) {
        return Vector3.zero;
      }
      else {
        average /= nearObstacleCount;
        return average;
      }
    }
  }
}
