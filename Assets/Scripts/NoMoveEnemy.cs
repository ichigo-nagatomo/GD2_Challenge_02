using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class NoMoveEnemy : MonoBehaviour {
    private int life = 3; // �G�̃��C�t
    public Transform player; // �v���C���[�̈ʒu
    public float viewDistance = 10f; // �G�̎��E�̋���
    public float viewAngle = 45f; // �G�̎��E�p�x

    private bool discoveryPlayer; // �v���C���[�����t���O
    private float shootTime; // �ˌ��܂ł̎���
    private int bulletsShot; // �������e�̐�
    private bool lookingAround; // ���񂵒��t���O
    private Quaternion currentRotation; // ���݂̉�]

    [SerializeField] private float headTurnSpeed = 45f; // ���U�鑬�x
    [SerializeField] private float maxHeadTurnAngle = 45f; // �ő�U��p�x
    [SerializeField] private float maxLookAroundTime = 2f; // �ő匩�񂵎���
    [SerializeField] private int maxBullets = 5; // �ő�e��
    [SerializeField] private float shootInterval = 0.3f; // �ˌ��Ԋu
    [SerializeField] private float reloadTime = 2f; // �����[�h����
    [SerializeField] private EnemyBullet enemyGun; // �e

    private Vector3 lastSeenPlayerPosition; // �Ō�Ɍ����v���C���[�̈ʒu
    private bool reloading; // �����[�h���t���O

    private float headTurnTimer; // ���U��^�C�}�[
    private bool turningLeft; // ���ɐU���Ă��邩�ǂ����̃t���O

    void Start() {
        ResetState();
    }

    void Update() {
        if (!lookingAround) {
            CheckPlayerInSight();
        }

        if (discoveryPlayer) {
            HandleShooting(); // �ˌ�����
        } else if (bulletsShot > 0) {
            StartHeadTurn(); // ���������ꍇ�Ɏ��U�铮����J�n
        }

        if (lookingAround) {
            HeadTurnBehaviour(); // ���U�铮��
        }

        if (life <= 0) {
            Destroy(gameObject); // �G�����񂾏ꍇ
        }
    }

    // �G�̏�Ԃ����Z�b�g
    private void ResetState() {
        discoveryPlayer = false;
        shootTime = 0f;
        bulletsShot = 0;
        lookingAround = false;
        reloading = false;
        turningLeft = true; // �����͍��ɐU��
    }

    // �v���C���[�����E���ɂ��邩�m�F
    private void CheckPlayerInSight() {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (angleToPlayer < viewAngle / 2 && distanceToPlayer < viewDistance) {
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, viewDistance)) {
                if (hit.collider.CompareTag("Player")) {
                    discoveryPlayer = true;
                    lastSeenPlayerPosition = player.position; // �Ō�Ɍ����ʒu��ۑ�
                    lookingAround = false; // �v���C���[�𔭌������猩�񂵂��I��
                }
            }
        } else {
            discoveryPlayer = false; // �v���C���[����������
        }
    }

    // �ˌ�����
    private void HandleShooting() {
        if (reloading)
            return; // �����[�h���Ȃ牽�����Ȃ�

        // �v���C���[�̍Ō�Ɍ����ꏊ�Ɍ�����
        transform.LookAt(new Vector3(lastSeenPlayerPosition.x, transform.position.y, lastSeenPlayerPosition.z));

        shootTime -= Time.deltaTime;

        if (shootTime <= 0f && bulletsShot < maxBullets) {
            enemyGun.Shoot();
            shootTime = shootInterval; // ���̔��˂܂ł̑҂�����
            bulletsShot++;
        }

        // �S�e�������I������烊���[�h�J�n
        if (bulletsShot >= maxBullets) {
            StartCoroutine(Reload());
        }
    }

    // �����[�h����
    private IEnumerator Reload() {
        reloading = true; // �����[�h�t���O�𗧂Ă�
        yield return new WaitForSeconds(reloadTime); // �����[�h���Ԃ�҂�
        bulletsShot = 0; // �ˌ��񐔃��Z�b�g
        reloading = false; // �����[�h����
    }

    // ���U�铮����J�n
    private void StartHeadTurn() {
        if (!lookingAround) { // ���łɎ��U���Ă��Ȃ��ꍇ
            lookingAround = true;
            currentRotation = transform.localRotation; // ���݂̉�]�����񂵊�ɐݒ�
            headTurnTimer = maxLookAroundTime; // ���U��^�C�}�[�̏�����
            turningLeft = true; // �ŏ��͍��ɐU��
        }
    }

    private void HeadTurnBehaviour() {
        headTurnTimer -= Time.deltaTime; // ���U��^�C�}�[������

        if (headTurnTimer > 0f) {
            Debug.Log(headTurnTimer);
            TurnHead(); // ���U��
        } else {
            lookingAround = false; // ���񂵏I��
            transform.localRotation = currentRotation; // �Ō�̎�̐U����]�ɖ߂�
            turningLeft = true; // �U����������Z�b�g
            bulletsShot = 0;
        }
    }

    // ������E�ɐU�铮��
    private void TurnHead() {
        float targetAngle = headTurnSpeed * Time.deltaTime;

        if (turningLeft) {
            transform.localRotation *= Quaternion.Euler(0, -targetAngle, 0); // ���ɐU��
        } else {
            transform.localRotation *= Quaternion.Euler(0, targetAngle, 0); // �E�ɐU��
        }

        // �ő�p�x�ɒB������U������𔽓]
        if (Mathf.Abs(transform.localEulerAngles.y - currentRotation.eulerAngles.y) >= maxHeadTurnAngle) {
            turningLeft = !turningLeft; // �����𔽓]
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
