using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Vector2 coordinates;
    [SerializeField] List<Tile> neighbors = new List<Tile>();
    bool isEmpty;

    public void SetCoordinates(float x, float y)
    {
        coordinates = new Vector2(x, y);
    }

    public void SetNeighbor(Tile newTile)
    {
        neighbors.Add(newTile);
    }

    public Vector3 GetTilePosition()
    {
        return transform.position;
    }

    public void SetIsEmpty(bool state)
    {
        isEmpty = state;

        if (isEmpty)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
            GetComponent<Renderer>().material.color = Color.red;
    }

    public bool IsEmpty() => isEmpty;

    public Vector2 GetCoordinates() => coordinates;

}
