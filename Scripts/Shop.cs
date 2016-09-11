using UnityEngine;

public class Shop : MonoBehaviour
{
    public int turretPrice, missileLauncherPrice;
    TextShower ts;

    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
        ts = TextShower.instance;
    }

    public void PurchaseStandardTurret()
    {
        Purchase("Turret", turretPrice, buildManager.standardTurretPrefab);
    }

    public void PurchaseMissileLauncher()
    {
        Purchase("Missile Launcher", missileLauncherPrice, buildManager.missileLauncherPrefab);
    }

    void Purchase(string name, int price, GameObject gun)
    {
        if(buildManager.GetTurretToBuild() != null)
        {
            ts.Show("Place the turret before buying a new one!");
            return;
        }

        if (ShopManager.money >= price)
        {
            buildManager.SetTurretToBuild(gun);
            ShopManager.money -= price;
            Debug.Log(name + " Selected");
        }
        else
        {
            ts.Show("Not enough money");
        }
    }
}