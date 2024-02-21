// Copyright 2024, Fedir Khodchenko, All rights reserved.
using UnityEngine;

[ExecuteInEditMode]
public class UISafeArea : MonoBehaviour
{
    void Awake()
    {
        ApplySafeArea();
    }

    private void OnRectTransformDimensionsChange()
    {
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        var safeAreaTransform = transform.Find("SafeArea") as RectTransform;
        if (safeAreaTransform == null)
            return;

        var canvas = GetComponent<Canvas>();
        var safeArea = Screen.safeArea;

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        safeAreaTransform.anchorMin = anchorMin;
        safeAreaTransform.anchorMax = anchorMax;
    }
}
