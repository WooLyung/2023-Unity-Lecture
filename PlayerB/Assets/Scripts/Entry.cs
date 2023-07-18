using SimpleServerAPI.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entry : MonoBehaviour
{
    public void StartButton()
    {
        ServerAPI.Start("PingPong-PlayerB");
        ServerAPI.PostClientData("PingPong-PlayerA", new PostData().AddInt(0));
        SceneManager.LoadScene("GameScene");
    }
}