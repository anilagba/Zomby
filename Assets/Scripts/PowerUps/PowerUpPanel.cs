using System.Collections;
using UnityEngine;
using TMPro;

public class PowerUpPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    float timer;
    WaitForSeconds coroutineDelay = new WaitForSeconds(1f);

    private void Awake()
    {
        gameObject.SetActive(false);
    }


    public void SetTimer(float timer)
    {
        this.timer = timer;
        StopAllCoroutines();
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        timerText.text = timer.ToString();
        timer--;
        if (timer < 0)
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
        else
        {
            yield return coroutineDelay;
            StartCoroutine(CountDown());
        }
    }
}