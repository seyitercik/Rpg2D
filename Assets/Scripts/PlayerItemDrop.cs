using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
   [Header("Player's drop ")] 
   [SerializeField] private float chanceToLoseItems;
   [SerializeField] private float chanceToLoseMaterials;

   
   

   public override void GenerateDrop()
   {
       Inventory inventory = Inventory.instance;
       List<InventoryItem> itemsToUnEquip = new List<InventoryItem>();
       List<InventoryItem> materialsToLoose = new List<InventoryItem>();
       
       foreach (InventoryItem item in  inventory.GetEquipmentList())
       {
           if (Random.Range(0, 100) <= chanceToLoseItems)
           {
               
               DropItem(item.data);
               itemsToUnEquip.Add(item);
           }
       }
       
       for (int i = 0; i < itemsToUnEquip.Count; i++)
       {
           inventory.UnEquipItem(itemsToUnEquip[i].data as ItemData_Equipment);
       }

       foreach (InventoryItem item in inventory.GetStashList())
       {
           if(Random.Range(0,100) <= chanceToLoseMaterials)
               DropItem(item.data);
           materialsToLoose.Add(item);
           
       }

       for (int i = 0; i < materialsToLoose.Count; i++)
       {
           inventory.RemoveItem(materialsToLoose[i].data);
       }
       

   }
}
