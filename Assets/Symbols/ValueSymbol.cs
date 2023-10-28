using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ValueSymbol : MonoBehaviour
{
    [SerializeField]
    private string symbolName;
    public int quality;     //0 = common, 1 = useful, 2 = uncommon, 3 = rare , 4 = treasure
    public int value;
    private string description;
}
