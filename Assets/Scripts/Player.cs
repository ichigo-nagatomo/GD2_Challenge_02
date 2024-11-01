using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private float moveSpeed;
    private Ray ray;
    private RaycastHit rayCastHit;

    [SerializeField] private GameObject hand;
    private Gun gun; // Gun�̐e�N���X���Q��

    void Start() {
        moveSpeed = 4f;
    }

    void Update() {
        // �ړ�����
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

        // �ˌ�����
        if (Input.GetMouseButton(0) && gun != null) {
            gun.Shoot(); // ���N���b�N�Ŏˌ�
        }

        // �}�E�X�̕����Ɍ�������
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayCastHit)) {
            transform.LookAt(new Vector3(rayCastHit.point.x, transform.position.y, rayCastHit.point.z));
        }
    }

    private void OnTriggerEnter(Collider other) {
        // �G�ꂽ�I�u�W�F�N�g���e�ł��邩���m�F
        if (other.gameObject.tag == "Gun") {
            Debug.Log("�e�ɓ��������I�u�W�F�N�g: " + other.gameObject.name);

            gun = other.gameObject.GetComponent<Gun>();
            if (gun != null) {
                gun.GunDir();
                AttachGunToHand(other.gameObject);
                Debug.Log("�e������Ɏ擾����܂���: " + gun.GetType().Name);
            } else {
                Debug.Log("Gun �R���|�[�l���g��������܂���");
            }
        }
    }


    // �e����ɕt���鏈�������ʉ�
    private void AttachGunToHand(GameObject gunToAttach) {
        gunToAttach.transform.parent = hand.transform;
        gunToAttach.transform.position = hand.transform.position;
        gunToAttach.transform.rotation = hand.transform.rotation;
    }
}
