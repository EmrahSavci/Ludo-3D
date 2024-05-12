using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParchisWheelControll : MonoBehaviour
{
    public static ParchisWheelControll Instance;

    public GameObject wheel;
    public float rotateSpeed = 50;
    public float lastRotationPos = 0;

    CharacterMoveControll character;
    public AudioSource rotateSound;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
      // StartCoroutine(WheelRotate());
    }

    public void PenaltyCharacter(CharacterMoveControll _character)
    {
       
        character = _character;
        rotateSound.Play();
        StartCoroutine(WheelRotate());
    }
   IEnumerator WheelRotate()
    {
       
        rotateSound.volume = 1;
        rotateSpeed = Random.Range(25, 30);
        float increaseValue = Random.Range(4, 6);
        while (rotateSpeed>3)
        {
            wheel.transform.Rotate(0, 0, rotateSpeed);
            rotateSpeed -=Time.deltaTime* increaseValue;
            
            yield return null;
        }
        while (rotateSpeed > 0)
        {
            wheel.transform.Rotate(0, 0, rotateSpeed);
            rotateSpeed -= Time.deltaTime;
            rotateSound.volume -= Time.deltaTime*0.47f;
            yield return null;
        }
        rotateSound.volume = 0;
        lastRotationPos = wheel.GetComponent<RectTransform>().eulerAngles.z;
        RotateValue(lastRotationPos);//lastRotationPos
    }
    void RotateValue(float rotate)
    {   //6 adým ileri
        if (rotate >= 0 && rotate < 90)
            Parchis3DManager.Instance.WheelPenalty(0);

        //Tekrar zar at
        else if (rotate >= 90 && rotate < 180)
            Parchis3DManager.Instance.WheelPenalty(1);
        //5 adým geri
        else if (rotate >= -180 && rotate < 270)
            Parchis3DManager.Instance.WheelPenalty(2);

        //Bir tur bekle
        else if (rotate >= -270 && rotate < 360)
            Parchis3DManager.Instance.WheelPenalty(3);
        else
            Parchis3DManager.Instance.IncreasePlayerIndex();
    }
}
