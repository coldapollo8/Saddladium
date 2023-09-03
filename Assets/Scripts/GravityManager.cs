using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GravityManager : MonoBehaviour
{
    public float highG = 13f;
    public float lowG = 5f;
    public float gravity = 10f;
    public GameObject gravityLabel;
    public GameObject gravityTimer;
    public GameObject playerTimer;

    public Sprite blueGravity;
    public Sprite redGravity;
    private Image image;
    private TextMeshProUGUI timerText;
    private TextMeshProUGUI  playerTimeText;
    private float time = 0f;
    private float playerTime = 0f;
    private bool start = false;

    [SerializeField]
    private bool highGravity = false;

    // Start is called before the first frame update
    void Start()
    {   
        playerTimeText = playerTimer.GetComponent<TextMeshProUGUI>();

        timerText = gravityTimer.GetComponent<TextMeshProUGUI>();

        image = gravityLabel.GetComponent<Image>();

        if(highGravity == true)
        {
            gravity = highG;
           image.sprite = redGravity;
        }else{
            gravity = lowG;
            image.sprite = blueGravity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(start == true){
        playerTime += Time.deltaTime;
        
        playerTimeText.text = "" + playerTime;
        time += Time.deltaTime;
        timerText.text = "" + (10f - Mathf.Round(time));
        if(time > 10f){
            changeGravity();
        }
        }
    }

    private void changeGravity(){
        if(highGravity == true)
        {
            highGravity = false;
            gravity = lowG;
            image.sprite = blueGravity;

        }else{
            highGravity = true;
            gravity = highG;
            image.sprite = redGravity;
        }
        time = 0;
    } 

    public void startTimer(){
        start = true;
    }

    public void stopTimer(){
        start = false;
    }

    public bool getStart(){
        return start;
    }
}
