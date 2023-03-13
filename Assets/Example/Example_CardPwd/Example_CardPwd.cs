using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Example_CardPwd : MonoBehaviour
{
    private string headUrl = "";
    private string head = "http://{0}:{1}";
    private string ip = "120.53.248.67";
    private string port = "8180";
    private string activeUrl = "/capi/card/valid?cardId=cardkey4";
    private string verifyUrl = "/capi/card/active?cardId=cardkey9&secret=12345678901234561&dvcKey=1112222";

    private string active_CardId = "cardkey4";
    private string verify_CardId = "cardkey9";
    private string secret = "12345678901234561";
    private string dvcKey = "1112222";

    // Start is called before the first frame update
    void Start()
    {
        headUrl = string.Format(head, ip, port);
        StartCoroutine(GetMsg(headUrl + activeUrl));
        //StartCoroutine(GetJsonMsg(headUrl + activeUrl));
        //StartCoroutine(PostMsg(headUrl + activeUrl));
    }

    IEnumerator GetMsg(string url)
    {
        Debug.LogError(url);
        //UnityWebRequest unityWebRequest = new UnityWebRequest(url);
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
        //yield return unityWebRequest.downloadHandler;
        yield return unityWebRequest.SendWebRequest();
        Debug.LogError("结果 = " + unityWebRequest.result);
        if (unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.LogError("具体为 = " + unityWebRequest.downloadHandler.text);
        }
    }

    IEnumerator GetJsonMsg(string url)
    {
        Debug.LogError(url);
        //UnityWebRequest unityWebRequest = new UnityWebRequest(url);
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
        //yield return unityWebRequest.downloadHandler;
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        yield return unityWebRequest.SendWebRequest();
        Debug.LogError("结果 = " + unityWebRequest.result);
        if (unityWebRequest.isDone & unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.LogError("具体为 = " + unityWebRequest.downloadHandler.text);
        }
    }

    IEnumerator PostMsg(string url)
    {
        WWWForm form = new WWWForm();
        //键值对
        form.AddField("cardId", "cardkey4");
        //请求链接，并将form对象发送到远程服务器
        UnityWebRequest webRequest = UnityWebRequest.Post(headUrl + "/capi/card/valid", form);
        Debug.Log("地址为 = " + webRequest.url);
        yield return webRequest.SendWebRequest();
        Debug.Log("结果为 = " + webRequest.result);
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.LogError("具体内容为 = " + webRequest.downloadHandler.text);
        }
    }
}