using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectButton : MonoBehaviour
{
    public void Connect()
    {
        if (CSocket.Instance.Connect())
        {
            bool result = CSocket.Instance.Join("nickname!!", "FF6600");
            if (result)
                SceneManager.LoadScene("GameScene");
        }
    }
}