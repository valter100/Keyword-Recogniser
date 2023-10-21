using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    [SerializeField] Tail tail;
    Tile currentTile;
    Grid grid;

    private void Start()
    {
        grid = FindObjectOfType<Grid>();
    }

    public void MoveToTile(Tile tileToMoveTo)
    {
        Tile oldTile = currentTile;

        SetCurrentTile(tileToMoveTo);
        transform.position = currentTile.transform.position + new Vector3(0, currentTile.gameObject.transform.localScale.y / 2 + transform.localScale.y / 2, 0);

        if (tail)
            tail.MoveToTile(oldTile);
    }

    public void SetCurrentTile(Tile newTile)
    {
        if (currentTile)
            currentTile.SetIsEmpty(true);

        currentTile = newTile;
        currentTile.SetIsEmpty(false);
        transform.position = currentTile.transform.position + new Vector3(0, currentTile.gameObject.transform.localScale.y / 2 + transform.localScale.y / 2, 0);
    }

    public void AssignTail(Tail newTail)
    {
        if (!tail)
            tail = newTail;
        else
            tail.AssignTail(newTail);
    }
}
