using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite1, sprite2;

    [SerializeField]
    private Transform body;

    [SerializeField]
    private Slider hpBar;

    [SerializeField]
    private Text nicknameText;

    public int id;
    public int hp;
    public string nickname;

    public void Init(OnEvent_Init.Other info)
    {
        Color color;
        ColorUtility.TryParseHtmlString("#" + info.color, out color);
        sprite1.color = color;
        sprite2.color = color;
        transform.position = new Vector2(info.x, info.y);
        body.eulerAngles = new Vector3(0, 0, info.angle);
        id = info.id;
        nickname = info.nickname;
        hp = info.hp;
        name = $"enemy #{id} : {nickname}";

        nicknameText.text = nickname.Trim();
        hpBar.value = info.hp / 100.0f;
    }

    public void Init(OnEvent_Join info)
    {
        Color color;
        ColorUtility.TryParseHtmlString("#" + info.color, out color);
        sprite1.color = color;
        sprite2.color = color;
        transform.position = new Vector2(info.x, info.y);
        body.eulerAngles = new Vector3(0, 0, info.angle);
        id = info.id;
        nickname = info.nickname;
        hp = info.hp;
        name = $"enemy #{id} : {nickname}";

        nicknameText.text = nickname.Trim();
        hpBar.value = info.hp / 100.0f;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void UpdateEvent(OnEvent_Update.Player info)
    {
        transform.position = new Vector2(info.x, info.y);
        body.eulerAngles = new Vector3(0, 0, info.angle);
        hp = info.hp;
        hpBar.value = info.hp / 100.0f;
    }
}
