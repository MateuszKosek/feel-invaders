using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int killsToUpgrade = 20;

    [Header("References")]
    public TextMeshProUGUI pointsText;

    public EnemyManager enemyManager;
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

    // Start is called before the first frame update
    void Awake()
    {
        gameOver = false;
        gameStarted = false;

        enemyManager.onEnemyDied += OnEnemyDied;
        enemyManager.isGameStarted = () => { return gameStarted; };

        killsToUpgradeLeft = killsToUpgrade;

        UpdatePoints();

        startMenu.SetActive(true);
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

        yield return new WaitForSeconds(1f);

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

        UpdatePoints();

        UpdateUpgradeProgress();
    }

    void OnPlayerDied()
    {
        gameOver = true;

        overMenu.SetActive(true);
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

    void UpdatePoints()
    {
        pointsText.text = points.ToString();
    }
}
