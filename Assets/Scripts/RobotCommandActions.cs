using UnityEngine;

public class RobotCommandActions : MonoBehaviour
{
    public void StoreItem(int shelfId)
    {
        Debug.Log($"Store {shelfId}");
    }

    public void LoadItem(int shelfId)
    {
        Debug.Log($"Load {shelfId}");
    }
}
    