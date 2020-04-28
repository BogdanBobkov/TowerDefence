using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text textSumEnemy;
    void Start()
    {
        textSumEnemy.text = "Количество убитых врагов = " + Manager.sumOfKilledEnemies.ToString();
    }

    public void playAgain()
    {
        SceneManager.LoadScene(0);

        Destroy(GameObject.FindGameObjectWithTag("LevelManager"));
    }
}
