using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

public class RobotCommands : MonoBehaviour
{
    private static readonly Color ColorFreeShelf = new Color(0.11f, 0.75f, 0f);
    private static readonly Color ColorOccupiedShelf = new Color(0.9f, 0f, 0f);
    
    [SerializeField] private GameObject smallDialogPrefab;

    [SerializeField] private List<GameObject> shelfButtonQuads;
    [SerializeField] private TextMeshPro selectedShelfText;

    private int shelfId = -1;
    private MRMqttClient client;

    private void Start()
    {
        client = GetComponent<MRMqttClient>();
        selectedShelfText.text = "";
    }

    public void StoreItem()
    {
        if (shelfId == -1)
        {
            ShowSelectShelfDialog();
            return;
        }

        Debug.Log($"Store to shelf {shelfId}");
        SetShelfButtonQuadColor(ColorOccupiedShelf);
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
        SetShelfButtonQuadColor(ColorFreeShelf);
        client.SendStoreOut(shelfId);
    }
    
    public void SelectShelf(int shelfId)
    {
        Debug.Log($"Selecting shelf {shelfId}");
        this.shelfId = shelfId;

        selectedShelfText.text = $"{shelfId}";
    }

    private void ShowSelectShelfDialog()
    {
        Dialog.Open(smallDialogPrefab, DialogButtonType.OK, "Notice", "Please select a shelf before using this command.", true);
    }

    private void SetShelfButtonQuadColor(Color color)
    {
        var quadRenderer = shelfButtonQuads[shelfId - 1].GetComponent<Renderer>();
        quadRenderer.material.color = color;
    }
}
    