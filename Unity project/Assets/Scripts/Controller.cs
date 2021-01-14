using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System.Linq;

public class Controller: MonoBehaviour {
  [Range(0, 360)]
  public float viewAngle;
  public float viewRadius;
  public float moveSpeed = 6;
  public LayerMask obstacleMask;
  public LayerMask targetMask;
  public Camera viewCamera;
  public GameObject character;
  private Vector3 velocity;
  private Rigidbody rigidbodyComponent;
  private List < GameObject > targetsInFOV = new List < GameObject > ();
  private List < GameObject > targetsOutsideFOV = new List < GameObject > ();
  private List < GameObject > targetsHideen = new List < GameObject > ();
  private GameObject[] allTargets;

  void Start() {
    rigidbodyComponent = GetComponent < Rigidbody > ();
    allTargets = GameObject.FindGameObjectsWithTag("targets");
  }

  void Update() {
    // Find position of the mouse on the screen.
    Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
    // Rotate the character to look at the mouse position
    transform.LookAt(mousePos + Vector3.up * transform.position.y);
    velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
  }

  void FixedUpdate() {
    rigidbodyComponent.MovePosition(rigidbodyComponent.position + velocity * Time.fixedDeltaTime);
    FindTargets(character.transform);
    foreach(GameObject target in allTargets) {
      var targetRenderer = target.GetComponent < Renderer > ();
      if (targetsInFOV.Contains(target)) {
        targetRenderer.material.SetColor("_Color", Color.green);
      } else if (targetsHideen.Contains(target)) {
        targetRenderer.material.SetColor("_Color", Color.red);
      } else if (targetsOutsideFOV.Contains(target)) {
        targetRenderer.material.SetColor("_Color", Color.blue);
      } else {
        targetRenderer.material.SetColor("_Color", Color.white);
      }
    }
  }

  private void FindTargets(Transform character) {
    targetsInFOV.Clear();
    targetsHideen.Clear();
    targetsOutsideFOV.Clear();
    // Find all targets in the given radius. 
    Collider[] targetsInViewRadius = Physics.OverlapSphere(character.position, viewRadius, targetMask);
    for (int i = 0; i < targetsInViewRadius.Length; i++) {
      Transform target = targetsInViewRadius[i].transform;
      Vector3 dirToTarget = (target.position - character.transform.position).normalized;
      float dstToTarget = Vector3.Distance(character.transform.position, target.position);
      // Check if the target is in the view angle.                
      if (Vector3.Angle(character.transform.forward, dirToTarget) < viewAngle / 2) {

        // Check if there are no obstacles between the charcter and the target.
        if (!Physics.Raycast(character.transform.position, dirToTarget, dstToTarget, obstacleMask) && character.transform.position != target.transform.position) {
          // Target seen in the FOV.
          targetsInFOV.Add(target.gameObject);
        } else {
          // Target hidden by an obstable in the fild of view.
          targetsHideen.Add(target.gameObject);
        }
      } else {
        // Check if there are no obstacles between the charcter and the target.
        if (!Physics.Raycast(character.transform.position, dirToTarget, dstToTarget, obstacleMask) && character.transform.position != target.transform.position) {
          // Target visible in the radius range but not in FOV.
          targetsOutsideFOV.Add(target.gameObject);
        }
      }
    }

  }

}