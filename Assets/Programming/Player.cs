using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {
	Camera playerCamera;
	Animator playerAnimator;
	Rigidbody playerRigidbody;
	float moveSpeed = 6f;
	float rotateSpeed = 4f;
	float jumpForce = 1f;
	float attackSpeed = 1f;
	float attackAnimationDuration = 0.5f;
	float groundCheckDistance = 0.4f;
	bool atGround = false;
	bool canMove = true;
	bool equipWeapon = false;
	bool inventoryCheck = false;
	int swordAttackCount = 0;
	Collider playerAttackCollider;
	Vector3 startPosition = new Vector3(11f, -0.8f, -15.4f);
	NavMeshAgent playerNavMeshAgent;
	CharacterEffect playerEffect;
	CombatManager playerCombat;
	Inventory playerInventory;
	Equipment playerEquipment;
	UIManager playerUI;
	List<GameObject> targets = new List<GameObject>();
	[SerializeField]LayerMask playerLayerMask;

	// Use this for initialization
	void Start () {
		playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		// playerAnimator = gameObject.GetComponent<Animator>();
		playerAnimator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
		playerRigidbody = gameObject.GetComponent<Rigidbody>();
		playerAttackCollider = GameObject.Find("PlayerPunch").GetComponent<Collider>();
		playerNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
		playerEffect = gameObject.GetComponent<CharacterEffect>();
		playerCombat = gameObject.GetComponent<CombatManager>();
		playerInventory = Inventory.instance;
		playerEquipment = Equipment.instance;
		playerUI = GameObject.Find("UICanvas").GetComponent<UIManager>();

		// playerRigidbody.isKinematic = true;
		playerRigidbody.useGravity = false;
		playerCombat.attacker = playerEffect;

		playerEquipment.OnEquipmentChangeCallback = playerEquipment.OnEquipmentChangeCallback + UpdateCollider;
	}
	
	// Update is called once per frame
	void Update () {
		Move();
		Attack();
		// shootRay();
	}

	void FixedUpdate () {
		checkGround();
	}

	void Move() {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 direction = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;

		if(
			(horizontal > 0 && horizontal <= 1) || (horizontal >= -1 && horizontal < 0) ||
			(vertical > 0 && vertical <= 1) || (vertical >= -1 && vertical < 0)
		) {
			float rotateAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
			Vector3 cameraRotation = new Vector3(0f, playerCamera.transform.rotation.eulerAngles.y, 0f);
			gameObject.transform.rotation = Quaternion.Euler(Vector3.up * rotateAngle + cameraRotation);
			// gameObject.transform.rotation = Quaternion.LookRotation(direction);
			// gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);

			direction = gameObject.transform.TransformDirection(Vector3.forward * moveSpeed * Time.deltaTime);

			PlayRunAnimation();
		}else if(horizontal == 0 && vertical == 0) {
			StopRunAnimation();
		}

		if(Input.GetKeyDown(KeyCode.Space) && atGround == true) {
			// Debug.Log("Jump");
			playerNavMeshAgent.updatePosition = false;
			playerNavMeshAgent.updateRotation = false;
			playerNavMeshAgent.isStopped = true;

			// playerRigidbody.isKinematic = false;
			playerRigidbody.useGravity = true;

			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + jumpForce, gameObject.transform.position.z);
			atGround = false;
		}

		if(canMove==true){
			gameObject.transform.position = gameObject.transform.position + direction;
		}
	}

	void Attack() {
		if(Input.GetKeyDown(KeyCode.Mouse0) && playerUI.inventoryDisplay == false) {
			Debug.Log("Attack");
			playerAttackCollider.enabled = true;
			PlayAttackAimation();
			canMove = false;
			StartCoroutine(ResetCanMove(attackAnimationDuration));
		}
		if(targets.Count > 0){
			foreach(GameObject target in targets){
				Debug.Log("Target: " + target);
				EnemyEffect targetEffect = target.GetComponent<EnemyEffect>();
				playerCombat.CalculateCombat(targetEffect, attackSpeed);
			}
			targets.Clear();
		}
	}

	void shootRay() {
		if(Input.GetMouseButtonDown(0)){
			Ray playerRay = playerCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit playerRaycastHit;

			if(Physics.Raycast(playerRay, out playerRaycastHit, 100, playerLayerMask)){
				Debug.Log("RaycastHit GameObject: " + playerRaycastHit.collider.gameObject);
			}
		}
	}

	void checkGround() {
		RaycastHit groundCheckHit;
		if(Physics.Raycast(gameObject.transform.position, Vector3.down, out groundCheckHit, groundCheckDistance)){
			Debug.DrawRay(gameObject.transform.position, Vector3.down * groundCheckDistance, Color.white);
			// Debug.Log("groundCheckHit.collider: " + groundCheckHit.collider);
			if(groundCheckHit.collider.gameObject.CompareTag("Ground")){
				playerNavMeshAgent.nextPosition = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
				playerNavMeshAgent.updatePosition = true;
				playerNavMeshAgent.updateRotation = true;
				playerNavMeshAgent.isStopped = false;

				// playerRigidbody.isKinematic = true;
				playerRigidbody.useGravity = false;

				atGround = true;
				// Debug.Log("atGround: " + atGround);
			}
		}
	}

	void UpdateCollider(EquipmentData originalItem, EquipmentData newItem) {
		Debug.Log("UpdateCollider");
		if(newItem && newItem.equipmentSlot == EquipmentPosition.Weapon && newItem.objectCollider){
			playerAttackCollider = GameObject.Find(playerEquipment.GetWeaponColliderName()).GetComponent<Collider>();
		}else if(newItem == null && originalItem && originalItem.equipmentSlot == EquipmentPosition.Weapon){
			playerAttackCollider = GameObject.Find("PlayerPunch").GetComponent<Collider>();
		}
	}

	void PlayRunAnimation(){
		playerAnimator.SetBool("PlayerRun", true);
	}

	void StopRunAnimation(){
		playerAnimator.SetBool("PlayerRun", false);
	}

	void PlayWalkAnimation(){
		playerAnimator.SetBool("PlayerWalk", true);
	}

	void StopWalkAnimation(){
		playerAnimator.SetBool("PlayerWalk", false);
	}

	void PlayAttackAimation(){
		equipWeapon = playerEquipment.weaponInHand;
		if(equipWeapon==true){
			// int attackRandom = Random.Range(1,3);
			if(swordAttackCount == 0 /*attackRandom==1*/){
				PlaySwordAttack_CutAnimation();
				swordAttackCount = swordAttackCount + 1;
			}else if(swordAttackCount == 1 /*attackRandom==2*/){
				PlaySwordAttack_StabAnimation();
				swordAttackCount = 0;
			}
		}else{
			PlayPunchAnimation();
		}
	}

	void PlayPunchAnimation(){
		playerAnimator.SetTrigger("Punch");
	}

	void PlaySwordAttack_CutAnimation(){
		playerAnimator.SetTrigger("SwordAttack_Cut");
	}

	void PlaySwordAttack_StabAnimation(){
		playerAnimator.SetTrigger("SwordAttack_Stab");
	}

	void PlayInCombatAnimation() {
		playerAnimator.SetBool("CombatDefault", true);
	}

	void StopInCombatAnimation() {
		playerAnimator.SetBool("CombatDefault", false);
	}

	void OnCollisionEnter(Collision objectCollision){
		ContactPoint[] objectContacts = objectCollision.contacts;
		ContactPoint objectContact = objectContacts[0];
		Collider currentCollider = objectContact.thisCollider;
		Collider otherCollider = objectContact.otherCollider;
		if(currentCollider.gameObject.CompareTag("AttackCollider") && otherCollider.gameObject.CompareTag("Enemy")){
			Debug.Log("Player attacks Enemy");
			targets.Add(otherCollider.gameObject);
		}
	}

	void OnTriggerEnter(Collider objectCollider) {
		if(objectCollider.gameObject.name == "FallPrevention"){
			gameObject.transform.position = startPosition;
		}
	}

	IEnumerator ResetCanMove(float duration) {
		yield return new WaitForSeconds(duration);
		playerAttackCollider.enabled = false;
		canMove = true;
	}
}