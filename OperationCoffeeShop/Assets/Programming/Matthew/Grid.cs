using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
   public int columnLength, rowLenght;
   public float x_Space, y_Space;
   public Grid(int columnLength,int rowLenght,float xSpace, float ySpace)
   {
      this.columnLength = columnLength;
      this.rowLenght = rowLenght;
      x_Space = xSpace;
      y_Space = ySpace;
   }

   public Vector3 GridLocation(int i,float xStart,float yStart)
   {
      return new Vector3(xStart+(x_Space * (i % columnLength)),yStart+(y_Space * (i / columnLength)));
   }
}
