using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap_RotateSphereMove : MonoBehaviour
{
    public float angle = 1f;
    private bool isRotate = false;
    private Vector2 lastScreenPos;

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.Controller.UI.Press.started += OnPressStarted;
        InputManager.Instance.Controller.UI.Press.canceled += OnPressCanceled;
    }

    private void OnPressCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.LogError("OnPressCanceled");
        isRotate = false;
    }

    private void OnPressStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.LogError("OnPressStarted");
        isRotate = true;
    }

    void Update()
    {
        if (isRotate)
        {
            Vector3 lastPoint = Vector3.zero;
            Vector3 curPoint = Vector3.zero;
            Vector2 curScreenPos = InputManager.Instance.Controller.UI.TouchScreen.ReadValue<Vector2>();
            //Vector2 viewportPos = Camera.main.ScreenToViewportPoint(screenPos);
            //Debug.LogError(viewportPos);
            if (lastScreenPos != Vector2.zero && curScreenPos !=Vector2.zero && lastScreenPos != curScreenPos)
            {
                Debug.LogError(lastScreenPos + ":::" + curScreenPos);
                float dirSpeed = 1;
                if(lastScreenPos.x > curScreenPos.x)
                {
                    dirSpeed = -1;
                }
                else if(lastScreenPos.x == curScreenPos.x)
                {
                    dirSpeed = 0;
                    Debug.LogError("===================================");
                }
                else
                {
                    dirSpeed = 1;
                }

                transform.RotateAround(Vector3.zero, Vector3.up, dirSpeed * angle * Time.deltaTime);
                lastScreenPos = curScreenPos;
                //Ray lastRay = Camera.main.ScreenPointToRay(lastScreenPos);
                //if (Physics.Raycast(lastRay, out var lasthit, 1000, 1 << LayerMask.NameToLayer("Ground")))
                //{
                //    lastPoint = lasthit.point;
                //}

                //Ray curRay = Camera.main.ScreenPointToRay(curScreenPos);
                //if (Physics.Raycast(curRay, out var curhit, 1000, 1 << LayerMask.NameToLayer("Ground")))
                //{
                //    curPoint = curhit.point;
                //}

                //float distance = Vector3.Distance(lastPoint, curPoint);
                //if (lastPoint != Vector3.zero && curPoint != Vector3.zero && lastPoint != curPoint && distance > 0.1f)
                //{
                //    Vector3 dir = curPoint - lastPoint;
                //    float dirSpeed = 1;
                //    if (dir.x > 0)
                //    {
                //        dirSpeed = 1;
                //    }
                //    else if (dir.x == 0)
                //    {
                //        Debug.LogError("=============================================================================");
                //    }
                //    else
                //    {
                //        dirSpeed = -1;
                //    }
                //    transform.RotateAround(Vector3.zero, Vector3.up, dirSpeed * angle * Time.deltaTime);

                //    Debug.LogError(dir + "::" + lastPoint + ":::" + curPoint + "::" + dirSpeed + "::" + distance);
                //    lastScreenPos = curScreenPos;
                //}

            }
            else
            {
                lastScreenPos = curScreenPos;
            }

        }
    }
}
