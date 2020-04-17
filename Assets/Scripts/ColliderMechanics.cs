using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderMechanics : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BoxCollider>().size = new Vector3(GetComponent<Image>().rectTransform.rect.width, GetComponent<Image>().rectTransform.rect.height, 20);
    }
}
