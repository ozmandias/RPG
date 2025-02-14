using UnityEngine;
using UnityEngine.UI;

public class HealthCanvas : MonoBehaviour {
    GameObject camera;
    [SerializeField]Image greenHealth;
    [SerializeField]CharacterEffect healthCharacterEffect;

    void Start(){
        camera = GameObject.Find("Main Camera");
        greenHealth.fillAmount = 1.0f;

        healthCharacterEffect.OnTakeDamage += UpdateHealthCanvas;
    }

    void Update(){
        
    }

    void LateUpdate(){
        FaceCamera();
    }

    void FaceCamera(){
        gameObject.transform.forward = camera.transform.forward;
    }

    void UpdateHealthCanvas(float healthReduceAmount){
        Debug.Log("OnAttack - UpdateHealthCanvas");
        // float totalHealth = healthCharacterEffect.GetTotalHealth();
        // greenHealth.fillAmount = greenHealth.fillAmount - healthReduceAmount / 100f /*greenHealth.fillAmount - 100f / healthAmount*/ /* * Time.deltaTime */;
        float healthPercent = healthCharacterEffect.currentHealth / healthCharacterEffect.maxHealth;
        greenHealth.fillAmount = healthPercent;
    }
}