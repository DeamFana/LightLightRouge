using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSymbol : ValueSymbol
{
    public GameObject Roulette;
    public GameObject symbolToAdd;
    private RouletteGenerator rou;


    private void Awake()
    {
        Roulette = GameObject.Find("Roulette");
        rou = Roulette.GetComponent<RouletteGenerator>();
    }

    public void AddSymbols()
    {
        GameObject target = GameObject.Instantiate(symbolToAdd);

        rou.toAddSymbolsList.Add(target);
        target.SetActive(false);
        //Debug.Log("Add a Symbol");
    }
}
