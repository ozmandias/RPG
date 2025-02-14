using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConverter : MonoBehaviour {
	#region Singleton
		public static ItemConverter instance;

		public void Awake() {
			if(instance != null) {
				return;
			}
			instance = this;
		}
	#endregion

	Transform itemSpawnPoint;

	// Use this for initialization
	void Start () {
		itemSpawnPoint = GameObject.Find("PlayerArt").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[ContextMenu("Convert to Mesh")]
	public void ConvertToMesh() {
		SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

		meshFilter.sharedMesh = skinnedMeshRenderer.sharedMesh;
		meshRenderer.sharedMaterials = skinnedMeshRenderer.sharedMaterials;

		DestroyImmediate(skinnedMeshRenderer);
		DestroyImmediate(this);
	}

	public void ItemToGameObject(ItemData item) {
		// Debug.Log("ItemToGameObject");
		SkinnedMeshRenderer itemMeshRenderer = item.itemModel;

		GameObject itemObject = new GameObject(item.name);
		MeshRenderer meshRenderer = itemObject.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = itemObject.AddComponent<MeshFilter>();
		Item itemObjectLoot = itemObject.AddComponent<Item>();

		meshFilter.sharedMesh = itemMeshRenderer.sharedMesh;
		meshRenderer.sharedMaterials = itemMeshRenderer.sharedMaterials;

		itemObjectLoot.radius = item.lootRadius;
		itemObjectLoot.itemData = item;

		itemObject.transform.position = itemSpawnPoint.transform.position;
		itemObject.transform.rotation = Quaternion.Euler(0, 0, 180f);
	}
}