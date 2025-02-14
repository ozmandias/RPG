using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterEffect))]
public class CombatManager : MonoBehaviour {
    public CharacterEffect attacker;
    public float attackCoolDown = 0f;
    public event System.Action OnAttack;

    #region Singleton
        public static CombatManager instance;

        public void Awake() {
            if(instance != null){
                return;
            }
            instance = this;
        }
    #endregion

    void Update() {
        reduceCoolDown();
    }

    public void CalculateCombat(CharacterEffect target, float attackSpeed) {
        Debug.Log("CalculateCombat");
        if(attackCoolDown <= 0f) {
            StartCoroutine(DoDamage(target, 0.5f));
            attackCoolDown = 1f / attackSpeed;

            if(OnAttack != null) {
                OnAttack();
            }
        }
    }

    void reduceCoolDown() {
        attackCoolDown = attackCoolDown - Time.deltaTime;
    }

    IEnumerator DoDamage(CharacterEffect target, float delay) {
        Debug.Log("DoDamage");
        yield return new WaitForSeconds(delay);
        Debug.Log("attacker.damage: " + attacker.damage.GetValue());
        target.TakeDamage(attacker.damage.GetValue());
    }
}