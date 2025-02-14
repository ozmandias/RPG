using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : CharacterEffect {
    Equipment effectEquipment;
    void Start() {
        damage.SetValue(10.0f);

        effectEquipment = Equipment.instance;
        effectEquipment.OnEquipmentChangeCallback = effectEquipment.OnEquipmentChangeCallback + UpdateEffect;
    }

    void UpdateEffect(EquipmentData originalItem, EquipmentData newItem) {
        Debug.Log("UpdateEffect");

        if(originalItem!=null && originalItem.icon!=null){
            damage.RemoveValueChanges(originalItem.damageEffect);
            armor.RemoveValueChanges(originalItem.armorEffect);
        }

        if(newItem!=null && newItem.icon!=null){
            Debug.Log(newItem);
            damage.AddValueChanges(newItem.damageEffect);
            armor.AddValueChanges(newItem.armorEffect);
        }
    }

    public override void Death() {
        base.Death();
        Debug.Log("Player Death");
    }
}