using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureController : MonoBehaviour
{
    private Touch screenTouch;
    private float distance;
    [SerializeField] float rotationSensitivity;
    [SerializeField] float moveSensitivity;
    [SerializeField] float minSignificance;
    [SerializeField] float minResize;
    [SerializeField] float maxResize;

    private Vector3 scale;
    private Vector3 position;
    private Vector3 startPosition;


    bool inBounds = true;

    private void Start()
    {
        GameObject startingObject = this.gameObject;
        startPosition = transform.position;
        scale = transform.localScale;
    }

    void Resize()
    {
        Touch touchOne = Input.GetTouch(0);
        Touch touchTwo = Input.GetTouch(1);

        if(touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
        {
            distance = Vector2.Distance(touchOne.position, touchTwo.position);
            scale = this.gameObject.transform.localScale;
        }
        else
        {
            float newDistance = Vector3.Distance(touchOne.position, touchTwo.position);
            if(distance > 0)
            {
                float scaleRate = newDistance / distance;
                if(scale.z * scaleRate >= minResize && scale.z * scaleRate <= maxResize)
                {
                    this.gameObject.transform.localScale = scale * scaleRate;
                }
            }
        }
    }

    void Translate()
    {
        position = transform.position;
        position.z -= screenTouch.deltaPosition.y * moveSensitivity;
        
        if(position.z > startPosition.z)
        {
            transform.position = position;
        }
    }

    void Rotate()
    {
        Quaternion rotation = Quaternion.Euler(0f, -screenTouch.deltaPosition.x * rotationSensitivity, 0f);
        transform.rotation = rotation * transform.rotation;
    }

    private void Update()
    {
    
            if (Input.touchCount == 1)
            {
                screenTouch = Input.GetTouch(0);
                if (screenTouch.phase == TouchPhase.Moved &&
                        (Mathf.Abs(screenTouch.deltaPosition.x) > minSignificance
                         || Mathf.Abs(screenTouch.deltaPosition.y) > minSignificance))
                {
                    if(Mathf.Abs(screenTouch.deltaPosition.x) > Mathf.Abs(screenTouch.deltaPosition.y))
                    {
                        Rotate();
                    }
                    else
                    {
                        Translate();
                    }
                }
            }
            else if(Input.touchCount == 2)
            {
                Resize();
            }
        }
    
}
