using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class RouletteGenerator : MonoBehaviour
{
    private int worldWidth = 8, worldHeight = 7;
    private bool[,] rouletteWorldData;
    private int maxValidSolts = 0;

    [SerializeField]
    public Tilemap roulette;
    public Tile rouletteSlotTile, InvalidTile, DebugTile;
    //public GameObject NullSymbolObject;
    public int xOffset = 2, yOffset = 2;
    public int width, height;
    public List<GameObject> toAddSymbolsList;
    public List<GameObject> symbolsList;
    public GameObject[,] symbolRoulette;    //Will become to the int[] array to store symbol identifier 
    //private List<int> symbolRoulette;


    private void Start()
    {
        width = (width + xOffset > worldWidth) ? worldWidth - xOffset : width;
        height = (height + yOffset > worldHeight) ? worldHeight - yOffset : height;
        rouletteWorldData = new bool[worldHeight, worldWidth];
        toAddSymbolsList = new List<GameObject>();
        symbolRoulette = new GameObject[worldHeight, worldWidth];

        for (int i = 0; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                if(i < yOffset + height && i >= yOffset && j < xOffset + width && j >= xOffset)
                {
                    rouletteWorldData[i, j] = true;
                    roulette.SetTile(new Vector3Int(j, i), rouletteSlotTile);
                    maxValidSolts++;
                }
                else roulette.SetTile(new Vector3Int(j, i), InvalidTile);
            }
        }

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Symbol"))
            symbolsList.Add(item);
        foreach (var item in GameObject.FindGameObjectsWithTag("ChangeStateSymbol"))
            symbolsList.Add(item);
        foreach (var item in GameObject.FindGameObjectsWithTag("ChangePositionSymbol"))
            symbolsList.Add(item);
        foreach (var item in GameObject.FindGameObjectsWithTag("RespinSymbol"))
            symbolsList.Add(item);
        foreach (var item in GameObject.FindGameObjectsWithTag("DestroySymbol"))
            symbolsList.Add(item);
        foreach (var item in GameObject.FindGameObjectsWithTag("AddSymbol"))
            symbolsList.Add(item);

        //while (symbolsList.Count < maxValidSolts) symbolsList.Add(Instantiate(NullSymbolObject));

        foreach (var item in symbolsList)
            item.SetActive(false);
    }

/*    private void manageNullSymbol()
    {
        // Add NullSymbols if SymbolList is too short
        while (symbolsList.Count < maxValidSolts)
        {
            symbolsList.Add(Instantiate(NullSymbolObject));
            symbolsList[-1].SetActive(false);
        }

        // Remove NullSymbols if SymbolList is too long
        while (symbolsList.Count > maxValidSolts)
        {
            symbolsList.Remove(NullSymbolObject);
            if (!symbolsList.Contains(NullSymbolObject)) break;
        }
    }*/
    private void InitRoulette()
    {
        maxValidSolts = 0;

        for (int i = 0; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                if (rouletteWorldData[i, j])
                {
                    roulette.SetTile(new Vector3Int(j, i), rouletteSlotTile);
                    maxValidSolts++;
                }
                else roulette.SetTile(new Vector3Int(j, i), InvalidTile);
            }
        }
    }
    private void InitRoulette(int x, int y)
    {
        rouletteWorldData[x, y] = true;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (rouletteWorldData[i, j])
                {
                    roulette.SetTile(new Vector3Int(yOffset + j, xOffset + i), rouletteSlotTile);
                    maxValidSolts++;
                }
            }
        }
    }
    public void lstTotoAdd()
    {
        int randomIndex;

        while (symbolsList.Count > 0 && toAddSymbolsList.Count <= maxValidSolts)
        {
            randomIndex = UnityEngine.Random.Range(0, symbolsList.Count);
            toAddSymbolsList.Add(symbolsList[randomIndex]);
            symbolsList.RemoveAt(randomIndex);
        }
    }
    public void toAddToRoulette()
    {
        int randomIndex;
        List<int[]> validSlots = new List<int[]>();

        //Plz clear the bug here
        for (int i = 0; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                if (rouletteWorldData[i, j] && symbolRoulette[i, j] == null) validSlots.Add(new int[] { i, j });
            }
        }

        while (validSlots.Count > 0 && toAddSymbolsList.Count > 0)
        {
            randomIndex = UnityEngine.Random.Range(0, validSlots.Count);
            Vector3 tileWorldPos = roulette.GetCellCenterWorld(new Vector3Int(validSlots[randomIndex][1], validSlots[randomIndex][0]));
            symbolRoulette[validSlots[randomIndex][0], validSlots[randomIndex][1]] = toAddSymbolsList[0];
            toAddSymbolsList[0].SetActive(true);
            toAddSymbolsList[0].transform.position = tileWorldPos;
            toAddSymbolsList.RemoveAt(0);
            validSlots.RemoveAt(randomIndex);
        }
    }
    public void ClearSymbols()
    {
        //Clear the symbols on roulette
        for (int i = 0; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                if (symbolRoulette[i, j] != null)
                {
                    symbolsList.Add(symbolRoulette[i, j]);
                    symbolRoulette[i, j].SetActive(false);
                }
            }
        }
    }
    public void RouToLst()
    {
        for (int i = 0; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                if (symbolRoulette[i, j] != null)
                {
                    symbolsList.Add(symbolRoulette[i, j]);
                    symbolRoulette[i, j].SetActive(false);
                    symbolRoulette[i, j] = null;
                }
            }
        }

        for (int i = 0; i < toAddSymbolsList.Count; i++)
        {
            symbolsList.Add(toAddSymbolsList[0]);
            toAddSymbolsList.Remove(toAddSymbolsList[0]);
        }
    }

    public void FillSymbolperSpin()
    {
        //将轮盘的符号放回到符号栏
        RouToLst();

        //清理轮盘上的所有符号
        //ClearSymbols();

        //控制空符号的数量
        //manageNullSymbol();

        //初始化轮盘
        InitRoulette();

        //把符号栏的符号加入到符号队列中
        lstTotoAdd();

        //将符号队列的元素加入到轮盘中
        toAddToRoulette();

        //New symbols should be added to toAddSymbolList
        //符号事件交互
        SymbolsEvent();
    }

    public void SymbolsEvent()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("ChangeStateSymbol"))
        {
            item.GetComponent<ChangeStateSymbol>().ChangeSymbolStatus();
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("ChangePositionSymbol"))
        {
            item.GetComponent<ChangePositionSymbol>().ChangeSymbolPosition();
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("RespinSymbol"))
        {
            item.GetComponent<RespinSymbol>().Respin();
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("DestroySymbol"))
        {
            item.GetComponent<DestroySymbol>().DestroySymbols();
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("AddSymbol"))
        {
            item.GetComponent<AddSymbol>().AddSymbols();
        }

        toAddToRoulette();
    }

    public List<int[]> getSurroundingPosition(GameObject target, int pattern)
    {
        int[,] direction = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 } };
        int[] targetPos = new int[2], indexPos = new int[2], temp;
        List<int[]> result = new List<int[]>();

        for (int i = 0; i < symbolRoulette.GetLength(0); i++)
        {
            for (int j = 0; j < symbolRoulette.GetLength(1); j++)
            {
                if (target == symbolRoulette[i, j])
                {
                    targetPos[0] = i;
                    targetPos[1] = j;
                }
            }
        }
        indexPos[0] = targetPos[0];
        indexPos[1] = targetPos[1];
        //Debug.Log((indexPos[0], indexPos[1], symbolRoulette[indexPos[0], indexPos[1]]));

        switch (pattern)
        {
            case 0:
                {
                    for (int i = 0; i < direction.GetLength(0); i++)
                    {
                        indexPos[0] += direction[i, 0];
                        indexPos[1] += direction[i, 1];
                        if (symbolRoulette[indexPos[0], indexPos[1]] != null)
                        {
                            temp = new int[2];
                            temp[0] = indexPos[0];
                            temp[1] = indexPos[1];
                            result.Add(temp);
                            //Debug.Log(symbolRoulette[indexPos[0], indexPos[1]].transform.position);
                        }
                        roulette.SetTile(new Vector3Int(indexPos[1], indexPos[0]), DebugTile);
                        indexPos[0] = targetPos[0];
                        indexPos[1] = targetPos[1];
                    }
                }
                break;
            case 1:
                {
                    for (int i = 1; i < direction.GetLength(0); i += 2)
                    {
                        indexPos[0] += direction[i, 0];
                        indexPos[1] += direction[i, 1];
                        if (symbolRoulette[indexPos[0], indexPos[1]] != null)
                        {
                            temp = new int[2];
                            temp[0] = indexPos[0];
                            temp[1] = indexPos[1];
                            result.Add(temp);
                        }
                        roulette.SetTile(new Vector3Int(indexPos[1], indexPos[0]), DebugTile);
                        indexPos[0] = targetPos[0];
                        indexPos[1] = targetPos[1];
                    }
                }
                break;
            case 2:
                {
                    for (int i = 0; i < direction.GetLength(0); i += 2)
                    {
                        indexPos[0] += direction[i, 0];
                        indexPos[1] += direction[i, 1];
                        if (symbolRoulette[indexPos[0], indexPos[1]] != null)
                        {
                            temp = new int[2];
                            temp[0] = indexPos[0];
                            temp[1] = indexPos[1];
                            result.Add(temp);
                        }
                        roulette.SetTile(new Vector3Int(indexPos[1], indexPos[0]), DebugTile);
                        indexPos[0] = targetPos[0];
                        indexPos[1] = targetPos[1];
                    }
                }
                break;
            case 3:
                {
                    for (int i = 0; i < direction.GetLength(0); i += 2)
                    {
                        indexPos[0] += direction[i, 0];
                        indexPos[1] += direction[i, 1];
                        if (symbolRoulette[indexPos[0], indexPos[1]] != null)
                        {
                            temp = new int[2];
                            temp[0] = indexPos[0];
                            temp[1] = indexPos[1];
                            result.Add(temp);
                        }
                        roulette.SetTile(new Vector3Int(indexPos[1], indexPos[0]), DebugTile);
                        indexPos[0] = targetPos[0];
                        indexPos[1] = targetPos[1];
                    }
                }
                break;
            default:
                {
                    for (int i = 0; i < direction.GetLength(0); i += 2)
                    {
                        indexPos[0] += direction[i, 0];
                        indexPos[1] += direction[i, 1];
                        if (symbolRoulette[indexPos[0], indexPos[1]] != null)
                        {
                            temp = new int[2];
                            temp[0] = indexPos[0];
                            temp[1] = indexPos[1];
                            result.Add(temp);
                        }
                        roulette.SetTile(new Vector3Int(indexPos[1], indexPos[0]), DebugTile);
                        indexPos[0] = targetPos[0];
                        indexPos[1] = targetPos[1];
                    }
                }
                break;
        }

        return result;
    }
    public List<GameObject> getSurroundingObjects(GameObject target, int pattern)
    {
        int[,] direction = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 } };
        int[] targetPos = new int[2], indexPos = new int[2], temp;
        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i < symbolRoulette.GetLength(0); i++)
        {
            for (int j = 0; j < symbolRoulette.GetLength(1); j++)
            {
                if (target == symbolRoulette[i, j])
                {
                    targetPos[0] = i;
                    targetPos[1] = j;
                }
            }
        }
        indexPos[0] = targetPos[0];
        indexPos[1] = targetPos[1];

        switch (pattern)
        {
            case 0:
                {
                    for (int i = 0; i < direction.GetLength(0); i++)
                    {
                        indexPos[0] += direction[i, 0];
                        indexPos[1] += direction[i, 1];
                        if (symbolRoulette[indexPos[0], indexPos[1]] != null)
                        {
                            temp = new int[2];
                            temp[0] = indexPos[0];
                            temp[1] = indexPos[1];
                            result.Add(symbolRoulette[temp[0], temp[1]]);
                        }
                        roulette.SetTile(new Vector3Int(indexPos[1], indexPos[0]), DebugTile);
                        indexPos[0] = targetPos[0];
                        indexPos[1] = targetPos[1];
                    }
                }
                break;
            case 1:
                {
                    for (int i = 1; i < direction.GetLength(0); i += 2)
                    {
                        indexPos[0] += direction[i, 0];
                        indexPos[1] += direction[i, 1];
                        if (symbolRoulette[indexPos[0], indexPos[1]] != null)
                        {
                            temp = new int[2];
                            temp[0] = indexPos[0];
                            temp[1] = indexPos[1];
                            result.Add(symbolRoulette[temp[0], temp[1]]);
                            //Debug.Log(symbolRoulette[indexPos[0], indexPos[1]].transform.position);
                        }
                        roulette.SetTile(new Vector3Int(indexPos[1], indexPos[0]), DebugTile);
                        indexPos[0] = targetPos[0];
                        indexPos[1] = targetPos[1];
                    }
                }
                break;
            case 2:
                {
                    for (int i = 0; i < direction.GetLength(0); i += 2)
                    {
                        indexPos[0] += direction[i, 0];
                        indexPos[1] += direction[i, 1];
                        if (symbolRoulette[indexPos[0], indexPos[1]] != null)
                        {
                            temp = new int[2];
                            temp[0] = indexPos[0];
                            temp[1] = indexPos[1];
                            result.Add(symbolRoulette[temp[0], temp[1]]);
                            //Debug.Log(symbolRoulette[indexPos[0], indexPos[1]].transform.position);
                        }
                        roulette.SetTile(new Vector3Int(indexPos[1], indexPos[0]), DebugTile);
                        indexPos[0] = targetPos[0];
                        indexPos[1] = targetPos[1];
                    }
                }
                break;
            case 3:
                {
                    for (int i = 1; i < direction.GetLength(0); i += 2)
                    {
                        indexPos[0] += direction[i, 0];
                        indexPos[1] += direction[i, 1];
                        if (symbolRoulette[indexPos[0], indexPos[1]] != null)
                        {
                            temp = new int[2];
                            temp[0] = indexPos[0];
                            temp[1] = indexPos[1];
                            result.Add(symbolRoulette[temp[0], temp[1]]);
                            //Debug.Log(symbolRoulette[indexPos[0], indexPos[1]].transform.position);
                        }
                        roulette.SetTile(new Vector3Int(indexPos[1], indexPos[0]), DebugTile);
                        indexPos[0] = targetPos[0];
                        indexPos[1] = targetPos[1];
                    }
                }
                break;
            default:
                {
                    for (int i = 0; i < direction.GetLength(0); i++)
                    {
                        indexPos[0] += direction[i, 0];
                        indexPos[1] += direction[i, 1];
                        if (symbolRoulette[indexPos[0], indexPos[1]] != null)
                        {
                            temp = new int[2];
                            temp[0] = indexPos[0];
                            temp[1] = indexPos[1];
                            result.Add(symbolRoulette[temp[0], temp[1]]);
                            //Debug.Log(symbolRoulette[indexPos[0], indexPos[1]].transform.position);
                        }
                        roulette.SetTile(new Vector3Int(indexPos[1], indexPos[0]), DebugTile);
                        indexPos[0] = targetPos[0];
                        indexPos[1] = targetPos[1];
                    }
                }
                break;
        }

        return result;
    }

    public void DestroySymbol(GameObject target)
    {
        symbolsList.Remove(target);
        //Destroy(target);
    }
}
