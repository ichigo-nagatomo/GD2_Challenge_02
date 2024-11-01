using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class NoMoveEnemy : MonoBehaviour {
    private int life = 3; // 敵のライフ
    public Transform player; // プレイヤーの位置
    public float viewDistance = 10f; // 敵の視界の距離
    public float viewAngle = 45f; // 敵の視界角度

    private bool discoveryPlayer; // プレイヤー発見フラグ
    private float shootTime; // 射撃までの時間
    private int bulletsShot; // 撃った弾の数
    private bool lookingAround; // 見回し中フラグ
    private Quaternion currentRotation; // 現在の回転

    [SerializeField] private float headTurnSpeed = 45f; // 首を振る速度
    [SerializeField] private float maxHeadTurnAngle = 45f; // 最大振り角度
    [SerializeField] private float maxLookAroundTime = 2f; // 最大見回し時間
    [SerializeField] private int maxBullets = 5; // 最大弾数
    [SerializeField] private float shootInterval = 0.3f; // 射撃間隔
    [SerializeField] private float reloadTime = 2f; // リロード時間
    [SerializeField] private EnemyBullet enemyGun; // 弾

    private Vector3 lastSeenPlayerPosition; // 最後に見たプレイヤーの位置
    private bool reloading; // リロード中フラグ

    private float headTurnTimer; // 首を振るタイマー
    private bool turningLeft; // 左に振っているかどうかのフラグ

    void Start() {
        ResetState();
    }

    void Update() {
        if (!lookingAround) {
            CheckPlayerInSight();
        }

        if (discoveryPlayer) {
            HandleShooting(); // 射撃処理
        } else if (bulletsShot > 0) {
            StartHeadTurn(); // 見失った場合に首を振る動作を開始
        }

        if (lookingAround) {
            HeadTurnBehaviour(); // 首を振る動作
        }

        if (life <= 0) {
            Destroy(gameObject); // 敵が死んだ場合
        }
    }

    // 敵の状態をリセット
    private void ResetState() {
        discoveryPlayer = false;
        shootTime = 0f;
        bulletsShot = 0;
        lookingAround = false;
        reloading = false;
        turningLeft = true; // 初期は左に振る
    }

    // プレイヤーが視界内にいるか確認
    private void CheckPlayerInSight() {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (angleToPlayer < viewAngle / 2 && distanceToPlayer < viewDistance) {
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, viewDistance)) {
                if (hit.collider.CompareTag("Player")) {
                    discoveryPlayer = true;
                    lastSeenPlayerPosition = player.position; // 最後に見た位置を保存
                    lookingAround = false; // プレイヤーを発見したら見回しを終了
                }
            }
        } else {
            discoveryPlayer = false; // プレイヤーを見失った
        }
    }

    // 射撃処理
    private void HandleShooting() {
        if (reloading)
            return; // リロード中なら何もしない

        // プレイヤーの最後に見た場所に向かう
        transform.LookAt(new Vector3(lastSeenPlayerPosition.x, transform.position.y, lastSeenPlayerPosition.z));

        shootTime -= Time.deltaTime;

        if (shootTime <= 0f && bulletsShot < maxBullets) {
            enemyGun.Shoot();
            shootTime = shootInterval; // 次の発射までの待ち時間
            bulletsShot++;
        }

        // 全弾を撃ち終わったらリロード開始
        if (bulletsShot >= maxBullets) {
            StartCoroutine(Reload());
        }
    }

    // リロード処理
    private IEnumerator Reload() {
        reloading = true; // リロードフラグを立てる
        yield return new WaitForSeconds(reloadTime); // リロード時間を待つ
        bulletsShot = 0; // 射撃回数リセット
        reloading = false; // リロード完了
    }

    // 首を振る動作を開始
    private void StartHeadTurn() {
        if (!lookingAround) { // すでに首を振っていない場合
            lookingAround = true;
            currentRotation = transform.localRotation; // 現在の回転を見回し基準に設定
            headTurnTimer = maxLookAroundTime; // 首を振るタイマーの初期化
            turningLeft = true; // 最初は左に振る
        }
    }

    private void HeadTurnBehaviour() {
        headTurnTimer -= Time.deltaTime; // 首を振るタイマーを減少

        if (headTurnTimer > 0f) {
            Debug.Log(headTurnTimer);
            TurnHead(); // 首を振る
        } else {
            lookingAround = false; // 見回し終了
            transform.localRotation = currentRotation; // 最後の首の振り基準回転に戻す
            turningLeft = true; // 振り方向をリセット
            bulletsShot = 0;
        }
    }

    // 首を左右に振る動作
    private void TurnHead() {
        float targetAngle = headTurnSpeed * Time.deltaTime;

        if (turningLeft) {
            transform.localRotation *= Quaternion.Euler(0, -targetAngle, 0); // 左に振る
        } else {
            transform.localRotation *= Quaternion.Euler(0, targetAngle, 0); // 右に振る
        }

        // 最大角度に達したら振り方向を反転
        if (Mathf.Abs(transform.localEulerAngles.y - currentRotation.eulerAngles.y) >= maxHeadTurnAngle) {
            turningLeft = !turningLeft; // 方向を反転
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward * viewDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward * viewDistance;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }

    public void Damage() {
        life--;
    }
}
