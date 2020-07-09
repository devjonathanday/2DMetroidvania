using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Health playerHealth = null;

    public int destinationIndex = 0;
    public bool playerFacingRight = true;
    public bool spawnFromLoadGame = true;

    Scene currentScene;

    //Upgrades do not include bullet types, since they are contained in PlayerManager.currentBullet
    public Upgrade[] upgrades = null;

    [System.Serializable]
    public class Upgrade
    {
        public string name;
        public bool unlocked;
    }

    void Awake()
    {
        //Ensure only one GameManager instance exists in the scene
        GameManager[] gameManagers = FindObjectsOfType<GameManager>();
        if (gameManagers.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //Assign self as static GameManager instance
        if (instance == null) instance = this;

        currentScene = SceneManager.GetActiveScene();

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        Initialize();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Does not run scene load functions if the existing scene is reloaded
        if (scene.path == currentScene.path) return;

        Initialize();
    }

    void Initialize()
    {
        if(spawnFromLoadGame)
        {
            spawnFromLoadGame = false;
            return;
        }
        PlayerManager.instance.facingRight = playerFacingRight;
        PlayerManager.instance.transform.position = LevelManager.instance.spawnPoints[destinationIndex].spawnLocation;
        CameraControls.instance.transform.position = LevelManager.instance.spawnPoints[destinationIndex].cameraStartPos;

        Door entryDoor = LevelManager.instance.spawnPoints[destinationIndex].door;
        if (entryDoor != null) entryDoor.StartOpen();
    }

    public bool CheckUpgradeUnlocked(string name)
    {
        int upgradeIndex = GetUpgradeFromName(name);
        if (upgradeIndex == -1)
        {
            return false;
        }
        else return upgrades[upgradeIndex].unlocked;
    }

    /// <summary>
    /// Returns an index corresponding to the given upgrade name.
    /// </summary>
    public int GetUpgradeFromName(string name)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            if (name == upgrades[i].name)
            {
                return i;
            }
        }
        Debug.LogError("Upgrade \"" + name + "\" is not valid.");
        return -1;
    }

    public void UnlockUpgrade(string name)
    {
        int upgradeIndex = GetUpgradeFromName(name);
        if (upgradeIndex == -1)
        {
            return;
        }
        else upgrades[upgradeIndex].unlocked = true;
    }
}
