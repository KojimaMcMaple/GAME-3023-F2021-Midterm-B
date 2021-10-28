using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropBehavior : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    //https://youtu.be/BGr-7GZJNXg - Simple Drag Drop (Unity Tutorial for Beginners)
    private Canvas canvas_ = null;
    private RectTransform rectt_;
    private CanvasGroup canvas_group_; //enables dropping into itemslots
    private ItemController item_controller_;

    void Awake()
    {
        rectt_ = GetComponent<RectTransform>();
        canvas_group_ = GetComponent<CanvasGroup>();
        canvas_ = GetComponentInParent<Canvas>(); //recurses upwards until it finds a GameObject with a matching component. Only components on active GameObjects are matched.
        if (canvas_ == null)
        {
            Debug.LogError("> KH_ERR: No Canvas component found in parents for " + transform.name);
        }
        item_controller_ = GetComponent<ItemController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log(transform.name + " clicked");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvas_group_.alpha = 1.0f;
        canvas_group_.blocksRaycasts = false; //enables raycast to reach itemslots
        if (item_controller_.GetContainer() != null)
        {
            foreach (var coord in item_controller_.GetSlotList())
            {
                item_controller_.GetContainer().GetGrid().GetItemSlotAtCoord(coord.x, coord.y).SetIsOccupied(false);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvas_group_.alpha = 1.0f;
        canvas_group_.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position += new Vector3(eventData.delta.x, eventData.delta.y, transform.position.z); //NOT USED, changed we move the icon, not the gameObj
        rectt_.anchoredPosition += eventData.delta / canvas_.scaleFactor; //icon normally moves by screen position ratio of 1, but canvas has to scale to fit screen 
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log(transform.name + " dropped");
    }
}
