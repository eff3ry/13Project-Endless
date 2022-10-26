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
    [SerializeField] const int CharLim = 20; //Username char max limit, constant for a more robust program
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

    void Awake()
    {
        //find the user manager
        userManager = FindObjectOfType<UserManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //set buttons
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

        //login the current user when scene loads
        if (userManager.currentUserIndex > -1)
        {
            hideAllPanels();
            profileMenu.SetActive(true);
            updateProfile(userManager.lastLoadedData[userManager.currentUserIndex]);
        }
    }

    void startGame()
    {
        SceneManager.LoadScene("Game");
    }

    //clear the input fields
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

    //switch ui panels
    void switchPanels(GameObject show, GameObject hide)
    {
        show.SetActive(true);
        hide.SetActive(false);
    }

    //hide all ui panels
    void hideAllPanels()
    {
        loginMenu.SetActive(false);
        registerMenu.SetActive(false);
        profileMenu.SetActive(false);
    }


    //register account
    //accepts username email and password
    void register()
    {
        //load data from file
        List<userData> usersData = userManager.loadData();

        string errorMsg = validateUsername(registerUsernameEntry.text, usersData);
        if (errorMsg != null)
        {
            registerErrorText.text = errorMsg;
            return;
        }

        errorMsg = validatePassword(registerPasswordEntry.text, registerPasswordConfirmEntry.text);
        if (errorMsg != null)
        {
            registerErrorText.text = errorMsg;
            return;
        }

        errorMsg = validateEmail(registerEmailEntry.text, usersData);
        if (errorMsg != null)
        {
            registerErrorText.text = errorMsg;
            return;
        }

        userData newUser = new userData(registerUsernameEntry.text, registerPasswordEntry.text, registerEmailEntry.text);
        usersData.Add(newUser);
        userManager.saveData(usersData);
        switchPanels(profileMenu, registerMenu);
        updateProfile(newUser);

        //PlayerPrefs.SetString("currentUser", newUser._userName);
        userManager.currentUserIndex = usersData.IndexOf(newUser);
        userManager.currentUser = newUser;

        clearFields();
    }

    //login account
    void login()
    {
        List<userData> usersData = userManager.loadData();

        //find username
        userData foundUser = null;
        foreach (userData data in usersData)
        {
            if (loginUsernameEntry.text == data._userName)
            {
                //username found
                if (loginPasswordEntry.text == data._password)
                {
                    //correct password
                    foundUser = data;
                } else 
                {
                    //wrong password
                    loginErrorText.text = "Username or Password is incorrect";
                    return;
                }
            }
        }

        if (foundUser == null)
        {
            //didnt find username
            loginErrorText.text = "Username or Password is incorrect";
            return;
        } else 
        {
            //both username and passowrd were correct
            switchPanels(profileMenu, loginMenu);
            updateProfile(foundUser);
            //PlayerPrefs.SetString("currentUser", foundUser._userName);
            userManager.currentUserIndex = usersData.IndexOf(foundUser);
            userManager.currentUser = foundUser;
            clearFields();
        }
    }

    //logout of account
    void logout()
    {
        updateProfile(new userData());

        switchPanels(loginMenu, profileMenu);

        //PlayerPrefs.SetString("currentUser", null);
        userManager.currentUser = null;
        userManager.currentUserIndex = -1;
    }

    //update the ui for the profile
    void updateProfile(userData userData)
    {
        profileUsername.text = userData._userName;
        profileEmail.text = userData._email;
        profileHighscore.text = $"Highscore: {userData.highScore}";
    }

    //returns an error message or null if username is valid
    string validateUsername(string username, List<userData> usersData)
    {
        //check if username is empty or has whitespaces
        if (string.IsNullOrWhiteSpace(username) || username.Contains(" "))
        {
            return "Username is empty or contains spaces";
        }

        if (username.Length > CharLim)
        {
            return "Username is longer than the 20 character limit";
        }

        //Check if username already exists
        foreach (userData userData in usersData)
        {
            if (username == userData._userName)
            {
                return "Username is already in use";
            }
        }   

        return null;
    }

    //returns an error message or null if password is valid
    string validatePassword(string password, string passwordConfirmation)
    {
        //check if password is empty or has whitespaces
        if (string.IsNullOrWhiteSpace(password) || password.Contains(" "))
        {
            return "Password is empty or contains spaces";
        }

        if (passwordConfirmation != password)
        {
            return "Passwords do not match";
        }
        return null;
    }

    //returns an error message or null if email is valid
    string validateEmail(string email, List<userData> usersData)
    {
        if (!email.Contains("@"))
        {
            return "That is not a valid email";
        }

        //Check if email already exists
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
