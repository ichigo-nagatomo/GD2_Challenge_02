using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected Ray ray;
    protected RaycastHit rayCastHit;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected Bullet bullets;

    protected Vector3 shootPoint;   // �ˌ������ꏊ
    protected Vector3 hitPoint;     // �q�b�g�����ꏊ��ۑ�

    // �ˌ��̃N�[���_�E������
    [SerializeField] protected float fireRate = 0.5f; // ���˃��[�g�i�b�j
    protected float nextFireTime = 0f; // ���ɔ��ˉ\�Ȏ���

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }

    public virtual void Shoot() {
        if (Time.time >= nextFireTime) {
            if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out rayCastHit)) {
                BulletHit();
                shootPoint = muzzle.transform.position;
                hitPoint = rayCastHit.point;
                Bullet bullet = Instantiate(bullets, shootPoint, Quaternion.identity);
                bullet.SetPoints(shootPoint, hitPoint);

                // ���̔��ˉ\�Ȏ��Ԃ��X�V
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    protected void BulletHit() {
        Debug.Log(rayCastHit.collider.gameObject.name + " hit");
        if (rayCastHit.collider.gameObject.tag == "Button") {
            Destroy(rayCastHit.collider.gameObject);
        }

        if (rayCastHit.collider.gameObject.tag == "Enemy") {
            Destroy(rayCastHit.collider.gameObject);
        }

        if (rayCastHit.collider.gameObject.tag == "Bomb") {
            Bomb bomb = rayCastHit.collider.GetComponent<Bomb>();
            if (bomb != null) {
                bomb.OnExplosion(); // Bomb��OnExplosion���\�b�h���Ăяo��
            }
        }
    }

    public void GunDir() {
        // �e�����}�E�X�̕��Ɍ���
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayCastHit)) {
            transform.LookAt(new Vector3(rayCastHit.point.x, transform.position.y, rayCastHit.point.z));
        }
    }
}
