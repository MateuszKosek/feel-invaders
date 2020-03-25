using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Config")]
    public int rows;
    public int cols;
    public float width;
    public float height;

    [Space(10)]
    public float maxTimetoMove = 2;
    public float maxDistance = 1.5f;
    public float singleMovementDistance = 0.5f;
    public float enemyRespawnDelay = 3f;

    [Header("References")]
    public GridSlot gridSlotPrefab;
    public List<EnemyScript> enemies = new List<EnemyScript>();

    // Events
    public Action onEnemyDied;

    // Internal
    private GridSlot[,] slots;

    private int movementSide = 1;
    private float currentPos = 0;

    public Func<bool> isGameStarted;

    void Start()
    {
        CreateGrid();

        InstanciateEnemies();

        StartCoroutine(MoveCounter());
    }

    void Update()
    {
        
    }

    IEnumerator MoveCounter()
    {
        while(true)
        {
            yield return new WaitForSeconds(maxTimetoMove);

            if (Mathf.Abs(currentPos + singleMovementDistance * movementSide) > maxDistance)
            {
                movementSide *= -1;
            }

            currentPos += singleMovementDistance * movementSide;

            StartCoroutine(MoveGrid(currentPos));
        }
    }

    IEnumerator MoveGrid(float targetX)
    {
        for (int y = 0; y <= slots.GetUpperBound(0); y++)
        {
            for (int x = 0; x <= slots.GetUpperBound(1); x++)
            {
                GridSlot slot = slots[y, x];
                slot.ShiftX(targetX);
            }

            yield return new WaitForSeconds(0.2f);
        }

        yield return null;
    }

    public void OnEnemyDied(GridSlot slot, EnemyScript enemy)
    {
        slot.InstanciateEnemyDelayed(enemyRespawnDelay, 
            enemies[Mathf.Min(slot.currentDifficulty, enemies.Count - 1)]);

        onEnemyDied.Invoke();
    }
    

    void InstanciateEnemies()
    {
        foreach(var slot in slots)
        {
            slot.InstanciateEnemy(enemies[0]);
        }
    }

    public void RefillSlots()
    {
        int i = 0;

        foreach (var slot in slots)
        {
            slot.timeToSpawn = 0.5f + i * 0.015f;

            i++;
        }
    }


    void CreateGrid()
    {
        slots = new GridSlot[rows, cols];

        IterateGrid((gridX, gridY, posX, posY) => {
            GridSlot slot = Instantiate(gridSlotPrefab, transform);
            slot.name = "GridSlot " + gridY + "x" + gridX;
            slot.transform.localPosition = new Vector3(posX, posY, 0);
            slot.onEnemyDied += e => OnEnemyDied(slot, e);
            slot.isGameStarted = isGameStarted;

            slots[gridY, gridX] = slot;
        });
    }

    private void OnDrawGizmosSelected()
    {
        // Helper to visualize current grid setup
        IterateGrid((gridX, gridY, posX, posY) => {
                Gizmos.DrawSphere(new Vector3(transform.position.x + posX,
                        transform.position.y + posY, 0), 0.2f);
        });

    }

    private void IterateGrid(Action<int, int, float, float> positionCallback)
    {
        for (int yi = 0; yi < rows; yi++)
        {
            float y = height * ((float)yi / (rows - 1)) - height / 2f;
            for (int xi = 0; xi < cols; xi++)
            {
                float x = width * ((float)xi / (float)(cols - 1)) - width / 2f;
                positionCallback?.Invoke(xi, yi, x, y);
            }
        }
    }
}
