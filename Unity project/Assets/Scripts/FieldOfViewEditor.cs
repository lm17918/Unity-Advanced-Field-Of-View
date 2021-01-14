using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof (Controller))]
public class FieldOfViewEditor: Editor {

  void OnSceneGUI() {
    Controller controller = (Controller) target;
    Handles.DrawWireArc(controller.transform.position, Vector3.up, Vector3.forward, 360, controller.viewRadius);
  }

}