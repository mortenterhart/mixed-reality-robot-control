using UnityEngine;

public class RobotCommandActions : MonoBehaviour
{
    private int _shelfId = -1;
    
    public void StoreItem()
    {
        Debug.Log($"Store to shelf {_shelfId}");
    }

    public void LoadItem()
    {
        Debug.Log($"Load from shelf {_shelfId}");
    }

    public void SelectShelf(int shelfId)
    {
        Debug.Log($"Selecting shelf {shelfId}");
        _shelfId = shelfId;
    }
}
    