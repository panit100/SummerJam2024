using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int column;
    public int row;

    int[][] inventorySlots;

    private void Start()
    {
        InitInventory();
        printIt();
    }

    void InitInventory()
    {
        inventorySlots = new int[row][];

        for (int x = 0; x < inventorySlots.Length; x++)
        {
            inventorySlots[x] = new int[column];
            for (int y = 0; y < inventorySlots[x].Length; y++)
            {
                inventorySlots[x][y] = -1;
            }
        }
    }

    void printIt()
    {
        for (int x = 0; x < inventorySlots.Length; x++)
        {
            string a = "";
            for (int y = 0; y < inventorySlots[x].Length; y++)
            {
                a = a + inventorySlots[x][y].ToString() + " ";
            }

            print(a);
        }
    }
}
