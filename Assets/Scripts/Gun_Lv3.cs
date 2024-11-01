using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Lv3 : Gun{
    [SerializeField] private GrenadeBullet GrenadeBulletPrefab;
    // 射撃のクールダウン時間
    [SerializeField] private float fireGrenadeRate = 0.5f; // 発射レート（秒）
    private float nextFireGrenadeTime = 0f; // 次に発射可能な時間

    public override void Shoot() {
        if (Time.time >= nextFireTime) {
            if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out rayCastHit)) {
                BulletHit();
                shootPoint = muzzle.transform.position;
                hitPoint = rayCastHit.point;
                Bullet bullet = Instantiate(bullets, shootPoint, Quaternion.identity);
                bullet.SetPoints(shootPoint, hitPoint);

                // 次の発射可能な時間を更新
                nextFireTime = Time.time + fireRate;
            }
        }

        //グレネードの処理
        if (Time.time >= nextFireGrenadeTime) {
            shootPoint = muzzle.transform.position;
            GrenadeBullet grenadeBullet = Instantiate(GrenadeBulletPrefab, shootPoint, transform.rotation);

            // 次の発射可能な時間を更新
            nextFireGrenadeTime = Time.time + fireGrenadeRate;
        }
    }
}
