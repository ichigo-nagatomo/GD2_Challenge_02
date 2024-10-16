using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamara : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Vector3 cameraDistance;
    // Start is called before the first frame update
    void Start()
    {
        cameraDistance = new Vector3(0, 13, -11);
    }

    // Update is called once per frame
    void Update()
    {
        //ƒJƒƒ‰‹——£‚Ì’²®
        transform.position = player.transform.position + cameraDistance;
    }
}
