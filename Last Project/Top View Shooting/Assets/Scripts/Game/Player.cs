using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite1, sprite2;

    [SerializeField]
    private Slider hpBar; // 체력을 위한 UI

    private int id;
    private string nickname;
    private int hp; // 체력 변수 추가

    public void UpdateEvent(OnEvent_Update evt)
    {
        foreach (var player in evt.players)
        {
            if (player.id == id)
            {
                hp = player.hp;
                hpBar.value = hp / 100.0f;
            }
        }
    }

    public void Init(int id, string nickname, string hexColor, float x, float y)
    {
        Color color;
        ColorUtility.TryParseHtmlString("#" + hexColor, out color);
        sprite1.color = color;
        sprite2.color = color;
        transform.position = new Vector3(x, y, 0);

        this.id = id;
        this.nickname = nickname;
        hp = 100; // 최대체력은 100
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

    [SerializeField]
    private GameObject line;
    private float shootTime = 0.0f;

    private void Update()
    {
        shootTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && shootTime >= 0.2f)
        {
            shootTime = 0.0f;
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = new Vector3(mouse.x - transform.position.x, mouse.y - transform.position.y).normalized;
            Vector3 start = new Vector3(transform.position.x + dir.x, transform.position.y + dir.y);

            RaycastHit2D hit = Physics2D.Raycast(start, dir, 20.0f);
            if (hit.collider != null)
            {
                GameObject newLine = Instantiate(line);
                newLine.GetComponent<LineRenderer>().SetPositions(new Vector3[]
                {
                    new Vector3(start.x, start.y, -5),
                    new Vector3(hit.point.x, hit.point.y, -5)
                });
                Destroy(newLine, 0.2f);

                if (hit.collider.tag == "Enemy")
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    CSocket.Instance.EmitEvent(new EmitEvent_Damage(id, enemy.id, 10));
                }
            }
        }
    }
}
