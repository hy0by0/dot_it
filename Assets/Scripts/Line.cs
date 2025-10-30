using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;
using static TreeEditor.TreeGroup;

public class Line : MonoBehaviour
{
    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;

    public Transform startPos;
    public GameObject endPosObj;
    public Transform endPos;
    EdgeLine edgeLine;

    public float tongueSpeed = 1.0f;
    public float maxMagnitude = 2.0f;
    public float canMagnitude;

    public float tongueScore = 0.0f;
    public int getCount = 0;

    //Vector3[] positions;
    private Vector3 currentDirection; //現在の伸びていく方向を保持するためのベクトル変数
    private List<Vector3> linePoints = new List<Vector3>(); //頂点座標の管理するリスト。始点・終点・折れ曲がった点座標をリストで管理


    public bool moving = false; //伸びるかどうかのフラグ
    public bool backFlag = false; //縮むかどうかのフラグ


    void Start()
    {
        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        edgeCollider = this.gameObject.GetComponent<EdgeCollider2D>();
        endPos = endPosObj.GetComponent<Transform>();
        edgeLine = endPosObj.GetComponent<EdgeLine>();

        // 初期化
        linePoints.Add(startPos.position);
        linePoints.Add(endPos.position);

        currentDirection = (endPos.position - startPos.position).normalized;
        UpdateLine();
        canMagnitude = maxMagnitude;


        // 点の数を指定する
        //lineRenderer.positionCount = positions.Length;

        // 線を引く場所を指定する
        //lineRenderer.SetPositions(positions);

    }

    // Update is called once per frame
    void Update()
    {
        //クリック中のマウスへの方向ベクトル　currentVectorを更新させて！！！！
        if (Input.GetMouseButton(0))
        {
            moving = true;
            currentDirection = endPos.up;

        }
        else if(Input.GetMouseButtonUp(0))
        {
            moving = false;
            backFlag = true;
        }

        if (moving) //伸びていくとき
        {
            int layerMask = 1 << LayerMask.NameToLayer("Wall");
            RaycastHit2D hit = Physics2D.Raycast(endPos.position, currentDirection, 0.5f, layerMask);
            Debug.DrawRay(endPos.position, currentDirection * 0.5f, Color.green);
            if (hit.collider != null)
            {
                //Debug.Log(hit.collider.gameObject.name);
                linePoints.Insert(linePoints.Count - 1, endPos.position);
                Vector2 normal = hit.normal;
                currentDirection = Vector2.Reflect(currentDirection, normal).normalized;
                endPos.up = currentDirection;
                //長さの最大値の残り長さをここで反映
                if (linePoints.Count >= 3)
                {
                    canMagnitude -= (linePoints[linePoints.Count - 2] - linePoints[linePoints.Count - 3]).magnitude;
                }

            }

            Vector3 newPos = currentDirection * tongueSpeed * Time.deltaTime;
            endPos.position += new Vector3(newPos.x,newPos.y, 0);

            //長さ制限の処理
            Vector3 limitPos = endPos.position - linePoints[linePoints.Count - 2];
            if (limitPos.magnitude > canMagnitude)
            {
                limitPos *= canMagnitude / limitPos.magnitude;
                endPos.position = limitPos + linePoints[linePoints.Count - 2];
            }

            linePoints[linePoints.Count - 1] = endPos.position;
            UpdateLine();
        }
        else if (backFlag) //縮んで行くとき
        {
            //１つ前の頂点or起点へ戻っていくようにする（そのまま再度伸びることもできるように

            float distance = Vector2.Distance(endPos.position, linePoints[linePoints.Count - 2]);
            if (distance < 0.5f)
            {
                endPos.position = linePoints[linePoints.Count - 2];              
                if (linePoints.Count > 2) //反射が１個でも残っているとき
                {
                    endPos.up = (endPos.position - linePoints[linePoints.Count - 3]).normalized;
                    canMagnitude += (linePoints[linePoints.Count - 2] - linePoints[linePoints.Count - 3]).magnitude;
                    linePoints.RemoveAt(linePoints.Count - 2);
                }
                else
                {
                    Debug.Log("戻った！");
                    backFlag = false;
                    edgeLine.backFlag = false;
                }
            }
            else if(distance >= 20.0f)
            {
                Debug.Log("異常値まで離れています");
                endPos.position = linePoints[linePoints.Count - 2];
                if (linePoints.Count > 2) //反射が１個でも残っているとき
                {
                    endPos.up = (endPos.position - linePoints[linePoints.Count - 3]).normalized;
                    canMagnitude += (linePoints[linePoints.Count - 2] - linePoints[linePoints.Count - 3]).magnitude;
                    linePoints.RemoveAt(linePoints.Count - 2);
                }
                else
                {
                    Debug.Log("緊急で戻ります");
                    backFlag = false;
                    edgeLine.backFlag = false;
                }
            }
        }
        else //起点に戻ったとき、伸ばしていないとき
        {
            //endPos.position = startPos.position;
            currentDirection = (endPos.position - startPos.position).normalized;
            canMagnitude = maxMagnitude;

            //とばして戻った場合
            if (linePoints.Count > 2)
            {
                Debug.Log("とばして戻ってます");
                for (int i = linePoints.Count; i > 2; i--)
                {
                    linePoints.RemoveAt(linePoints.Count - 2);
                }
            }
            
        }

        linePoints[0] = startPos.position;
        linePoints[linePoints.Count - 1] = endPos.position;
        UpdateLine();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            Debug.Log("ミス");
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Item")
        {
            Debug.Log("うますぎる");
            //float score += collision.gameObject.Item().score;//アイテムのスコアを受け取るようにする
            Item ItemScript = collision.gameObject.GetComponent<Item>();
            getCount += 1;
            tongueScore += ItemScript.scoreValue;
            Destroy(collision.gameObject);
        }
    }

    //ここで、LinRendererとedgeColliderの変化を反映させている
    void UpdateLine()
    {
        //線の頂点の数を反映させる
        lineRenderer.positionCount = linePoints.Count; 
        //ToArrayメソッドで、リストから配列へ変換させた上で引数として与えている。ここでLineRenderの頂点の座標を反映させている
        lineRenderer.SetPositions(linePoints.ToArray()); 

        // EdgeCollider のために Vector2 に変換
        List<Vector2> edgePoints = new List<Vector2>();
        foreach (var point in linePoints)
        {
            edgePoints.Add(new Vector2(point.x, point.y));
        }
        //ここでedgeColliderの頂点の座標を反映させている
        edgeCollider.SetPoints(edgePoints);

    }


}
