using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor {
    private Waypoint waypoint;
    public SerializedProperty water;
    public SerializedProperty forceToGround;
    public SerializedProperty fork;

    void OnEnable()
    {
        water = serializedObject.FindProperty("water");
        forceToGround = serializedObject.FindProperty("forceToGround");
        fork = serializedObject.FindProperty("fork");
    }

    public override void OnInspectorGUI() {
        //DrawDefaultInspector();
        waypoint = ((Waypoint)this.target);
        serializedObject.Update();
        // Handle the fork array
        EditorGUI.BeginChangeCheck();
        //waypoint.water = EditorGUILayout.Toggle("Water Waypoint", waypoint.water);
        //waypoint.forceToGround = EditorGUILayout.Toggle("Force To Ground", waypoint.forceToGround);
        EditorGUILayout.PropertyField(water, new GUIContent("Water"));
        EditorGUILayout.PropertyField(forceToGround, new GUIContent("Force Height To Zero"));
        
        EditorGUILayout.PropertyField(fork, true);
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
        
        waypoint.Seq = waypoint.transform.GetSiblingIndex();
        if (!waypoint.forceToGround)
        {
            waypoint.height = Mathf.Max(waypoint.transform.GetComponent<CapsuleCollider>().radius, waypoint.transform.GetComponent<CapsuleCollider>().height / 2) / 2;
        }
        else
        {
            waypoint.height = 0;
        }
        EditorGUILayout.LabelField("Seqence Number", waypoint.Seq.ToString());
        EditorGUILayout.LabelField("Height", waypoint.height.ToString());
    }
}