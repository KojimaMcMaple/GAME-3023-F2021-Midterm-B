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
        for (int i = 0; i < grid_dimensions_.y; i++)
        {
            for (int j = 0; j < grid_dimensions_.x; j++)
            {
                if (count< transform.childCount)
                {
                    slot_list_[i, j] = transform.GetChild(count).GetComponent<ItemSlot>();
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
            int x = 0, y = 0;

            for (int i = 0; i < grid_dimensions_.y; i++)
            {
                for (int j = 0; j < grid_dimensions_.x; j++)
                {
                    if (slot_list_[i, j].IsColliding() && !slot_list_[i, j].IsOccupied())
                    {
                        has_collision = true;
                        y = i;
                        x = j;
                        slot_rectt = slot_list_[i,j].GetRectt();
                        break;
                    }
                }
                if (has_collision){break;}
            }

            if (has_collision)
            {
                RectTransform item_rectt = eventData.pointerDrag.GetComponent<RectTransform>();
                Vector2Int dimensions = eventData.pointerDrag.GetComponent<ItemController>().GetGridDimensions();
                item_rectt.position = new Vector3((item_rectt.rect.width / 2 - slot_rectt.rect.width / 2) * canvas_.scaleFactor + slot_rectt.position.x,
                                                    (-item_rectt.rect.height / 2 + slot_rectt.rect.height / 2) * canvas_.scaleFactor + slot_rectt.position.y);
                for (int i = y; i < y+dimensions.y; i++)
                {
                    for (int j = x; j < x+dimensions.x; j++)
                    {
                        slot_list_[i, j].SetIsOccupied(true);
                    }
                }
            }
        }
    }
}
