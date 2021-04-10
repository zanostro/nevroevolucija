using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveElement
{
    List<WeightedTile> possibleTiles;

    public bool collapsed = false;

    public bool[] coeff;
    public float[] prob;
    float[] logProb;
    float[] probLogProb;

    public float entropy;
    public int coeffSum;
    float weightSum;

    float noise;

    public WaveElement(List<WeightedTile> input)
    {
        possibleTiles = input;

        coeff = new bool[possibleTiles.Count];
        prob = new float[coeff.Length];
        logProb = new float[coeff.Length];
        probLogProb = new float[coeff.Length];

        coeffSum = 0;
        weightSum = 0;

        noise = Random.Range(.99f, 1.01f);
        //Debug.LogError(noise);

        for (int i = 0; i < coeff.Length; i++)
        {
            if (possibleTiles[i].weight > 0)
            {
                coeff[i] = true;
                coeffSum++;
                weightSum += possibleTiles[i].weight;
            }
            else coeff[i] = false;
        }
        CalculateProbAndEntropy();
    }

    void CalculateProbAndEntropy()
    {
        entropy = 0;
        for (int i = 0; i < coeff.Length; i++)
        {
            if (coeff[i])
            {
                prob[i] = possibleTiles[i].weight / weightSum;
                if(1 - prob[i] > 0.0001)
                {
                    logProb[i] = Mathf.Log(prob[i], 2);
                    probLogProb[i] = prob[i] * logProb[i];
                    entropy -= probLogProb[i];
                }
            }
            else prob[i] = 0;
        }
        entropy *= noise;
    }

    public int Collapse()
    {
        int idx = -1;

        float choice = Random.value;
        //Debug.LogError(choice);
        float minRange = 0;
        for (int i = 0; i < coeff.Length; i++)
        {
            float maxRange = minRange + prob[i];
            if (coeff[i] && minRange <= choice && choice < maxRange)
            {
                idx = i;
            }
            else
            {
                Remove(i);
            }
            minRange = maxRange;
        }
        CalculateProbAndEntropy();

        collapsed = true;

        return idx;
    }

    void Remove(int idx)
    {
        if (coeff[idx])
        {
            coeff[idx] = false;
            coeffSum--;
            weightSum -= possibleTiles[idx].weight;
        }
    }

    public bool ChangeToMatch(WaveElement origin, Tile.Direction to)
    {
        if (collapsed) return false;

        bool changed = false;

        for(int i=0; i<this.coeff.Length; i++)
        {
            bool edgeMatchFound = false;
            for(int j=0; j<origin.coeff.Length; j++)
            {
                if (this.coeff[i] && origin.coeff[j])
                {
                    //if (possibleTiles[j].side[(int)to] == possibleTiles[i].side[(int)from])
                    Tile checkedTile = this.possibleTiles[i].tile;
                    Tile originTile = origin.possibleTiles[j].tile;
                    if(checkedTile.Match(originTile, to))
                    {
                        edgeMatchFound = true;
                        break;
                    }
                }
            }

            if (!edgeMatchFound && coeff[i]) //if the edge doesn't match and the tile isn't already disabled
            {
                Remove(i);
                changed = true;
            }
        }

        if (changed) CalculateProbAndEntropy();

        return changed;
    }

    public bool RemoveTilesNotOfType(Tile.TileType allowedType)
    {
        bool changed = false;

        for(int i=0; i<coeff.Length; i++)
        {
            if(coeff[i] && possibleTiles[i].tile.tileType != allowedType)
            {
                Remove(i);
                changed = true;
            }
        }

        if (changed) CalculateProbAndEntropy();

        return changed;
    }

    public int getTileIdx()
    {
        if (coeffSum != 1) return -1;
        else
        {
            for(int i=0; i<coeff.Length; i++)
            {
                if (coeff[i]) return i;
            }
        }
        return -1;
    }
}