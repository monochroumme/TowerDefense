using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Vector3 TurretPositionOffset;
    public Vector3 MissileLauncherPositionOffset;

    TextShower ts;
    GameObject turret;
    Renderer rend;
    Color startColor;

    BuildManager buildManager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
        ts = TextShower.instance;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (buildManager.GetTurretToBuild() == null)
            return;

        if (turret != null)
        {
            ts.Show("Can't build there!");
            return;
        }
        
        GameObject turretToBuild = buildManager.GetTurretToBuild();
        if(turretToBuild.name == "Turret")
            turret = (GameObject)Instantiate(turretToBuild, transform.position + TurretPositionOffset, transform.rotation);
        else if(turretToBuild.name == "MissileLauncher")
            turret = (GameObject)Instantiate(turretToBuild, transform.position + MissileLauncherPositionOffset, transform.rotation);

        buildManager.SetTurretToBuild(null);
    }

    void OnMouseEnter()
    {
        if (buildManager == null)
        {
            Start();
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (buildManager.GetTurretToBuild() == null)
            return;

        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}