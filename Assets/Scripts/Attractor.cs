using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
  static public Vector3 POSITION = Vector3.zero;

  [Header("Inscribed")]
  public Vector3 range = new Vector3(40, 10, 40);
  public Vector3 phase = new Vector3(0.5f, 0.4f, 0.1f);

  void FixedUpdate()
  {
    Vector3 tPosition = transform.position;
    tPosition.x = Mathf.Sin(phase.x * Time.time) * range.x; 
    tPosition.y = Mathf.Sin(phase.y * Time.time) * range.y; 
    tPosition.z = Mathf.Sin(phase.z * Time.time) * range.z;

    transform.position = tPosition;
    POSITION = tPosition; 
  }
}
