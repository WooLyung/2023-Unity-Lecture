using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite1, sprite2;

    private int id;
    private string nickname;

    public void Init(int id, string nickname, string hexColor, float x, float y)
    {
        Color color;
        ColorUtility.TryParseHtmlString("#" + hexColor, out color);
        sprite1.color = color;
        sprite2.color = color;
        transform.position = new Vector3(x, y, 0);

        this.id = id;
        this.nickname = nickname;
    }

    private void FixedUpdate()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);

        Vector3 vec = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
            vec.y += 1;
        if (Input.GetKey(KeyCode.S))
            vec.y -= 1;
        if (Input.GetKey(KeyCode.D))
            vec.x += 1;
        if (Input.GetKey(KeyCode.A))
            vec.x -= 1;
        transform.position += vec.normalized * Time.fixedDeltaTime * 5.0f;
    }
}
