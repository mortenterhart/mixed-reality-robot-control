using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class RobotCommands : MonoBehaviour
{
    [SerializeField] private GameObject smallDialogPrefab;

    private int shelfId = -1;
    private MRMqttClient client;

    private void Awake()
    {
        client = GetComponent<MRMqttClient>();
    }

    public void StoreItem()
    {
        if (shelfId == -1)
        {
            ShowSelectShelfDialog();
            return;
        }

        Debug.Log($"Store to shelf {shelfId}");
        client.SendStoreIn(shelfId);
    }

    public void LoadItem()
    {
        if (shelfId == -1)
        {
            ShowSelectShelfDialog();
            return;
        }
        
        Debug.Log($"Load from shelf {shelfId}");
        client.SendStoreOut(shelfId);
    }

    private void ShowSelectShelfDialog()
    {
        Dialog.Open(smallDialogPrefab, DialogButtonType.OK, "Notice", "Please select a shelf before using this command.", true);
    }

    public void SelectShelf(int shelfId)
    {
        Debug.Log($"Selecting shelf {shelfId}");
        this.shelfId = shelfId;
    }
}
    