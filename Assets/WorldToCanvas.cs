using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldToCanvas : MonoBehaviour
{
    [SerializeField] GameObject worldObj;
    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] Camera Cam;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 ViewportPosition = Cam.WorldToViewportPoint(worldObj.transform.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * canvasRectTransform.sizeDelta.x) - (canvasRectTransform.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * canvasRectTransform.sizeDelta.y) - (canvasRectTransform.sizeDelta.y * 0.5f)));

        rectTransform.anchoredPosition = WorldObject_ScreenPosition;
    }

}
