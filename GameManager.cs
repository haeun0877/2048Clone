using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] n;

    int x, y, j, k, l;
    bool wait, move, stop;
    Vector3 firstPos, gap, twoPos;


    GameObject[,] Square = new GameObject[4, 4];

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        // 뒤로가기
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (Input.GetMouseButtonDown(0))
        {
            wait = true;
            firstPos = Input.mousePosition;

        }
        if (Input.GetMouseButton(0))
        {
            twoPos = Input.mousePosition;
            gap = twoPos - firstPos;
            if (gap.magnitude < 100) return;
            gap.Normalize(); // 크기는 빼고 방향값만 받음

            if (wait)
            {
                wait = false;
                // 위 up
                if (gap.y > 0 && gap.x > -0.5f && gap.x < 0.5f)
                { 
                    for(x=0; x<=3; x++)
                    {
                        for(y=0; y<=2; y++)
                        {
                            for(int i=3; i>=y+1; i--)
                            {
                                MoveOrCombine(x, i-1, x, i);
                            }
                        }
                    }
                }
                // 아래 down 
                else if (gap.y < 0 && gap.x > -0.5f && gap.x < 0.5f)
                {
                    for(x=0; x<=3; x++)
                    {
                        for(y=3; y>=1; y--)
                        {
                            for(int i=0; i<y-1; i++)
                            {
                                MoveOrCombine(x, i+1, x, i);
                            }
                        }
                    }
                }
                // 오른쪽 
                else if (gap.x > 0 && gap.y > -0.5f && gap.y < 0.5f)
                {
                    for(y=0; y<=3; y++)
                    {
                        for(x=0; x<=2; x++)
                        {
                            for(int i=3; i>=x+1; i--)
                            {
                                MoveOrCombine(i - 1, y, i, y);
                            }
                        }
                    }
                }
                // 왼쪽
                else if (gap.x < 0 && gap.y > -0.5f && gap.y < 0.5f)
                {
                    for (y = 0; y <= 3; y++)
                    {
                        for (x = 3; x >= 1; x--)
                        {
                            for (int i = 0; i <= x - 1; i++)
                            {
                                MoveOrCombine(i + 1, y, i, y);
                            }
                        }
                    }
                }
                else return;

                if (move)
                {
                    move = false;
                    Spawn();
                    k = 0;
                    l = 0;
                    for(x =0; x<=3; x++)
                    {
                        for (y=0; y<=3; y++)
                        {
                            if (Square[x, y] == null)
                            {
                                k++;
                                continue;
                            }
                            if (Square[x, y].tag == "Combine")
                            {
                                Square[x, y].tag = "Untagged";
                            }
                        }
                    }
                    
                    if(k == 0)
                    {
                        //가로, 세로 같은 블럭이 없으면 l이 0이되어서 게임오버
                        for(y=0; y<=3; y++)
                        {
                            for(x=0; x<=2; x++)
                            {
                                if (Square[x, y].name == Square[x + 1, y].name) l++;
                            }
                        }
                        for(x=0; x<=3; x++)
                        {
                            for (y = 0; y <= 2; y++)
                                if (Square[x, y].name == Square[x, y + 1].name) l++;
                        }
                    }
                }

            }
        }

        /*
        //모바일꺼까지 코드 넣으면 작동 안됨 왜인지 모르겠음 ㅠㅠ
        //컴퓨터 마우스로 좌클릭하거나 모바일로 하나의 손가락으로만 선택했을때
        if (Input.GetMouseButtonDown(0) || (Input.touchCount==1&& Input.GetTouch(0).phase == TouchPhase.Began))
        {
            wait = true;
            firstPos = Input.GetMouseButtonDown(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;

        }
        if (Input.GetMouseButton(0) || (Input.touchCount ==1 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            twoPos = Input.GetMouseButtonDown(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;
            gap = twoPos - firstPos;
            if (gap.magnitude < 100) return;
            gap.Normalize(); // 크기는 빼고 방향값만 받음

            if (wait)
            {
                wait = false;
                // 위 up
                if (gap.y > 0 && gap.x > -0.5f && gap.x < 0.5f) { Debug.Log("up"); }
                // 아래 down 
                else if (gap.y < 0 && gap.x > -0.5f && gap.x < 0.5f) { Debug.Log("down"); }
                // 오른쪽 
                else if (gap.x > 0 && gap.y > -0.5f && gap.y < 0.5f) { Debug.Log("right"); }
                // 왼쪽
                else if (gap.x < 0 && gap.y > -0.5f && gap.y < 0.5f) { Debug.Log("left"); }
                else return;
            }
        }
        */
    }

    //[x1, y1]은 이동전 좌표, [x2, y2]는 이동 후 좌표
    void MoveOrCombine(int x1, int y1, int x2, int y2)
    {
        //이동될 좌표가 비어있고 이동 전 좌표에 존재하면 이동
        if ( Square[x2, y2] == null && Square[x1,y1] != null)
        {
            move = true;
            Square[x1, y1].GetComponent<Moving>().Move(x2, y2, false);
            Square[x2, y2] = Square[x1, y1];
            Square[x1, y1] = null;
        }

        //둘다 같은 수일 때 결합
        if ( Square[x1, y1] !=null && Square[x2, y2] != null && Square[x1, y1].name == Square[x2,y2].name && Square[x1,y1].tag != "Combine" && Square[x2,y2].tag != "Combine")
        {
            move = true;
            for (j=0; j<= 16; j++)
            {
                if(Square[x2,y2].name == n[j].name + "(Clone)")
                {
                    //결합하고자하는 j값이 나오게됨
                    break;
                }
            }
            Square[x1, y1].GetComponent<Moving>().Move(x2, y2, true);
            Destroy(Square[x2, y2]);
            Square[x1, y1] = null;
            Square[x2, y2] = Instantiate(n[j + 1], new Vector3(-1.8f + 1.2f * x2, -1.8f + 1.2f * y2), Quaternion.identity);
            Square[x2, y2].tag = "Combine";
            Square[x2, y2].GetComponent<Animator>().SetTrigger("Combine");
        }
    }

    void Spawn()
    {
        while (true) { 
            x = Random.Range(0, 4);
            y = Random.Range(0, 4);
            if (Square[x, y] == null)
                break;
        }

        //instantiate는 객체를 만드는 함수로서 Destroy와 반대되는 개념
        //8분의 7확률로 n[0]인 2를 띄우고 8분의1 확률로 n[1]인 4를 띄운다, -1.8f는 0,0의 초기값 위치를 표현하고 한칸 옮길 때 마다 1.2f씩 차이남
        //마지막 Quaternion은 굳이 회전값을 주고싶지 않다면 Quaternion.identityf를 써서 그냥 기본값을 줌
        Square[x, y] = Instantiate(Random.Range(0, 8) > 0 ? n[0] : n[1], new Vector3(-1.8f + 1.2f * x, -1.8f + 1.2f * y), Quaternion.identity);
        Square[x, y].GetComponent<Animator>().SetTrigger("Spawn");
    }

    // 재시작
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
