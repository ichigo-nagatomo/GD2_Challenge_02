using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour{
    private Ray ray;
    private RaycastHit rayCastHit;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Bullet bullets;

    private Vector3 shootPoint;   // éÀåÇÇµÇΩèÍèä
    private Vector3 hitPoint;     // ÉqÉbÉgÇµÇΩèÍèäÇï€ë∂

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Shoot() {
        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out rayCastHit)) {
            BulletHit();
            shootPoint = muzzle.transform.position;
            hitPoint = rayCastHit.point;
            Bullet bullet = Instantiate(bullets, shootPoint, Quaternion.identity);
            bullet.SetPoints(hitPoint, shootPoint);
        }
    }

    void BulletHit() {
        if (rayCastHit.collider.gameObject.tag == "Button") {
            Destroy(rayCastHit.collider.gameObject);
        }
    }
}
