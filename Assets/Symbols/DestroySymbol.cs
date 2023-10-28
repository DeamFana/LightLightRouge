using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySymbol : MonoBehaviour
{
    public GameObject Roulette;
    public List<GameObject> symbolToAdd;
    private RouletteGenerator rou;

    private void Awake()
    {
        Roulette = GameObject.Find("Roulette");
        rou = Roulette.GetComponent<RouletteGenerator>();
    }

    public void DestroySymbols()
    {
/*        GameObject target = GameObject.Instantiate(symbolToAdd[0]);

        rou.toAddSymbolsList.Add(target);
        target.SetActive(false);*/
        //Debug.Log("Add a Symbol");
    }
}
