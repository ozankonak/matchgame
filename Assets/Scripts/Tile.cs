using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour 
{
    public int xIndex;
    public int yIndex;

    public bool clicked = false;

    Board myBoard = null;

    public void Init(int x, int y, Board board)
    {
        xIndex = x;
        yIndex = y;
        myBoard = board;
    }

}
