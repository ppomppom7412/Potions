using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPool : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> flowerList;

    public int lenght;
    public Vector2 firstPos;

    private void Start()
    {
        CreatePool();
    }

    /// <summary>
    /// 오브젝트 풀을 만든다.
    /// </summary>
    void CreatePool()
    {
        flowerList = new List<GameObject>();

        if (prefab == null)
            CreatePrefab();

        //자식들을 리스트에 담는다.
        for (int c = 0; c < transform.childCount; ++c) {
            flowerList.Add(transform.GetChild(c).gameObject);

            if (transform.GetChild(c).GetComponent<Flower>()){
                transform.GetChild(c).gameObject.AddComponent<Flower>();
            }
        }

        //부족한 개수를 채워 만든다.
        for (int i = flowerList.Count ; i < lenght; ++i)
        {
            GameObject newOb = Instantiate(prefab, transform);
            newOb.name = "flower_" + i;
            flowerList.Add(newOb);
        }

        AllOff();
    }

    /// <summary>
    /// 프리팹을 임시로 만든다.
    /// </summary>
    void CreatePrefab()
    {
        prefab = new GameObject();
        prefab.AddComponent<RectTransform>();
        prefab.AddComponent<SpriteRenderer>();
        prefab.AddComponent<Flower>();
    }

    /// <summary>
    /// 모두 끄다
    /// </summary>
    public void AllOff()
    {
        foreach (GameObject ob in flowerList) {
            ob.SetActive(false);
        }
    }

    /// <summary>
    /// 쉬고 있는 오브젝트를 반환
    /// </summary>
    /// <returns></returns>
    public Flower GetRelaxedFlower()
    {
        if (flowerList == null)
            return null;

        GameObject returnObject = flowerList.Find(item => item.activeSelf == false);

        //쉬고 있는 오브젝트가 없으면 생성
        if (returnObject == null){
            returnObject = Instantiate(prefab, transform);

            flowerList.Add(returnObject);
        }

        //활성화
        returnObject.SetActive(true);

        return returnObject.GetComponent<Flower>();
    }

}
