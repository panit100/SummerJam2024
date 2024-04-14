using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryPanel : Singleton<InventoryPanel>
{
    public InventoryGrid gridPrefab;
    public RectTransform gridContainer;
    public int row;
    public int column;

    public Vector2 startPos;
    public float offset = 100f;

    public RectTransform itemContainer;

    int[][] inventorySlots;

    InventoryGrid[,] grids;

    public List<Item> items = new List<Item>();

    protected override void InitAfterAwake()
    {
    }

    private void Start()
    {
        InitInventory();

        AddItemToInventory(0, 0, 0);
    }

    void AddItemToInventory(int id, int gridX, int gridY)
    {
        var newItem = ItemManager.Instance.createItem(id);
        StoreItem(gridX, gridY, newItem);
        newItem.EnableOnClickItem(false);
    }

    void InitInventory()
    {
        inventorySlots = new int[row][];
        grids = new InventoryGrid[row, column];

        for (int x = 0; x < inventorySlots.Length; x++)
        {
            inventorySlots[x] = new int[column];
            for (int y = 0; y < inventorySlots[x].Length; y++)
            {
                inventorySlots[x][y] = -1;
                CreateGrid(new Vector2(x, y));
            }
        }
    }

    void CreateGrid(Vector2 gridIndex)
    {
        var gridPos = GetPositionByGrid(gridIndex);
        var newGrid = Instantiate(gridPrefab, gridContainer);
        newGrid.rect.localPosition = gridPos;
        newGrid.Init((int)gridIndex.x, (int)gridIndex.y);
        newGrid.onClickGrid += OnClickGrid;
        grids[(int)gridIndex.x, (int)gridIndex.y] = newGrid;
    }

    Vector2 GetPositionByGrid(Vector2 gridPos)
    {
        float xPos = startPos.x + (gridPos.x * offset);
        float yPos = startPos.y - (gridPos.y * offset);
        return new Vector2(xPos, yPos);
    }

    void StoreItem(int gridX, int gridY, Item item)
    {
        var itemRow = item.isRotate == false ? item.ItemData.row : item.ItemData.column;
        var itemColumn = item.isRotate == false ? item.ItemData.column : item.ItemData.row;

        for (int x = 0; x < itemRow; x++)
        {
            int _gridX = gridX + x;
            for (int y = 0; y < itemColumn; y++)
            {
                int _gridY = gridY + y;
                if (_gridX >= inventorySlots.Length || _gridY >= inventorySlots[_gridX].Length)
                {
                    print("Index out of range");
                }
                else
                {
                    inventorySlots[_gridX][_gridY] = 0;
                    grids[_gridX, _gridY].StoreItem(item);

                }
            }
        }

        Vector2 gridPos = new Vector2(gridX, gridY);
        item.SetPosition(gridPos, GetPositionByGrid(gridPos));
        item.rect.SetParent(itemContainer);
        items.Add(item);
    }

    public void OnPickupItem(Item item)
    {
        PlayerManager.Instance.OnPickUpItem(item);
        items.Remove(item);
    }

    bool isGridAvailable(int girdX, int gridY, Item item)
    {
        var itemRow = item.isRotate == false ? item.ItemData.row : item.ItemData.column;
        var itemColumn = item.isRotate == false ? item.ItemData.column : item.ItemData.row;

        for (int x = 0; x < itemRow; x++)
        {
            int _gridX = girdX + x;
            for (int y = 0; y < itemColumn; y++)
            {
                int _gridY = gridY + y;
                if (_gridX >= inventorySlots.Length || _gridY >= inventorySlots[_gridX].Length)
                {
                    print("Index out of range");
                    return false;
                }
                else if (inventorySlots[_gridX][_gridY] != -1)
                {
                    print("Grid not Available");
                    return false;
                }
            }
        }

        return true;
    }

    void OnClickGrid(int x, int y)
    {
        if (inventorySlots[x][y] == -1)
        {
            if (PlayerManager.Instance.holdingItem == null)
                return;

            var item = PlayerManager.Instance.holdingItem;


            if (!isGridAvailable(x, y, item))
            {
                print("Can't put this item to this slot");
                return;
            }

            StoreItem(x, y, item);
            PlayerManager.Instance.OnPutDownItem();
            return;
        }

        if (inventorySlots[x][y] == 0)
        {
            if (PlayerManager.Instance.holdingItem != null)
                return;

            var grid = grids[x, y];
            Item item = grid.GetItem();

            RemoveItem(item);

            OnPickupItem(item);
        }



    }

    void RemoveItem(Item item)
    {
        var itemRow = item.isRotate == false ? item.ItemData.row : item.ItemData.column;
        var itemColumn = item.isRotate == false ? item.ItemData.column : item.ItemData.row;

        for (int x = 0; x < itemRow; x++)
        {
            int _gridX = item.gridX + x;
            for (int y = 0; y < itemColumn; y++)
            {
                int _gridY = item.gridY + y;
                inventorySlots[_gridX][_gridY] = -1;
                grids[_gridX, _gridY].StoreItem(null);
            }
        }
    }

    void printGrid()
    {
        for (int y = 0; y < inventorySlots[0].Length; y++)
        {
            string a = "";
            for (int x = 0; x < inventorySlots.Length; x++)
            {
                a = a + inventorySlots[x][y].ToString() + " ";
            }
            print(a);
        }

    }
}
