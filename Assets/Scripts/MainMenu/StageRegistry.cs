using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "NewStage", menuName = "ScriptableObjects/StageRegistry")]
public class StageRegistry : ScriptableObject
{
    public string displayName;
    public string subtitle;
    public string info;
    public Sprite icon;
    public SceneAsset scene;

}