using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Example_Animator : MonoBehaviour
{
    private Animator _animator;

    private const string test01 = "Test01";
    private const string test02 = "Test02";

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.qKey.wasPressedThisFrame)
        {
            _animator.SetTrigger(test01);
        }

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            _animator.SetTrigger(test02);
        }
    }
}
