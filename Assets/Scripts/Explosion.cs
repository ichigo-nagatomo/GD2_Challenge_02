using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    // �����̊g�呬�x
    private float expansionRate;
    // �ő�X�P�[��
    private float maxScale;

    void Start() {
        expansionRate = 40.0f;
        maxScale = 10.0f;
    }

    // Update is called once per frame
    void Update() {
        // �X�P�[�����g�傷��
        transform.localScale += Vector3.one * expansionRate * Time.deltaTime;

        // ���̃X�P�[���𒴂�����I�u�W�F�N�g��j�󂷂�
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

    // ��Q�����`�F�b�N���ēG��j�󂷂�
    private void CheckForObstaclesAndDestroy(Collider other, string tag) {
        Vector3 directionToTarget = other.transform.position - transform.position;
        Ray ray = new Ray(transform.position, directionToTarget);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, directionToTarget.magnitude)) {
            if (hit.collider.gameObject != other.gameObject) {
                Debug.Log($"{tag}�ɑ΂���Raycast����Q���Ƀq�b�g���܂���: {hit.collider.gameObject.name}");
                return;
            }
        }

        Debug.Log($"{tag}�ɑ΂���Raycast���������܂���: {other.gameObject.name}");
        Destroy(other.gameObject);
    }

    // ��Q�����`�F�b�N���Ĕ��e�𔚔j����
    private void CheckForObstaclesAndTriggerBomb(Collider other) {
        Vector3 directionToBomb = other.transform.position - transform.position;
        Ray ray = new Ray(transform.position, directionToBomb);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, directionToBomb.magnitude)) {
            if (hit.collider.gameObject != other.gameObject) {
                Debug.Log($"Bomb�ɑ΂���Raycast����Q���Ƀq�b�g���܂���: {hit.collider.gameObject.name}");
                return;
            }
        }

        Debug.Log($"Bomb�ɑ΂���Raycast���������܂���: {other.gameObject.name}");
        Bomb bomb = other.GetComponent<Bomb>();
        if (bomb != null) {
            bomb.OnExplosion();
        } else {
            Debug.LogWarning("Bomb�R���|�[�l���g��������܂���ł����B");
        }
    }
}
