using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.UI; 
using TMPro; 

public class Player : MonoBehaviour 
{ 
    // Movement Vars
    [Header("Movement Vars")]
    [SerializeField] private const float speed = 10f; //constant for a more robust program
    [SerializeField] private const float rotationSpeed = 10f;//constant for a more robust program
    [SerializeField] private const float targetAngle = 30f;//constant for a more robust program

    // Effects
    [Header("Effects")]
    [SerializeField] private GameObject coinEffect;
    private ParticleSystem coinEffectParticleSystem;
    [SerializeField] private GameObject collisionEffect;
    private ParticleSystem collisionEffectParticleSystem;

    // Gameplay
    [Header("Gameplay")]
    [SerializeField] public int score;
    [SerializeField] private int lives;
    // Constant for a more robust program,
    // a constant because this should'nt be changed during runtime
    [SerializeField] private const int maxLives = 5; 
    public bool isAlive;

    // UI
    [Header("UI")]
    private UiController UiController;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private Image healthBar; 

    void Awake()
    {
        // Find the UI controller in the scene
        UiController = FindObjectOfType<UiController>();
    }
   
    void Start() 
    { 
        // Find the particle systems components
        coinEffectParticleSystem = coinEffect.GetComponent<ParticleSystem>();
        collisionEffectParticleSystem = collisionEffect.GetComponent<ParticleSystem>();

        // Set inital lives
        lives = maxLives;
        isAlive = true;

        // Update health UI on scene start
        healthBar.fillMethod = Image.FillMethod.Horizontal;
        updateLives(lives);
    } 

    void Update() 
    { 
        Movement(); 
    } 

    // Handle player movement
    void Movement() 
    {   

        // Limit x movement
        Vector3 pos = transform.position;
        
        if (pos.x < -4.5f)
        {
            pos.x = -4.5f;
        } else if (pos.x > 4.5f)
        {
            pos.x = 4.5f;
        }
        transform.position = pos;

        // Only move if the character is within the boundrys
        if (transform.position.x >= -4.5f && transform.position.x <= 4.5f)
        {
            transform.position = transform.position + new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);  
        }
        
        // Rotate character by 30 degrees 
        float z = Input.GetAxis("Horizontal") * -targetAngle; 
        Vector3 euler = transform.localEulerAngles; 
        euler.z = Mathf.LerpAngle(euler.z, z, Time.deltaTime * rotationSpeed); 
        transform.localEulerAngles = euler; 

        
    } 

    // Updates the ui elements displaying the score counter
    private void updateScore(int score)
    {
        scoreText.text = $"Score: {score}";  
    }

    // Updates the ui elements displaying the life counter and healthbar
    private void updateLives(int lives)
    {
        livesText.text = $"Lives: {lives}";
        // The Health bar is adaptive and changes appropriately if maxLives is changes
        float healthBarPercentage = (float)lives/(float)maxLives;
        healthBar.fillAmount = healthBarPercentage;
        Debug.Log($"Lives: {lives}");
        Debug.Log($"HealthBar: {healthBarPercentage}");
    }

 
    // Ensure trigger is ticked for gameObject 
    private void OnTriggerEnter(Collider collision) 
    { 
        // If object is a reward
        if (collision.CompareTag("Reward")) 
        {
            Reward reward = collision.GetComponent<Reward>();
            Debug.Log("Collided with " + collision);
            // If its a coin
            if (reward.rewardType == "Coin")
            {
                score++;
                updateScore(score); 
                coinEffect.transform.position = collision.transform.position;
                coinEffectParticleSystem.Play();
            } 
            // If its a life
            if (reward.rewardType == "Life")
            {
                // Increase lives
                if (lives < maxLives)
                {
                    lives++;
                    updateLives(lives);
                }
            }
            // Destroy the Object
            Destroy(collision.gameObject); 
        } 

        // If the object is an obstacle
        if (collision.gameObject.CompareTag("Obstacle")) 
        {  
            // Play the effect
            collisionEffect.transform.position = collision.transform.position; 
            collisionEffectParticleSystem.Play();
            // Destroy the object
            GameObject.Destroy(collision.gameObject);
            // Decrease lives
            lives--;
            updateLives(lives);
            // Check if the player is dead
            if (lives < 1)
            {
                isAlive = false;
                Invoke("DisplayGameOverUI", 0);
            } 
        } 
    } 
 
    // Freeze time and swap to the Game Over ui
    private void DisplayGameOverUI() 
    { 
        UiController.ChangeUiPanel(1);
        Time.timeScale = 0f;
    } 
} 