using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SphereWalk
{
    public class PlayerController : MonoBehaviour
    {
        [Header("�������")]
        public float speed = 5f;
        [Header("�����ת�ٶ�")]
        public float rotateSpeed = 150f;

        private float gravity = -100f;//����
        private Rigidbody rigidbody;
        private Vector3 moveAmount;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.freezeRotation = true;
        }

        void Update()
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            moveAmount = new Vector3(0, 0, v) * Time.deltaTime * speed;
            transform.Rotate(new Vector3(0, h * Time.deltaTime * rotateSpeed, 0));
        }

        private void FixedUpdate()
        {
            rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveAmount));
            Vector3 localUp = transform.up;
            Vector3 gravityDir = transform.position.normalized;

            transform.rotation = Quaternion.FromToRotation(transform.up, gravityDir) * transform.rotation;
            rigidbody.AddForce(gravity * gravityDir);
        }
    }
}

