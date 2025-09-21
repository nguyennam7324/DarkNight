using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class DoubleDoorController : MonoBehaviour
{
    [Header("References")]
    public Tilemap tilemap;

    [Header("Danh sách cửa 2 cánh")]
    public List<DoubleDoorData> doubleDoors = new List<DoubleDoorData>();

    [Header("Cửa đang chọn")]
    public int selectedDoorIndex = 0;
    internal static DoubleDoorController instance;

    private void Awake()
    {
        instance = this;
    }
    // Thêm cửa mới
    public void AddDoubleDoor(string name, Vector3Int leftPos, Vector3Int rightPos,
                             TileBase leftClosed, TileBase rightClosed,
                             TileBase leftOpen = null, TileBase rightOpen = null)
    {
        doubleDoors.Add(new DoubleDoorData(name, leftPos, rightPos, leftClosed, rightClosed, leftOpen, rightOpen));
    }

    // Mở cửa cụ thể
    public void OpenDoor(int doorIndex)
    {
        if (!IsValidDoorIndex(doorIndex)) return;

        DoubleDoorData door = doubleDoors[doorIndex];

        // Đặt tile mở cho cả 2 cánh
        tilemap.SetTile(door.leftDoorPosition, door.leftOpenTile != null ? door.leftOpenTile : null);
        tilemap.SetTile(door.rightDoorPosition, door.rightOpenTile != null ? door.rightOpenTile : null);

        door.isOpen = true;
        Debug.Log($"✅ Đã mở cửa: {door.doorName}");
    }

    public void OpenNextDoor()
    {
        OpenDoor(selectedDoorIndex);
        selectedDoorIndex++;
    }

    // Đóng cửa cụ thể
    public void CloseDoor(int doorIndex)
    {
        if (!IsValidDoorIndex(doorIndex)) return;

        DoubleDoorData door = doubleDoors[doorIndex];

        // Đặt tile đóng cho cả 2 cánh
        tilemap.SetTile(door.leftDoorPosition, door.leftClosedTile);
        tilemap.SetTile(door.rightDoorPosition, door.rightClosedTile);

        door.isOpen = false;
        Debug.Log($"✅ Đã đóng cửa: {door.doorName}");
    }

    // Toggle cửa cụ thể
    public void ToggleDoor(int doorIndex)
    {
        if (!IsValidDoorIndex(doorIndex)) return;

        DoubleDoorData door = doubleDoors[doorIndex];
        if (door.isOpen)
        {
            CloseDoor(doorIndex);
        }
        else
        {
            OpenDoor(doorIndex);
        }
    }

    // Mở tất cả cửa
    public void OpenAllDoors()
    {
        foreach (var door in doubleDoors)
        {
            tilemap.SetTile(door.leftDoorPosition, door.leftOpenTile != null ? door.leftOpenTile : null);
            tilemap.SetTile(door.rightDoorPosition, door.rightOpenTile != null ? door.rightOpenTile : null);
            door.isOpen = true;
        }
        Debug.Log("✅ Đã mở tất cả cửa");
    }

    // Đóng tất cả cửa
    public void CloseAllDoors()
    {
        foreach (var door in doubleDoors)
        {
            tilemap.SetTile(door.leftDoorPosition, door.leftClosedTile);
            tilemap.SetTile(door.rightDoorPosition, door.rightClosedTile);
            door.isOpen = false;
        }
        Debug.Log("✅ Đã đóng tất cả cửa");
    }

    // Kiểm tra index hợp lệ
    private bool IsValidDoorIndex(int index)
    {
        if (tilemap == null)
        {
            Debug.LogError("❌ Tilemap reference missing!");
            return false;
        }

        if (index < 0 || index >= doubleDoors.Count)
        {
            Debug.LogError($"❌ Door index {index} không hợp lệ!");
            return false;
        }

        return true;
    }

    // Lấy thông tin cửa
    public string GetDoorInfo(int index)
    {
        if (!IsValidDoorIndex(index)) return "Invalid door";

        DoubleDoorData door = doubleDoors[index];
        return $"{door.doorName} - Trái: {door.leftDoorPosition}, Phải: {door.rightDoorPosition} - {(door.isOpen ? "Mở" : "Đóng")}";
    }

    private void Update()
    {
       
    }
}