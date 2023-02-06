using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideManManager : MonoBehaviour
{
    [SerializeField]
    GameObject foodPrefab;

    [SerializeField]
    GameObject foodInstance;

    [SerializeField]
    GameObject playerInstance;

    [SerializeField]
    Vector3 topRightBoundary;
    [SerializeField]
    Vector3 btmLeftBoundary;

    Vector3 lastknownfoodpos;
    int lasthitElement = 0;
    public Transform[] targetPoints;
    //Start is called before the first frame update
    void Start()
    {
        lastknownfoodpos = foodInstance.transform.localPosition;
    }

    //try to move it close to the original location
    //public Vector3 MoveFood()
    //{
    //    return foodInstance.transform.localPosition = new Vector3(
    //            Random.Range(btmLeftBoundary.x, topRightBoundary.x),
    //           Random.Range(btmLeftBoundary.y, topRightBoundary.y),
    //            Random.Range(btmLeftBoundary.z, topRightBoundary.z));
    //}


    public Vector3 MoveFood(float distFrom = 5, bool nextFood = false)
    {
        if (nextFood)
        {
            if (lasthitElement < targetPoints.Length-1)
            {
                lasthitElement++;
            }
            else
            {
                lasthitElement = 0;
            }
        }
        Vector3 foodpos = targetPoints[lasthitElement].localPosition
        +new Vector3(
            Random.Range(-distFrom, distFrom),
            0,
            Random.Range(-distFrom, distFrom));

        foodInstance.transform.localPosition = foodpos;
        return foodpos;
    }


    public void MoveAgent()
    {
        playerInstance.transform.localPosition = new Vector3(
                Random.Range(btmLeftBoundary.x, topRightBoundary.x),
               Random.Range(btmLeftBoundary.y, topRightBoundary.y),
                Random.Range(btmLeftBoundary.z, topRightBoundary.z));
    }

}
