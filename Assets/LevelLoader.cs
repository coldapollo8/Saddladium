using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{

    public Animator transition;
    public float transitionTime;

    public bool isLevel = true;
    public GravityManager logic;
    public GameObject audio;
    private AudioSource audioSource;
    public AudioClip startBeat;
    public AudioClip mainTheme;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = audio.GetComponent<AudioSource>();
        audioSource.clip = startBeat;
        audioSource.Play();
        if(isLevel == true)
        {
            transition.SetTrigger("isLevel");
   
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void loadMenu(){
        StartCoroutine(LoadLevel(0));
    }
    public void LoadLevel(){
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex){
        
         transition.SetTrigger("Start");

 
         yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
        
    }
    public void LoadNextLevel(){
        StartCoroutine(LoadNextLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadNextLevel(int levelIndex){
         transition.SetBool("Exit", true);

 
         yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
        
    }

    public void StartTimer(){
        logic.startTimer();

        audioSource.clip = mainTheme;
        audioSource.Play();
    }

}
