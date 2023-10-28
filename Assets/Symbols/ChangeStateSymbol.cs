using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ChangeStateSymbol : ValueSymbol
{
    public GameObject Roulette;
    public GameObject changeToSymbol;
    private GameObject target;

    private void Awake()
    {
        Roulette = GameObject.Find("Roulette");
    }

    public void ChangeSymbolStatus()
    {
        RouletteGenerator rou = Roulette.GetComponent<RouletteGenerator>();

        foreach (var item in rou.getSurroundingPosition(this.gameObject, 0))
        {
            target = rou.symbolRoulette[item[0], item[1]];
            rou.symbolRoulette[item[0], item[1]] = GameObject.Instantiate(changeToSymbol, target.transform.position, new Quaternion());
            rou.symbolsList.Add(target);
            rou.symbolsList.Remove(target);
        }
    }

}
