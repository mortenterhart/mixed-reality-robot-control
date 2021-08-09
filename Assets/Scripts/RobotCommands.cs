using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

public class RobotCommands : MonoBehaviour
{
    private static readonly Color ColorFreeShelf = new Color(0.11f, 0.75f, 0f);
    private static readonly Color ColorOccupiedShelf = new Color(0.9f, 0f, 0f);
    
    private static readonly int ParamShelfId = Animator.StringToHash("shelfId");
    private static readonly int ParamMoveObject = Animator.StringToHash("moveObject");
    private static readonly int TriggerStoreIn = Animator.StringToHash("StoreIn");
    private static readonly int TriggerStoreOut = Animator.StringToHash("StoreOut");

    [SerializeField] private GameObject smallDialogPrefab;
    
    [SerializeField] private List<GameObject> shelfButtonQuads;
    [SerializeField] private TextMeshPro selectedShelfText;

    [SerializeField] private Animator robotAnimator;
    [SerializeField] private StorageObject storageObjectScript;

    private MRMqttClient client;
    private int selectedShelfId = -1;
    
    private int objectPos = 0;

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
        
        if (objectPos == 0)
        {
            objectPos = selectedShelfId;
            SetShelfButtonQuadColor(ColorOccupiedShelf);
        }

        if (robotAnimator != null)
        {
            robotAnimator.SetTrigger(TriggerStoreIn);

            if (objectPos == 0)
            {
                storageObjectScript.SetObjectPos(objectPos);

                robotAnimator.SetBool(ParamMoveObject, true);
            }
            else
            {
                robotAnimator.SetBool(ParamMoveObject, false);
            }
        }

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
        
        if (objectPos == selectedShelfId)
        {
            objectPos = 0;
            SetShelfButtonQuadColor(ColorFreeShelf);
        }

        if (robotAnimator != null)
        {
            robotAnimator.SetTrigger(TriggerStoreOut);

            if (objectPos == selectedShelfId)
            {
                storageObjectScript.SetObjectPos(objectPos);

                robotAnimator.SetBool(ParamMoveObject, true);
            }
            else
            {
                robotAnimator.SetBool(ParamMoveObject, false);
            }
        }

        client.SendStoreOut(selectedShelfId);
    }
    
    public void SelectShelf(int shelfId)
    {
        Debug.Log($"Selecting shelf {shelfId}");
        
        selectedShelfId = shelfId;
        selectedShelfText.text = $"{shelfId}";

        if (robotAnimator != null)
        {
            robotAnimator.SetInteger(ParamShelfId, shelfId);
        }
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
    