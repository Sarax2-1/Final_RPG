using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class LeftClick : MonoBehaviour
{
    public static LeftClick instance;

    private Camera cam;

    //[SerializeField]
    //private Character curChar;
    //public Character CurChar { get { return curChar; } }

    [SerializeField]
    private RectTransform boxSelection;
    private Vector2 oldAnchoredPos;
    private Vector2 startPos;

    [SerializeField]
    private LayerMask layerMask;

    private void Start()
    {
        instance = this;
        cam = Camera.main;
        layerMask = LayerMask.GetMask("Ground", "Character", "Building", "Item");

        boxSelection = UIManager.instance.SelectionBox;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startPos = Mouse.current.position.value;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            //clearEverything();
        }

        if (Mouse.current.leftButton.isPressed)
        {
            // if (EventSystem.current.IsPointerOverGameObject())
            //     return;

            UpdateSelectionBox(Mouse.current.position.value);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ReleaseSelectionBox(Mouse.current.position.value);
            TrySelect(Mouse.current.position.value);
        }
    }

    private int SelectCharacter(RaycastHit hit)
    {
        clearEverything();

        Character hero = hit.collider.GetComponent<Character>();
        // Debug.Log("Select Char: " + hit.collider.gameObject);

        int i = PartyManager.instance.FidIndexFromClass(hero);
        UIManager.instance.ToggleAvatar[i].isOn = true;

        return i;
    }

    private void TrySelect(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        int i = 0;

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            switch (hit.collider.tag)
            {
                case "Player":
                case "Hero":
                    i = SelectCharacter(hit);
                    break;
                case "Item":
                    SelectItem(hit);
                    break;
            }
        }

        if (PartyManager.instance.SelectChars.Count == 0)
        {
            UIManager.instance.ToggleAvatar[i].isOn = true;
        }
    }
    private void ClearRingSelection()
    {
        foreach (Character h in PartyManager.instance.SelectChars)
            h.ToggleRingSelection(false);
    }
    private void clearEverything()
    {
        foreach (Toggle t in UIManager.instance.ToggleAvatar)
            t.isOn = false;

        ClearRingSelection();
        PartyManager.instance.SelectChars.Clear();
    }

    private void UpdateSelectionBox(Vector2 mousePos)
    {
        //Debug.log("Mouse Pos -" + mousePos);
        if (!boxSelection.gameObject.activeInHierarchy)
            boxSelection.gameObject.SetActive(true);

        float width = mousePos.x - startPos.x;
        float height = mousePos.y - startPos.y;

        boxSelection.anchoredPosition = startPos + new Vector2(width / 2, height / 2);

        width = Mathf.Abs(width);
        height = Mathf.Abs(height);

        boxSelection.sizeDelta = new Vector2(width, height);

        oldAnchoredPos = boxSelection.anchoredPosition;
    }

    private void ReleaseSelectionBox(Vector2 mousePos)
    {
        Vector2 corner1; //down-left corner
        Vector2 corner2; //top-right corner

        boxSelection.gameObject.SetActive(false);

        corner1 = oldAnchoredPos - (boxSelection.sizeDelta / 2);
        corner2 = oldAnchoredPos + (boxSelection.sizeDelta / 2);

        bool anyNewCharSelect = false;

        foreach (Character member in PartyManager.instance.Members)
        {
            Vector2 unitPos = cam.WorldToScreenPoint(member.transform.position);
            if ((unitPos.x > corner1.x && unitPos.x < corner2.x)
                && (unitPos.y > corner1.y && unitPos.y < corner2.y))
            {
                if (anyNewCharSelect == false)
                {
                    anyNewCharSelect = true;
                    clearEverything();
                }

                int i = PartyManager.instance.FidIndexFromClass(member);
                UIManager.instance.ToggleAvatar[i].isOn = true;
            }
            boxSelection.sizeDelta = new Vector2(0, 0);
        }
    }

    private void SelectItem(RaycastHit hit)
    {
        ItemPick itemPick = hit.collider.GetComponent<ItemPick>();
        //Debug.Log("Pick Item: " + itemPick.Item.ItemName);

        if (PartyManager.instance.SelectChars.Count == 0)
            UIManager.instance.ToggleAvatar[0].isOn = true;

        if (itemPick != null)
            itemPick.PickUpItem(PartyManager.instance.SelectChars[0]);
    }
}