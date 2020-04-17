using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoButton : MonoBehaviour
{
   private GameObject panelOfInfo;

   private void Start()
   {
       panelOfInfo = transform.GetChild(0).gameObject;
   }

    public void activateInfo()
    {
        if (panelOfInfo.activeSelf == false)
            panelOfInfo.SetActive(true);
        else panelOfInfo.SetActive(false);
    }
}
