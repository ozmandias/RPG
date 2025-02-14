using UnityEngine;

public class Item : Interactable {
    public ItemData itemData;
    public override void InteractableAction() {
        Debug.Log("Item - InteractableAction");
        Destroy(gameObject);
        Debug.Log("Item Collected!");

        Debug.Log("itemData: " + itemData);
        
        Inventory.instance.AddItem(itemData);
    }
}