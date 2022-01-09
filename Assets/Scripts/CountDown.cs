using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public GameObject countdown;
    public GameObject LapTimer;
    public GameObject CarControls;
    public CarMovement car;
    public CarMovement car2;
    public AudioSource LevelMusic;

    void Start()
    {
        car.motorForce = 0;
        car2.motorForce = 0;
        StartCoroutine(CountStart());
    }

    IEnumerator CountStart()
    {
        yield return new WaitForSeconds(0.5f);
        countdown.GetComponent<Text>().text = "3";
        countdown.SetActive(true);
        yield return new WaitForSeconds(1);
        countdown.SetActive(false);
        countdown.GetComponent<Text>().text = "2";
        countdown.SetActive(true);
        yield return new WaitForSeconds(1);
        countdown.SetActive(false);
        countdown.GetComponent<Text>().text = "1";
        countdown.SetActive(true);
        yield return new WaitForSeconds(1);
        countdown.SetActive(false);
        countdown.GetComponent<Text>().text = "GO";
        countdown.SetActive(true);
        LapTimer.SetActive(true);
        CarControls.SetActive(true);
        LevelMusic.Play();
        car.motorForce = 1000f;
        car2.motorForce = 1000f;
        yield return new WaitForSeconds(1);
        countdown.SetActive(false);
        
    }
}
