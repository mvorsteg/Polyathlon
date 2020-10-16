using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Waves : MonoBehaviour
{
    protected Mesh mesh;

    public abstract float GetHeight(Vector3 position);
}