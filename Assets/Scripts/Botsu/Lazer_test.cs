using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lazer_test : MonoBehaviour
{
    [SerializeField] private GameObject lazerPrefab;
    const float MAX_DISTANCE = 23.0f;  //箱の対角線の長さ（レーザーの最大長を設定）
    const int REFLECT_NUM = 3;  //反射回数
    private GameObject preWall;

    /**
     * レーザー生成関数
     * @param {Vector3} origin - レーザーの原点
     * @param {Vector3} direction - レーザーの方向
     * @param {int} n - 何本目か(0はじまり)
    **/
    public void creat(Vector3 origin, Vector2 direction, int n)
    {
        if (n == 0) preWall = null;
        if (n < REFLECT_NUM + 1)
        {
            GameObject lazerChild = Instantiate(lazerPrefab, this.transform);
            lazerChild.name = "lazer_" + n;
            LineRenderer line = lazerChild.GetComponent<LineRenderer>();
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, MAX_DISTANCE);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject != preWall)
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
