using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : InventorySlot {
    public void AddEquipmentToSlot(EquipmentData newItem) {
        inventoryItem = newItem;
        itemImage.sprite = newItem.icon;
        itemImage.enabled = true;
        itemButton.interactable = true;
    }

    public void ClearEquipmentSlot() {
        inventoryItem = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemButton.interactable = false;
    }

    public void RemoveEquipmentFromSlot(int equipmentPosition) {
        Equipment.instance.UnequipItem(equipmentPosition);
        Equipment.instance.EquipDefaultItem(equipmentPosition);
    }
}