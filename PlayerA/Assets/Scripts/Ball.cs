using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    [SerializeField] GameManager manager;

    Vector2 dir = new(0, 0);

    public float randomLeftAngle() => Random.Range(-Mathf.PI / 4, Mathf.PI / 4);
    public float randomRightAngle() => randomLeftAngle() + Mathf.PI;

    public void Go()
    {
        float theta = 0.0f;
        if (Random.Range(0, 2) == 0)
            theta = randomLeftAngle();
        else
            theta = randomRightAngle();

        dir = new(Mathf.Cos(theta), Mathf.Sin(theta));
    }

    void Update()
    {
        transform.Translate(dir * Time.deltaTime * 4.0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Top"
            || collision.gameObject.name == "Bottom")
            dir.y *= -1;
        else if (collision.gameObject.name == "Player")
        {
            float theta = randomLeftAngle();
            dir = new(Mathf.Cos(theta), Mathf.Sin(theta));
        }
        else if (collision.gameObject.name == "Opponent")
        {
            float theta = randomRightAngle();
            dir = new(Mathf.Cos(theta), Mathf.Sin(theta));
        }
        else if (collision.gameObject.name == "Left")
        {
            manager.Goal(1);
            transform.position = new Vector3();
            dir = new Vector2();
        }
        else if (collision.gameObject.name == "Right")
        {
            manager.Goal(0);
            transform.position = new Vector3();
            dir = new Vector2();
        }
    }
}
