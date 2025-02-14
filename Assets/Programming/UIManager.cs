using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    Inventory uiInventory;
    Equipment uiEquipment;
    InventorySlot[] inventorySlots;
    [SerializeField]EquipmentSlot[] equipmentSlots;
    [SerializeField]GameObject inventoryPanel;
    [SerializeField]GameObject equipmentPanel;
    [SerializeField]Transform inventorySlotPanelTransform;
    [SerializeField]Transform equipmentSlotPanelTransform;
    public bool inventoryDisplay = false;

    #region Singleton
        public static UIManager instance;
        public void Awake() {
            if(instance != null){
                return;
            }
            instance = this;
        }
    #endregion

    void Start() {
        uiInventory = Inventory.instance;
        uiInventory.OnInventoryChangeCallback = uiInventory.OnInventoryChangeCallback + UpdateInventoryUI;

        uiEquipment = Equipment.instance;
        uiEquipment.OnEquipmentChangeCallback = uiEquipment.OnEquipmentChangeCallback + UpdateEquipmentUI;

        inventorySlots = inventorySlotPanelTransform.GetComponentsInChildren<InventorySlot>();
        equipmentSlots = equipmentSlotPanelTransform.GetComponentsInChildren<EquipmentSlot>();

        // inventoryPanel.SetActive(false);
    }

    void Update() {
        toggleInventoryPanel();
    }

    void UpdateInventoryUI() {
        Debug.Log("UpdateInventoryUI");
        for(int i=0; i<inventorySlots.Length; i=i+1) {
            if(i < uiInventory.items.Count) {
                inventorySlots[i].AddItemToSlot(uiInventory.items[i]);
            }else{
                inventorySlots[i].ClearItemSlot();
            }
        }
    }

    void UpdateEquipmentUI(EquipmentData originalItem, EquipmentData newItem) {
        Debug.Log("UpdateEquipmentUI");
        for(int i=0; i<equipmentSlots.Length; i=i+1) {
            if(uiEquipment.equipment[i] && uiEquipment.equipment[i].icon != null && i < uiEquipment.equipment.Length) {
                Debug.Log("EquipmentData: " + uiEquipment.equipment[i]);
                equipmentSlots[i].AddEquipmentToSlot(uiEquipment.equipment[i]);
            }else if(uiEquipment.equipment[i] == null){
                Debug.Log("ClearEquipmentSlot");
                equipmentSlots[i].ClearEquipmentSlot();
            }
        }
    }

    public void toggleInventoryPanel() {
        if(Input.GetButtonDown("Inventory")) {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            inventoryDisplay = !inventoryDisplay;
            equipmentPanel.SetActive(!equipmentPanel.activeSelf);
            GameManager.instance.LockCursor(!GameManager.instance.getLockState());
        }
    }

    public EquipmentSlot[] GetEquipmentSlots() {
        return equipmentSlots;
    }
}