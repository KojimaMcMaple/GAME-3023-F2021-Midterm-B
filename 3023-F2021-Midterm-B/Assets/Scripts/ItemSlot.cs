using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Display item in the slot, update image, make clickable when there is an item, invisible when there is not
public class ItemSlot : MonoBehaviour, IDropHandler
{
    //https://youtu.be/BGr-7GZJNXg - Simple Drag Drop (Unity Tutorial for Beginners)

    public Item itemInSlot = null;

    [SerializeField]
    private int itemCount = 0;
    public int ItemCount
    {
        get
        {
            return itemCount;
        }
        set
        {
            itemCount = value;
        }
    }

    //[SerializeField]
    //private Image icon;
    //[SerializeField]
    //private TMPro.TextMeshProUGUI itemCountText;

    private bool is_colliding_ = false;
    private bool is_occupied_ = false;

    private RectTransform rectt_;
    private Image image_;
    private Canvas canvas_ = null;

    void Awake()
    {
        //RefreshInfo();
        rectt_ = GetComponent<RectTransform>();
        image_ = GetComponent<Image>();
        canvas_ = GetComponentInParent<Canvas>(); //recurses upwards until it finds a GameObject with a matching component. Only components on active GameObjects are matched.
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
        //Debug.Log(transform.name + " dropped");
        //if (eventData.pointerDrag != null)
        //{
        //    //eventData.pointerDrag.GetComponent<RectTransform>().position = new Vector3( rectt_.position.x + rectt_.rect.width / 2 * canvas_.scaleFactor, 
        //    //                                                                            rectt_.position.y - rectt_.rect.height/ 2 * canvas_.scaleFactor );

        //    eventData.pointerDrag.GetComponent<RectTransform>().position = new Vector3(rectt_.position.x,
        //                                                                                rectt_.position.y);
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        is_colliding_ = true;
        if (!is_occupied_)
        {
            image_.color = Color.green;
        }
        else
        {
            image_.color = Color.red;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        is_colliding_ = false;
        if (!is_occupied_)
        {
            image_.color = Color.white;
        }
    }

    public bool IsColliding()
    {
        return is_colliding_;
    }

    public bool IsOccupied()
    {
        return is_occupied_;
    }

    public void SetIsOccupied(bool value)
    {
        is_occupied_ = value;
    }

    public RectTransform GetRectt()
    {
        return rectt_;
    }

    private void OnDrawGizmos()
    {
        RectTransform rectt = GetComponent<RectTransform>();
        Canvas canvas = GetComponentInParent<Canvas>();
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(rectt.position.x, rectt.position.y), 10.0f);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(new Vector3(rectt.position.x + rectt.rect.width/2 * canvas.scaleFactor, rectt.position.y - rectt.rect.height/2 * canvas.scaleFactor), 10.0f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(rectt.position.x - rectt.rect.width / 2 * canvas.scaleFactor, rectt.position.y + rectt.rect.height / 2 * canvas.scaleFactor), 5.0f);
    }
}
