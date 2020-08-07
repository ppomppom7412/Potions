using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeGame : MonoBehaviour
{
    //맵 계수
    const int maxX = 13;
    const int maxY = 10;

    //맵 정보
    public int[,] map = new int[maxX, maxY];
    public Dictionary<Vector2, Flower> flowerDiction;

    //레시피 대기열
    public Queue<Recipe> recipeQueue;//대기열
    Recipe curRecipe;                //현재 레시피

    //보여주는 이미지
    //대기열 꽃
    public Flower[] queueflowers = new Flower[9];
    //포션
    public SpriteRenderer potionsprite;
    

    //레시피 가능한 종류리스트
    public List<int> licenser;

    //꽃/포션 스프라이트
    public Sprite[] flowerSprites = new Sprite[6];
    public Sprite[] potionSprites = new Sprite[6];
    public Sprite[] grassSprites = new Sprite[6];

    //꽃 오브젝트 풀
    public FlowerPool flowerPool;

    //터치관련
    public bool isdragged = false;
    public Flower curflower;            //만지고 있는 꽃
    public Vector2 startpoint;          //시작지점
    public Vector3 endpoint;            //종료지점
    public float sensitive = 0.11f;      //민감도


    void Start()
    {
        Create_licenser();
        Create_Queue();

        Create_Map();
        ViewMap();

        AddRandomQueue();
        AddRandomQueue();
        AddRandomQueue();
        NextQueue(false);
    }

    void Update()
    {
        GetMouse();
    }

    /// <summary>
    /// 라이센스 생성 
    /// </summary>
    void Create_licenser()
    {
        licenser = new List<int>();

        for (int i = 0; i < 6; ++i) {
            AddLicense(i);
        }
    }

    /// <summary>
    /// 라이센스 추가
    /// </summary>
    /// <param name="id">레시피 아이디</param>
    void AddLicense(int id)
    {
        //객체 확인
        if (licenser == null) {
            Create_licenser();
        }

        //가지고 있지 않으면 추가
        if (!licenser.Contains(id)) {
            licenser.Add(id);
        }
    }

    /// <summary>
    /// 대기열 생성
    /// </summary>
    void Create_Queue()
    {
        recipeQueue = new Queue<Recipe>();

        //라이센서 생성 확인 및 생성
        if (licenser == null)
            Create_licenser();
    }

    /// <summary>
    /// 대기열 추가
    /// </summary>
    /// <param name="id"></param>
    void AddQueue(int id)
    {
        if (recipeQueue == null) {
            Create_Queue();
        }

        recipeQueue.Enqueue(new Recipe(id));
    }

    /// <summary>
    /// 대기열 랜덤 추가
    /// </summary>
    void AddRandomQueue()
    {
        if (recipeQueue == null){
            Create_Queue();
        }

        //허가된 레시피 개수 확인
        if (licenser.Count < 1) {
            Debug.Log("D02 :: recipe made error");
        }

        int recipeid = licenser[Random.Range(0, licenser.Count)];

        recipeQueue.Enqueue(new Recipe(recipeid));

    }

    /// <summary>
    /// 대기열 보여주기
    /// </summary>
    void ViewQueue()
    {
        //보강필요 ****
        int index = 0;
        foreach (Recipe rec in recipeQueue){
            for (int i =0; i < 3; ++i ) {
                if (i >= rec.count)
                    queueflowers[index].SetSprite(null);
                else
                    queueflowers[index].SetSprite(flowerSprites[rec.list[i]]);

                ++index;
            }
        }
    }

    /// <summary>
    /// 다음 대기열
    /// </summary>
    void NextQueue(bool ismake)
    {
        curRecipe = recipeQueue.Peek();
        StartCoroutine(ViewPotionGrass(recipeQueue.Peek().id, ismake));

        recipeQueue.Dequeue();
        AddRandomQueue();

        ViewQueue();

        curRecipe = recipeQueue.Peek();
        Debug.Log("[레시피] " + curRecipe.ReadRecipe());
    }

    #region 맵
    /// <summary>
    /// 맵 생성
    /// </summary>
    void Create_Map()
    {
        for (int x = 0; x < maxX; ++x){
            for (int y = 0; y < maxY; ++y){
                map[x,y] = Random.Range((int)Flower.Materials.red, (int)Flower.Materials.max);
            }//end for x
        }//end for y
    }

    /// <summary>
    /// 맵에 맞게 꽃 배치
    /// </summary>
    void ViewMap()
    {
        flowerDiction = new Dictionary<Vector2, Flower>();
        Flower curflower;

        for (int x = 0; x < maxX; ++x){
            for (int y = 0; y < maxY; ++y){
                curflower = flowerPool.GetRelaxedFlower();
                curflower.SetId(map[x, y]);
                curflower.SetPos(new Vector2(x, y));
                curflower.SetSprite(flowerSprites[map[x, y]]);

                flowerDiction.Add(new Vector2(x, y), curflower);
            }//end for x
        }//end for y
    }

    /// <summary>
    /// 맵에 있는 꽃 삭제
    /// </summary>
    void DeleteFlower(int x, int y)
    {
        Vector2 pos = new Vector2(x, y);
        Flower saveflower;

        //맵의 꽃 삭제
        flowerDiction[pos].SetOff();
        flowerDiction.Remove(pos);

        //맵 밀기
        for (int i = x+1; i < maxX; ++i){

            pos.x = i;
            map[i-1, y] = map[i, y];
            saveflower = flowerDiction[pos];
            flowerDiction.Remove(pos);

            pos = new Vector2(i - 1, y);
            flowerDiction.Add(new Vector2(i - 1, y), saveflower);
            saveflower.SetPos(pos, true);
        }

        //빈곳에 새로 생성
        map[maxX-1,y] = Random.Range((int)Flower.Materials.red, (int)Flower.Materials.max);

        //보여지는 새로운 꽃 생성
        saveflower = flowerPool.GetRelaxedFlower();
        saveflower.SetId(map[maxX - 1, y]);
        saveflower.SetPos(new Vector2(maxX - 1, y));
        saveflower.SetSprite(flowerSprites[map[maxX - 1, y]]);
        flowerDiction.Add(new Vector2(maxX - 1, y), saveflower);

        Debug.Log("Delete [" +x+", "+y+"]");
    }

    /// <summary>
    /// 맵에서 재료를 모두 찾는다.
    /// </summary>
    /// <param name="id">재료 아이디</param>
    /// <returns></returns>
    List<Vector2> FindingMaterialOnTheMap(int id)
    {
        List<Vector2> result = new List<Vector2>();

        for (int x = 0; x < maxX; ++x){
            for (int y = 0; y < maxY; ++y){
                if (map[x, y] == id){
                    result.Add(new Vector2(x, y));
                }//end if ==id
            }//end for x
        }//end for y

        return result;
    }
    #endregion

    /// <summary>
    /// 포션병 표기
    /// </summary>
    IEnumerator ViewPotionGrass(int id, bool ismake = false )
    {
        if (!ismake) {
            potionsprite.sprite = grassSprites[id];
        }
        else {
            potionsprite.sprite = potionSprites[id];
            yield return new WaitForSeconds(1.5f);
            potionsprite.sprite = grassSprites[curRecipe.id];
        }
    }

    /// <summary>
    /// 해당 위치의 꽃을 원하는 방향의 꽃과 교체한다. 교체실패시 false
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="pos"></param>
    bool SwapFlowers(Vector2 dir, Vector2 pos)
    {
        Vector2 newpos = new Vector2(pos.x + dir.x , pos.y + (dir.y * -1));
        Flower storepos;
        Flower storenewpos;


        //스왑 가능 확인
        if (newpos.x < 0 || newpos.x >= maxY)
            return false;
        if (newpos.y < 0 || newpos.y >= maxY)
            return false;
        if (dir == Vector2.zero)
            return false;

        //저장공간에 저장
        storepos = flowerDiction[pos];
        storenewpos = flowerDiction[newpos];

        //사전에서 삭제
        flowerDiction.Remove(pos);
        flowerDiction.Remove(newpos);

        //맵 값 변경
        map[(int)pos.x, (int)pos.y] = storenewpos.id;
        map[(int)newpos.x, (int)newpos.y] = storepos.id;

        //사전에 새로 등록
        flowerDiction.Add(newpos, storepos);
        flowerDiction.Add(pos, storenewpos);

        //위치변경
        flowerDiction[pos].SetPos(pos);
        flowerDiction[newpos].SetPos(newpos);

        //Debug.Log("Move " + pos.ToString() + " >> " + newpos.ToString());

        return true;
    }

    /// <summary>
    /// 시작점과 끝점을 통한 이동방향 찾기
    /// </summary>
    /// <param name="start">시작점</param>
    /// <param name="end">끝점</param>
    /// <returns></returns>
    Vector2 FindDirection(Vector2 start, Vector2 end)
    {
        Vector2 result = new Vector2();
        Vector2 rimit = new Vector2();

        //민감도와 함께 계산
        if (start.x + sensitive <= end.x){
            result.x = 1;
            rimit.x = end.x - (start.x + sensitive);
        }
        else if (start.x >= end.x + sensitive){
            result.x = -1;
            rimit.x = start.x - (end.x + sensitive);
        }
        else{
            result.x = 0;
        }

        if (start.y + sensitive <= end.y){
            result.y = 1;
            rimit.y = end.y - (start.y + sensitive);
        }
        else if (start.y >= end.y + sensitive){
            result.y = -1;
            rimit.y = start.y - (end.y + sensitive);
        }
        else{
            result.y = 0;
        }

        //대각제어
        rimit *= rimit;

        if (rimit.x >= rimit.y)
            result.y = 0;

        else if (rimit.x < rimit.y)
            result.x = 0;

        return result;
    }

    /// <summary>
    /// 포션 제조
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    bool MakePotion(int x, int y)
    {
        curRecipe = recipeQueue.Peek();
        List<Vector2> checklist = new List<Vector2>();

        for (int i = 0; i < curRecipe.count; ++i){

            //레시피 위치에 있는 지 확인
            if (curRecipe.list[i] != map[x, y]) continue;

            for (int h = 0; h < curRecipe.count; ++h) {

                if (x - i + h < 0 || x - i + h >= maxY) continue;

                if (curRecipe.list[h] == map[x - i + h, y]){
                    checklist.Add(new Vector2(x - i + h, y));
                }
            }
            if (checklist.Count == curRecipe.count)
            {
                //포션 추가 
                Debug.Log(curRecipe.name + "+ 1");

                //다음 대기열
                NextQueue(true);

                //맵의 꽃 삭제
                for (int j = checklist.Count -1; j >= 0; --j){
                    DeleteFlower((int)checklist[j].x, (int)checklist[j].y);
                }
                return true;
            }

            checklist.Clear();

            for (int v = 0; v < curRecipe.count; ++v){

                if (y - i + v < 0 || y - i + v >= maxY) continue;

                if (curRecipe.list[v] == map[x, y - i + v]){
                    checklist.Add(new Vector2(x, y - i + v));
                }
            }
            if (checklist.Count == curRecipe.count)
            {
                //포션 추가 
                Debug.Log(curRecipe.name + "+ 1");
                StartCoroutine(ViewPotionGrass(curRecipe.id, true));

                //다음 대기열 
                NextQueue(true);

                //맵의 꽃 삭제
                for (int j = checklist.Count - 1; j >= 0; --j){
                    DeleteFlower((int)checklist[j].x, (int)checklist[j].y);
                }

                return true;
            }

            checklist.Clear();
        }
        return false;
    }

    /// <summary>
    /// 마우스(컴퓨터) 인식
    /// </summary>
    void GetMouse()
    {

        if (Input.GetMouseButtonDown(0))
        {
            startpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(startpoint, transform.forward, 10f);
            if (hit.collider != null)
            {
                curflower = hit.collider.gameObject.GetComponent<Flower>();
                if (curflower != null)
                {
                    isdragged = true;
                    Debug.Log("Drag :: " + curflower.name + " , " + Camera.main.ScreenToWorldPoint(Input.mousePosition).ToString());
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!isdragged) return;

            isdragged = false;
            curflower.SetPos(curflower.position);

            Vector2 movedir = FindDirection(startpoint, endpoint);

            //이동 확인 
            if (SwapFlowers(movedir, curflower.position)){
                MakePotion((int)(curflower.position.x - movedir.x), (int)(curflower.position.y - (movedir.y * -1)));
                MakePotion((int)curflower.position.x, (int)curflower.position.y);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (!isdragged) return;

            endpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endpoint.z = 0;
            curflower.transform.position = endpoint;
        }
    }
}
