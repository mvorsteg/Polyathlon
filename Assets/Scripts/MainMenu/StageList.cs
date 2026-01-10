using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageList", menuName = "ScriptableObjects/StageList")]
public class StageList : ScriptableObject
{
    public List<StageRegistry> stages;
}