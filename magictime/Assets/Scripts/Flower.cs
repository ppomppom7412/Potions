using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour 
{
    public enum Materials { red, blue, yellow, pink, white, black, max, none = -1 };

    static public float sellX = 0.5f;
    static public float sellY = -0.5f;

    public int id;
    public Vector2 position;

    RectTransform rectTr;
    SpriteRenderer spriteRd;

    bool isdragged = false;
    float speed = 3f;

    public void Start()
    {
        rectTr = GetComponent<RectTransform>();
        spriteRd = GetComponent<SpriteRenderer>();

        if (rectTr == null)
            rectTr = gameObject.AddComponent<RectTransform>();

        if (spriteRd == null)
            spriteRd = gameObject.AddComponent<SpriteRenderer>();
    }

    public void Update()
    {

    }

    public void SetPos(Vector2 pos, bool ismove = false)
    {
        if (rectTr == null) {
            rectTr = GetComponent<RectTransform>();

            if (rectTr == null)
                rectTr = gameObject.AddComponent<RectTransform>();
        }

        if (ismove)
        {
            StartCoroutine(MovingPosition(new Vector3(sellX * pos.x, sellY * pos.y, 0)));
        }
        else
        {
            rectTr.localPosition = new Vector3(sellX * pos.x, sellY * pos.y, 0);
        }

        position = pos;
    }

    public void SetSprite(Sprite sp)
    {
        if (spriteRd == null)
        {
            spriteRd = GetComponent<SpriteRenderer>();

            if (spriteRd == null)
                spriteRd = gameObject.AddComponent<SpriteRenderer>();
        }

        spriteRd.sprite = sp;
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public int GetId()
    {
        return id;
    }

    public void SetOff()
    {
        gameObject.SetActive(false);
    }

    IEnumerator MovingPosition(Vector3 end)
    {
        while (Vector3.Distance(end, rectTr.localPosition) > 0.05f){
            rectTr.localPosition = Vector3.Lerp(rectTr.localPosition, end, speed * Time.deltaTime);
            yield return null;
        }

        rectTr.localPosition = end;
    }

    static public string StringMaterials(Materials mater)
    {
        //red, blue, yellow, pink, white, black, max, none
        switch (mater)
        {
            case Materials.red:
                return "red";
            case Materials.blue:
                return "blue";
            case Materials.yellow:
                return "yellow";
            case Materials.pink:
                return "pink";
            case Materials.white:
                return "white";
            case Materials.black:
                return "black";
            case Materials.max:
                return "max";
            case Materials.none:
                return "none";
            default:
                return "error!";
        }
    }

    static public string StringMaterials(int mater)
    {
        //red, blue, yellow, pink, white, black, max, none
        switch (mater)
        {
            case 0:
                return "red";
            case 1:
                return "blue";
            case 2:
                return "yellow";
            case 3:
                return "pink";
            case 4:
                return "white";
            case 5:
                return "black";
            case 6:
                return "max";
            case -1:
                return "none";
            default:
                return "error!";
        }
    }
}
