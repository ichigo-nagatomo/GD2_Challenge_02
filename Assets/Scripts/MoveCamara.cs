using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {
    private Ray ray;
    private RaycastHit rayCastHit;
    [SerializeField] private Player player;
    [SerializeField] private Vector3 cameraDistance;
    [SerializeField] private float cameraMoveSpeed = 2.0f; // カメラの移動速度
    [SerializeField] private float maxDistance = 10.0f; // 最大移動距離の制限

    // Start is called before the first frame update
    void Start() {
        cameraDistance = new Vector3(0, 13, -11);
    }

    // Update is called once per frame
    void Update() {
        // プレイヤーの位置にカメラを配置
        Vector3 targetPosition = player.transform.position + cameraDistance;

        // マウス位置に向かってRayを投射
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out rayCastHit)) {
            // マウス位置のワールド座標を取得
            Vector3 mouseWorldPosition = rayCastHit.point;

            // プレイヤーとマウスの間の方向を計算
            Vector3 directionToMouse = mouseWorldPosition - player.transform.position;

            // 移動距離を制限する
            float distanceToMouse = directionToMouse.magnitude;
            float clampedDistance = Mathf.Min(distanceToMouse, maxDistance);

            // 制限した距離に基づいて方向を調整
            Vector3 limitedDirection = directionToMouse.normalized * clampedDistance;

            // カメラ位置を調整
            Vector3 adjustedPosition = player.transform.position + cameraDistance + limitedDirection * 0.2f;

            // カメラをスムーズに移動
            transform.position = Vector3.Lerp(transform.position, adjustedPosition, cameraMoveSpeed * Time.deltaTime);
        } else {
            // マウスが対象にヒットしていない場合、プレイヤー基準でカメラを配置
            transform.position = Vector3.Lerp(transform.position, targetPosition, cameraMoveSpeed * Time.deltaTime);
        }
    }
}
