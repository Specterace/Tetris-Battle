using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownTimer : MonoBehaviour {

    [SerializeField]
    private float maxTimeLeft;
    private float currTime;
    Text countdownText;
    // Use this for initialization
    void OnEnable()
    {
        currTime = maxTimeLeft;
        countdownText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        currTime -= Time.deltaTime;
        if (currTime <= 0)
        {
            currTime = 0;
			countdownText.text = currTime.ToString("F1");
			gameObject.transform.parent.gameObject.SetActive (false);
        }
        else
        {
			countdownText.text = currTime.ToString("F1");
        }

    }

    public float getTime()
    {
        return currTime;
    }
}