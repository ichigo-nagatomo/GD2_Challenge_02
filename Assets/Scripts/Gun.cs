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

    protected Vector3 shootPoint;   // 射撃した場所
    protected Vector3 hitPoint;     // ヒットした場所を保存

    // 射撃のクールダウン時間
    [SerializeField] protected float fireRate = 0.5f; // 発射レート（秒）
    protected float nextFireTime = 0f; // 次に発射可能な時間

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

                // 次の発射可能な時間を更新
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
                bomb.OnExplosion(); // BombのOnExplosionメソッドを呼び出す
            }
        }
    }

    public void GunDir() {
        // 銃口がマウスの方に向く
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayCastHit)) {
            transform.LookAt(new Vector3(rayCastHit.point.x, transform.position.y, rayCastHit.point.z));
        }
    }
}
