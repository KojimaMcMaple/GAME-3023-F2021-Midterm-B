using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [SerializeField] private Item item_stats_;
    [SerializeField] private Vector2Int grid_dimensions_ = new Vector2Int(1, 3);
    [SerializeField] private Vector2 cell_size_ = new Vector2(96, 96);
    [SerializeField] private GameObject base_item_icon_prefab_;
    [SerializeField] private List<Vector2Int> slot_list_ = new List<Vector2Int>();
    private ContainerController container_ = null; //parent

    private void Start() //depends on Inventory -> itemTable.AssignItemIDs();
    {
        //SetupItem();
        Debug.Log(">>> " + item_stats_.Id);
    }

    public void SetupItem()
    {
        RectTransform rectt = GetComponent<RectTransform>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        rectt.sizeDelta = new Vector2(cell_size_.x * grid_dimensions_.x, cell_size_.y * grid_dimensions_.y);
        collider.size = new Vector2(cell_size_.x * grid_dimensions_.x, cell_size_.y * grid_dimensions_.y);
        GameObject icon_obj = Instantiate(base_item_icon_prefab_, this.transform);
        //GameObject icon_obj = transform.Find("ItemIcon").gameObject;
        icon_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(cell_size_.x * grid_dimensions_.x, cell_size_.y * grid_dimensions_.y);
        icon_obj.GetComponent<Image>().sprite = item_stats_.icon;
    }

    public Vector2Int GetGridDimensions()
    {
        return grid_dimensions_;
    }

    public List<Vector2Int> GetSlotList()
    {
        return slot_list_;
    }

    

    public ContainerController GetContainer()
    {
        return container_;
    }

    public void SetContainer(ContainerController value)
    {
        container_ = value;
    }
}