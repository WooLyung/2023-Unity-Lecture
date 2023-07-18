using SimpleServerAPI.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject opponent;
    [SerializeField] GameObject ball;
    [SerializeField] Text scoreText;
    [SerializeField] Text waitText;

    float sendTime = 0.0f;

    void PlayerMove()
    {
        sendTime += Time.deltaTime;
        if (sendTime > 0.02f)
        {
            sendTime -= 0.02f;
            ServerAPI.PostClientData("PingPong-PlayerA",
                  new PostData().AddInt(1).AddFloat(player.transform.position.y));
        }
  

        player.transform.Translate(0, Input.GetAxisRaw("Vertical") * Time.deltaTime * 3, 0);
    }

    void Update()
    {
        PlayerMove();

        GetData getData;
        while (ServerAPI.GetClientData(out getData))
        {
            int code = getData.GetInt();
            if (code == 0)
            {
                float y = getData.GetFloat();
                Vector3 pos = opponent.transform.position;
                pos.y = y;
                opponent.transform.position = pos;

                float bx = getData.GetFloat();
                float by = getData.GetFloat();
                Vector3 bpos = ball.transform.position;
                bpos.x = bx;
                bpos.y = by;
                ball.transform.position = bpos;
            }
            else if (code == 1)
            {
                int a = getData.GetInt();
                int b = getData.GetInt();
                scoreText.text = $"{a}:{b}";
            }
            else if (code == 2)
            {
                string text = getData.GetString();
                waitText.text = text;
            }
            else if (code == 3)
            {
                int winner = getData.GetInt();
                if (winner == 0)
                    SceneManager.LoadScene("Lose");
                else
                    SceneManager.LoadScene("Win");
            }
        }
    }
}
