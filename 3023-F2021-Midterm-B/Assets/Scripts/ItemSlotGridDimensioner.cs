using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Instantiates prefabs to fill a grid
[RequireComponent(typeof(GridLayout))]
public class ItemSlotGridDimensioner : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Vector2Int grid_dimensions_ = new Vector2Int(5, 5);

    private ItemSlot[,] slot_list_;
    private Canvas canvas_ = null;
    [SerializeField] private ContainerController container_ = null;

    void Awake()
    {
        canvas_ = GetComponentInParent<Canvas>(); //recurses upwards until it finds a GameObject with a matching component. Only components on active GameObjects are matched.

        SetupGrid();
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
                    List<Vector2Int> item_slot_list = item_controller.GetSlotList();
                    // CLEAR PREV SLOTS
                    if (item_controller.GetContainer() != null)
                    {
                        if (item_slot_list.Count != 0)
                        {
                            foreach (var coord in item_slot_list)
                            {
                                item_controller.GetContainer().GetGrid().GetItemSlotAtCoord(coord.x, coord.y).SetIsOccupied(false);
                            }
                        }
                        item_slot_list.Clear();
                        item_controller.GetContainer().RemoveItemFromContainer(item_controller);
                    }
                    
                    // SET CURR SLOTS
                    for (int y = collided_y; y < collided_y + item_controller.GetGridDimensions().y; y++)
                    {
                        for (int x = collided_x; x < collided_x + item_controller.GetGridDimensions().x; x++)
                        {
                            slot_list_[x, y].SetIsOccupied(true);
                            item_slot_list.Add(new Vector2Int(x, y));
                        }
                    }
                    item_controller.SetContainer(container_);
                    container_.AddItemToContainer(item_controller);
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
            Debug.Log("GetLength()");
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

    public void SetupGrid()
    {
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
                if (count < transform.childCount)
                {
                    slot_list_[x, y] = transform.GetChild(count).GetComponent<ItemSlot>();
                    count++;
                }
            }
        }
    }

    public ItemSlot GetItemSlotAtCoord(int x, int y)
    {
        return slot_list_[x, y];
    }

    public void ClearSlotList()
    {
        for (int y = 0; y < grid_dimensions_.y; y++)
        {
            for (int x = 0; x < grid_dimensions_.x; x++)
            {
                slot_list_[x, y].SetIsOccupied(false);
            }
        }
    }

    public void SetItemAtSlotCoord(ItemController item_controller, int x_idx, int y_idx)
    {
        RectTransform slot_rectt = slot_list_[x_idx, y_idx].GetRectt();
        RectTransform item_rectt = item_controller.transform.GetComponent<RectTransform>();
        item_rectt.position = new Vector3((item_rectt.rect.width / 2 - slot_rectt.rect.width / 2) * canvas_.scaleFactor + slot_rectt.position.x,
                                        (-item_rectt.rect.height / 2 + slot_rectt.rect.height / 2) * canvas_.scaleFactor + slot_rectt.position.y);
        for (int y = y_idx; y < y_idx + item_controller.GetGridDimensions().y; y++)
        {
            for (int x = x_idx; x < x_idx + item_controller.GetGridDimensions().x; x++)
            {
                slot_list_[x, y].SetIsOccupied(true);
            }
        }
    }

    public void SetContainer(ContainerController value)
    {
        container_ = value;
    }
}
