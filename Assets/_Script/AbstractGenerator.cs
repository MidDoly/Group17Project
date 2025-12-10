using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;

    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    // Thêm đoạn này vào bên trong class AbstractGenerator
    private void Start()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        tilemapVisualizer.Clear();
        RunProceduralGeneration();
    }


    protected abstract void RunProceduralGeneration();
}
