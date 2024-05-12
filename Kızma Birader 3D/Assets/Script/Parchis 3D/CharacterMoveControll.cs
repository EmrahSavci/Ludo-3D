using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveControll : MonoBehaviour
{
    public List<Transform> movePoints = new List<Transform>();
    public List<GameObject> rocks = new List<GameObject>();
    public int pointIndex = 0;
    public int totalMovePoint = 0;
    public int moveCount = 0;

    [Space(10)]
    [Header("Boolen")]
    public bool move = false;
    public bool moveForward = false;
    public bool outArea = false;
    public bool isSecondCh = false;
    public ParchisPlayerControll parentPlayer;
    public int playerIndex = 0;

    [Space(20)]
    [Header("Transform Values")]
    public Vector3 firstPos;
    public Vector3 firstScale;
    public Vector3 firstRotation;
    [Space(15)]
    [Header("Particle Effects")]
    public ParticleSystem failEffect;
    public ParticleSystem smokeEffect;
    public ParticleSystem selectEffect;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        AnimatorEnable(false);

        parentPlayer = GetComponentInParent<ParchisPlayerControll>();
        totalMovePoint = movePoints.Count;

        firstPos = transform.position;
        firstScale = transform.localScale;
        firstRotation = transform.localEulerAngles;
    }

    private void OnMouseDown()
    {

        if (move)
        {
            parentPlayer.CalculetaScaleCharacters();
            Move(false);
        }

    }
    public void MoveRay()
    {
        if (move)
        {
            parentPlayer.CalculetaScaleCharacters();
            Move(false);
        }
    }
    public void Move(bool wheelPenalty)
    {
        if (totalMovePoint <= 0)
            return;

        GetComponent<Collider>().enabled = false;
        LeanTween.scale(gameObject, Vector3.one, 0.05f);
        move = false;

        parentPlayer.selectedCharacter = GetComponent<CharacterMoveControll>();
        if(moveCount==6)
        parentPlayer.isDiceValueSixCount--;
        RemoveCharacterToRock();

        if (moveForward && outArea && !wheelPenalty)
            MoveForward();
        else if (moveForward && outArea && wheelPenalty)
            GotoBack();
        else if (!outArea && moveCount >= 6 && !moveForward)
            GotoStartPos();


    }

    void GotoStartPos()
    {
        parentPlayer.CloseArrowOfCharacters();
        outArea = true;
        parentPlayer.moveCharacters.Add(GetComponent<CharacterMoveControll>());
        LeanTween.move(gameObject, movePoints[0].position, 0.3f).setOnComplete(() =>
        {
            LeanTween.moveLocalY(rocks[0], 1.4f, 0.1f);
            pointIndex++;
            AddCharacterToRock();

            moveForward = true;
            GetComponent<Collider>().enabled = true;
            Parchis3DManager.Instance.againRotateDice = true;
            Parchis3DManager.Instance.NextPlayer(false);
        });
    }
    void AnimatorEnable(bool _enable)
    {
        animator.enabled = _enable;
    }
    void GotoBack()
    {
        parentPlayer.CloseArrowOfCharacters();
        AnimatorEnable(true);
        if (moveCount >= 5)
        {   //KARAKTER GÝTMESÝ GEREKEN ALANA GÝTTÝÐÝNDE TEMAS ÝÞLEMLERÝ BURADA YAPILACAK
            GetComponent<Collider>().enabled = true;


            Parchis3DManager.Instance.againRotateDice = true;
            Parchis3DManager.Instance.NextPlayer(false);
            AddCharacterToRock();
            AnimatorEnable(false);
            return;
        }

        SoundManager.Instance.SoundPlay(3);
        RockAnimUp(pointIndex);
        pointIndex--;
        LeanTween.move(gameObject, movePoints[pointIndex].position, 0.2f).setOnComplete(() =>
         {

             
             totalMovePoint++;
             moveCount++;
             isSecondCh = false;

             GotoBack();
         });
    }
    void MoveForward()
    {
        AnimatorEnable(true);
        parentPlayer.CloseArrowOfCharacters();
        if (moveCount <= 0)
        {   //KARAKTER GÝTMESÝ GEREKEN ALANA GÝTTÝÐÝNDE TEMAS ÝÞLEMLERÝ BURADA YAPILACAK
            GetComponent<Collider>().enabled = true;


            Parchis3DManager.Instance.againRotateDice = true;
            Parchis3DManager.Instance.NextPlayer(false);
            AddCharacterToRock();
            AnimatorEnable(false);
            return;
        }

        SoundManager.Instance.SoundPlay(3);

        RockAnimUp(pointIndex - 1);

        LeanTween.move(gameObject, movePoints[pointIndex].position, 0.2f).setOnComplete(() =>
        {
            LocalRotateValue();

            RockAnimDown(pointIndex);

            


        });
    }
    void PointIndexIncrease()
    {
        pointIndex++;
        //if (pointIndex >= movePoints.Count)
        //    CharacterWin();
        totalMovePoint--;
        moveCount--;
        isSecondCh = false;

        MoveForward();
    }
    void RockAnimDown(int rockIndex)
    {
        LeanTween.moveLocalY(rocks[rockIndex], 1.4f, 0.1f).setOnComplete(() =>
        {

            PointIndexIncrease();

        });
    }
    void RockAnimUp(int rockIndex)
    {
        LeanTween.moveLocalY(rocks[rockIndex], 2, 0.1f);
        if (movePoints[rockIndex].transform.childCount >= 1)
            movePoints[rockIndex].GetChild(0).gameObject.SetActive(true);
    }
    void CharacterWin()
    {
        parentPlayer.FinishCharacter(GetComponent<CharacterMoveControll>());
    }
    void AddCharacterToRock()
    {
        movePoints[pointIndex - 1].GetComponent<Parchis3DMovePoint>().AddCharacter(GetComponent<CharacterMoveControll>());
    }
    public void RemoveCharacterToRock()
    {
        if (pointIndex >= 1)
            movePoints[pointIndex - 1].GetComponent<Parchis3DMovePoint>().RemovoCharacter(GetComponent<CharacterMoveControll>());
    }
    public void AgainScaleCalculate()
    {
        if (pointIndex >= 1)
            movePoints[pointIndex - 1].GetComponent<Parchis3DMovePoint>().ChildLocalPos();
    }
    void LocalRotateValue()
    {
        if (pointIndex >= 1)
            transform.localEulerAngles = new Vector3(0, movePoints[pointIndex].GetComponent<Parchis3DMovePoint>().piyonRotZ, 0);
    }
    public void Fail()
    {
        transform.parent = parentPlayer.transform;
        //RemoveCharacterToRock();
        GetComponent<Collider>().enabled = false;
        failEffect.Play();
        RockAnimUp(pointIndex-1);
        Invoke("GotoFirstArea", 1);
    }
    public void GotoFirstArea()
    {



        parentPlayer.moveCharacters.Remove(GetComponent<CharacterMoveControll>());
        parentPlayer.myGameStart = false;
        LeanTween.move(gameObject, firstPos, 0.3f).setOnComplete(() =>
        {
            pointIndex = 0;
            totalMovePoint = movePoints.Count;
            moveCount = 0;
            outArea = false;
            moveForward = false;
            transform.localScale = firstScale;
            transform.localEulerAngles = firstRotation;
            GetComponent<Collider>().enabled = true;
            //animator.enabled = false;
            smokeEffect.Play();

        });

        //}

    }
}
