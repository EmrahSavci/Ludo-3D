using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParchisMultiTouch : MonoBehaviour
{
    public List<touchLocation> touches = new List<touchLocation>();
    public Camera cam;
    public LayerMask parchisPiyon;
    public LayerMask diceLayer;
    // Update is called once per frame
    void Update()
    {
      

        int i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            if (t.phase == TouchPhase.Began)
            {
                //Debug.Log("touch began");

                Ray touchPosition = cam.ScreenPointToRay(t.position);
                RaycastHit hit;

                if(Physics.Raycast(touchPosition, out hit,100,parchisPiyon))
                {
                    if (hit.collider != null && hit.collider.GetComponent<CharacterMoveControll>() != null)
                    {
                        //Debug.Log("KARAKTER SEÇÝLDÝ " + hit.collider.name);
                        hit.collider.GetComponent<CharacterMoveControll>().MoveRay();
                        touches.Add(new touchLocation(t.fingerId, hit.collider.gameObject));
                    }
                }
               else if (Physics.Raycast(touchPosition, out hit, 100, diceLayer))
                {
                    if (hit.collider != null && hit.collider.GetComponent<DiceRotate>() != null)
                    {
                        //Debug.Log("ZAR SEÇÝLDÝ " + hit.collider.name);
                        hit.collider.GetComponent<DiceRotate>().TouchToDice();
                        touches.Add(new touchLocation(t.fingerId, hit.collider.gameObject));
                    }
                }


            }
           
            ++i;
        }
    }
    Vector2 getTouchPosition(Vector2 touchPosition)
    {
        return cam.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, transform.position.z));
    }
}
