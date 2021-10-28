using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerController : MonoBehaviour
{
    [SerializeField] private List<ItemController> item_list_ = new List<ItemController>(); //children
    [SerializeField] private ItemSlotGridDimensioner ui_grid_;

    public void AddItemToContainer(ItemController item)
    {
        item_list_.Add(item);
    }

    public void RemoveItemFromContainer(ItemController item)
    {
        item_list_.Remove(item);
    }

    public List<ItemController> GetItemList()
    {
        return item_list_;
    }

    public ItemSlotGridDimensioner GetGrid()
    {
        return ui_grid_;
    }

    public void SetGrid(ItemSlotGridDimensioner grid)
    {
        ui_grid_ = grid;
    }

    public void PopulateItemsOnGrid()
    {
        foreach (var item in item_list_)
        {
            item.gameObject.SetActive(true);
            if (item.GetSlotList().Count > 0)
            {
                ui_grid_.SetItemAtSlotCoord(item, item.GetSlotList()[0].x, item.GetSlotList()[0].y);
            }
        }
    }

    public void ClearContainer()
    {
        foreach (var item in item_list_)
        {
            item.gameObject.SetActive(false);
        }
        ui_grid_.ClearSlotList();
    }
}
