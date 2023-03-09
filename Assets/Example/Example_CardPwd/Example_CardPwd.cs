using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Example_CardPwd : MonoBehaviour
{
    private string url = "http://{0}:{1}";
    private string ip = "120.53.248.67";
    private string port = "8180";
    //private string url = "/capi/card/valid?cardId=cardkey4";
    //private string url = "/capi/card/active?cardId=cardkey9&secret=12345678901234561&dvcKey=1112222";

    // Start is called before the first frame update
    void Start()
    {
        string all = Path.Combine(url, ip, port);
        Debug.LogError(all);
    }

    private void PostMsg()
    {

    }
}
