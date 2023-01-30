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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject SpawnFood()
    {
        return Instantiate(foodPrefab,
            new Vector3(
                Random.Range(btmLeftBoundary.x, topRightBoundary.x),
               Random.Range(btmLeftBoundary.y, topRightBoundary.y),
                Random.Range(btmLeftBoundary.z, topRightBoundary.z)) + transform.position,
            Quaternion.identity, transform);
    }

    public void MoveFood()
    {
        foodInstance.transform.localPosition = new Vector3(
                Random.Range(btmLeftBoundary.x, topRightBoundary.x),
               Random.Range(btmLeftBoundary.y, topRightBoundary.y),
                Random.Range(btmLeftBoundary.z, topRightBoundary.z));
    }

    public void MoveAgent()
    {
        playerInstance.transform.localPosition = new Vector3(
                Random.Range(btmLeftBoundary.x, topRightBoundary.x),
               Random.Range(btmLeftBoundary.y, topRightBoundary.y),
                Random.Range(btmLeftBoundary.z, topRightBoundary.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (foodInstance == null)
        {
            foodInstance = SpawnFood();
        }   
    }
}
