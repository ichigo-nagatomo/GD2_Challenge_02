using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Vector3 endPoint;
    [SerializeField] private LineRenderer lineRenderer;  // LineRendererの参照を追加

    private float speed = 50f;  // 弾の速度
    private float lifetime = 1f; // 弾の寿命（秒数）

    // Start is called before the first frame update
    void Start() {
        // 初期状態でLineRendererのポイントをセット
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint); // 開始位置で初期化

        // 指定された寿命が過ぎたら弾を破壊
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update() {
        // 弾の方向を取得
        Vector3 direction = (endPoint - transform.position).normalized;

        // 弾を移動させる
        transform.position += direction * speed * Time.deltaTime;

        // LineRendererのエンドポイントを更新
        lineRenderer.SetPosition(0, transform.position);

        // 目標地点に到達したらオブジェクトを破壊
        if (Vector3.Distance(transform.position, endPoint) < 0.1f) {
            Destroy(gameObject);
        }
    }

    public void SetPoints(Vector3 start, Vector3 end) {
        transform.position = start;
        endPoint = end;
    }
}
