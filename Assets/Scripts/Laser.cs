using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Laser : MonoBehaviour
{
    [Header("LaserのPrefabをここに入れる")]
    [SerializeField] private GameObject laserPrefab;
    [Header("レーザーの最大長")]
    public float max_Length = 23.0f;  //レーザーの最大長

    [Header("反射回数")]
    public int max_Reflect = 3;  //反射回数

    private GameObject preWall; //同じ方向の壁に連続で当たらないようにするための変数
    public LineRenderer[] lasers_Line;
    public EdgeCollider2D[] lasers_Collider;
    public Vector3 nowOrigin; //現在のレーザー原点座標を格納するベクトル変数
    public Vector2 nowDirection; //現在のレーザーの方向を格納するベクトル変数
    public List<Vector3> linePoints = new List<Vector3>(); //頂点座標の管理するリスト。

    /// <summary>
    /// レーザー生成関数
    /// </summary>
    /// <param name="origin">レーザーの原点座標</param>
    /// <param name="direction">レーザーの方向</param>
    /// <param name="n">今何本目か(0ははじまり)</param>
    public void creat(Vector3 origin, Vector2 direction, int n) 
    {
        if (n == 0)
        {
            nowOrigin = origin; //現在のレーザーの始点座標を初期化
            nowDirection = direction; //現在のレーザーの方向を初期化
            lasers_Line = new LineRenderer[max_Reflect + 1];//LineRendererを最大反射回数＋１本用意して格納しておく
            lasers_Collider = new EdgeCollider2D[max_Reflect + 1];
            preWall = null; //preWallの初期化
        }
        // 反射回数１以上からはここで生成
        if (n < max_Reflect + 1)
        {
            GameObject laserChild = Instantiate(laserPrefab, this.transform); //レーザーオブジェクトを生成
            laserChild.name = "laser_" + n;
            LineRenderer line = laserChild.GetComponent<LineRenderer>();
            EdgeCollider2D edge = laserChild.GetComponent<EdgeCollider2D>();
            lasers_Line[n] = line; //LineRendererのN番目を新たに生成されたLaserに更新
            lasers_Collider[n] = edge; //LineRendererのN番目を新たに生成されたLaserに更新
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, max_Length);

            foreach (RaycastHit2D hit in hits)
            {
                //同じ方向の壁に連続で当たらないようにするため、preWallを除外、反射ごとに更新させる。
                if (hit.collider.gameObject != preWall && hit.collider.gameObject.tag == "Wall")
                {
                    Vector3 endPos = hit.point;
                    Vector2 reflectDirection = Vector2.Reflect(direction, hit.normal);

                    line.SetPosition(0, origin);
                    line.SetPosition(1, endPos);

                    linePoints.Add(origin);
                    linePoints.Add(endPos);
                    // EdgeCollider2D のために 頂点ベクトルをVector2 に変換
                    List<Vector2> edgePoints = new List<Vector2>();
                    foreach (var point in linePoints)
                    {
                        // ワールド座標 → ローカル座標へ変換しておく
                        Vector2 localPoint = edge.transform.InverseTransformPoint(point);
                        edgePoints.Add(localPoint);
                    }
                    edge.SetPoints(edgePoints); //ここでedgeの頂点の座標を反映
                    linePoints = new List<Vector3>(); //１つのレーザーのedgeCollider2dを反映ごとに初期化させておく
                    edgePoints = new List<Vector2>(); // 同様に初期化させておく

                    preWall = hit.collider.gameObject; //直前に衝突した壁オブジェクトを更新

                    creat(endPos, reflectDirection, ++n); //取得した終点を原点としてさらにcreat関数自身を呼び出す

                    break;
                }
            }
        }
    }

    /// <summary>
    /// レーザー移動時に実行する関数
    /// </summary>
    /// <param name="origin">レーザーの原点座標</param>
    /// <param name="direction">レーザーの方向</param>
    public void move(Vector3 origin, Vector2 direction)
    {
        preWall = null;
        nowOrigin = origin; //レーザー原点座標更新
        nowDirection = direction; //レーザーの向き更新
        Vector3 startPos = origin;
        Vector2 nextDirection = direction;

        // LineRendererとEdgeCollider2Dを更新
        for (int i = 0; i < lasers_Line.Length; i++)
        {
            LineRenderer line = lasers_Line[i];
            EdgeCollider2D edge = lasers_Collider[i];

            if (line == null || edge == null) continue;

            RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, nextDirection, max_Length);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject != preWall && hit.collider.CompareTag("Wall"))
                {
                    Vector3 endPos = hit.point;
                    Vector2 reflectDirection = Vector2.Reflect(nextDirection, hit.normal);

                    // --- LineRenderer更新 ---
                    line.SetPosition(0, startPos);
                    line.SetPosition(1, endPos);

                    // --- EdgeCollider2D更新 ---
                    var localPoints = new List<Vector2>
                {
                    edge.transform.InverseTransformPoint(startPos),
                    edge.transform.InverseTransformPoint(endPos)
                };
                    edge.SetPoints(localPoints);

                    // 次のレーザー反射用に情報更新
                    preWall = hit.collider.gameObject;
                    startPos = endPos;
                    nextDirection = reflectDirection;

                    break; // 次のレーザーへ
                }
            }
        }
    }



    /// <summary>
    /// 現在のレーザーの原点座標を取得する関数
    /// </summary>
    /// <returns></returns>
    public Vector3 getOrigin()
    {
        return nowOrigin;
    }
    /// <summary>
    /// 現在のレーザーの方向を取得する関数
    /// </summary>
    /// <returns></returns>
    public Vector2 getDirection()
    {
        return nowDirection;
    }

}