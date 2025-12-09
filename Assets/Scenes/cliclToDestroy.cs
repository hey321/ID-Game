using UnityEngine;
using UnityEngine.UI;

public class cliclToDestroy : MonoBehaviour
{

    public Text timerText;
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timerText.text = timer + "";
    }
}
