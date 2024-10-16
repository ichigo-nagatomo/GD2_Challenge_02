using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float moveSpeed;
    private Ray ray;
    private RaycastHit rayCastHit;

    private bool isHaveGun;
    [SerializeField] private PlayerBullet gun;
    [SerializeField] private GameObject hand;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 4f;
        isHaveGun = false;
    }

    // Update is called once per frame
    void Update()
    {
        //ˆÚ“®ˆ—
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

        //ËŒ‚ˆ—
        if(Input.GetMouseButtonDown(0)) {
            gun.Shoot();
        }

        //ƒ}ƒEƒX‚Ì•ûŒü‚ÉŒü‚­ˆ—
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayCastHit)) {
            transform.LookAt(new Vector3(rayCastHit.point.x, transform.position.y, rayCastHit.point.z));
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Gun") {
            isHaveGun = true;
            gun.transform.parent = hand.transform;
            gun.transform.position = hand.transform.position;
        }
    }

    public bool GetIsHaveGun() {
        return isHaveGun;
    }
}
