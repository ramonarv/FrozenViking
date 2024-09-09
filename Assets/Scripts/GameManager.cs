using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{

    public static GameManager manager;

    public string currentLevel;
    public float health; // current health left
    public float previousHealth; // this is the health before we took some damage
    public float maxHealth; // this tells what is the maximum value of health

    public bool Level1;
    public bool Level2;
    public bool Level3;
    public bool Level4;
    public bool Level5;
    
    private void Awake()
    {
        // Singleton
        // we want to make sure we have only one instance of GameManager in our game
        if(manager == null)
        {
            // if we do not have a manager, lets tell that this class instance is the manager
            // we also tell that this manager cannot be destroyed if we change the scene
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else
        {
            // we'll run this if there is already a manager in the scene for some reason.
            // then this manager will be a second manager and that is not allowed. we'll destroy the second "King"
            // in this game

            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.M))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData data = new PlayerData();
        // copy information from game manager to PlayerData
        data.health = health;
        data.previousHealth = previousHealth;
        data.maxHealth = maxHealth;
        data.Level1 = Level1;
        data.Level2 = Level2;
        data.Level3 = Level3;
        data.Level4 = Level4;
        data.Level5 = Level5;
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        // we check if there is a saved file in the persistent folder
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            // we continue with the loading
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            // the data is now loaded to data object
            file.Close();

            // move the information to game manager
            health = data.health;
            previousHealth = data.previousHealth;
            maxHealth = data.maxHealth;
            Level1 = data.Level1;
            Level2 = data.Level2;
            Level3 = data.Level3;
            Level4 = data.Level4;
            Level5 = data.Level5;

        }
    }
}

// another class that we can serialize. this contains only the information we are going to store
[Serializable]
class PlayerData
{
    public string currentLevel;
    public float health;
    public float previousHealth; 
    public float maxHealth; 
    public bool Level1;
    public bool Level2;
    public bool Level3;
    public bool Level4;
    public bool Level5;
}
