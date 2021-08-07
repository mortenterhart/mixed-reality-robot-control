using System.Collections;
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

    [SerializeField] private GameObject storageObject;
    // [SerializeField] private List<GameObject> objectPositions;
    [SerializeField] private List<AnimationClip> objectStoreInAnimations;
    [SerializeField] private List<AnimationClip> objectStoreOutAnimations;

    private MRMqttClient client;
    private int selectedShelfId = -1;
    
    private int objectPos = 0;
    private StorageObject storageObjectScript;

    private void Start()
    {
        client = GetComponent<MRMqttClient>();
        selectedShelfText.text = "";

        storageObjectScript = storageObject.GetComponent<StorageObject>();
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
        if (objectPos == 0)
        {
            Debug.Log($"Animate object: Store in {selectedShelfId}");
            objectAnimator.SetTrigger(TriggerStoreIn);
            objectPos = selectedShelfId;
            storageObjectScript.SetObjectPos(objectPos);
            
            // StartObjectStoreInAnimation();

            // Invoke(nameof(SetStorageObjectParentPosition), objectStoreInAnimations[selectedShelfId - 1].length + 0.5f);
        }
            
        // client.SendStoreIn(selectedShelfId);
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
        if (objectPos == selectedShelfId)
        {
            objectAnimator.SetTrigger(TriggerStoreOut);
            objectPos = 0;
            storageObjectScript.SetObjectPos(objectPos);
            
            // Invoke(nameof(SetStorageObjectParentPosition), objectStoreOutAnimations[selectedShelfId - 1].length);
        }
        
        // client.SendStoreOut(selectedShelfId);
    }
    
    public void SelectShelf(int shelfId)
    {
        Debug.Log($"Selecting shelf {shelfId}");
        
        selectedShelfId = shelfId;
        selectedShelfText.text = $"{shelfId}";
        
        robotAnimator.SetInteger(ParamShelfId, shelfId);
        objectAnimator.SetInteger(ParamShelfId, shelfId);
    }

    private void ShowSelectShelfDialog()
    {
        Dialog.Open(smallDialogPrefab, DialogButtonType.OK, "Notice", "Please select a shelf before using this command.", true);
    }

    /*public void SetStorageObjectParentPosition()
    {
        Debug.Log("Set object position to " + objectPositions[objectPos].transform.position);
        storageObject.transform.position = objectPositions[objectPos].transform.position;
    }*/

    private void SetShelfButtonQuadColor(Color color)
    {
        var quadRenderer = shelfButtonQuads[selectedShelfId - 1].GetComponent<Renderer>();
        quadRenderer.material.color = color;
    }

    private void StartObjectStoreInAnimation()
    {
        switch (selectedShelfId)
        {
            case 1:
                StartCoroutine(AnimateObjectStoreIn1());
                break;
        }
    }

    private IEnumerator MoveObjectTimed(Vector3 move, float duration)
    {
        var startPos = storageObject.transform.position;
        var endPos = startPos + move;
        
        var timer = 0f;
        while (timer < duration)
        {
            storageObject.transform.position = Vector3.Lerp(startPos, endPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator AnimateObjectStoreIn1()
    {
        // yield return new WaitForSeconds(6 + 40 / 60f);
        yield return new WaitForSeconds(6.94f);

        yield return StartCoroutine(MoveObjectTimed(new Vector3(0, 0.02f, 0), 20 / 60f));
        yield return StartCoroutine(MoveObjectTimed(new Vector3(0, 0, 0.4f), 3f));
        yield return StartCoroutine(MoveObjectTimed(new Vector3(-0.975f, 1.2f, 0), 5f));
        yield return StartCoroutine(MoveObjectTimed(new Vector3(0, 0, -0.4f), 3f));
        yield return StartCoroutine(MoveObjectTimed(new Vector3(0, -0.02f, 0), 20 / 60f));
        
        /*var duration = 20/60f;
        var timer = 0f;
        
        var startPos = storageObject.transform.position;
        var endPos = startPos + new Vector3(0, 0.02f, 0);
        
        while (timer < duration)
        {
            storageObject.transform.position = Vector3.Lerp(startPos, endPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        
        duration = 3f;
        timer = 0f;

        startPos = endPos;
        endPos = startPos + new Vector3(0, 0, 0.4f);

        while (timer < duration)
        {
            storageObject.transform.position = Vector3.Lerp(startPos, endPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        duration = 5f;
        timer = 0f;

        startPos = endPos;
        endPos = startPos + new Vector3(-0.975f, 1.2f, 0);

        while (timer < duration)
        {
            storageObject.transform.position = Vector3.Lerp(startPos, endPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        duration = 3f;
        timer = 0f;

        startPos = endPos;
        endPos = startPos + new Vector3(0, 0, -0.4f);

        while (timer < duration)
        {
            storageObject.transform.position = Vector3.Lerp(startPos, endPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        duration = 20/60f;
        timer = 0f;
        
        startPos = endPos;
        endPos = startPos + new Vector3(0, -0.02f, 0);
        
        while (timer < duration)
        {
            storageObject.transform.position = Vector3.Lerp(startPos, endPos, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }*/
    }
}
    