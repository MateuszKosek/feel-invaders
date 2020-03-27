using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int killsToUpgrade = 20;

    [Header("References")]
    public EnemyManager enemyManager;
    public PointsScript pointsScript;

    private PlayerScript playerScript;
    

    [Header("UI References")]
    public GameObject startMenu;
    public GameObject overMenu;
    
    private int points;
    private int killsToUpgradeLeft;

    [HideInInspector]
    public bool gameOver;
    [HideInInspector]
    public bool gameStarted;

    private CameraScript cameraScript;
    private TimeManager timeManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameOver = false;
        gameStarted = false;

        enemyManager.onEnemyDied += OnEnemyDied;
        enemyManager.isGameStarted = () => { return gameStarted; };

        killsToUpgradeLeft = killsToUpgrade;
        
        pointsScript.UpdatePoints(points);

        startMenu.SetActive(true);

        cameraScript = Camera.main.GetComponent<CameraScript>();
        timeManager = Camera.main.GetComponent<TimeManager>();
    }

    private void Start()
    {
        playerScript = FindObjectOfType<PlayerScript>();
        playerScript.onDeath += OnPlayerDied;
        playerScript.isStarted = () => { return gameStarted; };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine(StartGameInternal());
    }

    public void ResetGame()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    public IEnumerator StartGameInternal()
    {
        startMenu.SetActive(false);

        enemyManager.MoveEnemiesIn();
        playerScript.MoveIn();

        yield return new WaitForSeconds(2f);

        gameStarted = true;
    }

    void OnEnemyDied()
    {
        points += 100;

        killsToUpgradeLeft--;

        if(killsToUpgradeLeft <= 0)
        {
            killsToUpgrade += 10;
            killsToUpgradeLeft = killsToUpgrade;

            UpgradePlayer();
        }

        pointsScript.UpdatePoints(points);

        UpdateUpgradeProgress();

        cameraScript.ShakeScreen(0.05f);    
    }

    void OnPlayerDied()
    {
        gameOver = true;

        overMenu.SetActive(true);

        cameraScript.ShakeScreen(0.2f);

        timeManager.TemporarySlowdown(1f, true, false);


        enemyManager.MoveEnemiesOut();
    }

    void UpdateUpgradeProgress()
    {
        if (gameOver)
            return;

        float progress = 1 - (float)killsToUpgradeLeft / killsToUpgrade;

        playerScript.upgradeSlider.SetProgress(progress);
    }

    void UpgradePlayer()
    {
        if (gameOver)
            return;

        playerScript.Upgrade();

        enemyManager.RefillSlots();

    }
    
}
