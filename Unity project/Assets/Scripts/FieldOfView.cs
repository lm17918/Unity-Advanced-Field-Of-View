using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView: MonoBehaviour {
  [HideInInspector]
  //number of rays will be casted per degree 
  private float meshResolution = 1;
  private int edgeResolveIterations = 4;
  private float edgeDstThreshold = 1;
  public MeshFilter viewMeshFiltergreen;
  public MeshFilter viewMeshFilterred;
  public MeshFilter viewMeshFilterblue;
  private Controller controller;
  private Mesh viewMeshgreen;
  private Mesh viewMeshred;
  private Mesh viewMeshblue;
  private int[] triangles;
  private Vector3[] vertices;
  void Start() {
    controller = this.GetComponent < Controller > ();
    viewMeshgreen = new Mesh();
    viewMeshFiltergreen.mesh = viewMeshgreen;

    viewMeshred = new Mesh();
    viewMeshFilterred.mesh = viewMeshred;

    viewMeshblue = new Mesh();
    viewMeshFilterblue.mesh = viewMeshblue;

  }

  void LateUpdate() {
    //show green FOV
    DrawFieldOfView("green", viewMeshgreen);
    //show blue FOV
    DrawFieldOfView("blue", viewMeshblue);
    // Show red FOV
    DrawFieldOfView("red", viewMeshred);
  }

  public void DrawFieldOfView(string colorSpace, Mesh viewMesh) {
    int stepCount;
    float stepAngleSize;
    if (colorSpace == "blue") {
      stepCount = Mathf.RoundToInt((360 - controller.viewAngle) * meshResolution);
      stepAngleSize = (360 - controller.viewAngle) / stepCount;
    } else {
      stepCount = Mathf.RoundToInt(controller.viewAngle * meshResolution);
      stepAngleSize = controller.viewAngle / stepCount;
    }

    List < Vector3 > viewPoints = new List < Vector3 > ();
    ViewCastInfo oldViewCast = new ViewCastInfo();
    for (int i = 0; i <= stepCount; i++) {
      float angle;
      if (colorSpace == "blue") {
        angle = transform.eulerAngles.y + controller.viewAngle / 2 + stepAngleSize * i;
      } else {
        angle = transform.eulerAngles.y - controller.viewAngle / 2 + stepAngleSize * i;
      }
      ViewCastInfo newViewCast = ViewCast(angle);

      if (i > 0) {
        bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
        if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded)) {
          EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
          if (edge.pointA != Vector3.zero) {
            viewPoints.Add(edge.pointA);
          }
          if (edge.pointB != Vector3.zero) {
            viewPoints.Add(edge.pointB);
          }
        }
      }
      viewPoints.Add(newViewCast.point);
      oldViewCast = newViewCast;
    }
    int vertexCount = viewPoints.Count + 1;
    if (colorSpace == "red") {
      vertices = new Vector3[(vertexCount) * 2];
      triangles = new int[(vertexCount) * 6];
      for (int i = 0; i < vertexCount; i++) {
        Vector3 vertex = transform.InverseTransformPoint(viewPoints[(i == viewPoints.Count) ? 0 : i]);
        vertices[i * 2] = vertex;
        vertices[i * 2 + 1] = vertex.normalized * controller.viewRadius;
      }
      for (int i = 0; i < (vertexCount); i++) {
        int j = (vertexCount - 1 == i) ? 0 : i;
        triangles[i * 6 + 0] = j * 2 + 0;
        triangles[i * 6 + 1] = j * 2 + 1;
        triangles[i * 6 + 2] = j * 2 + 2;
        triangles[i * 6 + 3] = j * 2 + 1;
        triangles[i * 6 + 4] = j * 2 + 3;
        triangles[i * 6 + 5] = j * 2 + 2;
      }
    } else {
      vertices = new Vector3[vertexCount];
      triangles = new int[(vertexCount - 2) * 3];
      vertices[0] = Vector3.zero;
      for (int i = 0; i < vertexCount - 1; i++) {
        vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
        if (i < vertexCount - 2) {
          triangles[i * 3] = 0;
          triangles[i * 3 + 1] = i + 1;
          triangles[i * 3 + 2] = i + 2;
        }
      }
    }
    viewMesh.Clear();
    viewMesh.vertices = vertices;
    viewMesh.triangles = triangles;
    viewMesh.RecalculateNormals();
  }

  EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
    float minAngle = minViewCast.angle;
    float maxAngle = maxViewCast.angle;
    Vector3 minPoint = Vector3.zero;
    Vector3 maxPoint = Vector3.zero;

    for (int i = 0; i < edgeResolveIterations; i++) {
      float angle = (minAngle + maxAngle) / 2;
      ViewCastInfo newViewCast = ViewCast(angle);

      bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
      if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) {
        minAngle = angle;
        minPoint = newViewCast.point;
      } else {
        maxAngle = angle;
        maxPoint = newViewCast.point;
      }
    }

    return new EdgeInfo(minPoint, maxPoint);
  }

  ViewCastInfo ViewCast(float globalAngle) {
    Vector3 dir = DirFromAngle(globalAngle, true);
    RaycastHit hit;

    if (Physics.Raycast(transform.position, dir, out hit, controller.viewRadius, controller.obstacleMask)) {
      return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
    } else {
      return new ViewCastInfo(false, transform.position + dir * controller.viewRadius, controller.viewRadius, globalAngle);
    }
  }

  public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
    if (!angleIsGlobal) {
      angleInDegrees += transform.eulerAngles.y;
    }
    return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
  }

  public struct ViewCastInfo {
    public bool hit;
    public Vector3 point;
    public float dst;
    public float angle;

    public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle) {
      hit = _hit;
      point = _point;
      dst = _dst;
      angle = _angle;
    }
  }

  public struct EdgeInfo {
    public Vector3 pointA;
    public Vector3 pointB;

    public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
      pointA = _pointA;
      pointB = _pointB;
    }
  }

}