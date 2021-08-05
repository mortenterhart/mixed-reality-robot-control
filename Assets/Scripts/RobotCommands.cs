using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

public class RobotCommands : MonoBehaviour
{
    private static readonly Color ColorFreeShelf = new Color(0.11f, 0.75f, 0f);
    private static readonly Color ColorOccupiedShelf = new Color(0.9f, 0f, 0f);
    
    private static readonly int ParamShelfId = Animator.StringToHash("shelfId");
    private static readonly int TriggerStoreIn = Animator.StringToHash("StoreIn");
    private static readonly int TriggerStoreOut = Animator.StringToHash("StoreOut");
    
    [SerializeField] private GameObject smallDialogPrefab;

    [SerializeField] private Animator robotAnimator;
    [SerializeField] private Animator objectAnimator;
    [SerializeField] private List<GameObject> shelfButtonQuads;
    [SerializeField] private TextMeshPro selectedShelfText;

    private int selectedShelfId = -1;
    private MRMqttClient client;

    private void Start()
    {
        client = GetComponent<MRMqttClient>();
        selectedShelfText.text = "";
    }

    public void StoreItem()
    {
        if (selectedShelfId == -1)
        {
            ShowSelectShelfDialog();
            return;
        }

        Debug.Log($"Store to shelf {selectedShelfId}");
        
        SetShelfButtonQuadColor(ColorOccupiedShelf);
        robotAnimator.SetTrigger(TriggerStoreIn);
            
        client.SendStoreIn(selectedShelfId);
    }

    public void LoadItem()
    {
        if (selectedShelfId == -1)
        {
            ShowSelectShelfDialog();
            return;
        }
        
        Debug.Log($"Load from shelf {selectedShelfId}");
        
        SetShelfButtonQuadColor(ColorFreeShelf);
        robotAnimator.SetTrigger(TriggerStoreOut);
        
        client.SendStoreOut(selectedShelfId);
    }
    
    public void SelectShelf(int shelfId)
    {
        Debug.Log($"Selecting shelf {shelfId}");
        
        selectedShelfId = shelfId;
        selectedShelfText.text = $"{shelfId}";
        
        robotAnimator.SetInteger(ParamShelfId, shelfId);
    }

    private void ShowSelectShelfDialog()
    {
        Dialog.Open(smallDialogPrefab, DialogButtonType.OK, "Notice", "Please select a shelf before using this command.", true);
    }

    private void SetShelfButtonQuadColor(Color color)
    {
        var quadRenderer = shelfButtonQuads[selectedShelfId - 1].GetComponent<Renderer>();
        quadRenderer.material.color = color;
    }
}
    