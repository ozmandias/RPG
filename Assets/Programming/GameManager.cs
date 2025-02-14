using UnityEngine;

public class GameManager : MonoBehaviour {
    Inventory gameInventory;
    Equipment gameEquipment;
    bool lockCursor = false;

    #region Singleton
        public static GameManager instance;
        public void Awake() {
            if(instance != null) {
                return;
            }
            instance = this;
        }
    #endregion
    
    void Start() {
        lockCursor = true;
    }

    void OnGUI() {
        ChangeCursorSettings();
    }

    void ChangeCursorSettings() {
        if(lockCursor == true) {
            Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void LockCursor(bool cursorState) {
        lockCursor = cursorState;
    }

    public bool getLockState() {
        return lockCursor;
    }
}