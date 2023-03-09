using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Example_CardPwd : MonoBehaviour
{
    private string url = "";
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
        url = string.Format(head, ip, port);
        Debug.LogError(url);
    }

    private void PostMsg()
    {

    }
}
