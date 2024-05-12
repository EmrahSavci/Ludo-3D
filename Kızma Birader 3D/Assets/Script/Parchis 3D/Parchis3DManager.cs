using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Parchis3DManager : MonoBehaviour
{
    public static Parchis3DManager Instance;

    public List<ParchisPlayerControll> players = new List<ParchisPlayerControll>();
    public List<GameObject> diceModel = new List<GameObject>();
    public List<GameObject> diceArrow = new List<GameObject>();
    public List<GameObject> player_Blur = new List<GameObject>();
    public int PlayerIndex = 0;

    public Sprite dicequestionmarkSprite;
    public bool againRotateDice = true;

    public int testDiceValue = 0;
    public List<GameObject> WinPanel = new List<GameObject>();
    public int WinnerCharacterRank = 1;
    public int PlayerCount = 4,PlayerCountIndex=3;
    public GameObject exitPanel;
    public bool isGameFinish = false;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        diceModel[PlayerIndex].SetActive(true);
        // player_Blur[PlayerIndex].SetActive(true);
        diceArrow[PlayerIndex].SetActive(true);
    }


    public int value = 0;
    public bool isValueSix = false;
    public void GotoNextPlayer(bool isCharacterFinish)
    {
        if (isCharacterFinish)
            return;

        if (value < 6 && !isValueSix)
        {
            IncreasePlayerIndex();
        }
    }

    public void IncreasePlayerIndex()
    {
        PlayerIndex++;

        if (PlayerIndex >PlayerCountIndex)
            PlayerIndex = 0;
        if (players[PlayerIndex].isFinishMyGame || players[PlayerIndex].isNextPlayer)
        {
            players[PlayerIndex].isNextPlayer = false;
            IncreasePlayerIndex();
        }


        NextPlayer(false);
    }
    public int righttorollthedice = 1;
    public void NextPlayer(bool isWheel)
    {  if (isGameFinish)
            return;
        for (int i = 0; i < players.Count; i++)
        {
            diceModel[i].SetActive(false);
            player_Blur[i].SetActive(false);
            diceArrow[i].SetActive(false);
        }

        if (!isWheel)
        {
            diceModel[PlayerIndex].SetActive(true);
            diceModel[PlayerIndex].GetComponent<DiceRotate>().rotateCount = 0;
            diceModel[PlayerIndex].GetComponent<BoxCollider>().enabled = true;
            //player_Blur[PlayerIndex].SetActive(true);
            diceArrow[PlayerIndex].SetActive(true);
            againRotateDice = true;
        }

    }
    public void WheelPenalty(int penaltyValue)
    {
        switch (penaltyValue)
        {
            case 0:
                players[PlayerIndex].MoveForwardSix();
                break;
            case 1:
                // players[PlayerIndex].isDiceValueSixCount++;
                NextPlayer(false);
                break;
            case 2:
                players[PlayerIndex].MoveBackFife();
                break;
            case 3:
                players[PlayerIndex].GotoFirstPos();

                IncreasePlayerIndex();
                break;
        }
    }
    public void WinnerCharacter(int playerIndex)
    {
        WinPanel[PlayerIndex].SetActive(true);
        WinPanel[PlayerIndex].GetComponentInChildren<TextMeshProUGUI>().text = WinnerCharacterRank.ToString() + ".";
        WinnerCharacterRank++;
        if (WinnerCharacterRank >= PlayerCount)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if(players[i].isFinishMyGame==false)
                {
                    WinPanel[players[i].playerIndex].SetActive(true);
                    WinPanel[players[i].playerIndex].GetComponentInChildren<TextMeshProUGUI>().text = WinnerCharacterRank.ToString() + ".";
                }
            }
            QuitGame.Instance.finishPanel.SetActive(true);
           // QuitGame.Instance.exitBtn.SetActive(false);
        }

    }

}



