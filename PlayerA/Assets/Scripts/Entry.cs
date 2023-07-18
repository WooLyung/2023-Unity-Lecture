using SimpleServerAPI.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entry : MonoBehaviour
{
    public void StartButton()
    {
        ServerAPI.Start("PingPong-PlayerA");
        SceneManager.LoadScene("GameScene");
    }
}
