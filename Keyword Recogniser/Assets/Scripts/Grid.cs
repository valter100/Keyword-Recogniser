using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject tileObject;
    [SerializeField] GameObject playerObject;
    [SerializeField] GameObject foodObject;
    [SerializeField] Vector2 gridDimensions;
    [SerializeField] float tileSpacing;
    [SerializeField] VoiceCommander voiceCommander;
    Tile[,] tiles;
    void Start()
    {
        tiles = new Tile[(int)gridDimensions.x, (int)gridDimensions.y];

        SetupGrid();
        ConnectGrid();
        SpawnPlayerOnRandomTile();
        SpawnFoodOnRandomTile();
    }

    public void SetupGrid()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                Vector3 tilePosition = new Vector3(
                    (tileObject.transform.localScale.x + tileSpacing) * i,
                    transform.position.y,
                    (tileObject.transform.localScale.z + tileSpacing) * j
                    ) + new Vector3(
                        -gridDimensions.x * tileSpacing * 1.5f,
                        0,
                        -gridDimensions.y * tileSpacing * 1.5f
                    ) + new Vector3(
                        tileObject.transform.localScale.x / 2,
                        0,
                        tileObject.transform.localScale.z / 2);

                Tile newTile = Instantiate(tileObject, tilePosition, Quaternion.identity).GetComponent<Tile>();
                newTile.gameObject.transform.parent = transform;
                newTile.gameObject.name = "X: " + i + " Z: " + j;
                newTile.SetCoordinates(i, j);
                newTile.SetIsEmpty(true);
                tiles[i, j] = newTile;
            }
        }
    }

    public void ConnectGrid()
    {
        for (int i = 0; i < gridDimensions.x; i++)
        {
            for (int j = 0; j < gridDimensions.y; j++)
            {
                if (i > 0)
                {
                    tiles[i, j].SetNeighbor(tiles[i - 1, j]);
                }

                if (j > 0)
                {
                    tiles[i, j].SetNeighbor(tiles[i, j - 1]);
                }

                if (i < tiles.GetLength(0) - 2)
                {
                    tiles[i, j].SetNeighbor(tiles[i + 1, j]);
                }

                if (j < tiles.GetLength(1) - 2)
                {
                    tiles[i, j].SetNeighbor(tiles[i, j + 1]);
                }
            }
        }
    }

    public Tile GetTileAtCoordinates(Vector2 coordinates)
    {
        if (coordinates.x >= gridDimensions.x || coordinates.y >= gridDimensions.y || coordinates.x < 0 || coordinates.y < 0)
            return null;

        return tiles[(int)coordinates.x, (int)coordinates.y];
    }

    public void SpawnPlayerOnRandomTile()
    {
        int xRandom = Random.Range(0, (int)gridDimensions.x);
        int yRandom = Random.Range(0, (int)gridDimensions.y);

        Vector3 spawnLocation = tiles[xRandom, yRandom].GetTilePosition();

        Player spawnedPlayer = Instantiate(playerObject, spawnLocation, Quaternion.identity).GetComponent<Player>();
        spawnedPlayer.SetCurrentTile(tiles[xRandom, yRandom]);
        spawnedPlayer.SetGrid(this);

        voiceCommander.SetPlayer(spawnedPlayer);
    }

    public void SpawnFoodOnRandomTile()
    {
        StartCoroutine(_FindFoodTile());
    }

    IEnumerator _FindFoodTile()
    {
        Tile newFoodTile = null;

        while (!newFoodTile)
        {
            int xRandom = Random.Range(0, (int)gridDimensions.x);
            int yRandom = Random.Range(0, (int)gridDimensions.y);

            if (tiles[xRandom, yRandom].IsEmpty())
            {
                newFoodTile = tiles[xRandom, yRandom];
            }

            yield return 0;
        }

        Vector3 spawnLocation = newFoodTile.GetTilePosition();

        Food food = Instantiate(foodObject, spawnLocation, Quaternion.identity).GetComponent<Food>();
        food.SetCurrentTile(newFoodTile);
        food.SetGrid(this);

        yield return 0;
    }
}
