using UnityEngine;

public class RobotCommands : MonoBehaviour
{
    private int shelfId = -1;
    private MRMqttClient client;

    private void Awake()
    {
        client = GetComponent<MRMqttClient>();
    }

    public void StoreItem()
    {
        Debug.Log($"Store to shelf {shelfId}");

        if (shelfId != -1)
        {
            client.SendStoreIn(shelfId);
        }
    }

    public void LoadItem()
    {
        Debug.Log($"Load from shelf {shelfId}");

        if (shelfId != -1)
        {
            client.SendStoreOut(shelfId);
        }
    }

    public void SelectShelf(int shelfId)
    {
        Debug.Log($"Selecting shelf {shelfId}");
        this.shelfId = shelfId;
    }
}
    