using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    #region Singleton
        public static Inventory instance;
        
        public void Awake() {
            if(instance != null) {
                return;
            }
            instance = this;
        }
    #endregion

    public List<ItemData> items = new List<ItemData>();
    int freeSpace = 20;
    [SerializeField]int spaceLimit = 20;

    public delegate void OnInventoryChange();
    public OnInventoryChange OnInventoryChangeCallback;
    public ItemConverter converter;

    public void Start() {
        converter = ItemConverter.instance;

        gameObject.SetActive(false);
    }

    public void AddItem(ItemData item) {
        Debug.Log("AddItem: " + item.name);
        if(items.Count < spaceLimit && item.icon != null) {
            items.Add(item);
            freeSpace = freeSpace - 1;
        }
        
        if(OnInventoryChangeCallback != null){
            OnInventoryChangeCallback.Invoke();
        }
    }

    public void RemoveItem(ItemData item, string notes) {
        Debug.Log("RemoveItem: " + item.name);
        if(items.Count > 0) {
            items.Remove(item);
            freeSpace = freeSpace + 1;
        }

        if(notes != "equip") {
            converter.ItemToGameObject(item);
        }

        if(OnInventoryChangeCallback != null){
            OnInventoryChangeCallback.Invoke();
        }
    }
}