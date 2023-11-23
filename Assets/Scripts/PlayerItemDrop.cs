using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
   [Header("Player's drop ")] 
   [SerializeField] private float chanceToLoseItems;

   
   

   public override void GenerateDrop()
   {
       Inventory inventory = Inventory.instance;
       
       List<InventoryItem> currentEquipment = inventory.GetEquipmentList();
       List<InventoryItem> itemsToUnEquip = new List<InventoryItem>();
       
       foreach (InventoryItem item in currentEquipment)
       {
           if (Random.Range(0, 100) <= chanceToLoseItems)
           {
               
               DropItem(item.data);
               itemsToUnEquip.Add(item);
           }

           for (int i = 0; i < itemsToUnEquip.Count; i++)
           {
               inventory.UnEquipItem(itemsToUnEquip[i].data as ItemData_Equipment);
           }

          

       }
   }
}
