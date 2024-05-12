using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parchis3DFlower : MonoBehaviour
{
    
    float rotateValue = 15;
    void Start()
    {
        Floweranim();
    }
    void Floweranim()
    {

        float rotateX = Random.Range(-rotateValue, rotateValue);
        float rotateY = Random.Range(-rotateValue, rotateValue);
        float rotateZ = Random.Range(-rotateValue, rotateValue);
        float randomTime = Random.Range(1, 2.5f);
        LeanTween.rotateLocal(gameObject, new Vector3(rotateX, 0, rotateZ), randomTime).setOnComplete(() => Floweranim());

    }

}
