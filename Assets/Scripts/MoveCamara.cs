using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {
    private Ray ray;
    private RaycastHit rayCastHit;
    [SerializeField] private Player player;
    [SerializeField] private Vector3 cameraDistance;
    [SerializeField] private float cameraMoveSpeed = 2.0f; // �J�����̈ړ����x
    [SerializeField] private float maxDistance = 10.0f; // �ő�ړ������̐���

    // Start is called before the first frame update
    void Start() {
        cameraDistance = new Vector3(0, 13, -11);
    }

    // Update is called once per frame
    void Update() {
        // �v���C���[�̈ʒu�ɃJ������z�u
        Vector3 targetPosition = player.transform.position + cameraDistance;

        // �}�E�X�ʒu�Ɍ�������Ray�𓊎�
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out rayCastHit)) {
            // �}�E�X�ʒu�̃��[���h���W���擾
            Vector3 mouseWorldPosition = rayCastHit.point;

            // �v���C���[�ƃ}�E�X�̊Ԃ̕������v�Z
            Vector3 directionToMouse = mouseWorldPosition - player.transform.position;

            // �ړ������𐧌�����
            float distanceToMouse = directionToMouse.magnitude;
            float clampedDistance = Mathf.Min(distanceToMouse, maxDistance);

            // �������������Ɋ�Â��ĕ����𒲐�
            Vector3 limitedDirection = directionToMouse.normalized * clampedDistance;

            // �J�����ʒu�𒲐�
            Vector3 adjustedPosition = player.transform.position + cameraDistance + limitedDirection * 0.2f;

            // �J�������X���[�Y�Ɉړ�
            transform.position = Vector3.Lerp(transform.position, adjustedPosition, cameraMoveSpeed * Time.deltaTime);
        } else {
            // �}�E�X���ΏۂɃq�b�g���Ă��Ȃ��ꍇ�A�v���C���[��ŃJ������z�u
            transform.position = Vector3.Lerp(transform.position, targetPosition, cameraMoveSpeed * Time.deltaTime);
        }
    }
}
