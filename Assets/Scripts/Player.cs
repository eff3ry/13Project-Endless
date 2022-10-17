using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI; 
using TMPro; 

public class Player : MonoBehaviour 
{ 
    //Movement Vars
    [SerializeField] private float speed = 10f; 
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float targetAngle = 30f;

    //Effects
    [SerializeField] private GameObject coinEffect;
    private ParticleSystem coinEffectParticleSystem;
    [SerializeField] private GameObject collisionEffect;
    private ParticleSystem collisionEffectParticleSystem;

    //Gameplay
    [SerializeField] public int score;
    [SerializeField] private int lives;
    [SerializeField] private int maxLives = 5;

    //UI
    private UiController UiController;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text livesText;
    
    void Awake()
    {
        UiController = FindObjectOfType<UiController>();
    }
   
    void Start() 
    { 
        coinEffectParticleSystem = coinEffect.GetComponent<ParticleSystem>();
        collisionEffectParticleSystem = collisionEffect.GetComponent<ParticleSystem>();
        lives = 5;

        updateLives(lives);
    } 

    void Update() 
    { 
        Movement(); 
    } 

    void Movement() 
    {   

        Vector3 pos = transform.position;
        
        if (pos.x < -4.5f)
        {
            pos.x = -4.5f;
        } else if (pos.x > 4.5f)
        {
            pos.x = 4.5f;
        }
        transform.position = pos;


        if (transform.position.x >= -4.5f && transform.position.x <= 4.5f)
        {
            transform.position = transform.position + new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);  
        }
        
        // rotate character by 30 degrees 
        float z = Input.GetAxis("Horizontal") * -targetAngle; 
        Vector3 euler = transform.localEulerAngles; 
        euler.z = Mathf.LerpAngle(euler.z, z, Time.deltaTime * rotationSpeed); 
        transform.localEulerAngles = euler; 

        
    } 

    private void updateScore(int score)
    {
        scoreText.text = $"Score: {score}";
        
    }

    private void updateLives(int lives)
    {
        livesText.text = $"Lives: {lives}";
    }

 
    // ensure trigger is ticked for gameObject 
    private void OnTriggerEnter(Collider collision) 
    { 
        if (collision.CompareTag("Reward")) 
        {
            Reward reward = collision.GetComponent<Reward>();
            Debug.Log("Collided with " + collision);
            if (reward.rewardType == "Coin")
            {
                score++;
                updateScore(score); 
                coinEffect.transform.position = collision.transform.position;
                coinEffectParticleSystem.Play();
            } 
            if (reward.rewardType == "Life")
            {
                //play life effect;
                if (lives < maxLives)
                {
                    lives++;
                    updateLives(lives);
                }
            }
            
            //Instantiate(coinEffect, transform.position, transform.rotation); 
            Destroy(collision.gameObject); 
        } 

        if (collision.gameObject.CompareTag("Obstacle")) 
        {  
            collisionEffect.transform.position = collision.transform.position; 
            collisionEffectParticleSystem.Play();
            GameObject.Destroy(collision.gameObject);
            lives--;
            updateLives(lives);
            if (lives < 1)
            {
                Invoke("DisplayGameOverUI", 0);
            }
            
        } 
    } 
 
    private void DisplayGameOverUI() 
    { 
        UiController.ChangeUiPanel(1);
        Time.timeScale = 0f;
    } 
} 