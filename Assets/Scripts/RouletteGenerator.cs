using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RouletteGenerator : MonoBehaviour
{
    private int worldWidth = 8, worldHeight = 7;
    private bool[,] rouletteWorldData;
    private int maxValidSolts = 0;
    private List<Tile> toAddSymbolsList;
    private int xOffset = 2, yOffset = 2;

    [SerializeField]
    public Tilemap roulette;
    public Tile rouletteSlotTile;
    public Tile NullSymbolObject;
    public int width, height;
    public List<Tile> symbolsList;

    private void Start()
    {
        width = (width + xOffset > worldWidth) ? worldWidth - xOffset : width;
        height = (height + yOffset > worldHeight) ? worldHeight - yOffset : height;
        rouletteWorldData = new bool[worldHeight, worldWidth];
        toAddSymbolsList = new List<Tile>();

        for (int i = 0; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                rouletteWorldData[i, j] = false;
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                rouletteWorldData[xOffset + i, yOffset + j] = true;
                roulette.SetTile(new Vector3Int(yOffset + j, xOffset + i), rouletteSlotTile);
                maxValidSolts++;
            }
        }

        while (symbolsList.Count < maxValidSolts)
        {
            symbolsList.Add(NullSymbolObject);
        }
    }

    private void InitRoulette()
    {
        maxValidSolts = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                rouletteWorldData[xOffset + i, yOffset + j] = true;
                roulette.SetTile(new Vector3Int(yOffset + j, xOffset + i), rouletteSlotTile);
                maxValidSolts++;
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

    private void manageNullSymbol()
    {
        // Add NullSymbols if SymbolList is too short
        while (symbolsList.Count < maxValidSolts)
        {
            symbolsList.Add(NullSymbolObject);
        }

        // Remove NullSymbols if SymbolList is too long
        while (symbolsList.Count > maxValidSolts)
        {
            symbolsList.Remove(NullSymbolObject);
        }
    }

    public void FillSymbolperSpin()
    {
        toAddSymbolsList.Clear();

        manageNullSymbol();

        // Add symbols from SymbolList to toAddSymbolsList randomly
        for (int i = 0; i < maxValidSolts; i++)
        {
            int randomIndex = Random.Range(0, symbolsList.Count);
            toAddSymbolsList.Add(symbolsList[randomIndex]);
            symbolsList.RemoveAt(randomIndex);
        }

        // Assign symbols from toAddSymbolList to Roulette randomly
        for (int i = 0; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                if (rouletteWorldData[i, j])
                {
                    roulette.SetTile(new Vector3Int(j, i), toAddSymbolsList[0]);
                    symbolsList.Add(toAddSymbolsList[0]);
                    toAddSymbolsList.RemoveAt(0);
                }
            }
        }
    }
}
