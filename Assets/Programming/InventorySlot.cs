using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour {
    public ItemData inventoryItem;
    public Image itemImage;
    public Button itemButton;
    public Image removeButtonImage;
    public Button removeButton;

    void Start() {
        if(inventoryItem == null && EventSystem.current.IsPointerOverGameObject()) {
            return;
        }
    }

    void Update() {
    }

    public void UseItem() {
        if(inventoryItem) {
            inventoryItem.Use();
        }
    }

    public void AddItemToSlot(ItemData newItem) {
        inventoryItem = newItem;
        itemImage.sprite = newItem.icon;
        itemImage.enabled = true;
        itemButton.interactable = true;
        removeButtonImage.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearItemSlot() {
        inventoryItem = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemButton.interactable = false;
        removeButtonImage.enabled = false;
        removeButton.interactable = false;
    }

    public void RemoveItemFromSlot() {
        Inventory.instance.RemoveItem(inventoryItem, "remove");
    }
}