using Unity.Mathematics;
using Unity.VisualScripting;

public class BulletSettings
{
    // Define a private static variable to create a single instance
    private static BulletSettings instance;

    // Properties of the BulletSettings class
    public float fireRate = 0.5f;
    public float bulletRange = 10f;
    public float nextFireTime = 0;
    public float bulletSpeed = 10f;
    public float bulletDamage = 1;
    public float gunYear = 2000;
    public float rangeUpgradeAmount;
    public float rateUpgradeAmount;

    // Private constructor to prevent external access
    private BulletSettings()
    {
        // Increase fire rate and bullet range during initialization
        IncreaseFireRate(rateUpgradeAmount);
        IncreaseBulletRange(rangeUpgradeAmount);
    }

    // Define a public static method to return a single instance
    public static BulletSettings Instance
    {
        get
        {
            // If the instance has not been created yet, create one
            if (instance == null)
            {
                instance = new BulletSettings();
            }
            return instance;
        }
    }

    // Method to increase fire rate
    public void IncreaseFireRate(float value)
    {
        fireRate += value / 10;
    }

    // Method to increase bullet range
    public void IncreaseBulletRange(float value)
    {
        bulletRange += value;
    }

    // Method to increase gun year and adjust bullet damage accordingly
    public void IncreaseGunYear(float value)
    {
        gunYear += value;
        IncreaseBulletDamage(value % 10);
    }

    // Method to increase bullet damage
    private void IncreaseBulletDamage(float value)
    {
        bulletDamage += value;
    }
}
