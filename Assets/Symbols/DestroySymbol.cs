using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DestroySymbol : ValueSymbol
{
    public GameObject Roulette;
    public List<GameObject> destroyContent;
    private RouletteGenerator rou;

    private void Awake()
    {
        Roulette = GameObject.Find("Roulette");
        rou = Roulette.GetComponent<RouletteGenerator>();
    }

    public void DestroySymbols()
    {
        List<GameObject> surroundingTargets = rou.getSurroundingObjects(gameObject, 0);

        for (int i = 0; i < surroundingTargets.Count; i++)
        {
            for (int j = 0; j < destroyContent.Count; j++)
            {
                if (surroundingTargets[i].GetComponent<ValueSymbol>().GetType() == destroyContent[j].GetComponent<ValueSymbol>().GetType())
                {
                    rou.symbolsList.Remove(surroundingTargets[i]);
                    Destroy(surroundingTargets[i]);
                }
            }
        }
/*        GameObject target = GameObject.Instantiate(symbolToAdd[0]);

        rou.toAddSymbolsList.Add(target);
        target.SetActive(false);*/
        //Debug.Log("Add a Symbol");


    }
}
