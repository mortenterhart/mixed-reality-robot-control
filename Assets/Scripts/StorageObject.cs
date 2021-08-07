using System.Collections.Generic;
using UnityEngine;

public class StorageObject : MonoBehaviour
{
    [SerializeField] private GameObject storageObjectParent;
    [SerializeField] private List<GameObject> objectPositions;

    private int objectPos = 0;

    public void SetObjectPos(int objectPos)
    {
        this.objectPos = objectPos;
    }

    public void SetStorageObjectParentPosition()
    {
        storageObjectParent.transform.position = objectPositions[objectPos].transform.position;
    }
}
