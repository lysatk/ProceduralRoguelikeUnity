using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface Surface2D;

    void Start()
    {
        Surface2D.BuildNavMeshAsync();
    }
}

