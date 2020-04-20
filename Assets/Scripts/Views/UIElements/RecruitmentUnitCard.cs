using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RecruitmentUnitCard : MonoBehaviour
{
    public GameObject selectionFrame;
    public Text nameElement;
    public GameObject imageParentElement;
    public GameObject controlsElement;
    public Text costElement;

    [HideInInspector]
    public ShopView buyView;

    [HideInInspector]
    public int quantity;

    [HideInInspector]
    public int prevQuantity;

    private UnitType _unitType;

    public RecruitmentUnitCard()
    {

    }

    void Start()
    {

    }

    public RecruitmentUnitCard(UnitType type, ShopView view)
    {
        Init(type, view);
    }

    public UnitType unitType
    {
        get { return _unitType; }
        set
        {
            _unitType = value;
            nameElement.text = value.name;
            Instantiate(_unitType.unitCardPrefab, imageParentElement.transform);
        }
    }

    public void Init(UnitType type, ShopView view)
    {
        unitType = type;
        buyView = view;
        selectionFrame.SetActive(false);
        controlsElement.SetActive(false);
    }

    public void Select(bool value)
    {
        selectionFrame.SetActive(value);
        controlsElement.SetActive(value);
    }

    public bool IsSelected()
    {
        return selectionFrame.activeSelf;
    }

    public void Clicked()
    {
        selectionFrame.SetActive(!selectionFrame.activeSelf);
        controlsElement.SetActive(!controlsElement.activeSelf);
    }

    public void OnQuantityChanged(string newValue)
    {
        Unit u;

        if (newValue.Length != 0)
        {
            prevQuantity = quantity;
            quantity = int.Parse(newValue);
            
            costElement.text = (unitType.data.cost * quantity).ToString();
            int goldLeft = buyView.RecalculatePlayerGold();
            if (goldLeft < 0)
            {
                buyView.fightButton.enabled = false;
            }

            int unitInd = G.session.FindFirstUnitIndexOfType(unitType, buyView.boughtUnits);
            if (unitInd != -1)
            {
                u = buyView.boughtUnits[unitInd];
                u.quantity = quantity;
            }
            else
            {
                u = new Unit(unitType, quantity) {

                };

                buyView.boughtUnits.Add(u);
            }
            buyView.UpdatePlayerArmyBar();
        }
        else
        {
            costElement.text = "";
        }
    }
}


