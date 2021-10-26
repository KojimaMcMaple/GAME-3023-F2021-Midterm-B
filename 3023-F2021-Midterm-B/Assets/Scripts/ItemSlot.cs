using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Display item in the slot, update image, make clickable when there is an item, invisible when there is not
public class ItemSlot : MonoBehaviour, IDropHandler
{
    //https://youtu.be/BGr-7GZJNXg - Simple Drag Drop (Unity Tutorial for Beginners)

    //public Item itemInSlot = null;

    //[SerializeField]
    //private int itemCount = 0;
    //public int ItemCount
    //{
    //    get
    //    {
    //        return itemCount;
    //    }
    //    set
    //    {
    //        itemCount = value;
    //    }
    //}

    //[SerializeField]
    //private Image icon;
    //[SerializeField]
    //private TMPro.TextMeshProUGUI itemCountText;

    private RectTransform rectt_;

    void Awake()
    {
        //RefreshInfo();
        rectt_ = GetComponent<RectTransform>();
    }

    //public void UseItemInSlot()
    //{
    //    if(itemInSlot != null)
    //    {
    //        itemInSlot.Use();
    //        if (itemInSlot.isConsumable)
    //        {
    //            itemCount--;
    //            RefreshInfo();
    //        }
    //    }
    //}

    //public void RefreshInfo()
    //{
    //    if(ItemCount < 1)
    //    {
    //        itemInSlot = null;
    //    }

    //    if(itemInSlot != null) // If an item is present
    //    {
    //        //update image and text
    //        itemCountText.text = ItemCount.ToString();
    //        icon.sprite = itemInSlot.icon;
    //        icon.gameObject.SetActive(true);
    //    } else
    //    {
    //        // No item
    //        itemCountText.text = "";
    //        icon.gameObject.SetActive(false);
    //    }
    //}

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(transform.name + " dropped");
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = rectt_.position;
        }
    }
}
