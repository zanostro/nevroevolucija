using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum Symmetry
    {
        I,
        X,
        L
    };

    public enum Direction
    {
        N,
        E,
        S,
        W,
    };

    public enum TileType
    {
        empty,
        road,
        building
    };

    //mirrored edges are even-odd pairs (0-1, 2-3, ...)
    public enum SideType
    {
        grass=0,
        grassRoad=2,
        pavement=4,
        pavementRoad=6,
        grassToPavement = 8,
        pavementToGrass = 9
    };

    public enum ConnType
    {
        same,
        mirror
    };



    public Symmetry symmetry;
    public Direction rotation;
    public TileType tileType;
    public SideType[] side = new SideType[4];
    public ConnType[] connectionType = new ConnType[4];



    static Direction OppositeDirection(Direction dir)
    {
        return (Direction)( ((int)dir + 2) % 4 );
    }
    
    public void Rotate()
    {
        rotation = (Direction)((int)(rotation + 1) % 4);

        SideType tempSide = side[3];
        ConnType tempConnType = connectionType[3];

        for(int i=3; i>0; i--)
        {
            side[i] = side[i-1];
            connectionType[i] = connectionType[i-1];
        }

        side[0] = tempSide;
        connectionType[0] = tempConnType;
    }
    
    public bool Match(Tile origin, Direction to)
    {
        Direction from = OppositeDirection(to);
        
        if(connectionType[(int)from] == ConnType.mirror)
        {
            //mirrored edges are even-odd pairs
            if ((int)this.side[(int)from] % 2 == 0) return (this.side[(int)from] == origin.side[(int)to] - 1);
            else return (this.side[(int)from] - 1) == origin.side[(int)to];
        }

        //if(conn == same) if (tiles same) return true : return false
        return this.side[(int)from] == origin.side[(int)to];
    }
}
