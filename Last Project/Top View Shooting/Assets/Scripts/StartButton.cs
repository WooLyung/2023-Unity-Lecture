using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void Connect()
    {
        bool connect = CSocket.Instance.Connect();
        if (connect)
        {
            bool join = CSocket.Instance.Join("name", "0000FF");
            if (join)
                SceneManager.LoadScene("GameScene");
        }
    }
}