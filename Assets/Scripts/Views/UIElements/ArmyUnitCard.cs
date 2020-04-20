using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ArmyUnitCard : MonoBehaviour
{
    public GameObject selectionFrame;
    public Text nameElement;
    public GameObject imageParentElement;
    public Text quantityElement;

    [HideInInspector]
    public ShopView buyView;

    private Unit _unit;

    public ArmyUnitCard()
    {

    }

    public ArmyUnitCard(Unit u, ShopView view)
    {
        Init(u, view);
    }

    public Unit unit
    {
        get { return _unit; }
        set
        {
            _unit = value;
            nameElement.GetComponent<Text>().text = value.type.name;
        }
    }

    public void Init(Unit u, ShopView view)
    {
        buyView = view;
        unit = u;
        //selectionFrame.SetActive(false);
        quantityElement.text = u.quantity.ToString();
        Instantiate(u.type.unitCardPrefab, imageParentElement.transform);
    }

    //public void Select(bool value)
    //{
    //    selectionFrame.SetActive(value);
    //}

    //public bool IsSelected()
    //{
    //    return selectionFrame.activeSelf;
    //}
}
