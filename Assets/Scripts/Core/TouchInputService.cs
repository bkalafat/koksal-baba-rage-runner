using System;
using UnityEngine;

namespace KoksalBaba.Core
{
    /// <summary>
    /// Concrete implementation of IInputService for touch/mouse input.
    /// Supports both mobile touch and mouse clicks for editor testing.
    /// </summary>
    public class TouchInputService : IInputService
    {
        public event Action OnTap;

        public void Initialize()
        {
            Debug.Log("TouchInputService: Initialized");
        }

        public void Update()
        {
            // Check for touch input (mobile)
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    OnTap?.Invoke();
                }
            }
            // Fallback to mouse input (editor/desktop testing)
            else if (Input.GetMouseButtonDown(0))
            {
                OnTap?.Invoke();
            }
        }
    }
}
