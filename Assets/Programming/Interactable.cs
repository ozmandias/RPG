using UnityEngine;

public class Interactable : MonoBehaviour {
    SphereCollider interactableCollider;
    public float radius = 3f;
    bool playerNearInteractable = false;

    public void Awake() {
        interactableCollider = gameObject.AddComponent<SphereCollider>() as SphereCollider;
        interactableCollider.radius = radius;
        interactableCollider.isTrigger = true;
    }

    public void Start() {
        
    }

    public void Update() {
        Interact();
    }

    public void Interact() {
        if(playerNearInteractable == true) {
            if(Input.GetKeyDown(KeyCode.E)){
                Debug.Log("Interact");
                InteractableAction();
            }
        }
    }

    public virtual void InteractableAction() {}

    public void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }

    public void OnTriggerEnter(Collider objectCollider) {
        if(objectCollider.gameObject.CompareTag("Player")){
            // Debug.Log("Player Trigger Enter");
            playerNearInteractable = true;
        }
    }

    public void OnTriggerExit(Collider objectCollider) {
        if(objectCollider.gameObject.CompareTag("Player")){
            // Debug.Log("Player Trigger Exit");
            playerNearInteractable = false;
        }
    }
}