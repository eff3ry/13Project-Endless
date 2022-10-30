using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class UserManager : MonoBehaviour
{
    public userData currentUser = null;
    public List<userData> lastLoadedData;
    public int currentUserIndex = -1;
    static bool created = false;

    // Make sure there is only ever one usermanager
    void Awake()
    {
        // This code makes sure there is only one of this object in the scene basically a singleton
        if (!created || GameObject.FindObjectsOfType<UserManager>().Length == 1) {
            // This is the first instance - make it persist
            DontDestroyOnLoad(this.gameObject);
            created = true;

            // Since it is the first instance set current user to empty user
            currentUser = null;
            currentUserIndex =-1;
        } else if (GameObject.FindObjectsOfType<UserManager>().Length > 1) {
            // This must be a duplicate from a scene reload - DESTROY!
            Destroy(this.gameObject);
        }
    }  

    // Load user info from file
    public List<userData> loadData()
    {
        // If the file exists load it if not return empty class object
        if (File.Exists(Application.persistentDataPath + "/usersData.dat"))
        {
            List<userData> usersData = new List<userData>();
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/usersData.dat", FileMode.Open);
            usersData = (List<userData>)bf.Deserialize(file);
            file.Close();
            lastLoadedData = usersData;
            return usersData;
        } 
        else 
        {
            lastLoadedData = new List<userData>();
            return new List<userData>();
        }
    }
    
    // Save user info to file
    public void saveData(List<userData> usersData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/usersData.dat");
        bf.Serialize(file, usersData);
        file.Close();
    } 
}


// User info class
// Stores username and password and email and highscore in a custom object for saving and loading
[Serializable]
public class userData
{
    // Read only for variables that should'nt be edited but still need to be read
    readonly public string _userName;
    readonly public string _password;
    readonly public string _email;
    public int highScore;

    // Constructor for a full object
    public userData(string userName, string password, string email)
    {
        _userName = userName;
        _password = password;
        _email = email;
        highScore = 0;
    }

    // Constructor for a empty object
    public userData()
    {
        _userName = null;
        _password = null;
        _email = null;
        highScore = 0;
    }
}
