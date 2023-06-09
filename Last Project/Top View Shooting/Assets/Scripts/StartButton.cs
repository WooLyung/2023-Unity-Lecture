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
            string color = "";
            for (int i = 0; i < 6; i++)
            {
                int j = Random.Range(6, 16);
                if (j >= 10)
                    color += 'A' + j - 10;
                else
                    color += '0' + j;
            }
            bool join = CSocket.Instance.Join("name", color);
            if (join)
                SceneManager.LoadScene("GameScene");
        }
    }
}