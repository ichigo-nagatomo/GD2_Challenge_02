using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    // 爆発の拡大速度
    private float expansionRate;
    // 最大スケール
    private float maxScale;

    void Start() {
        expansionRate = 40.0f;
        maxScale = 10.0f;
    }

    // Update is called once per frame
    void Update() {
        // スケールを拡大する
        transform.localScale += Vector3.one * expansionRate * Time.deltaTime;

        // 一定のスケールを超えたらオブジェクトを破壊する
        if (transform.localScale.x >= maxScale) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy") {
            CheckForObstaclesAndDestroy(other, "Enemy");
        }

        if (other.gameObject.tag == "Bomb") {
            CheckForObstaclesAndTriggerBomb(other);
        }
    }

    // 障害物をチェックして敵を破壊する
    private void CheckForObstaclesAndDestroy(Collider other, string tag) {
        Vector3 directionToTarget = other.transform.position - transform.position;
        Ray ray = new Ray(transform.position, directionToTarget);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, directionToTarget.magnitude)) {
            if (hit.collider.gameObject != other.gameObject) {
                Debug.Log($"{tag}に対するRaycastが障害物にヒットしました: {hit.collider.gameObject.name}");
                return;
            }
        }

        Debug.Log($"{tag}に対するRaycastが成功しました: {other.gameObject.name}");
        Destroy(other.gameObject);
    }

    // 障害物をチェックして爆弾を爆破する
    private void CheckForObstaclesAndTriggerBomb(Collider other) {
        Vector3 directionToBomb = other.transform.position - transform.position;
        Ray ray = new Ray(transform.position, directionToBomb);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, directionToBomb.magnitude)) {
            if (hit.collider.gameObject != other.gameObject) {
                Debug.Log($"Bombに対するRaycastが障害物にヒットしました: {hit.collider.gameObject.name}");
                return;
            }
        }

        Debug.Log($"Bombに対するRaycastが成功しました: {other.gameObject.name}");
        Bomb bomb = other.GetComponent<Bomb>();
        if (bomb != null) {
            bomb.OnExplosion();
        } else {
            Debug.LogWarning("Bombコンポーネントが見つかりませんでした。");
        }
    }
}
