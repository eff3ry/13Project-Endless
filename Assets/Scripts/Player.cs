using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI; 
using TMPro; 

public class Player : MonoBehaviour 
{ 
    //Movement Vars
    [SerializeField] private const float speed = 10f; //constant for a more robust program
    [SerializeField] private const float rotationSpeed = 10f;//constant for a more robust program
    [SerializeField] private const float targetAngle = 30f;//constant for a more robust program

    //Effects
    [SerializeField] private GameObject coinEffect;
    private ParticleSystem coinEffectParticleSystem;
    [SerializeField] private GameObject collisionEffect;
    private ParticleSystem collisionEffectParticleSystem;

    //Gameplay
    [SerializeField] public int score;
    [SerializeField] private int lives;
    [SerializeField] private const int maxLives = 5; //constant for a more robust program
    public bool isAlive;

    //UI
    private UiController UiController;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private Image healthBar;    
    void Awake()
    {
        UiController = FindObjectOfType<UiController>();
    }
   
    void Start() 
    { 
        //set the particle systems
        coinEffectParticleSystem = coinEffect.GetComponent<ParticleSystem>();
        collisionEffectParticleSystem = collisionEffect.GetComponent<ParticleSystem>();

        //set inital lives
        lives = maxLives;
        isAlive = true;

        //update ui on scene start
        healthBar.fillMethod = Image.FillMethod.Horizontal;
        updateLives(lives);
    } 

    void Update() 
    { 
        Movement(); 
    } 

    //handle player movement
    void Movement() 
    {   

        //limit x movement
        Vector3 pos = transform.position;
        
        if (pos.x < -4.5f)
        {
            pos.x = -4.5f;
        } else if (pos.x > 4.5f)
        {
            pos.x = 4.5f;
        }
        transform.position = pos;

        //only move if the character is within the boundrys
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

    //updates the ui elements displaying the score counter
    private void updateScore(int score)
    {
        scoreText.text = $"Score: {score}";
        
    }

    //updates the ui elements displaying the life counter and healthbar
    private void updateLives(int lives)
    {
        livesText.text = $"Lives: {lives}";
        float healthBarPercentage = (float)lives/(float)maxLives;
        healthBar.fillAmount = healthBarPercentage;
        Debug.Log($"Lives: {lives}");
        Debug.Log($"HealthBar: {healthBarPercentage}");
    }

 
    // ensure trigger is ticked for gameObject 
    private void OnTriggerEnter(Collider collision) 
    { 
        //if object is a reward
        if (collision.CompareTag("Reward")) 
        {
            Reward reward = collision.GetComponent<Reward>();
            Debug.Log("Collided with " + collision);
            // if its a coin
            if (reward.rewardType == "Coin")
            {
                score++;
                updateScore(score); 
                coinEffect.transform.position = collision.transform.position;
                coinEffectParticleSystem.Play();
            } 
            //if its a life
            if (reward.rewardType == "Life")
            {
                //increase lives
                if (lives < maxLives)
                {
                    lives++;
                    updateLives(lives);
                }
            }
            
            //Instantiate(coinEffect, transform.position, transform.rotation); 
            Destroy(collision.gameObject); 
        } 

        //if the object is an obstacle
        if (collision.gameObject.CompareTag("Obstacle")) 
        {  
            //play the effect
            collisionEffect.transform.position = collision.transform.position; 
            collisionEffectParticleSystem.Play();
            //destroy the object
            GameObject.Destroy(collision.gameObject);
            //decrease lives
            lives--;
            updateLives(lives);
            //check if the player is dead
            if (lives < 1)
            {
                isAlive = false;
                Invoke("DisplayGameOverUI", 0);
            }
            
        } 
    } 
 
    //freeze time and swap to the Game Over ui
    private void DisplayGameOverUI() 
    { 
        UiController.ChangeUiPanel(1);
        Time.timeScale = 0f;
    } 
} 