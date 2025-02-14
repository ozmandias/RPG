using UnityEngine;

public class CharacterEffect : MonoBehaviour{
    public Effect damage;
    public Effect armor;
    public float maxHealth = 100f;
    public float currentHealth {get; private set;}
    public event System.Action<float> OnTakeDamage;

    public void Awake() {
        currentHealth = maxHealth;
    }

    void Update() {

    }

    public void TakeDamage(float incomingDamage) {
        incomingDamage = incomingDamage - armor.GetValue();
        incomingDamage = Mathf.Clamp(incomingDamage, 0, int.MaxValue);

        if(incomingDamage > 0) {
            currentHealth = currentHealth - incomingDamage;
        }
        
        if(OnTakeDamage != null) {
            OnTakeDamage(incomingDamage);
        }

        if(currentHealth <= 0) {
            Death();
        }
    }

    public virtual void Death() {
        Debug.Log("Death");
    }

    public float GetTotalHealth() {
        return currentHealth + armor.GetValue();
    }
}