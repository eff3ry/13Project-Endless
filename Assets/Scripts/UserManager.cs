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

    //make sure there is only ever one usermanager
    void Awake()
    {
        if (!created || GameObject.FindObjectsOfType<UserManager>().Length == 1) {
            // this is the first instance - make it persist
            DontDestroyOnLoad(this.gameObject);
            created = true;

            //since it is the first instance set current user to empty user
            currentUser = null;
            currentUserIndex =-1;
        } else if (GameObject.FindObjectsOfType<UserManager>().Length > 1) {
            // this must be a duplicate from a scene reload - DESTROY!
            Destroy(this.gameObject);
        }
    }  

    //load user info from file
    public List<userData> loadData()
    {
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
    
    //save user info to file
    public void saveData(List<userData> usersData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/usersData.dat");
        bf.Serialize(file, usersData);
        file.Close();
    } 
}


//user info class
[Serializable]
public class userData
{
    readonly public string _userName;
    readonly public string _password;
    readonly public string _email;
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
