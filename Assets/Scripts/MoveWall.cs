using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour {
    [SerializeField] private List<GameObject> triggers = new List<GameObject>();
    private bool allTriggersDestroyed;
    // Start is called before the first frame update
    void Start() {
        allTriggersDestroyed = true;
    }

    // Update is called once per frame
    void Update() {
        allTriggersDestroyed = true;

        foreach (var trigger in triggers) {
            if (trigger != null) {
                allTriggersDestroyed = false;
                break;
            }
        }

        // 全てのボタンが無効であれば、壁を動かす
        if (allTriggersDestroyed) {
            transform.position -= new Vector3(0, 3f * Time.deltaTime, 0);
        }

        if (transform.position.y <= -5f) {
            Destroy(gameObject);
        }
    }
}
