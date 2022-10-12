using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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


    // Start is called before the first frame update
    void Start()
    {
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

        
        if (PlayerPrefs.GetString("currentUser") != null)
        {
            string userToFind = PlayerPrefs.GetString("currentUser");
            List<userData> usersData = loadData();
            foreach (userData data in usersData)
            {
                if (data._userName == userToFind)
                {
                    hideAllPanels();
                    profileMenu.SetActive(true);
                    updateProfile(data);
                    break;
                }
            }
        }
    }

    void startGame()
    {
        SceneManager.LoadScene("Game");
    }

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

    void switchPanels(GameObject show, GameObject hide)
    {
        show.SetActive(true);
        hide.SetActive(false);
    }

    void hideAllPanels()
    {
        loginMenu.SetActive(false);
        registerMenu.SetActive(false);
        profileMenu.SetActive(false);
    }

    void register()
    {
        List<userData> usersData = loadData();

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
        saveData(usersData);
        switchPanels(profileMenu, registerMenu);
        updateProfile(newUser);
        PlayerPrefs.SetString("currentUser", newUser._userName);
        clearFields();
    }

    void login()
    {
        List<userData> usersData = loadData();

        //find username
        //loginUsernameEntry.text
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
            PlayerPrefs.SetString("currentUser", foundUser._userName);
            clearFields();
        }
    }

    void logout()
    {
        updateProfile(new userData());

        switchPanels(loginMenu, profileMenu);

        PlayerPrefs.SetString("currentUser", null);
    }

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

    List<userData> loadData()
    {
        if (File.Exists(Application.persistentDataPath + "/usersData.dat"))
        {
            List<userData> usersData = new List<userData>();
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/usersData.dat", FileMode.Open);
            usersData = (List<userData>)bf.Deserialize(file);
            file.Close();
            return usersData;
        } 
        else 
        {
            return new List<userData>();
        }
    }
    
    void saveData(List<userData> usersData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/usersData.dat");
        bf.Serialize(file, usersData);
        file.Close();
    }

}


[Serializable]
public class userData
{
    public string _userName;
    public string _password;
    public string _email;
    public int highScore;

    public userData(string userName, string password, string email)
    {
        _userName = userName;
        _password = password;
        _email = email;
        highScore = 0;
    }

    public userData()
    {
        _userName = null;
        _password = null;
        _email = null;
        highScore = 0;
    }
}
