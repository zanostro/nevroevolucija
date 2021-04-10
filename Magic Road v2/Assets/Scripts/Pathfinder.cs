using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    static Tile[,] tiles;
    static bool[,] visited;

    public static bool PathExists(Tile[,] input, Vector2Int startPos, Vector2Int endPos)
    {
        tiles = input;
        visited = new bool[tiles.GetLength(0), tiles.GetLength(1)];
        Tile startTile = tiles[startPos.x, startPos.y];
        Tile endTile = tiles[endPos.x, endPos.y];
        if (startTile.tileType != Tile.TileType.road || endTile.tileType != Tile.TileType.road) return false;
        
        return dfs(startPos, endPos);
    }

    public static bool dfs(Vector2Int currentPos, Vector2Int endPos)
    {
        if (currentPos == endPos) return true;

        if (currentPos.x < 0 || currentPos.x >= tiles.GetLength(0)) return false;
        if (currentPos.y < 0 || currentPos.y >= tiles.GetLength(1)) return false;
        if (endPos.x < 0 || endPos.x >= tiles.GetLength(0)) return false;
        if (endPos.y < 0 || endPos.y >= tiles.GetLength(1)) return false;

        if (visited[currentPos.x, currentPos.y]) return false;

        visited[currentPos.x, currentPos.y] = true;

        Tile.SideType side = tiles[currentPos.x, currentPos.y].side[(int)Tile.Direction.N];
        if (side == Tile.SideType.grassRoad || side == Tile.SideType.pavementRoad)
        {
            if (dfs(new Vector2Int(currentPos.x - 1, currentPos.y), endPos)) return true;
        }

        side = tiles[currentPos.x, currentPos.y].side[(int)Tile.Direction.S];
        if (side == Tile.SideType.grassRoad || side == Tile.SideType.pavementRoad)
        {
            if (dfs(new Vector2Int(currentPos.x + 1, currentPos.y), endPos)) return true;
        }

        side = tiles[currentPos.x, currentPos.y].side[(int)Tile.Direction.E];
        if (side == Tile.SideType.grassRoad || side == Tile.SideType.pavementRoad)
        {
            if (dfs(new Vector2Int(currentPos.x, currentPos.y + 1), endPos)) return true;
        }

        side = tiles[currentPos.x, currentPos.y].side[(int)Tile.Direction.W];
        if (side == Tile.SideType.grassRoad || side == Tile.SideType.pavementRoad)
        {
            if (dfs(new Vector2Int(currentPos.x, currentPos.y - 1), endPos)) return true;
        }

        return false;
    }
}
