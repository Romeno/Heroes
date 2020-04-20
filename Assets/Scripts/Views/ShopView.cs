using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    /* UNITY GLUE */
    public GameObject shopContentElement;
    public GameObject playerArmyContentElement;
    public GameObject opponentArmyContentElement;
    public Text playerGoldElement;
    public Button fightButton;

    public GameObject shopUnitCard;
    public GameObject armyUnitCard;

    /* END UNITY GLUE*/

    public int playerGold;

    [HideInInspector]
    public List<Unit> boughtUnits;

    [HideInInspector]
    public List<Unit> resultingPlayerUnits;


    public void Activate(bool value)
    {
        gameObject.SetActive(value);
    }


    void Start()
    {
        Debug.Log("smart");

        resultingPlayerUnits = G.session.playerArmy.CloneT().units;
        boughtUnits = new List<Unit>();

        playerGoldElement.text = playerGold.ToString();

        CreateShop();

        CreatePlayerArmy();

        CreateOpponentArmy();
    }


    void CreateShop()
    {
        foreach (var ut in G.game.unitTypes)
        {
            GameObject g = Instantiate(shopUnitCard, shopContentElement.transform);

            RecruitmentUnitCard card = g.GetComponent<RecruitmentUnitCard>();
            card.Init(ut, this);
        }
    }


    public void CreatePlayerArmy()
    {
        playerArmyContentElement.DestroyChildrenImmediate();

        foreach (var u in resultingPlayerUnits)
        {
            GameObject g = Instantiate(armyUnitCard, playerArmyContentElement.transform);

            ArmyUnitCard card = g.GetComponent<ArmyUnitCard>();
            card.Init(u, this);
        }
    }


    void CreateOpponentArmy()
    {
        foreach (var u in G.session.opponentArmy.units)
        {
            GameObject g = Instantiate(armyUnitCard, opponentArmyContentElement.transform);

            ArmyUnitCard card = g.GetComponent<ArmyUnitCard>();
            card.Init(u, this);
        }
    }


    public void UpdatePlayerArmyBar()
    {
        resultingPlayerUnits = G.session.playerArmy.CloneT().units;

        foreach (Unit u in boughtUnits)
        {
            int unitInd = G.session.FindFirstUnitIndexOfType(u.type, resultingPlayerUnits);
            if (unitInd != -1)
            {
                Unit armyUnit = resultingPlayerUnits[unitInd];
                armyUnit.quantity += u.quantity;
            }
            else
            {
                resultingPlayerUnits.Add(u);
            }
        }

        CreatePlayerArmy();
    }


    public int RecalculatePlayerGold()
    {
        int goldLeft = playerGold;

        foreach (Transform t in shopContentElement.transform)
        {
            RecruitmentUnitCard card = t.gameObject.GetComponent<RecruitmentUnitCard>();

            goldLeft -= (int)(card.unitType.data.cost * card.quantity);
        }

        playerGoldElement.text = goldLeft.ToString();

        return goldLeft;
    }


    public void Fight()
    {
        G.game.ActivateBattlefieldView();
        G.session.playerArmy.units = resultingPlayerUnits;
        
        // this view is disabled so we are forced to run coroutine in an active gameobject
        G.game.StartCoroutine(G.game.ExecuteNextFrame(G.game.StartBattle));
    }

    public void Quit()
    {
        ApplicationUtil.Quit();
    }
}
