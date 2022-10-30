using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginMenuController : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject loginMenu;
    [SerializeField] GameObject registerMenu;
    [SerializeField] GameObject profileMenu;

    [Header("Login Menu Objects")]
    [SerializeField] TMP_InputField loginUsernameEntry;
    [SerializeField] TMP_InputField loginPasswordEntry;
    [SerializeField] Button loginButton;
    [SerializeField] TMP_Text loginErrorText;
    [SerializeField] Button switchToRegisterButton;

    [Header("Register Menu Objects")]
    [SerializeField] TMP_InputField registerUsernameEntry;
    [SerializeField] const int CharLim = 20; // Username character max limit is set as a constant for a more robust program.
    [SerializeField] TMP_InputField registerEmailEntry;
    [SerializeField] TMP_InputField registerPasswordEntry;
    [SerializeField] TMP_InputField registerPasswordConfirmEntry;
    [SerializeField] Button registerButton;
    [SerializeField] TMP_Text registerErrorText;
    [SerializeField] Button switchToLoginButton;

    [Header("Profile Objects")]
    [SerializeField] TMP_Text profileUsername;
    [SerializeField] TMP_Text profileEmail;
    [SerializeField] TMP_Text profileHighscore;
    [SerializeField] Button logoutButton;
    [SerializeField] Button startGameButton;

    [Header("UserManager")]
    [SerializeField] UserManager userManager;

    [SerializeField] Button quitButton;

    void Awake()
    {
        // Find the user manager to access usefull user related functions
        userManager = FindObjectOfType<UserManager>();
    }

    void Start()
    {
        // Set the buttons to call the appropriate methods
        switchToRegisterButton.onClick.AddListener(delegate { 
            switchPanels(registerMenu, loginMenu);
            clearFields();
        });
        switchToLoginButton.onClick.AddListener(delegate {
            switchPanels(loginMenu, registerMenu);
            clearFields();
        });
        registerButton.onClick.AddListener(delegate {
            register();
        });
        loginButton.onClick.AddListener(delegate {
            login();
        });
        logoutButton.onClick.AddListener(delegate {
            logout();
        });

        startGameButton.onClick.AddListener(delegate {
            startGame();
        });

        quitButton.onClick.AddListener(delegate {
            quitGame();
        });

        // If the user is already logged in when loading the scene,
        // Update information and display the main menu
        if (userManager.currentUserIndex > -1)
        {
            hideAllPanels();
            profileMenu.SetActive(true);
            updateProfile(userManager.lastLoadedData[userManager.currentUserIndex]);
        }
    }

    // Exit the program
    void quitGame()
    {
        Application.Quit();
    }

    // Load the game scene
    void startGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Clear all the text input fields by setting the text to empty strings
    void clearFields()
    {
        loginUsernameEntry.text = "";
        loginPasswordEntry.text = "";
        loginErrorText.text = "";

        registerUsernameEntry.text = "";
        registerEmailEntry.text = "";
        registerPasswordEntry.text = "";
        registerPasswordConfirmEntry.text = "";
        registerErrorText.text = "";
    }

    // Hides the current object and shows the requested one,
    // Used for menu purposes
    void switchPanels(GameObject show, GameObject hide)
    {
        show.SetActive(true);
        hide.SetActive(false);
    }

    // Hides all menu GameObjects
    void hideAllPanels()
    {
        loginMenu.SetActive(false);
        registerMenu.SetActive(false);
        profileMenu.SetActive(false);
    }


    // Register account
    // Accepts username email and password
    // Validates all inputs
    void register()
    {
        // Load data from file
        List<userData> usersData = userManager.loadData();

        // Vaidates username
        string errorMsg = validateUsername(registerUsernameEntry.text, usersData);
        if (errorMsg != null)
        {
            registerErrorText.text = errorMsg;
            return;
        }

        // Validates password
        errorMsg = validatePassword(registerPasswordEntry.text, registerPasswordConfirmEntry.text);
        if (errorMsg != null)
        {
            registerErrorText.text = errorMsg;
            return;
        }

        // Validates email
        errorMsg = validateEmail(registerEmailEntry.text, usersData);
        if (errorMsg != null)
        {
            registerErrorText.text = errorMsg;
            return;
        }

        // All the checks above passed,
        // So create a new user, add to the list of users and save
        userData newUser = new userData(registerUsernameEntry.text, registerPasswordEntry.text, registerEmailEntry.text);
        usersData.Add(newUser);
        userManager.saveData(usersData);
        switchPanels(profileMenu, registerMenu);
        updateProfile(newUser);

        userManager.currentUserIndex = usersData.IndexOf(newUser);
        userManager.currentUser = newUser;

        clearFields();
    }

    //login account
    void login()
    {
        List<userData> usersData = userManager.loadData();

        // Find username
        userData foundUser = null;
        foreach (userData data in usersData)
        {
            if (loginUsernameEntry.text == data._userName)
            {
                // Username found
                if (loginPasswordEntry.text == data._password)
                {
                    // Correct password
                    foundUser = data;
                } else 
                {
                    // Wrong password
                    loginErrorText.text = "Username or Password is incorrect";
                    return;
                }
            }
        }

        if (foundUser == null)
        {
            // Didnt find username
            loginErrorText.text = "Username or Password is incorrect";
            return;
        } else 
        {
            // Both username and passowrd were correct,
            // Log in
            switchPanels(profileMenu, loginMenu);
            updateProfile(foundUser);
            userManager.currentUserIndex = usersData.IndexOf(foundUser);
            userManager.currentUser = foundUser;
            clearFields();
        }
    }

    // Logout of account
    void logout()
    {
        // Set the displayed data to empty data
        updateProfile(new userData());
        // Switch to main menu
        switchPanels(loginMenu, profileMenu);
        // Set curent user to null
        userManager.currentUser = null;
        userManager.currentUserIndex = -1;
    }

    // Update the UI for the profile
    void updateProfile(userData userData)
    {
        profileUsername.text = userData._userName;
        profileEmail.text = userData._email;
        profileHighscore.text = $"Highscore: {userData.highScore}";
    }

    // Validate Username Method.
    // Returns an error message or null if username is valid.
    string validateUsername(string username, List<userData> usersData)
    {
        // Check if username is empty or has whitespaces
        if (string.IsNullOrWhiteSpace(username) || username.Contains(" "))
        {
            return "Username is empty or contains spaces";
        }
        // Check if the username is more than the character limit
        if (username.Length > CharLim)
        {
            return "Username is longer than the 20 character limit";
        }
        // Check if username already exists
        foreach (userData userData in usersData)
        {
            if (username == userData._userName)
            {
                return "Username is already in use";
            }
        }   

        return null;
    }

    // Returns an error message or null if password is valid
    string validatePassword(string password, string passwordConfirmation)
    {
        // Check if password is empty or has whitespaces
        if (string.IsNullOrWhiteSpace(password) || password.Contains(" "))
        {
            return "Password is empty or contains spaces";
        }
        // Check that both password and confirmation password match
        if (passwordConfirmation != password)
        {
            return "Passwords do not match";
        }
        return null;
    }

    // Returns an error message or null if email is valid
    string validateEmail(string email, List<userData> usersData)
    {
        // Check if the email is valid by looking for a '@' symbol
        if (!email.Contains("@"))
        {
            return "That is not a valid email";
        }

        // Check if email already exists
        foreach (userData userData in usersData)
        {
            if (email == userData._email)
            {
                return "Email is already in use";
            }
        }

        return null;
    }
}
