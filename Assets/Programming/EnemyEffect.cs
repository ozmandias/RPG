using UnityEngine;

public class EnemyEffect : CharacterEffect {
    void Start(){
        damage.SetValue(5.0f);
    }

    public override void Death(){
        base.Death();
        Debug.Log("Enemy Death");
        Destroy(gameObject);
    }
}