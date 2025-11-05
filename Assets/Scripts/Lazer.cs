using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Lazer : MonoBehaviour
{
    [Header("LazerのPrefabをここに入れる")]
    [SerializeField] private GameObject lazerPrefab;
    [Header("レーザーの最大長")]
    public const float max_Length = 23.0f;  //レーザーの最大長

    [Header("反射回数")]
    public const int max_Reflect = 3;  //反射回数

    private GameObject preWall; //同じ方向の壁に連続で当たらないようにするための変数
    public LineRenderer[] lazers_Line;
    public Vector3 nowOrigin; //現在のレーザー原点座標を格納するベクトル変数
    public Vector2 nowDirection; //現在のレーザーの方向を格納するベクトル変数

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
            nowOrigin = origin; //現在のレーザーの原点座標を更新
            nowDirection = direction; //現在のレーザーの方向を更新
            lazers_Line = new LineRenderer[max_Reflect + 1];//LineRendererを最大反射回数＋１本用意して格納しておく
            preWall = null;
        }
        // 反射回数１以上からはここで生成
        if (n < max_Reflect + 1)
        {
            GameObject lazerChild = Instantiate(lazerPrefab, this.transform); //レーザーオブジェクトを生成
            lazerChild.name = "lazer_" + n;
            LineRenderer line = lazerChild.GetComponent<LineRenderer>();
            lazers_Line[n] = line; //LinRendererのN番目を新たに生成されたLazerに更新
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

        //全部のレーザー辺に対して始点と終点を更新し、LineRendererを1つずつ更新していく
        foreach (LineRenderer line in lazers_Line)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, nextDirection, max_Length);

            foreach (RaycastHit2D hit in hits)
            {
                //同じ方向の壁に連続で当たらないようにするため、preWallを除外
                if (hit.collider.gameObject != preWall && hit.collider.gameObject.tag == "Wall")
                {
                    Vector3 endPos = hit.point;
                    Vector2 reflectDirection = Vector2.Reflect(nextDirection, hit.normal);
                    line.SetPosition(0, startPos);
                    line.SetPosition(1, endPos);
                    preWall = hit.collider.gameObject;
                    startPos = endPos; //終点の座標を次のレーザーオブジェの始点座標へ反映
                    nextDirection = reflectDirection; //反射ベクトルを次のレーザーオブジェの方向へ反映
                    break;
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