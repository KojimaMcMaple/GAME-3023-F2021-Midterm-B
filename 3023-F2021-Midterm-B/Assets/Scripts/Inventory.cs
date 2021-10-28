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

    public void OpenContainer(ContainerController container)
    {
        containerCanvas.SetActive(true);
        container.GetGrid().SetContainer(container);
        container.PopulateItemsOnGrid();
    }

    public void CloseContainer(ContainerController container)
    {
        container.ClearContainer();
        container.GetGrid().SetContainer(null);
        containerCanvas.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
       // if (collision.gameObject.tag == "Container")
        {
            if (collision.gameObject.GetComponentInParent<ContainerController>() != null)
            {
                OpenContainer(collision.gameObject.GetComponentInParent<ContainerController>());
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
      //  if (collision.gameObject.tag == "Container")
        {
            if (collision.gameObject.GetComponentInParent<ContainerController>() != null)
            {
                CloseContainer(collision.gameObject.GetComponentInParent<ContainerController>());
            }
        }
    }

    private IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
