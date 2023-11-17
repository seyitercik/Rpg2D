
using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{

   
    [SerializeField] private ItemData itemData;
    


    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "item object - " + itemData.name;
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player.Player>() != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);  
        }
           
    }
}
