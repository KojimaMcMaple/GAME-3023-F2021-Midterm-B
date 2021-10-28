using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Instantiates prefabs to fill a grid
[RequireComponent(typeof(GridLayout))]
public class ItemSlotGridDimensioner : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Vector2Int grid_dimensions_ = new Vector2Int(5, 5);

    private ItemSlot[,] slot_list_;
    private Canvas canvas_ = null;

    void Awake()
    {
        canvas_ = GetComponentInParent<Canvas>(); //recurses upwards until it finds a GameObject with a matching component. Only components on active GameObjects are matched.
        
        int numCells = grid_dimensions_.x * grid_dimensions_.y;
        slot_list_ = new ItemSlot[grid_dimensions_.x, grid_dimensions_.y];
        int count = transform.childCount;
        while (transform.childCount < numCells)
        {
            GameObject newObject = Instantiate(itemSlotPrefab, this.transform);
            newObject.name = newObject.name + count;
            count++;
        }
        count = 0;
        for (int y = 0; y < grid_dimensions_.y; y++)
        {
            for (int x = 0; x < grid_dimensions_.x; x++)
            {
                if (count< transform.childCount)
                {
                    slot_list_[x, y] = transform.GetChild(count).GetComponent<ItemSlot>();
                    count++;
                }
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform slot_rectt = new RectTransform();
            bool has_collision = false;
            int collided_x = 0, collided_y = 0;

            for (int y = 0; y < grid_dimensions_.y; y++)
            {
                for (int x = 0; x < grid_dimensions_.x; x++)
                {
                    if (slot_list_[x,y].IsColliding() && !slot_list_[x, y].IsOccupied())
                    {
                        has_collision = true;
                        collided_y = y;
                        collided_x = x;
                        slot_rectt = slot_list_[x, y].GetRectt();
                        break;
                    }
                }
                if (has_collision){break;}
            }

            if (has_collision)
            {
                RectTransform item_rectt = eventData.pointerDrag.GetComponent<RectTransform>();
                ItemController item_controller = eventData.pointerDrag.GetComponent<ItemController>();
                if (CanPlaceOnSlots(collided_x, collided_y, item_controller))
                {
                    List<ItemSlot> item_slot_list = item_controller.GetSlotList();
                    if (item_slot_list.Count != 0)
                    {
                        foreach (var slot in item_slot_list)
                        {
                            slot.SetIsOccupied(false);
                        }
                    }
                    item_slot_list.Clear();
                    for (int y = collided_y; y < collided_y + item_controller.GetGridDimensions().y; y++)
                    {
                        for (int x = collided_x; x < collided_x + item_controller.GetGridDimensions().x; x++)
                        {
                            slot_list_[x, y].SetIsOccupied(true);
                            item_slot_list.Add(slot_list_[x, y]);
                        }
                    }
                    item_rectt.position = new Vector3((item_rectt.rect.width / 2 - slot_rectt.rect.width / 2) * canvas_.scaleFactor + slot_rectt.position.x,
                                                    (-item_rectt.rect.height / 2 + slot_rectt.rect.height / 2) * canvas_.scaleFactor + slot_rectt.position.y);
                }
            }
        }
    }

    private bool CanPlaceOnSlots(int collided_x, int collided_y, ItemController item_controller)
    {
        if (slot_list_[collided_x, collided_y].IsOccupied())
        {
            Debug.Log("slot_list_["+ collided_x + ", "+ collided_y + "].IsOccupied()");
            return false;
        }
        if (collided_y + item_controller.GetGridDimensions().y - 1 >= slot_list_.GetLength(0) ||
            collided_x + item_controller.GetGridDimensions().x - 1 >= slot_list_.GetLength(1))
        {
            Debug.Log("GetLength");
            return false;
        }
        for (int y = collided_y; y < collided_y + item_controller.GetGridDimensions().y; y++)
        {
            for (int x = collided_x; x < collided_x + item_controller.GetGridDimensions().x; x++)
            {
                if (slot_list_[x, y].IsOccupied())
                {
                    Debug.Log("slot_list_[" + x + ", " + y + "].IsOccupied()");
                    return false;
                }
            }
        }
        return true;
    }
}
