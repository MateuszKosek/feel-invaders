using DG.Tweening;
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

    private Vector3 targetPosition = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {

    }

    public void MoveUpDown(float delay)
    {
        StartCoroutine(MoveUpDownInternal(delay));
    }

    IEnumerator MoveUpDownInternal(float delay)
    {
        yield return new WaitForSeconds(delay);

        DOTween.Sequence()
            .Append(transform.DOLocalMoveY(-0.3f, 1f).SetRelative(true))
            .Append(transform.DOLocalMoveY(0.3f, 1f).SetRelative(true))
            .SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRespawn();

        shipHolder.transform.localPosition = Vector3.Lerp(shipHolder.transform.localPosition, targetPosition, 10f * Time.deltaTime);
    }

    private void UpdateRespawn()
    {
        if (!enemyToRespawn)
            return;

        timeToSpawn -= Time.deltaTime;

        if(timeToSpawn <= 0)
        {
            EnemyScript spawned = InstanciateEnemy(enemyToRespawn);
            spawned.TweenMoveIn(0, null);
            enemyToRespawn = null;
        }
    }

    public void InstanciateEnemyDelayed(float time, EnemyScript enemyPrefab)
    {
        timeToSpawn = time;
        enemyToRespawn = enemyPrefab;
    }
    

    public EnemyScript InstanciateEnemy(EnemyScript enemyPrefab)
    {
        currentEnemy = Instantiate(enemyPrefab, shipHolder);
        currentEnemy.transform.localPosition = new Vector3(0, 0, 0);
        currentEnemy.onDied += (e) => {
            currentDifficulty++;
            onEnemyDied.Invoke(e);
        };

        currentEnemy.canShoot = isGameStarted;

        return currentEnemy;
    }

    public void ShiftX(float x)
    {
        targetPosition = new Vector3(x, shipHolder.localPosition.y,
            shipHolder.localPosition.z);
    }
   
}
