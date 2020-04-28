using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : Loader<Manager>
{
    public enum gameStatus
    {
        gameOver, play
    }

    public gameStatus currentState = gameStatus.play;

    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> towers = new List<GameObject>();

    public Transform [] points;

    private GameObject spawnPoint;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject tower;

    [SerializeField] private Text goldText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text infoUpgradeText;
    [SerializeField] private Text infoCostText;

    public static int sumOfKilledEnemies = 0;

    private int healthOfBase = 0;
    private int totalGold = 0;
    private int escapedEnemies = 0;
    private int numOfWave = 0;
    private int maxNewEnemiesOnWave = 3;

    private int valueUpgradeOfEnemyHealth = 10;
    private int valueUpgradeOfDamage = 5;
    private int valueUpgradeOfReward = 5;

    private float valueUpgradeOfTowerSpeedOfShoot = 0.2f;
    private int valueUpgradeOfTowerDamage = 10;

    private float spawnDelay = 1f;
    private float delayBetweenWaves = 0;
    private float counterWave = 0f;

    private void Start()
    {
        readInfoFromFile();
        spawnPoint = GameObject.FindGameObjectWithTag("Start");
        counterWave = delayBetweenWaves;
    }

    private void Update()
    {
        if (currentState == gameStatus.play)
        {
            if (counterWave > 0f)
            {
                if (enemies.Count == 0)
                {
                    counterWave -= Time.deltaTime;

                    if (counterWave <= 0)
                    {
                        escapedEnemies = 0;
                        ++numOfWave;
                        int totalEnemies = numOfWave + Random.Range(0, maxNewEnemiesOnWave + 1);
                        StartCoroutine(Spawn(totalEnemies));
                    }
                }
            }
        }
    }

    public List<GameObject> Enemies { get => enemies; }

    public int HealthOfBase
    {
        set
        {
            healthOfBase = value;
            healthText.text = healthOfBase.ToString();
            if(healthOfBase <= 0)
            {
                endCurrentLevel();
            }
        }
        get
        {
            return healthOfBase;
        }
    }

    public int TotalGold
    {
        set
        {
            totalGold = value;
            goldText.text = totalGold.ToString();
        }
        get
        {
            return totalGold;
        }
    }

    public float ValueUpgradeOfTowerSpeedOfShoot { get => valueUpgradeOfTowerSpeedOfShoot; set => valueUpgradeOfTowerSpeedOfShoot = value; }
    public int ValueUpgradeOfTowerDamage { get => valueUpgradeOfTowerDamage; set => valueUpgradeOfTowerDamage = value; }

    private IEnumerator Spawn(int totalEnemies)
    {
        if (escapedEnemies < totalEnemies)
        {
            GameObject enemy = Instantiate(this.enemy);
            enemy.transform.position = spawnPoint.transform.position;
            ++escapedEnemies;
            if (escapedEnemies == totalEnemies)
            {
                counterWave = delayBetweenWaves;
                this.enemy.GetComponent<EnemyMechanics>().upgradeCharacteristics(valueUpgradeOfEnemyHealth, valueUpgradeOfDamage, valueUpgradeOfReward);
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn(totalEnemies));
        }
    }

    public void endCurrentLevel()
    {
        currentState = gameStatus.gameOver;

        SceneManager.LoadScene(1);
    }

    public void registerEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void unRegisterEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void changeHealthOfBase(int change)
    {
        HealthOfBase += change;
    }

    public void addGold(int gold)
    {
        TotalGold += gold;
    }

    public void substractGold(int gold)
    {
        TotalGold -= gold;
    }

    public void SetTower()
    {
        if (TotalGold >= tower.GetComponent<TowerMechanics>().CostOfTower)
        {
            GameObject newTower = Instantiate(tower);
            towers.Add(newTower);
            GameObject.FindGameObjectWithTag("TowerButton").GetComponent<Button>().enabled = false;
        }
    }

    private void readInfoFromFile()
    {
        Dictionary<string, float> map = new Dictionary<string, float>();

        string path = Application.dataPath + "/Configurations.txt";

        string[] lines = File.ReadAllLines(path);


        foreach (string line in lines)
        {
            string dataString = line.Trim();

            if (string.IsNullOrEmpty(dataString)) continue;

            int pos = dataString.IndexOf("=");
            string key = dataString.Substring(0, pos).Trim();
            float value;
            if (!dataString.Contains("("))
            {
                value = System.Convert.ToSingle(dataString.Substring(pos + 1, dataString.Length - pos - 1).Trim());
            }
            else
            {
                value = System.Convert.ToSingle(dataString.Substring(pos + 1, dataString.IndexOf("(") - pos - 1).Trim());
            }

            map.Add(key, value);
        }

        ICollection<string>  keys = map.Keys;

        foreach(string element in keys)
        {
            switch(element)
            {
                case "HealthOfBase":
                    {
                        HealthOfBase = (int)(map[element]);
                        break;
                    }
                case "delayBetweenWaves":
                    {
                        delayBetweenWaves = map[element];
                        break;
                    }
                case "costOfBuyTower":
                    {
                        tower.GetComponent<TowerMechanics>().CostOfTower = (int)(map[element]);
                        infoCostText.text = "Cost of buying = " + map[element].ToString();
                        break;
                    }
                case "startGold":
                    {
                        TotalGold = (int)(map[element]);
                        break;
                    }
                case "startHealthOfEnemy":
                    {
                        enemy.GetComponent<EnemyMechanics>().Health = (int)(map[element]);
                        break;
                    }
                case "startDamageOfEnemy":
                    {
                        enemy.GetComponent<EnemyMechanics>().Damage = (int)(map[element]);
                        break;
                    }
                case "startRewardOfEnemy":
                    {
                        enemy.GetComponent<EnemyMechanics>().RewardGoldForEnemy = (int)(map[element]);
                        break;
                    }
                case "maxNewEnemiesOnNextWave":
                    {
                        maxNewEnemiesOnWave = (int)(map[element]);
                        break;
                    }
                case "CostOfUpgradeTower":
                    {
                        tower.GetComponent<TowerMechanics>().CostOfUpgradeTower = (int)(map[element]);
                        infoUpgradeText.text = "Cost of upgrading = " + map[element].ToString();
                        break;
                    }
                case "startDamageAttackOfTower":
                    {
                        tower.GetComponent<TowerMechanics>().setDamageOfCannon((int)(map[element]));
                        break;
                    }
                case "startSpeedOfShootOfTower":
                    {
                        tower.GetComponent<TowerMechanics>().setSpeedOfShootOfCannon(map[element]);
                        break;
                    }
            }
        }
    }
}
