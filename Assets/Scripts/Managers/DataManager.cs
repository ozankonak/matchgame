using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private GameData data;

    public bool isGameStartedFirstTime;
    public bool isMusicOn;
    public bool isVibrationOn;
    public bool isADSOn;

    public int coins;

    public int equippedOutfitNum;

    public int[] inventory;

    public bool[] stuffes;

    public bool[] outfits;
    public bool[] unlockedOutfits;

    public bool resetData = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");

        InitializeGameVariables();
    }

    private void InitializeGameVariables()
    {
        Load();

        if (data != null)
        {
            isGameStartedFirstTime = data.isGameStartedFirstTime;
        }
        else
        {
            isGameStartedFirstTime = true;
        }


        if (isGameStartedFirstTime)
            IsGameStartedFirstTime();
        else
            DataManagerEqualGameData();

    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Save.save";

        FileStream stream = new FileStream(path, FileMode.Create);

        GameDataEqualDataManager();

        bf.Serialize(stream, data);
        stream.Close();
    }

    private void DataManagerEqualGameData()
    {
        isGameStartedFirstTime = data.isGameStartedFirstTime;
        isMusicOn = data.isMusicOn;
        isVibrationOn = data.isVibrationOn;
        inventory = data.inventory;
        equippedOutfitNum = data.equippedOutfitNum;
        coins = data.coins;
        stuffes = data.stuffes;
        outfits = data.outfits;
        unlockedOutfits = data.unlockedOutfits;
    }

    private void GameDataEqualDataManager()
    {
        data = new GameData();

        data.isGameStartedFirstTime = isGameStartedFirstTime;
        data.isMusicOn = isMusicOn;
        data.isVibrationOn = isVibrationOn;
        data.inventory = inventory;
        data.equippedOutfitNum = equippedOutfitNum;
        data.coins = coins;
        data.stuffes = stuffes;
        data.outfits = outfits;
        data.unlockedOutfits = unlockedOutfits;
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/Save.save";

        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = bf.Deserialize(stream) as GameData;
        }
        else
        {
            Debug.LogError("Save File not found in " + path);
        }
    }

    private void IsGameStartedFirstTime()
    {
        isGameStartedFirstTime = false;
        isMusicOn = true;
        isVibrationOn = true;
        isADSOn = true;

        coins = 0;

        equippedOutfitNum = 0;

        inventory = new int[1];

        inventory[0] = 0;

        stuffes = new bool[15];

        stuffes[0] = true;

        for (int i = 1; i < stuffes.Length; i++)
        {
            stuffes[i] = false;
        }


        outfits = new bool[15];

        outfits[0] = true;

        for (int i = 1; i < outfits.Length; i++)
        {
            outfits[i] = false;
        }

        unlockedOutfits = new bool[15];


        unlockedOutfits[0] = true;

        for (int i = 1; i < unlockedOutfits.Length; i++)
        {
            unlockedOutfits[i] = false;
        }

        GameDataEqualDataManager();

        Save();
        Load();
    }

    private void OnApplicationQuit()
    {
       if (resetData)
        {
            IsGameStartedFirstTime();
        }
    }
}

[Serializable]
public class GameData
{
    public bool isGameStartedFirstTime;
    public bool isMusicOn;
    public bool isVibrationOn;
    public bool isADSOn;

    public int coins;

    public int equippedOutfitNum;

    public int[] inventory;

    public bool[] stuffes;
    public bool[] outfits;
    public bool[] unlockedOutfits;
}
