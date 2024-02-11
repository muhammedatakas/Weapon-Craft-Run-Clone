using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // Static variables to track game state
    public static bool isGameStarted = false;
    public static bool isLevelOver = false;

    // Reference to the UI TextMeshProUGUI for displaying money
    TextMeshProUGUI moneyText;

    // Initial amount of money the player has
    public int startingMoney = 100;

    // Percentage of income the player receives
    private int incomePercentage;

    // Singleton instance of GameManager
    public static GameManager Instance{ get; private set; }

    // Reference to the BulletPool for managing bullets
    public BulletPool bulletManager;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Find and set reference to BulletPool
        bulletManager = FindObjectOfType<BulletPool>();

        // Find and set reference to the money TextMeshProUGUI
        moneyText = GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>();

        // Check and load player's money from PlayerPrefs
        if (!PlayerPrefs.HasKey("Money"))
        {
            SaveMoney(startingMoney);
        }
        else
        {
            moneyText.text = GetMoney().ToString();
        }

        // Check and load income percentage from PlayerPrefs
        if (!PlayerPrefs.HasKey("incomePercentage"))
        {
            PlayerPrefs.SetInt("incomePercentage", 100);
        }
        else
        {
            incomePercentage = PlayerPrefs.GetInt("incomePercentage");
        }

        // Check and set default values for range and rate upgrade costs
        if (!PlayerPrefs.HasKey("RangeUpgradeMoney"))
        {
            PlayerPrefs.SetInt("RangeUpgradeMoney", 100);
        }

        if (!PlayerPrefs.HasKey("RateUpgradeMoney"))
        {
            PlayerPrefs.SetInt("RateUpgradeMoney", 100);
        }

        // Update the upgrade buttons' text with their respective costs
        GameObject.Find("RateUpgradeButton").GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade " + PlayerPrefs.GetInt("RateUpgradeMoney").ToString();
        BulletSettings.Instance.rangeUpgradeAmount = PlayerPrefs.GetInt("RangeUpgradeMoney") / 100;
        BulletSettings.Instance.rateUpgradeAmount = PlayerPrefs.GetInt("RateUpgradeMoney") / 1000;
    }

    // Method to get the player's current money amount
    public int GetMoney()
    {
        return PlayerPrefs.GetInt("Money");
    }

    // Method to save the player's money amount to PlayerPrefs
    void SaveMoney(int moneyAmount)
    {
        PlayerPrefs.SetInt("Money", moneyAmount);
    }

    // Method to increase the player's money amount
    public void IncreaseMoney(int amount)
    {
        int currentMoney = PlayerPrefs.GetInt("Money");
        currentMoney += amount * incomePercentage / 100;
        PlayerPrefs.SetInt("Money", currentMoney);
        moneyText.text = GetMoney().ToString();
    }

    // Method to decrease the player's money amount
    public void DecreaseMoney(int amount)
    {
        int currentMoney = PlayerPrefs.GetInt("Money");
        currentMoney -= amount;
        PlayerPrefs.SetInt("Money", currentMoney);
        moneyText.text = GetMoney().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game has started by touch input
        if (Input.touchCount > 0 && !isGameStarted && !isLevelOver)
        {
            isGameStarted = true;
            GameObject.Find("Title").SetActive(false);
            GameObject.Find("FireRateUpgrade").SetActive(false);
        }
    }

    // Method for handling range upgrade button click
    public void OnRangeUpgrade()
    {
        int rangeUpgradeMoney = PlayerPrefs.GetInt("RangeUpgradeMoney");
        if (PlayerPrefs.GetInt("Money") >= rangeUpgradeMoney)
        {
            // Upgrade bullet range
            BulletSettings.Instance.rangeUpgradeAmount = rangeUpgradeMoney / 100;
            BulletSettings.Instance.IncreaseBulletRange(BulletSettings.Instance.rangeUpgradeAmount);
            DecreaseMoney(rangeUpgradeMoney);

            // Increase upgrade cost and update button text
            rangeUpgradeMoney += 100;
            PlayerPrefs.SetInt("RangeUpgradeMoney", rangeUpgradeMoney);
            GameObject.Find("RangeUpgradeButton").GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade " + rangeUpgradeMoney.ToString();
        }
        else
        {
            Debug.Log("Not enough money for range upgrade");
        }
    }

    // Method for handling rate upgrade button click
    public void OnRateUpgrade()
    {
        int rateUpgradeMoney = PlayerPrefs.GetInt("RateUpgradeMoney");
        if (PlayerPrefs.GetInt("Money") >= rateUpgradeMoney)
        {
            // Upgrade bullet fire rate
            BulletSettings.Instance.rateUpgradeAmount = rateUpgradeMoney / 1000;
            BulletSettings.Instance.IncreaseFireRate(BulletSettings.Instance.rateUpgradeAmount);
            DecreaseMoney(rateUpgradeMoney);

            // Increase upgrade cost and update button text
            rateUpgradeMoney += 100;
            PlayerPrefs.SetInt("RateUpgradeMoney", rateUpgradeMoney);
            GameObject.Find("RateUpgradeButton").GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade " + rateUpgradeMoney.ToString();
        }
        else
        {
            Debug.Log("Not enough money for rate upgrade");
        }
    }

    // Method for handling income upgrade button click
    public void OnIncomeUpgrade()
    {
        // Increase income percentage by 10
        incomePercentage += 10;

        // Calculate the cost based on the increased percentage
        int cost = ((PlayerPrefs.GetInt("incomePercentage") - 100) / 10) * 100;

        // Check if the player has enough money for the upgrade
        if (PlayerPrefs.GetInt("Money") >= cost)
        {
            // Decrease money, update income percentage, and update button text
            DecreaseMoney(cost);
            PlayerPrefs.SetInt("incomePercentage", incomePercentage);
            GameObject.Find("IncomeUpgradeButton").GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade " + cost.ToString();
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    // Method to handle end of level
    public void LevelOver()
    {
        // Activate UI elements for upgrades
        GameObject.Find("GameManager/Canvas/RangeUpgrade").SetActive(true);
        GameObject.Find("GameManager/Canvas/RangeUpgrade/RangeUpgradeButton").GetComponentInChildren<TextMeshProUGUI>().text = "Upgrade " + PlayerPrefs.GetInt("RangeUpgradeMoney").ToString();
        GameObject.Find("GameManager/Canvas/IncomeUpgrade").SetActive(true);

        // Reset game state flags
        isGameStarted = false;
        isLevelOver = true;
    }
}
