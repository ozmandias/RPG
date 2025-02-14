using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGCamera : MonoBehaviour {
    GameObject rpgPlayer;
    Vector3 cameraAndPlayerDistance;
    float distance = 4.4f;
    float cameraUp = 0.4f;
    float cameraDownAngle = 10f;
    float cameraRotateSpeed = 4f;
    float minMouseVertical = -20f;
    float maxMouseVertical = 20f;

    float mouseHorizontal = 0f;
    float mouseVertical = 0f;

    public void Start() {
        // rpgPlayer = GameObject.FindWithTag("Player");
        rpgPlayer = GameObject.Find("PlayerCameraFocus");
        cameraAndPlayerDistance = gameObject.transform.position - rpgPlayer.gameObject.transform.position;
    }

    public void Update() {
        CameraMove();
    }

    public void CameraMove() {
        mouseHorizontal += Input.GetAxis("Mouse X") * cameraRotateSpeed;
        mouseVertical -= Input.GetAxis("Mouse Y") * cameraRotateSpeed;
        mouseVertical = Mathf.Clamp(mouseVertical, minMouseVertical, maxMouseVertical);

        Vector3 mouseDirection = new Vector3(mouseVertical + cameraDownAngle, mouseHorizontal, 0f);

        gameObject.transform.rotation = Quaternion.Euler(mouseDirection);
        // float rotateAngle = Quaternion.Angle(gameObject.transform.rotation, Quaternion.Euler(mouseDirection.x, mouseDirection.y, mouseDirection.z));
        // gameObject.transform.rotation = Quaternion.AngleAxis(rotateAngle, gameObject.transform.position);
        // gameObject.transform.Rotate(mouseDirection);

        Vector3 followPosition = new Vector3(rpgPlayer.transform.position.x, rpgPlayer.transform.position.y + cameraUp, rpgPlayer.transform.position.z);
        
        gameObject.transform.position = followPosition - gameObject.transform.forward * distance;
        // gameObject.transform.position = rpgPlayer.transform.position - gameObject.transform.forward * distance;
    }
}