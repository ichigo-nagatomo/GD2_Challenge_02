using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Lv3 : Gun{
    [SerializeField] private GrenadeBullet GrenadeBulletPrefab;
    // �ˌ��̃N�[���_�E������
    [SerializeField] private float fireGrenadeRate = 0.5f; // ���˃��[�g�i�b�j
    private float nextFireGrenadeTime = 0f; // ���ɔ��ˉ\�Ȏ���

    public override void Shoot() {
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

        //�O���l�[�h�̏���
        if (Time.time >= nextFireGrenadeTime) {
            shootPoint = muzzle.transform.position;
            GrenadeBullet grenadeBullet = Instantiate(GrenadeBulletPrefab, shootPoint, transform.rotation);

            // ���̔��ˉ\�Ȏ��Ԃ��X�V
            nextFireGrenadeTime = Time.time + fireGrenadeRate;
        }
    }
}
