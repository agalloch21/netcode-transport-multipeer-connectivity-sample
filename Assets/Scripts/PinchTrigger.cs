using UnityEngine;
using Unity.PolySpatial.InputDevices;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Events;


public class PinchTrigger : MonoBehaviour
{
    [SerializeField]
    UnityEvent<Vector3> onPinch;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void Update()
    {
        if (Touch.activeTouches.Count > 0)
        {
            foreach (var touch in Touch.activeTouches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    SpatialPointerState touchData = EnhancedSpatialPointerSupport.GetPointerState(touch);

                    // https://docs.unity3d.com/Packages/com.unity.polyspatial.visionos@1.0/manual/PolySpatialInput.html
                    // onPinch?.Invoke(touchData.interactionPosition);
                    onPinch?.Invoke(touchData.inputDevicePosition); 
                    break;
                }
            }
        }
    }
}

