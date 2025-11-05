using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lazer : MonoBehaviour
{
    [Header("LazerのPrefabをここに入れる")]
    [SerializeField] private GameObject lazerPrefab;
    [Header("レーザーの最大長")]
    public const float max_Length = 23.0f;  //レーザーの最大長

    [Header("反射回数")]
    public const int max_Reflect = 3;  //反射回数
    private GameObject preWall; //同じ方向の壁に連続で当たらないようにするため

    /// <summary>
    /// レーザー生成関数
    /// </summary>
    /// <param name="origin">レーザーの原点座標</param>
    /// <param name="direction">レーザーの方向</param>
    /// <param name="n">今何本目か(0ははじまり)</param>
    public void creat(Vector3 origin, Vector2 direction, int n) //
    {
        if (n == 0) preWall = null;
        if (n < max_Reflect + 1)
        {
            GameObject lazerChild = Instantiate(lazerPrefab, this.transform);
            lazerChild.name = "lazer_" + n;
            LineRenderer line = lazerChild.GetComponent<LineRenderer>();
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
                    preWall = hit.collider.gameObject;

                    creat(endPos, reflectDirection, ++n);

                    break;
                }
            }
        }
    }
}