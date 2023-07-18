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
    [SerializeField] Ball ballComp;
    [SerializeField] Text scoreText;
    [SerializeField] Text waitText;

    enum State
    {
        Wait, Game, Term
    }

    State gameState = State.Wait;
    int scoreA = 0;
    int scoreB = 0;
    float time = 1.0f;
    int waitTime = 4;
    float sendTime = 0.0f;

    public void Goal(int x)
    {
        if (x == 0)
        {
            scoreA++;
            if (scoreA == 3)
            {
                ServerAPI.PostClientData("PingPong-PlayerB",
                    new PostData().AddInt(3).AddInt(0));
                SceneManager.LoadScene("Win");
            }
            else
            {
                time = 1.0f;
                waitTime = 4;
                gameState = State.Term;
                scoreText.text = $"{scoreA}:{scoreB}";
                ServerAPI.PostClientData("PingPong-PlayerB",
                    new PostData().AddInt(1).AddInt(scoreA).AddInt(scoreB));
            }
        }
        else if (x == 1)
        {
            scoreB++;
            if (scoreB == 3)
            {
                ServerAPI.PostClientData("PingPong-PlayerB",
                    new PostData().AddInt(3).AddInt(1));
                SceneManager.LoadScene("Lose");
            }
            else
            {
                time = 1.0f;
                waitTime = 4;
                gameState = State.Term;
                scoreText.text = $"{scoreA}:{scoreB}";
                ServerAPI.PostClientData("PingPong-PlayerB",
                    new PostData().AddInt(1).AddInt(scoreA).AddInt(scoreB));
            }
        }
    }

    void PlayerMove()
    {
        sendTime += Time.deltaTime;
        if (sendTime > 0.02f)
        {
            sendTime -= 0.02f;
            ServerAPI.PostClientData("PingPong-PlayerB",
                new PostData().AddInt(0).AddFloat(player.transform.position.y).AddFloat(ball.transform.position.x).AddFloat(ball.transform.position.y));
        }

        player.transform.Translate(0, Input.GetAxisRaw("Vertical") * Time.deltaTime * 3, 0);
    }

    void Update()
    {
        PlayerMove();

        if (gameState == State.Wait)
        {
            GetData getData;
            while (ServerAPI.GetClientData(out getData))
            {
                int code = getData.GetInt();
                if (code == 0)
                    gameState = State.Term;
            }
        }
        else if (gameState == State.Term)
        {
            GetData getData;
            while (ServerAPI.GetClientData(out getData))
            {
                int code = getData.GetInt();
                if (code == 1)
                {
                    float y = getData.GetFloat();
                    Vector3 pos = opponent.transform.position;
                    pos.y = y;
                    opponent.transform.position = pos;
                }
            }

            time += Time.deltaTime;
            if (time >= 1.0f)
            {
                time -= 1.0f;
                waitTime--;

                if (waitTime == 0)
                {
                    ServerAPI.PostClientData("PingPong-PlayerB",
                        new PostData().AddInt(2).AddString("Start"));
                    waitText.text = "Start";
                    gameState = State.Game;
                    ballComp.Go();
                    time = 0.0f;
                }
                else
                {
                    ServerAPI.PostClientData("PingPong-PlayerB",
                        new PostData().AddInt(2).AddString($"{waitTime}"));
                    waitText.text = $"{waitTime}";
                }
            }
        }
        else if (gameState == State.Game)
        {
            time += Time.deltaTime;
            if (time >= 1.0f)
            {
                ServerAPI.PostClientData("PingPong-PlayerB",
                    new PostData().AddInt(2).AddString(""));
                waitText.text = "";
                time = -100000000.0f;
            }

            GetData getData;
            while (ServerAPI.GetClientData(out getData))
            {
                int code = getData.GetInt();
                if (code == 1)
                {
                    float y = getData.GetFloat();
                    Vector3 pos = opponent.transform.position;
                    pos.y = y;
                    opponent.transform.position = pos;
                }
            }
        }
    }
}
