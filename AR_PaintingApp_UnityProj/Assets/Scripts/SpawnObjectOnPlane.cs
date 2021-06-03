using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

//The following script was taken from:
//https://youtu.be/VMjZ70PmnPs

//Used to place objects on plane, not currently being used but good reference for AR Raycasts and may be useful in future

[RequireComponent(typeof(ARRaycastManager))]
public class SpawnObjectOnPlane : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    private List<GameObject> placedPrefabList = new List<GameObject>();

    [SerializeField]
    private int maxPrefabSpawnCount = 0;
    private int placedPrefabCount;

    [SerializeField]
    public GameObject PlaceablePrefab;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;


    }

    public void SetPrefabType(GameObject prefabType)
    {
        PlaceablePrefab = prefabType;
    }


    private void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPos = s_Hits[0].pose;
    
            if (placedPrefabCount < maxPrefabSpawnCount)
            {
                SpawnPrefab(hitPos);
            }

            
        }
    }

    private void SpawnPrefab(Pose hitPos)
    {
      spawnedObject = Instantiate(PlaceablePrefab, hitPos.position, hitPos.rotation);
    placedPrefabList.Add(spawnedObject);
         placedPrefabCount++;
    }

    public void DeleteAllPrefabs()
    {
        for (int i = 0; i < placedPrefabList.Count; i++)
        {
            Destroy(placedPrefabList[i], 0.0f);
        }
        placedPrefabCount = 0;
    }

}
