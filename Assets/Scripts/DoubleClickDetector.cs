// 9/13/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;

public class DoubleClickDetector : MonoBehaviour
{
    private float lastClickTime = 0f; // Thời gian của lần click trước
    private float doubleClickThreshold = 0.3f; // Ngưỡng thời gian giữa hai lần click để tính là double-click

    void OnMouseDown()
    {
        float currentTime = Time.time;

        // Kiểm tra nếu khoảng thời gian giữa hai lần click nhỏ hơn ngưỡng double-click
        if (currentTime - lastClickTime <= doubleClickThreshold)
        {
            OnDoubleClick();
        }

        // Cập nhật thời gian của lần click cuối cùng
        lastClickTime = currentTime;
    }

    private void OnDoubleClick()
    {
       
    }
}
