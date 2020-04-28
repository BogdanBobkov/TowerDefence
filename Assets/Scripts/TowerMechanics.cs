using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerMechanics : MonoBehaviour
{
    [SerializeField] private GameObject cannon;

    [SerializeField] private int costOfTower = 0;
    [SerializeField] private int costOfUpgradeTower = 0;

    private Color cannonColor;

    void Start()
    {
        cannonColor = cannon.GetComponent<Renderer>().material.color;
        cannonColor = Color.red;
        followMouse();
    }

    void Update()
    {
        if (cannonColor != Color.white)
        {
            followMouse();
            if (Input.GetMouseButtonDown(0))
            {
                if (cannonColor == Color.green)
                {
                    setPlaceTower();
                }
            }
            else if (Input.GetMouseButton(1))
            {
                Destroy(gameObject);
                GameObject.FindGameObjectWithTag("TowerButton").GetComponent<Button>().enabled = true;
            }
        }
    }

    private void OnMouseDown()
    {
        if (Manager.Instance.TotalGold >= costOfUpgradeTower && cannonColor == Color.white)
        {
            upgradeCharacteristics();
        }
    }

    private void setPlaceTower()
    {
        cannonColor = Color.white;
        cannon.GetComponent<CannonControl>().enabled = true;
        GameObject.FindGameObjectWithTag("TowerButton").GetComponent<Button>().enabled = true;
        Manager.Instance.substractGold(costOfTower);
    }

    private void upgradeCharacteristics()
    {
        setDamageOfCannon(cannon.GetComponent<CannonControl>().Damage + Manager.Instance.ValueUpgradeOfTowerDamage);
        setSpeedOfShootOfCannon(cannon.GetComponent<CannonControl>().SpeedOfShoot + Manager.Instance.ValueUpgradeOfTowerSpeedOfShoot);
        Manager.Instance.substractGold(costOfUpgradeTower);
    }

    public void setDamageOfCannon(int damage)
    {
        cannon.GetComponent<CannonControl>().Damage = damage;
    }

    public void setSpeedOfShootOfCannon(float speed)
    {
        cannon.GetComponent<CannonControl>().SpeedOfShoot = speed;
    }

    public int CostOfTower { get => costOfTower; set => costOfTower = value; }
    public int CostOfUpgradeTower { get => costOfUpgradeTower; set => costOfUpgradeTower = value; }

    private void followMouse()
    {
        float towerToCameraDistance = transform.position.z - Camera.main.transform.position.z;
        Vector3 mousePosNearClipPlane = new Vector3(Input.mousePosition.x, Input.mousePosition.y, towerToCameraDistance);
        Vector3 worldPointPos = Camera.main.ScreenToWorldPoint(mousePosNearClipPlane);
        transform.position = new Vector3(worldPointPos.x, worldPointPos.y, 90);
    }

    private void OnTriggerStay(Collider collision)
    {
        if(cannonColor != Color.white) cannonColor = Color.red;
    }

    private void OnTriggerExit(Collider other)
    {
        if (cannonColor != Color.white) cannonColor = Color.green;
    }
}
