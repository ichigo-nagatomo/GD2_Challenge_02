using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {
    private Ray ray;
    private RaycastHit rayCastHit;
    [SerializeField] private Player player;
    [SerializeField] private Transform muzzle;
    [SerializeField] private LineRenderer lineRenderer;  // LineRendererの参照を追加
    [SerializeField] private float lineDisplayTime = 0.1f;  // 弾道を表示する時間

    // Start is called before the first frame update
    void Start() {
        lineRenderer.positionCount = 2;  // LineRendererは2つの点を持つ
        lineRenderer.enabled = false;    // デフォルトで非表示
    }

    // Update is called once per frame
    void Update() {
        // playerがGunを持っていたら
        if (player.GetIsHaveGun()) {
            // 銃口がマウスの方に向く
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayCastHit)) {
                transform.LookAt(new Vector3(rayCastHit.point.x, transform.position.y, rayCastHit.point.z));
            }
        }
    }

    public void Shoot() {
        // playerがGunを持っていたら撃つ
        if (player.GetIsHaveGun()) {
            if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out rayCastHit)) {
                BulletHit();
                DrawBulletPath(rayCastHit.point);  // 弾道を描画
            }
        }
    }

    // 弾道を描画する関数
    void DrawBulletPath(Vector3 hitPoint) {
        lineRenderer.SetPosition(0, muzzle.position);  // 銃口の位置
        lineRenderer.SetPosition(1, hitPoint);         // ヒットした場所

        lineRenderer.enabled = true;  // 弾道を表示

        // 一定時間後に弾道を非表示にする
        StartCoroutine(DisableLineRendererAfterTime(lineDisplayTime));
    }

    // 一定時間後にLineRendererを無効にするコルーチン
    IEnumerator DisableLineRendererAfterTime(float time) {
        yield return new WaitForSeconds(time);
        lineRenderer.enabled = false;  // 弾道を非表示
    }

    void BulletHit() {
        Debug.Log(rayCastHit.collider.gameObject.name + " hit");
    }
}
