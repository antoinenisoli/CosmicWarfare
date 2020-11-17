using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_MainBase : Sc_Building
{
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
}
