using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private float moveSpeed;
    private Ray ray;
    private RaycastHit rayCastHit;

    [SerializeField] private GameObject hand;
    private Gun gun; // Gunの親クラスを参照

    void Start() {
        moveSpeed = 4f;
    }

    void Update() {
        // 移動処理
        if (Input.GetKey(KeyCode.W)) {
            transform.position += new Vector3(0, 0, moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position += new Vector3(0, 0, -moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
        }

        // 射撃処理
        if (Input.GetMouseButton(0) && gun != null) {
            gun.Shoot(); // 左クリックで射撃
        }

        // マウスの方向に向く処理
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayCastHit)) {
            transform.LookAt(new Vector3(rayCastHit.point.x, transform.position.y, rayCastHit.point.z));
        }
    }

    private void OnTriggerEnter(Collider other) {
        // 触れたオブジェクトが銃であるかを確認
        if (other.gameObject.tag == "Gun") {
            Debug.Log("銃に当たったオブジェクト: " + other.gameObject.name);

            gun = other.gameObject.GetComponent<Gun>();
            if (gun != null) {
                gun.GunDir();
                AttachGunToHand(other.gameObject);
                Debug.Log("銃が正常に取得されました: " + gun.GetType().Name);
            } else {
                Debug.Log("Gun コンポーネントが見つかりません");
            }
        }
    }


    // 銃を手に付ける処理を共通化
    private void AttachGunToHand(GameObject gunToAttach) {
        gunToAttach.transform.parent = hand.transform;
        gunToAttach.transform.position = hand.transform.position;
        gunToAttach.transform.rotation = hand.transform.rotation;
    }
}
