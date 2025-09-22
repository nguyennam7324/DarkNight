#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(DoubleDoorController))]
public class DoubleDoorControllerEditor : Editor
{
    private enum SelectionMode { None, SelectLeft, SelectRight }
    private SelectionMode currentMode = SelectionMode.None;
    private int controlID;
    private Vector2 scrollPos;
    private Vector3Int leftDoorTempPos;
    private TileBase leftDoorTempTile;

    void OnEnable()
    {
        controlID = GUIUtility.GetControlID(FocusType.Passive);
    }

    void OnSceneGUI()
    {
        DoubleDoorController controller = (DoubleDoorController)target;
        if (controller.tilemap == null) return;

        Event e = Event.current;

        if (currentMode != SelectionMode.None)
        {
            GUIUtility.hotControl = controlID;

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                HandleUtility.AddDefaultControl(controlID);

                Vector2 mousePos = e.mousePosition;
                Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
                Vector3 worldPos = ray.origin;

                Vector3Int tilePos = controller.tilemap.WorldToCell(worldPos);

                if (controller.tilemap.HasTile(tilePos))
                {
                    TileBase clickedTile = controller.tilemap.GetTile(tilePos);

                    if (currentMode == SelectionMode.SelectLeft)
                    {
                        leftDoorTempPos = tilePos;
                        leftDoorTempTile = clickedTile;
                        currentMode = SelectionMode.SelectRight;
                        Debug.Log($"✅ Đã chọn cánh trái: {tilePos}");
                    }
                    else if (currentMode == SelectionMode.SelectRight)
                    {
                        string doorName = $"Cửa_{controller.doubleDoors.Count + 1}";
                        controller.AddDoubleDoor(doorName, leftDoorTempPos, tilePos,
                                                leftDoorTempTile, clickedTile);

                        Debug.Log($"✅ Đã thêm cửa: {doorName}");
                        Debug.Log($"Cánh trái: {leftDoorTempPos}, Cánh phải: {tilePos}");
                        currentMode = SelectionMode.None;
                    }

                    e.Use();
                    Repaint();
                }
            }
        }

        DrawDoorsGizmos(controller);
    }

    void DrawDoorsGizmos(DoubleDoorController controller)
    {
        if (controller.tilemap == null) return;

        for (int i = 0; i < controller.doubleDoors.Count; i++)
        {
            DoubleDoorData door = controller.doubleDoors[i];

            // Màu sắc khác nhau cho từng cửa
            Color doorColor = GetDoorColor(i);
            Handles.color = doorColor;

            // Vẽ cánh trái
            Vector3 leftCenter = controller.tilemap.GetCellCenterWorld(door.leftDoorPosition);
            Handles.DrawWireCube(leftCenter, controller.tilemap.cellSize);
            Handles.Label(leftCenter + Vector3.up * 0.2f, "Trái", EditorStyles.miniLabel);

            // Vẽ cánh phải
            Vector3 rightCenter = controller.tilemap.GetCellCenterWorld(door.rightDoorPosition);
            Handles.DrawWireCube(rightCenter, controller.tilemap.cellSize);
            Handles.Label(rightCenter + Vector3.up * 0.2f, "Phải", EditorStyles.miniLabel);

            // Vẽ đường nối giữa 2 cánh
            Handles.DrawDottedLine(leftCenter, rightCenter, 2f);

            // Hiển thị tên và trạng thái
            Vector3 centerPos = (leftCenter + rightCenter) / 2f;
            string status = door.isOpen ? "MỞ" : "ĐÓNG";
            Handles.Label(centerPos + Vector3.up * 0.5f,
                         $"{door.doorName}\n{status}",
                         new GUIStyle(EditorStyles.boldLabel)
                         {
                             normal = { textColor = doorColor },
                             fontSize = 10
                         });
        }
    }

    Color GetDoorColor(int index)
    {
        Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.cyan, Color.magenta };
        return colors[index % colors.Length];
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DoubleDoorController controller = (DoubleDoorController)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("🚪 QUẢN LÝ CỬA 2 CÁNH", EditorStyles.boldLabel);

        if (controller.tilemap == null)
        {
            EditorGUILayout.HelpBox("❌ Chưa gán Tilemap!", MessageType.Error);
            return;
        }

        // Nút thêm cửa
        EditorGUILayout.LabelField("Thêm cửa mới:", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("🎯 Chọn Cánh Trái"))
        {
            currentMode = SelectionMode.SelectLeft;
            Debug.Log("🖱️ Click vào tile cho cánh TRÁI");
        }
        if (GUILayout.Button("🎯 Chọn Cánh Phải"))
        {
            currentMode = SelectionMode.SelectRight;
            Debug.Log("🖱️ Click vào tile cho cánh PHẢI");
        }
        GUILayout.EndHorizontal();

        if (currentMode != SelectionMode.None)
        {
            EditorGUILayout.HelpBox($"Đang chọn: {(currentMode == SelectionMode.SelectLeft ? "Cánh Trái" : "Cánh Phải")}", MessageType.Warning);
        }

        EditorGUILayout.Space();

        // DANH SÁCH CỬA
        EditorGUILayout.LabelField("📋 DANH SÁCH CỬA", EditorStyles.boldLabel);

        if (controller.doubleDoors.Count == 0)
        {
            EditorGUILayout.HelpBox("Chưa có cửa nào. Chọn cánh trái và phải để thêm cửa.", MessageType.Warning);
        }
        else
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));

            for (int i = 0; i < controller.doubleDoors.Count; i++)
            {
                DoubleDoorData door = controller.doubleDoors[i];
                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.BeginHorizontal();
                // Nút chọn cửa này
                if (GUILayout.Button("👉", GUILayout.Width(30)))
                {
                    controller.selectedDoorIndex = i;
                }

                EditorGUILayout.LabelField($"{i}. {door.doorName}", EditorStyles.boldLabel);
                EditorGUILayout.LabelField(door.isOpen ? "🟢 Mở" : "🔴 Đóng", GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField($"Cánh trái: {door.leftDoorPosition} - {door.leftClosedTile?.name}");
                EditorGUILayout.LabelField($"Cánh phải: {door.rightDoorPosition} - {door.rightClosedTile?.name}");

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndScrollView();
        }

        EditorGUILayout.Space();

        // ĐIỀU KHIỂN CỬA ĐANG CHỌN
        if (controller.doubleDoors.Count > 0)
        {
            EditorGUILayout.LabelField($"🎯 CỬA ĐANG CHỌN: {controller.selectedDoorIndex}", EditorStyles.boldLabel);
            EditorGUILayout.LabelField(controller.GetDoorInfo(controller.selectedDoorIndex));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("🚪 Mở Cửa Này", GUILayout.Height(30)))
            {
                controller.OpenDoor(controller.selectedDoorIndex);
            }
            if (GUILayout.Button("🚪 Đóng Cửa Này", GUILayout.Height(30)))
            {
                controller.CloseDoor(controller.selectedDoorIndex);
            }
            if (GUILayout.Button("🔄 Toggle Cửa Này", GUILayout.Height(30)))
            {
                controller.ToggleDoor(controller.selectedDoorIndex);
            }
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        // ĐIỀU KHIỂN TẤT CẢ
        if (controller.doubleDoors.Count > 0)
        {
            EditorGUILayout.LabelField("🎮 ĐIỀU KHIỂN TẤT CẢ", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("✅ Mở Tất Cả", GUILayout.Height(30)))
            {
                controller.OpenAllDoors();
            }
            if (GUILayout.Button("❌ Đóng Tất Cả", GUILayout.Height(30)))
            {
                controller.CloseAllDoors();
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif