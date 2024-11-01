using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform clearLine;
    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private Text result;
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 720, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z >= clearLine.transform.position.z) {
            player.transform.position += new Vector3(0, 0, 3f * Time.deltaTime);
            fadeManager.Out = true;

            if(fadeManager.alfa >= 1) {
                result.enabled = true;

                if(Input.GetKeyDown(KeyCode.Space)) {
                    SceneManager.LoadScene(0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0);
        }
    }
}
