using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    Tile currentTile;
    Grid grid;

    public void SetCurrentTile(Tile newTile)
    {
        currentTile = newTile;

        transform.position = currentTile.transform.position + new Vector3(0, currentTile.gameObject.transform.localScale.y / 2 + transform.localScale.y / 2, 0);
    }

    public void SetGrid(Grid newGrid)
    {
        grid = newGrid;
    }
}
