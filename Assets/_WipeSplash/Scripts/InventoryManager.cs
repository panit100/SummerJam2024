using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int column;
    public int row;

    int[][] inventorySlots;

    public List<Item> items = new List<Item>();

    private void Start()
    {
        InitInventory();

        for (int i = 0; i < items.Count; i++)
        {
            StoreItem(items[i], i);
        }

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

    void StoreItem(Item item, int index)
    {
        for (int x = 0; x < item.row; x++)
        {
            int gridX = item.gridX + x;
            for (int y = 0; y < item.column; y++)
            {
                int gridY = item.gridY + y;
                inventorySlots[gridX][gridY] = index;
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
