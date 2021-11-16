using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float moveSpeed;
    private float rotSpeed;

    private Vector3 origPos;
    private Vector3 origRot;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 10.0f;
        rotSpeed = 50f;

        origPos = this.transform.position;
        origRot = this.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = new Vector3(0f, 0f, 0f);
        //left
        if (Input.GetKey("a")) {
            moveDir = moveDir + (-1) * this.transform.right;
        }
        //right
        if (Input.GetKey("d")) {
            moveDir = moveDir + this.transform.right;
        }
        //up
        if (Input.GetKey("w")) {
            moveDir = moveDir + this.transform.up;
           
        }
        //down
        if (Input.GetKey("s")) {
            
            moveDir = moveDir + (-1) * this.transform.up;
        }
        if (Input.GetKey("q")) {
            moveDir = moveDir + this.transform.forward;
        }
        if (Input.GetKey("e")) {
            moveDir = moveDir + (-1) * this.transform.forward;
        }

        moveDir.Normalize();
        this.transform.position += moveDir * moveSpeed * Time.deltaTime;

        Vector3 currEulerAngle = this.transform.eulerAngles;
        Vector3 rotDir = new Vector3(0f, 0f, 0f);
        if (Input.GetKey("up")) {
            rotDir = rotDir + new Vector3(-1f, 0f, 0f);
        }
        if (Input.GetKey("down")) {
            rotDir = rotDir + new Vector3(1f, 0f, 0f);
        }
        if (Input.GetKey("left")) {
            rotDir = rotDir + new Vector3(0f, -1f, 0f);
        }
        if (Input.GetKey("right")) {
            rotDir = rotDir + new Vector3(0f, 1f, 0f);
        }
        rotDir.Normalize();
        currEulerAngle += rotDir * Time.deltaTime * rotSpeed;

        if (currEulerAngle.x > 180) currEulerAngle.x -= 360f;
        currEulerAngle.x = Mathf.Clamp(currEulerAngle.x, -90f, 90f);
        this.transform.rotation = Quaternion.Euler(currEulerAngle);

        if (Input.GetKey("r")) {
            this.transform.position = origPos;
            this.transform.rotation = Quaternion.Euler(origRot);
        }
    }
}
