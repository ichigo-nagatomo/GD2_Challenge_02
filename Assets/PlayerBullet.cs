using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {
    private Ray ray;
    private RaycastHit rayCastHit;
    [SerializeField] private Player player;
    [SerializeField] private Transform muzzle;
    [SerializeField] private LineRenderer lineRenderer;  // LineRenderer�̎Q�Ƃ�ǉ�
    [SerializeField] private float lineDisplayTime = 0.1f;  // �e����\�����鎞��

    // Start is called before the first frame update
    void Start() {
        lineRenderer.positionCount = 2;  // LineRenderer��2�̓_������
        lineRenderer.enabled = false;    // �f�t�H���g�Ŕ�\��
    }

    // Update is called once per frame
    void Update() {
        // player��Gun�������Ă�����
        if (player.GetIsHaveGun()) {
            // �e�����}�E�X�̕��Ɍ���
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayCastHit)) {
                transform.LookAt(new Vector3(rayCastHit.point.x, transform.position.y, rayCastHit.point.z));
            }
        }
    }

    public void Shoot() {
        // player��Gun�������Ă����猂��
        if (player.GetIsHaveGun()) {
            if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out rayCastHit)) {
                BulletHit();
                DrawBulletPath(rayCastHit.point);  // �e����`��
            }
        }
    }

    // �e����`�悷��֐�
    void DrawBulletPath(Vector3 hitPoint) {
        lineRenderer.SetPosition(0, muzzle.position);  // �e���̈ʒu
        lineRenderer.SetPosition(1, hitPoint);         // �q�b�g�����ꏊ

        lineRenderer.enabled = true;  // �e����\��

        // ��莞�Ԍ�ɒe�����\���ɂ���
        StartCoroutine(DisableLineRendererAfterTime(lineDisplayTime));
    }

    // ��莞�Ԍ��LineRenderer�𖳌��ɂ���R���[�`��
    IEnumerator DisableLineRendererAfterTime(float time) {
        yield return new WaitForSeconds(time);
        lineRenderer.enabled = false;  // �e�����\��
    }

    void BulletHit() {
        Debug.Log(rayCastHit.collider.gameObject.name + " hit");
    }
}
