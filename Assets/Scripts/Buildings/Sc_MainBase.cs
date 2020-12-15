using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_MainBase : Sc_Building
{
    public override string type => "MainBase";

    [Header("MainBase")]
    [SerializeField] float constructionRadius = 50;
    [SerializeField] Projector projectArea;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, constructionRadius);
    }

    public bool InRange(Vector3 buildingToPlace)
    {
        float dist = Vector3.Distance(transform.position, buildingToPlace);
        return dist < constructionRadius;
    }

    public void SwitchBuildingMode(bool b)
    {
        projectArea.orthographicSize = constructionRadius * 1.15f;
        projectArea.gameObject.SetActive(b);
    }

    public override void Death()
    {
        base.Death();
        switch (myTeam)
        {
            case Team.Player:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case Team.Enemy:
                print("Victory");
                break;
        }
    }

    public override void UseBuilding()
    {
        
    }
}
