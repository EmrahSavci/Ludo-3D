using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parchis3DMovePoint : MonoBehaviour
{
    public List<CharacterMoveControll> Character = new List<CharacterMoveControll>();

    public bool isDestroyCharacter = true;
    public bool isBonusPoint = false;
    public GameObject boxBonus;
    [SerializeField] List<CharacterPos3D> characterPos = new List<CharacterPos3D>();
    public float piyonRotZ = 0;
    public bool areTheretwoCh = false;
    public bool isFinishPoint = false;
    public ParchisPlayerControll myPlayer;
    public int childPiyonIndex = 0;
    void Start()
    {
        if (boxBonus != null)
            LeanTween.rotateAroundLocal(boxBonus, new Vector3(0, 0, 360), 360, 1f).setRepeat(-1);
    }


    public void AddCharacter(CharacterMoveControll parchisPlayerMove)
    {
        
        parchisPlayerMove.transform.parent = transform;

        if (!Character.Contains(parchisPlayerMove))
            Character.Add(parchisPlayerMove);

        if(Character.Count==1 && isFinishPoint)
        {
            areTheretwoCh = true;
            myPlayer.FinishGameControll(1);
            Parchis3DManager.Instance.GotoNextPlayer(false);
        }
        else if(isBonusPoint)
        {
            Parchis3DManager.Instance.NextPlayer(true);
            //boxBonus.SetActive(false);
            ParchisWheelControll.Instance.PenaltyCharacter(parchisPlayerMove);
            if (Character.Count > 1 && Character[0].playerIndex != Character[1].playerIndex && isDestroyCharacter == true)
            {

                Character[0].Fail();

                SoundManager.Instance.SoundPlay(2);
                Character.RemoveAt(0);
            }
        }
       else if (Character.Count > 1 && Character[0].playerIndex != Character[1].playerIndex && isDestroyCharacter == true)
        {

            Character[0].Fail();

            SoundManager.Instance.SoundPlay(2);
            Character.RemoveAt(0);
        }
        else if (Character.Count > 1 && Character[0].playerIndex == Character[1].playerIndex && isDestroyCharacter == true)
        {
            childPiyonIndex = Character[0].playerIndex;
            GetChildChCount(true, 1); 
            Parchis3DManager.Instance.GotoNextPlayer(false);
        }
        
        else if (parchisPlayerMove.totalMovePoint > 1)
        {
            parchisPlayerMove.parentPlayer.ChangePlayer();
        }
        ChildLocalPos();
    }
    public void ChildLocalPos()
    {
        for (int i = 0; i < Character.Count; i++)
        {
            Character[i].transform.localScale = new Vector3(characterPos[Character.Count - 1].localScale, characterPos[Character.Count - 1].localScale, characterPos[Character.Count - 1].localScale);
            Character[i].transform.localPosition = characterPos[Character.Count - 1].localPosition[i];
            Character[i].transform.localEulerAngles = new Vector3(0, piyonRotZ, 0);
        }
    }
    public void RemovoCharacter(CharacterMoveControll parchisPlayerMove)
    {
        Character.Remove(parchisPlayerMove);
        ChildLocalPos();
        if (Character.Count <= 1)
            GetChildChCount(false, 0);
        if(isFinishPoint)
            myPlayer.FinishGameControll(-1);
        
    }
    void GetChildChCount(bool _enable, int startIndex)
    {   
        areTheretwoCh = _enable;
        for (int i = startIndex; i < Character.Count; i++)
        {
            Character[i].isSecondCh = _enable;
        }
        if (boxBonus != null)
            boxBonus.SetActive(true);
    }
}
[System.Serializable]
public class CharacterPos3D
{
    public List<Vector3> localPosition = new List<Vector3>();
    public float localScale;
}

