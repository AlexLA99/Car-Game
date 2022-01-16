using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Manager : MonoBehaviour
{
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1)
        {
            PhotonNetwork.Instantiate("PhotonPrefabs/GameManager", Vector3.zero, Quaternion.identity);
            PhotonNetwork.Instantiate("PhotonPrefabs/GameManager", Vector3.zero, Quaternion.identity);
        }
    }
}
