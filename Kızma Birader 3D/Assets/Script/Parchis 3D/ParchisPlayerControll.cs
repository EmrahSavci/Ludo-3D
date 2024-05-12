using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParchisPlayerControll : MonoBehaviour
{

    public int playerIndex = 0;
    public List<CharacterMoveControll> characters = new List<CharacterMoveControll>();
    public List<CharacterMoveControll> moveCharacters = new List<CharacterMoveControll>();

    public int diceValue = 0;
    public bool myGameStart = false;
    public int countOfEndCharacter = 0;
    public ParticleSystem confetti;
    public bool isFinishMyGame = false;
    public bool isNextPlayer = false;
    public DiceRotate myDice;
    public CharacterMoveControll selectedCharacter;
    void Start()
    {

    }
    bool areThereMoveCharacter = false;

    // Update is called once per frame
    public void AdvanceCharacter(int _diceValue)
    {
        diceValue = _diceValue;
        if (diceValue >= 6)
            myGameStart = true;

        if (!myGameStart && moveCharacters.Count == 0)
        {
            Invoke("ChangePlayer", 1);
            return;
           
        }
        areThereMoveCharacter = false;
        for (int i = 0; i < characters.Count; i++)
        {
            CharacterMoveControll MovingCharacter = characters[i];
            if (MovingCharacter.totalMovePoint >= diceValue)
            {
                try
                {
                    MovingCharacter.moveCount = diceValue;
                    if (diceValue >= 6 && !MovingCharacter.outArea)
                    {


                        MovingCharacter.move = true;
                        areThereMoveCharacter = true;
                        MovingCharacter.selectEffect.Play();
                    }



                  
                   
                     if ((MovingCharacter.outArea && !MovingCharacter.isSecondCh && !MovingCharacter.movePoints[((MovingCharacter.pointIndex + MovingCharacter.moveCount) - 1)].GetComponent<Parchis3DMovePoint>().areTheretwoCh)|| (MovingCharacter.outArea && MovingCharacter.movePoints[((MovingCharacter.pointIndex + MovingCharacter.moveCount) - 1)].GetComponent<Parchis3DMovePoint>().childPiyonIndex==MovingCharacter.playerIndex))
                    {
                      
                        CanTheCharacterMove(MovingCharacter);
                    }
                }

                catch (Exception e)
                {
                    Debug.Log("hata oluþtu");
                }
            }


        }
        if (!areThereMoveCharacter)
        {
            Invoke("ChangePlayer2", 1);
        }
        
    }
    void CanTheCharacterMove(CharacterMoveControll character)
    {
        
        if (character.pointIndex <=44 &&
            (!character.movePoints[(character.pointIndex + character.moveCount) - 1].GetComponent<Parchis3DMovePoint>().areTheretwoCh || 
             character.movePoints[(character.pointIndex + character.moveCount) - 1].GetComponent<Parchis3DMovePoint>().childPiyonIndex==playerIndex))
        {
            character.move = true;
            areThereMoveCharacter = true;
            character.selectEffect.Play();
            character.transform.localPosition = Vector3.zero;
            character.transform.localScale = Vector3.one * 1.5f;
            
        }
        
    }
    #region  WHEEL_PEANLTY
    public void MoveForwardSix()
    {   
        
        if(!selectedCharacter.movePoints[(selectedCharacter.pointIndex + 6) - 1].GetComponent<Parchis3DMovePoint>().areTheretwoCh)
        {
            selectedCharacter.moveCount = 6;
            selectedCharacter.Move(false);
        }
        
        else
            Parchis3DManager.Instance.IncreasePlayerIndex();
    }
    public void MoveBackFife()
    {
        selectedCharacter.moveCount = 0;
        selectedCharacter.Move(true);
    }
    public void GotoFirstPos()
    {
        isNextPlayer = true;
       // selectedCharacter.Fail();
    }
    #endregion
    public int isDiceValueSixCount = 1;
    public void ChangePlayer()
    {
        if ((diceValue < 6 || isFinishMyGame )&& isDiceValueSixCount<=0)
            Parchis3DManager.Instance.IncreasePlayerIndex();
    }
    void ChangePlayer2()
    {
       Parchis3DManager.Instance.IncreasePlayerIndex();
    }
    public void CloseArrowOfCharacters()
    {
        for (int i = 0; i < characters.Count; i++)
        {

            characters[i].move = false;
            characters[i].selectEffect.Stop();
           

        }
    }
    public void FinishGameControll(int finishChCount)
    {
        myGameStart = false;
        countOfEndCharacter+=finishChCount;
        SoundManager.Instance.SoundPlay(4);
        
        
        if (countOfEndCharacter >= characters.Count)
        {
            confetti.Play();
            isFinishMyGame = true;
            Parchis3DManager.Instance.WinnerCharacter(playerIndex);
            ChangePlayer();
        }
        else
        {
            Parchis3DManager.Instance.GotoNextPlayer(true);
        }
    }
    public void FinishCharacter(CharacterMoveControll parchisPlayerMove)
    {
        myGameStart = false;
        countOfEndCharacter++;
        SoundManager.Instance.SoundPlay(4);
        characters.Remove(parchisPlayerMove);
        confetti.Play();
        if (countOfEndCharacter >= characters.Count)
        {
            isFinishMyGame = true;
            Parchis3DManager.Instance.WinnerCharacter(playerIndex);
            ChangePlayer();
        }
        else
        {
            Parchis3DManager.Instance.GotoNextPlayer(true);
        }
    }
    public void CalculetaScaleCharacters()
    {
        myDice.gameObject.SetActive(false);
        for (int i = 0; i < characters.Count; i++)
        {

            characters[i].AgainScaleCalculate();


        }
    }
}
