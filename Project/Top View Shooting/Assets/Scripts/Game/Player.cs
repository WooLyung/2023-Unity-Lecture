using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite;

    private int id;
    private string nickname;

    public void Init(int id, string nickname, string hexColor, float x, float y)
    {
        Color color;
        ColorUtility.TryParseHtmlString("#" + hexColor, out color);
        sprite.color = color;
        transform.position = new Vector3(x, y, 0);

        this.id = id;
        this.nickname = nickname;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(0, Time.fixedDeltaTime * 5.0f, 0);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(0, Time.fixedDeltaTime * -5.0f, 0);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Time.fixedDeltaTime * 5.0f, 0, 0);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Time.fixedDeltaTime * -5.0f, 0, 0);
    }
}
