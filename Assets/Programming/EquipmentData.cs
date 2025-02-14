using UnityEngine;

[CreateAssetMenu(fileName="Equipment", menuName="Inventory/Equipment")]
public class EquipmentData : ItemData {
    public float armorEffect = 0.0f;
    public float damageEffect = 0.0f;
    public EquipmentPosition equipmentSlot;
    public EquipmentBlendShapes[] equipmentCoverRegions;

    public override void Use() {
        base.Use();
        Equipment.instance.EquipItem(this);
        Inventory.instance.RemoveItem(this, "equip");
    }
}

public enum EquipmentPosition {Head, Chest, Legs, Weapon, Shield, Feet};

public enum EquipmentBlendShapes {Legs, Arms, Torso}; 