using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
  private Neighborhood neighborhood;
  private Rigidbody rigid;

  void Awake() {
    neighborhood = GetComponent<Neighborhood>();
    rigid = GetComponent<Rigidbody>();

    velocity = Random.onUnitSphere * BoidSpawner.SETTINGS.velocity;

    LookAhead();
    Colorize(); 
  }

  void FixedUpdate() {
    Vector3 sumVelocity = Vector3.zero;
    
    //ATTRACTOR LOGIC: Moves toward or away from attractor
    Vector3 delta = Attractor.POSITION - position;
    if (delta.magnitude > BoidSpawner.SETTINGS.attractPushDist) {
      sumVelocity += delta.normalized * BoidSpawner.SETTINGS.attractPull;
    } else {
      sumVelocity -= delta.normalized * BoidSpawner.SETTINGS.attractPush;
    }

    if (neighborhood.CloseToObstacle()) {
      //OBSTACLE AVOIDANCE LOGIC: Boids swarm away from obstacles
      Vector3 obstacleAvoidanceVelocity = Vector3.zero;
      Vector3 tooNearObstaclePosition = neighborhood.averageNearObstaclePosition;

      if (tooNearObstaclePosition != Vector3.zero) {
        obstacleAvoidanceVelocity = position - tooNearObstaclePosition;
        obstacleAvoidanceVelocity.Normalize();
        sumVelocity += obstacleAvoidanceVelocity * BoidSpawner.SETTINGS.obstacleAvoid;
      }
    }

    //COLLISION AVOIDANCE LOGIC: Avoids boids that are near
    Vector3 boidAvoidanceVelocity = Vector3.zero;
    Vector3 tooNearBoidsPosition = neighborhood.averageNearNeighborPosition;

    if (tooNearBoidsPosition != Vector3.zero) {
      boidAvoidanceVelocity = position - tooNearBoidsPosition;
      boidAvoidanceVelocity.Normalize();
      sumVelocity += boidAvoidanceVelocity * BoidSpawner.SETTINGS.boidAvoid;
    }

    //VELOCITY MATCHING LOGIC: Tries to match neighbor velocity
    Vector3 neighborAlignmentDirection = neighborhood.averageNeighborVelocity;
    if (neighborAlignmentDirection != Vector3.zero) {
      neighborAlignmentDirection.Normalize();
      sumVelocity += neighborAlignmentDirection * BoidSpawner.SETTINGS.velocityMatching;
    }

    //FLOCK CENTERING LOGIC: Moves toward center of the local neighborhood
     Vector3 velocityCenter = neighborhood.averageNeighborPosition;
     if (velocityCenter != Vector3.zero) {
       velocityCenter -= transform.position;
       velocityCenter.Normalize();
       sumVelocity += velocityCenter * BoidSpawner.SETTINGS.flockCentering;
     }

    //VELOCITY INTERPOLATION: Keep velocity between normalized velocity and sumVelocity
    sumVelocity.Normalize();
    velocity = Vector3.Lerp(velocity.normalized, sumVelocity, BoidSpawner.SETTINGS.velocityEasing);
    velocity *= BoidSpawner.SETTINGS.velocity;

    //look in direction of new velocity
    LookAhead();
  }

  // Makes boid look in direction of it's velocity
  void LookAhead() {
    transform.LookAt(position + rigid.velocity);
  }

  // Gives boid a random color
  void Colorize() {
    Color randColor = Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1);

    Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
    foreach (Renderer renderer in renderers) {
      renderer.material.color = randColor;
    }

    TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
    trailRenderer.startColor = randColor;
    randColor.a = 0;
    trailRenderer.endColor = randColor;
    trailRenderer.endWidth = 0;
  }
  
  public Vector3 position {
    get {return transform.position;}
    private set {transform.position = value;}
  }
  
  public Vector3 velocity {
    get {return rigid.velocity;}
    private set {rigid.velocity = value;}
  }
}
