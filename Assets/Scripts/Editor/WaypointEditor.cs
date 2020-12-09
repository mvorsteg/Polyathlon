using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor {

    public override void OnInspectorGUI() {
        //DrawDefaultInspector();
        Waypoint waypoint = target as Waypoint;
        waypoint.water = EditorGUILayout.Toggle("Water Waypoint", waypoint.water);
        waypoint.seq = waypoint.transform.GetSiblingIndex();
        waypoint.height = Mathf.Max(waypoint.transform.GetComponent<CapsuleCollider>().radius, waypoint.transform.GetComponent<CapsuleCollider>().height / 2) / 2;
        EditorGUILayout.LabelField("Seqence Number", waypoint.seq.ToString());
        EditorGUILayout.LabelField("Height", waypoint.height.ToString());
    }
}