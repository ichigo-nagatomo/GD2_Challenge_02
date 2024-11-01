using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Vector3 endPoint;
    [SerializeField] private LineRenderer lineRenderer;  // LineRenderer�̎Q�Ƃ�ǉ�

    private float speed = 50f;  // �e�̑��x
    private float lifetime = 1f; // �e�̎����i�b���j

    // Start is called before the first frame update
    void Start() {
        // ������Ԃ�LineRenderer�̃|�C���g���Z�b�g
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint); // �J�n�ʒu�ŏ�����

        // �w�肳�ꂽ�������߂�����e��j��
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update() {
        // �e�̕������擾
        Vector3 direction = (endPoint - transform.position).normalized;

        // �e���ړ�������
        transform.position += direction * speed * Time.deltaTime;

        // LineRenderer�̃G���h�|�C���g���X�V
        lineRenderer.SetPosition(0, transform.position);

        // �ڕW�n�_�ɓ��B������I�u�W�F�N�g��j��
        if (Vector3.Distance(transform.position, endPoint) < 0.1f) {
            Destroy(gameObject);
        }
    }

    public void SetPoints(Vector3 start, Vector3 end) {
        transform.position = start;
        endPoint = end;
    }
}
