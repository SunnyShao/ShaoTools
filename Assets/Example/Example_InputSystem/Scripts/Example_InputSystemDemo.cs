using UnityEngine;
using UnityEngine.InputSystem.Interactions;

//测试在电脑和手机端 连续点击十次打开GM工具界面
public class Example_InputSystemDemo : MonoBehaviour
{
    public GameObject gmObj;

    private void Awake()
    {
        gmObj.SetActive(false);
        InputManager.Instance.Controller.UI.Press.started += OnPressStarted;
        InputManager.Instance.Controller.UI.Press.canceled += OnPressCanceled;
        InputManager.Instance.Controller.UI.Press.performed += OnPressPerformed;
    }

    private void OnPressPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.LogError("OnPressPerformed");
        if (obj.interaction is MultiTapInteraction)
        {
            var multi = (MultiTapInteraction)obj.interaction;
            Debug.LogError($"MultiTapInteraction  tapCount = {multi.tapCount}    tapTime = {multi.tapTime}");
            gmObj.SetActive(!gmObj.activeInHierarchy);
        }
    }

    private void OnPressCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.LogError("OnPressCanceled");
    }

    private void OnPressStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.LogError("OnPressStarted");
    }

    private void OnDestroy()
    {
        InputManager.Instance.Controller.UI.Press.started -= OnPressStarted;
        InputManager.Instance.Controller.UI.Press.canceled -= OnPressCanceled;
        InputManager.Instance.Controller.UI.Press.performed -= OnPressPerformed;
    }
}
