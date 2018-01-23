using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BinaryFormatterTest : MonoBehaviour
{
    [SerializeField]
    public static BinaryFormatterTest singleton;

    [SerializeField]
    private int health;

    [SerializeField]
    private int experience;

    public void RestoreHealth(int amount) { health += amount; }
    public void GainExperience (int amount) { experience += amount; }

    private void Awake()
    {
        if(singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            singleton = this;
        }
        else if(singleton != this)
        {
            Destroy(gameObject);
        }        
    }

    private void OnGUI ()
    {
        GUI.Label(new Rect(10, 10, 100, 30), "Health: " + health);
        GUI.Label(new Rect(10, 40, 150, 30), "Experience: " + experience);
    }

    public void Save ()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/playerinfo.dat");

        PlayerData data = new PlayerData();

        data.health = health;
        data.experience = experience;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load ()
    {
        if(File.Exists(Application.persistentDataPath + "/playerinfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            health = data.health;
            experience = data.experience;
        }
    }
}

[Serializable]
class PlayerData
{
    public int health;
    public int experience;
}