using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
  private Rigidbody rigid;

  void Awake() {
    rigid = GetComponent<Rigidbody>();
  }
  
  public Vector3 position {
    get {return transform.position;}
    private set {transform.position = value;}
  }
}
