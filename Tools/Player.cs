using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 150f;
    float gravity = -100f;

    Rigidbody rb;
    Vector3 moveAmount;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        moveAmount = new Vector3(0, 0, v) * moveSpeed * Time.deltaTime;

        float h = Input.GetAxis("Horizontal");
        transform.Rotate(0, h * rotateSpeed * Time.deltaTime, 0);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount));

        Vector3 localUp = transform.up;
        Vector3 gravityDir = (transform.position).normalized;

        transform.rotation = Quaternion.FromToRotation(localUp, gravityDir) * transform.rotation;
        rb.AddForce(gravity * gravityDir);
    }
}
