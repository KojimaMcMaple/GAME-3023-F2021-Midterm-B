using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player inventory
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField]
    GameObject containerCanvas;

    [SerializeField]
    ItemTable itemTable;

    private void Awake()
    {
        itemTable.AssignItemIDs();
    }

    public void OpenContainer()
    {
        containerCanvas.SetActive(true);
    }

    public void CloseContainer()
    {
        containerCanvas.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
       // if (collision.gameObject.tag == "Container")
        {
            OpenContainer();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
      //  if (collision.gameObject.tag == "Container")
        {
            CloseContainer();
        }
    }
}
