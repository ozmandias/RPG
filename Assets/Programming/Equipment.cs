using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {
    public EquipmentData[] defaultItems;
    public EquipmentData[] equipment;
    int equipmentPositionCount = 0;
    public SkinnedMeshRenderer[] equipmentArts;
    public SkinnedMeshRenderer playerMeshRenderer;

    public delegate void OnEquipmentChange(EquipmentData originalItem, EquipmentData newItem);
    public OnEquipmentChange OnEquipmentChangeCallback;
    public bool weaponInHand = false;
    GameObject weaponCollider;
    public GameObject weaponHandRig;

    #region Singleton
        public static Equipment instance;

        public void Awake() {
            if(instance != null) {
                return;
            }
            instance = this;
        }
    #endregion

    void Start(){
        equipmentPositionCount = System.Enum.GetNames(typeof(EquipmentPosition)).Length;
        equipment = new EquipmentData[equipmentPositionCount];
        equipmentArts = new SkinnedMeshRenderer[equipmentPositionCount];

        EquipDefaultItems();

        gameObject.SetActive(false);
    }

    public void EquipItem(EquipmentData newItem) {
        int slotIndex = (int) newItem.equipmentSlot;

        UnequipItem(slotIndex);
        
        EquipmentData originalItem = null;
        if(equipment[slotIndex] == null) {
            equipment[slotIndex] = newItem;
        }else{
            originalItem = equipment[slotIndex];
            SwitchItem(originalItem, newItem, slotIndex);
        }

        SkinnedMeshRenderer newEquipmentModel = Instantiate<SkinnedMeshRenderer>(newItem.itemModel);
        newEquipmentModel.transform.parent = playerMeshRenderer.transform;
        newEquipmentModel.bones = playerMeshRenderer.bones;
        newEquipmentModel.rootBone = playerMeshRenderer.rootBone;
        equipmentArts[slotIndex] = newEquipmentModel;
        SetBlendShapes(newItem, 100);

        if(newItem.equipmentSlot == EquipmentPosition.Weapon && newItem.objectCollider){
            weaponCollider = Instantiate(newItem.objectCollider, weaponHandRig.transform);
        }
        
        if(OnEquipmentChangeCallback != null){
            OnEquipmentChangeCallback.Invoke(originalItem, newItem);
        }
    }

    public void SwitchItem(EquipmentData originalItem, EquipmentData newItem, int slotPosition) {
        Debug.Log("SwitchItem");
        equipment[slotPosition] = newItem;
        Inventory.instance.AddItem(originalItem);
    }

    public void UnequipItem(int slotPosition) {
        EquipmentData originalItem = equipment[slotPosition];
        equipment[slotPosition] = null;
        if(originalItem){
            Inventory.instance.AddItem(originalItem);
        }

        if(equipmentArts[slotPosition] != null){
            Destroy(equipmentArts[slotPosition].gameObject);
        }
        SetBlendShapes(originalItem, 0);

        if(slotPosition == 3 && weaponCollider) {
            Destroy(weaponCollider);
        }

        if(OnEquipmentChangeCallback != null){
            OnEquipmentChangeCallback.Invoke(originalItem, null);
        }
    }

    public void UnequipAll() {
        for(int i=0; i<equipmentPositionCount; i=i+1){
            UnequipItem(i);
        }
        EquipDefaultItems();
    }

    public void SetBlendShapes(EquipmentData equipmentItem, int weight) {
        if(equipmentItem){
            foreach (EquipmentBlendShapes blendShape in equipmentItem.equipmentCoverRegions){
                playerMeshRenderer.SetBlendShapeWeight((int)blendShape, weight);
            }
        }
    }

    public void EquipDefaultItems() {
        if(defaultItems.Length > 0) {
            foreach (EquipmentData deafultItem in defaultItems)
            {
                if(deafultItem){
                    EquipItem(deafultItem);
                }
            }
        }
    }

    public void EquipDefaultItem(int equipmentPosition) {
        EquipmentData defaultItem = defaultItems[equipmentPosition];
        if(defaultItem){
            EquipItem(defaultItem);
        }
    }

    public string GetWeaponColliderName() {
        string weaponColliderName = "";
        if(weaponCollider){
            weaponColliderName = weaponCollider.name;
        }
        return weaponColliderName;
    }
}