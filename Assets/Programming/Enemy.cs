using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Interactable {
    GameObject player;
    Animator enemyAnimator;
    bool playerNearEnemy = false;
    bool enemyCanMove = true;
    float followDistance = 5f;
    float rotateSpeed = 20.0f;
    float attackSpeed = 1f;
    float playerAndEnemyDistance = 0f;
    float enemyAttackDistance = 2f;
    float enemyAttackDuration = 1f;
    float enemyAttackSpeed = 1f;
    int enemyAttackCount = 0;
    Collider enemyAttackCollider;
    NavMeshAgent enemyNavMeshAgent;
    CharacterEffect enemyEffect;
    CombatManager enemyCombat;
    GameObject target;
    Transform enemyMoveTowardsLocation = null;
    
    void Start() {
        player = GameObject.FindWithTag("Player");

        enemyAnimator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        enemyAttackCollider = GameObject.Find("SkeletonAttack").GetComponent<Collider>();
        enemyNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        enemyEffect = gameObject.GetComponent<CharacterEffect>();
        enemyCombat = gameObject.GetComponent<CombatManager>();
        
        enemyNavMeshAgent.updateRotation = false;
        enemyCombat.attacker = enemyEffect;
        radius = followDistance;
    }

    void Update() {
        Interact();
        EnemyMove();
        EnemyAttack();
    }

    void EnemyMove() {
        playerNearEnemy = CheckPlayer();
        if(playerNearEnemy && enemyCanMove) {
            enemyMoveTowardsLocation = null;

            Vector3 targetDirection = player.transform.position - transform.position;
            
            Vector3 lookDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0.0f);
            gameObject.transform.rotation = Quaternion.LookRotation(lookDirection);
            // Quaternion lookDirection = Quaternion.LookRotation(targetDirection, Vector3.up);
            // gameObject.transform.rotation = new Quaternion(0,lookRotation.y,0,1);
            
            enemyNavMeshAgent.destination = player.transform.position;

            if(playerAndEnemyDistance > enemyNavMeshAgent.stoppingDistance){
                PlayEnemyWalkAnimation();
            }else{
                StopEnemyWalkAnimation();
            }
        }else if(playerNearEnemy==false && enemyMoveTowardsLocation){
            float moveLocationAndEnemyDistance = Vector3.Distance(enemyMoveTowardsLocation.position, transform.position);

            Vector3 targetDirection = enemyMoveTowardsLocation.position - transform.position;

            Vector3 lookDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0.0f);
            gameObject.transform.rotation = Quaternion.LookRotation(lookDirection);

            enemyNavMeshAgent.destination = enemyMoveTowardsLocation.position;

            if(moveLocationAndEnemyDistance > enemyNavMeshAgent.stoppingDistance){
                PlayEnemyWalkAnimation();
            }else{
                enemyMoveTowardsLocation = null;
                StopEnemyWalkAnimation();
            }
        }else{
            StopEnemyWalkAnimation();
        }
    }

    void EnemyAttack() {
        if(playerAndEnemyDistance <= enemyAttackDistance && enemyCanMove) {
            if(enemyAttackCollider){
                enemyAttackCollider.enabled = true;
            }
            if(enemyAttackCount == 0) {
                PlayEnemyAttack_ScratchAnimation();
                enemyAttackCount = enemyAttackCount + 1;
            }else if(enemyAttackCount == 1){
                PlayEnemyAttack_UppercutAnimation();
                enemyAttackCount = 0;
            }
            enemyCanMove = false;
            StartCoroutine(ResetEnemyCanMove(enemyAttackDuration));
        }
        if(target){
            PlayerEffect targetEffect = target.GetComponent<PlayerEffect>();
            enemyCombat.CalculateCombat(targetEffect, enemyAttackSpeed);
        }
        target = null;
    }

    bool CheckPlayer() {
        playerAndEnemyDistance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if(playerAndEnemyDistance <= followDistance) {
            return true;
        }
        return false;
    }

    public void MoveTowards(Transform moveLocation) {
        enemyMoveTowardsLocation = moveLocation;
    }

    public override void InteractableAction() {
        Debug.Log("Enemy-Interact");
    }

    void PlayEnemyWalkAnimation() {
        enemyAnimator.SetBool("EnemyWalk", true);
    }

    void StopEnemyWalkAnimation() {
        enemyAnimator.SetBool("EnemyWalk", false);
    }

    void PlayEnemyAttack_ScratchAnimation() {
        enemyAnimator.SetTrigger("EnemyAttack_Scratch");
    }

    void PlayEnemyAttack_UppercutAnimation() {
        enemyAnimator.SetTrigger("EnemyAttack_Uppercut");
    }

    void OnCollisionEnter(Collision objectCollision) {
        ContactPoint[] objectContatPoints = objectCollision.contacts;
        ContactPoint objectContactPoint = objectContatPoints[0];
        Collider currentCollider = objectContactPoint.thisCollider;
        Collider otherCollider = objectContactPoint.otherCollider;
        if(currentCollider.gameObject.CompareTag("AttackCollider") && otherCollider.gameObject.CompareTag("Player")) {
            Debug.Log("Enemy attacks Player");
            target = otherCollider.gameObject;
        }
    }

    void OnTriggerEnter(Collider objectCollider) {
        
    }

    IEnumerator ResetEnemyCanMove(float duration) {
        yield return new WaitForSeconds(duration);
        enemyCanMove = true;
        if(enemyAttackCollider){
            enemyAttackCollider.enabled = false;
        }
    }
}