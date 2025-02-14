using UnityEngine;

[CreateAssetMenu(fileName="Item", menuName="Inventory/Item")]
public class ItemData : ScriptableObject {
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;
    public SkinnedMeshRenderer itemModel;
    public float lootRadius = 0f;
    public GameObject objectCollider;

    public virtual void Use() {
        Debug.Log("Use");
    }
}