using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlot : MonoBehaviour
{
    public Transform shipHolder;
    
    [HideInInspector]
    public EnemyScript currentEnemy;

    [HideInInspector]
    public int currentDifficulty = 0;
    
    public Action<EnemyScript> onEnemyDied;

    [HideInInspector]
    public float timeToSpawn;
    private EnemyScript enemyToRespawn;

    public Func<bool> isGameStarted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRespawn();
    }

    private void UpdateRespawn()
    {
        if (!enemyToRespawn)
            return;

        timeToSpawn -= Time.deltaTime;

        if(timeToSpawn <= 0)
        {
            InstanciateEnemy(enemyToRespawn);
            enemyToRespawn = null;
        }
    }

    public void InstanciateEnemyDelayed(float time, EnemyScript enemyPrefab)
    {
        timeToSpawn = time;
        enemyToRespawn = enemyPrefab;
    }
    

    public void InstanciateEnemy(EnemyScript enemyPrefab)
    {
        currentEnemy = Instantiate(enemyPrefab, shipHolder);
        currentEnemy.transform.localPosition = new Vector3(0, 0, 0);
        currentEnemy.onDied += (e) => {
            currentDifficulty++;
            onEnemyDied.Invoke(e);
        };

        currentEnemy.canShoot = isGameStarted;
    }

    public void ShiftX(float x)
    {
        shipHolder.transform.localPosition = new Vector3(x, shipHolder.localPosition.y,
            shipHolder.localPosition.z);
    }
   
}
