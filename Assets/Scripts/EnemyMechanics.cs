using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMechanics : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int damage = 10;
    [SerializeField] private int rewardGoldForEnemy = 20;

    private int target = 0;
    private float counterToSetWhiteColor = 0;

    private float speedOfRotation = 5f;
    private float speedOfMove = 7f;

    private Transform[] points;
    private GameObject finish;

    void Start()
    {
        points = Manager.Instance.points;
        finish = GameObject.FindGameObjectWithTag("End");
        Manager.Instance.registerEnemy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            die();
        }
        if (counterToSetWhiteColor <= 0)
        {
            GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.white;
        }
        else
        {
            counterToSetWhiteColor -= Time.deltaTime;
        }
        if (points != null)
        {
            moveToPoint(target == points.Length ? finish.transform.position : points[target].transform.position);
        }
    }

    public void upgradeCharacteristics(int valueUpgradeOfHealth, int valueUpgradeOfDamage, int valueUpgradeOfReward)
    {
        GetComponent<EnemyMechanics>().health += Random.Range(0, 2) * valueUpgradeOfHealth;
        GetComponent<EnemyMechanics>().damage += Random.Range(0, 2) * valueUpgradeOfDamage;
        GetComponent<EnemyMechanics>().rewardGoldForEnemy += Random.Range(0, 2) * valueUpgradeOfReward;
    }

    public int Health { get => health; set => health = value; }
    public int Damage { get => damage; set => damage = value; }
    public int RewardGoldForEnemy { get => rewardGoldForEnemy; set => rewardGoldForEnemy = value; }
    public float CounterToSetWhiteColor { get => counterToSetWhiteColor; set => counterToSetWhiteColor = value; }

    public void damageToBase(int damage)
    {
        Manager.Instance.changeHealthOfBase(-damage);
    }

    private void die()
    {
        ++Manager.sumOfKilledEnemies;
        Manager.Instance.unRegisterEnemy(gameObject);
        Manager.Instance.addGold(rewardGoldForEnemy);
    }

    private void moveToPoint(Vector3 point)
    {
        Vector3 targetDirection = point - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * speedOfRotation, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        float rotation = transform.eulerAngles.z;
        if (transform.position.x > point.x)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
        else transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90);
        transform.position += transform.forward * Time.deltaTime * speedOfMove;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (target < points.Length && collision.transform.position == points[target].transform.position)
        {
            ++target;
        }
        else if (collision.gameObject.tag == "End")
        {
            damageToBase(damage);
            Manager.Instance.unRegisterEnemy(this.gameObject);
        }
    }
}
