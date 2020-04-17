using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonControl : MonoBehaviour
{
    private float attackRadius = 25f;
    private float attackCounter = 0;
    private float timeToSetWhiteColorEnemy = 0.1f;

    private float speedOfRotation = 5f;


    [SerializeField] private float startTimeBetweenAttack = 1f;
    [SerializeField] private int damage = 0;
    [SerializeField] private float speedOfShoot = 0;

    private GameObject targetEnemy;

    private void Start()
    {
        attackCounter = getCurrentDelayBetweenShot();
    }


    private void Update()
    {
        attackCounter -= Time.deltaTime;

        if(targetEnemy == null)
        {
            GameObject nearestEnemy = getNearestEnemy();
            if(nearestEnemy != null && Vector3.Distance(transform.position, nearestEnemy.transform.position) < attackRadius)
            {
                targetEnemy = nearestEnemy;
            }
        }
        else
        {
            if (attackCounter <= 0)
            {
                attack(targetEnemy);
            }
            if (targetEnemy != null)
            {
                if (Vector3.Distance(transform.position, targetEnemy.transform.position) > attackRadius)
                {
                    targetEnemy = null;
                }
                else
                {
                    Vector3 targetDirection = targetEnemy.transform.position - transform.position;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * speedOfRotation, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -transform.localEulerAngles.z);
                }
            }
        }
    }

    public float SpeedOfShoot
    {
        set
        {
            speedOfShoot = value;
        }
        get
        {
            return speedOfShoot;
        }
    }

    public int Damage
    {
        set
        {
            damage = value;
        }
        get
        {
            return damage;
        }
    }

    private void attack(GameObject enemy)
    {
        attackCounter = getCurrentDelayBetweenShot();
        enemy.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.red;
        enemy.GetComponent<EnemyMechanics>().CounterToSetWhiteColor = timeToSetWhiteColorEnemy;
        enemy.GetComponent<EnemyMechanics>().Health -= damage;
    }

    private List<GameObject> getEnemiesInRange()
    {
        List<GameObject> enemiesInRange = new List<GameObject>();
        foreach(GameObject enemy in Manager.Instance.Enemies)
        {
            if(Vector3.Distance(transform.position, enemy.transform.position) < attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }

    private float getCurrentDelayBetweenShot()
    {
        return 1f / speedOfShoot;
    }

    private GameObject getNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;

        foreach(GameObject enemy in getEnemiesInRange())
        {
            if(Vector3.Distance(transform.position, enemy.transform.position) < smallestDistance)
            {
                smallestDistance = Vector3.Distance(transform.position, enemy.transform.position);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }
}
