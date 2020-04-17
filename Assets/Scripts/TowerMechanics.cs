using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerMechanics : MonoBehaviour
{
   [SerializeField] private GameObject cannon;

    void Start()
    {
        cannon.GetComponent<Renderer>().material.color = Color.red;
        followMouse();
    }

    void Update()
    {
        if (cannon.GetComponent<Renderer>().material.color != Color.white)
        {
            followMouse();
            if (Input.GetMouseButtonDown(0))
            {
                if (cannon.GetComponent<Renderer>().material.color == Color.green)
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
        if (Manager.Instance.TotalGold >= Manager.Instance.CostOfUpgradeTower && cannon.GetComponent<Renderer>().material.color == Color.white)
        {
            upgradeCharacteristics();
        }
    }

    private void setPlaceTower()
    {
        cannon.GetComponent<Renderer>().material.color = Color.white;
        cannon.GetComponent<CannonControl>().enabled = true;
        GameObject.FindGameObjectWithTag("TowerButton").GetComponent<Button>().enabled = true;
        Manager.Instance.substractGold(Manager.Instance.CostOfTower);
    }

    private void upgradeCharacteristics()
    {
        setDamageOfCannon(cannon.GetComponent<CannonControl>().Damage + Manager.Instance.ValueUpgradeOfTowerDamage);
        setSpeedOfShootOfCannon(cannon.GetComponent<CannonControl>().SpeedOfShoot + Manager.Instance.ValueUpgradeOfTowerSpeedOfShoot);
        Manager.Instance.substractGold(Manager.Instance.CostOfUpgradeTower);
    }

    public void setDamageOfCannon(int damage)
    {
        cannon.GetComponent<CannonControl>().Damage = damage;
    }

    public void setSpeedOfShootOfCannon(float speed)
    {
        cannon.GetComponent<CannonControl>().SpeedOfShoot = speed;
    }

    private void followMouse()
    {
        float towerToCameraDistance = transform.position.z - Camera.main.transform.position.z;
        Vector3 mousePosNearClipPlane = new Vector3(Input.mousePosition.x, Input.mousePosition.y, towerToCameraDistance);
        Vector3 worldPointPos = Camera.main.ScreenToWorldPoint(mousePosNearClipPlane);
        transform.position = new Vector3(worldPointPos.x, worldPointPos.y, 90);
    }

    private void OnTriggerStay(Collider collision)
    {
        if(cannon.GetComponent<Renderer>().material.color != Color.white) cannon.GetComponent<Renderer>().material.color = Color.red;
    }

    private void OnTriggerExit(Collider other)
    {
        if (cannon.GetComponent<Renderer>().material.color != Color.white) cannon.GetComponent<Renderer>().material.color = Color.green;
    }
}
