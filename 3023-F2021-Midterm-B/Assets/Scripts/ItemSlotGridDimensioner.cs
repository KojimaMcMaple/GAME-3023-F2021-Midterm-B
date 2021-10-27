using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Instantiates prefabs to fill a grid
[RequireComponent(typeof(GridLayout))]
public class ItemSlotGridDimensioner : MonoBehaviour, IDropHandler
{
    [SerializeField]
    GameObject itemSlotPrefab;

    [SerializeField]
    Vector2Int GridDimensions = new Vector2Int(6, 6);

    private List<ItemSlot> slot_list_ = new List<ItemSlot>();
    private Canvas canvas_ = null;

    void Awake()
    {
        int numCells = GridDimensions.x * GridDimensions.y;
        int count = 1;
        foreach (Transform child in transform)
        {
            slot_list_.Add(child.GetComponent<ItemSlot>());
        }
        while (transform.childCount < numCells)
        {
            GameObject newObject = Instantiate(itemSlotPrefab, this.transform);
            newObject.name = newObject.name + count;
            slot_list_.Add(newObject.GetComponent<ItemSlot>());
            count++;
        }
        canvas_ = GetComponentInParent<Canvas>(); //recurses upwards until it finds a GameObject with a matching component. Only components on active GameObjects are matched.
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log(transform.name + " dropped");
        if (eventData.pointerDrag != null)
        {
            RectTransform slot_rectt = new RectTransform();
            bool has_collision = false;
            for (int i = 0; i < slot_list_.Count; i++)
            {
                if (slot_list_[i].IsColliding())
                {
                    has_collision = true;
                    slot_rectt = slot_list_[i].GetRectt();
                    break;
                }
            }
            if (has_collision)
            {
                RectTransform item_rectt = eventData.pointerDrag.GetComponent<RectTransform>();
                item_rectt.position = new Vector3(  (item_rectt.rect.width / 2 - slot_rectt.rect.width / 2) * canvas_.scaleFactor + slot_rectt.position.x  ,
                                                    (-item_rectt.rect.height / 2 + slot_rectt.rect.height / 2) * canvas_.scaleFactor + slot_rectt.position.y ) ;
            }

            //eventData.pointerDrag.GetComponent<RectTransform>().position = new Vector3( rectt_.position.x + rectt_.rect.width / 2 * canvas_.scaleFactor, 
            //                                                                            rectt_.position.y - rectt_.rect.height/ 2 * canvas_.scaleFactor );

            //eventData.pointerDrag.GetComponent<RectTransform>().position = new Vector3(rectt_.position.x ,
            //                                                                            rectt_.position.y );
        }
    }
}
