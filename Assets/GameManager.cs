using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    GameObject player;

    public Transform spawnPoint;
    public CinemachineVirtualCamera spectatorCam;

    private void Start() {
        player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    }
    private void Update() {
        if(player==null){
            spectatorCam.gameObject.SetActive(true);
            if(Input.GetButtonDown("Fire1")){
                spectatorCam.gameObject.SetActive(false);
                Spawn();
            }
        }
    }

    void Spawn(){
        player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    }
}
