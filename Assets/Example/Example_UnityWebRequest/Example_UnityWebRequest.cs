using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class RequestMsg
{
    public string msg;
    public int code;
    public RequestData data;
    public long timestamp;
}

[Serializable]
public class RequestData
{
    public long timeRemain;
    public long expAt;
    public int status;
}

public class Example_UnityWebRequest : MonoBehaviour
{
    private string headUrl = "";
    private string head = "http://{0}:{1}";
    private string ip = "120.53.248.67";
    private string port = "8180";
    private string activeUrl = "/capi/card/valid?cardId={0}";
    private string verifyUrl = "/capi/card/active?cardId=cardkey9&secret=12345678901234561&dvcKey=1112222";

    private string active_CardId = "cardkey4";
    private string verify_CardId = "cardkey9";
    private string secret = "12345678901234561";
    private string dvcKey = "1112222";

    // Start is called before the first frame update
    void Start()
    {
        headUrl = string.Format(head, ip, port);
        activeUrl = string.Format(activeUrl, active_CardId);
        StartCoroutine(GetMsg(headUrl + activeUrl));
        //StartCoroutine(PostMsg(headUrl + activeUrl));
    }

    IEnumerator GetMsg(string url)
    {
        Debug.Log("��ַΪ = " + url);
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isDone && unityWebRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.LogError("��������Ϊ = " + unityWebRequest.downloadHandler.text);

            //RequestMsg requestMsg = JsonUtility.FromJson<RequestMsg>(unityWebRequest.downloadHandler.text);
            //Debug.Log(requestMsg.msg);
            //Debug.Log(requestMsg.code);
            //Debug.Log(requestMsg.data.timeRemain);
            //Debug.Log(requestMsg.data.expAt);
            //Debug.Log(requestMsg.data.status);
            //Debug.Log(requestMsg.timestamp);

            RequestMsg requestMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestMsg>(unityWebRequest.downloadHandler.text);
            Debug.Log(requestMsg.msg);
            Debug.Log(requestMsg.code);
            Debug.Log(requestMsg.data.timeRemain);
            Debug.Log(requestMsg.data.expAt);
            Debug.Log(requestMsg.data.status);
            Debug.Log(requestMsg.timestamp);
        }
    }

    IEnumerator PostMsg(string url)
    {
        WWWForm form = new WWWForm();
        //��ֵ��
        form.AddField("cardId", "cardkey4");
        //�������ӣ�����form�����͵�Զ�̷�����
        UnityWebRequest webRequest = UnityWebRequest.Post(headUrl + "/capi/card/valid", form);
        webRequest.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("��ַΪ = " + webRequest.url);
        yield return webRequest.SendWebRequest();
        Debug.Log("���Ϊ = " + webRequest.result);
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.LogError("��������Ϊ = " + webRequest.downloadHandler.text);
        }
    }
}