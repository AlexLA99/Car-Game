using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Manager : MonoBehaviour
{
    void Awake()
    {
        
        PhotonNetwork.Instantiate("PhotonPrefabs/Client", Vector3.zero, Quaternion.identity);
        
    }
}
