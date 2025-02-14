using UnityEngine;

public class AnimationManager : MonoBehaviour {
    Animator animationAnimator;
    Equipment animationEquipment;

    void Start() {
        // animationAnimator = GetComponent<Animator>();
        animationAnimator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();

        animationEquipment = Equipment.instance;
        animationEquipment.OnEquipmentChangeCallback = animationEquipment.OnEquipmentChangeCallback + UpdateAnimation;
    }

    void UpdateAnimation(EquipmentData originalItem, EquipmentData newItem) {
        Debug.Log("UpdateAnimation");
        if(newItem && newItem.equipmentSlot == EquipmentPosition.Weapon){
            animationAnimator.SetLayerWeight(2,1);
            animationEquipment.weaponInHand = true;
        }else if(newItem == null && originalItem && originalItem.equipmentSlot == EquipmentPosition.Weapon){
            animationAnimator.SetLayerWeight(2,0);
            animationEquipment.weaponInHand = false;
        }

        if(newItem && newItem.equipmentSlot == EquipmentPosition.Shield){
            animationAnimator.SetLayerWeight(1,1);
        }else if(newItem == null && originalItem && originalItem.equipmentSlot == EquipmentPosition.Shield){
            animationAnimator.SetLayerWeight(1,0);
        }
    }
}