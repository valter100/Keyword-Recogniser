using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Tile currentTile;
    Grid grid;
    Vector2 direction;
    Vector2 oldDirection;
    [SerializeField] GameObject tailObject;
    [SerializeField] float timeBetweenMovement;
    [SerializeField] Tail tail;
    float movementTimer;
    bool spawningTail;

    void Update()
    {
        movementTimer += Time.deltaTime;

        if (movementTimer >= timeBetweenMovement && direction != Vector2.zero)
        {
            MoveToTile(direction);
            movementTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetDirection(new Vector2(-1, 0));
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetDirection(new Vector2(1, 0));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetDirection(new Vector2(0, 1));
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetDirection(new Vector2(0, -1));
        }
    }

    public void MoveToTile(Vector2 movement)
    {
        Tile oldTile = currentTile;
        currentTile = grid.GetTileAtCoordinates(currentTile.GetCoordinates() + movement);

        if (!currentTile)
        {
            currentTile = oldTile;
            return;
        }
        Debug.Log(currentTile.name);

        currentTile.SetIsEmpty(false);
        oldTile.SetIsEmpty(true);
        transform.position = currentTile.transform.position + new Vector3(0, currentTile.gameObject.transform.localScale.y / 2 + transform.localScale.y / 2, 0);
      
        if (tail)
        {
            tail.MoveToTile(oldTile);
        }

        if(spawningTail)
        {
            spawningTail = false;

            GameObject spawnedTail = Instantiate(tailObject, oldTile.GetTilePosition(), Quaternion.identity);
            spawnedTail.GetComponent<Tail>().SetCurrentTile(oldTile);

            if (!tail)
            {
                tail = spawnedTail.GetComponent<Tail>();
            }
            else
                tail.AssignTail(spawnedTail.GetComponent<Tail>());
        }

        UpdateOldDirection();
    }

    public void SetGrid(Grid newGrid)
    {
        grid = newGrid;
    }

    public void SetCurrentTile(Tile newTile)
    {
        if (currentTile)
            currentTile.SetIsEmpty(true);

        currentTile = newTile;
        currentTile.SetIsEmpty(false);
        transform.position = currentTile.transform.position + new Vector3(0, currentTile.gameObject.transform.localScale.y / 2 + transform.localScale.y / 2, 0);
    }

    public void SetDirection(Vector2 newDirection)
    {
        if (direction * -1 == newDirection)
            return;

        direction = newDirection;
    }

    public void UpdateOldDirection()
    {
        oldDirection = direction;
    }

    public void AddTail()
    {
        spawningTail = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            AddTail();
            Destroy(other.gameObject);
            grid.SpawnFoodOnRandomTile();
        }
        else if(other.CompareTag("Tail"))
        {
            //Game OVER
        }
    }
}
