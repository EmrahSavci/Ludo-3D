using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRotate : MonoBehaviour
{
    public ParchisPlayerControll playerControll;
    public List<Vector3> rotateValues = new List<Vector3>();
    public int diceValue = 0;
    public int testValue = 0;
    BoxCollider boxCollider;
    public GameObject rollObject;
    private void OnMouseDown()
    {
        SoundManager.Instance.SoundPlay(0);
        rollObject.SetActive(false);
        Rotate();
    }
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    public int rotateCount = 0;
    public int rotateTurn = 1;
    public void TouchToDice()
    {
        SoundManager.Instance.SoundPlay(0);
        rollObject.SetActive(false);
        Rotate();
    }
    void Rotate()
    {
        boxCollider.enabled = false;
        float rotateX = 0;
        float rotateY = 0;
        float rotateZ = 0;
        if (rotateCount <= 10)
        {
            if (rotateTurn == 1)
                rotateX = Random.Range(0, 360);
            else
                rotateY = Random.Range(0, 360);

            rotateZ = Random.Range(0, 360);
            LeanTween.rotate(gameObject, new Vector3(rotateX, rotateY, rotateZ), 0.1f).setOnComplete(() =>
            {
                rotateCount++;
                Rotate();
                rotateTurn *= -1;
            });

        }
        else
        {
            if (playerControll.moveCharacters.Count == 0)
                diceValue = 5;
            else
                diceValue = Random.Range(0, 6);

            LeanTween.rotate(gameObject, rotateValues[diceValue], 0.1f);
            playerControll.AdvanceCharacter((diceValue + 1));//(diceValue + 1)

            if ((diceValue + 1) == 6)
                playerControll.isDiceValueSixCount++;
            else
                playerControll.isDiceValueSixCount--;

            Parchis3DManager.Instance.value =( diceValue + 1);
            if ((diceValue + 1) == 6)
                SoundManager.Instance.SoundPlay(1);

        }


    }
   
}
