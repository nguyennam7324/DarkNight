using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class DoubleDoorData
{
    public string doorName; // Tên cửa
    public Vector3Int leftDoorPosition; // Vị trí cánh trái
    public Vector3Int rightDoorPosition; // Vị trí cánh phải
    public TileBase leftClosedTile; // Tile đóng cánh trái
    public TileBase rightClosedTile; // Tile đóng cánh phải
    public TileBase leftOpenTile; // Tile mở cánh trái (có thể null)
    public TileBase rightOpenTile; // Tile mở cánh phải (có thể null)
    public bool isOpen = false; // Trạng thái

    public DoubleDoorData(string name, Vector3Int leftPos, Vector3Int rightPos,
                         TileBase leftClosed, TileBase rightClosed,
                         TileBase leftOpen = null, TileBase rightOpen = null)
    {
        doorName = name;
        leftDoorPosition = leftPos;
        rightDoorPosition = rightPos;
        leftClosedTile = leftClosed;
        rightClosedTile = rightClosed;
        leftOpenTile = leftOpen;
        rightOpenTile = rightOpen;
    }
}