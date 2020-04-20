using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitFE : MonoBehaviour
{
    [Tooltip("element that will be used as a guide to pisition UI element")]
    public GameObject unitQuantityPos;

    [HideInInspector]
    public Unit unit;

    private Text unitQuantityElement;

    Heroes game;

    void Start()
    {
        game = Camera.main.gameObject.GetComponent<Heroes>();

        unitQuantityElement = Instantiate(game.unitQuantityElement, game.battlefieldUIParent.transform);
        unitQuantityElement.text = unit.curData.quantity.ToString();
    }

    void Update()
    {
        Vector3 pos = game.GetComponent<Camera>().WorldToScreenPoint(unitQuantityPos.transform.position);
        unitQuantityElement.transform.position = pos;
    }

    
}
