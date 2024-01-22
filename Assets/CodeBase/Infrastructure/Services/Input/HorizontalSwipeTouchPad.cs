using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleInputNamespace
{
    [RequireComponent( typeof( SimpleInputMultiDragListener ) )]
    public class HorizontalSwipeTouchPad : SelectivePointerInput, ISimpleInputDraggableMultiTouch
    {
        public SimpleInput.AxisInput xAxis = new SimpleInput.AxisInput( "Horizontal" );

        public bool invertHorizontal;
        public float deltaTreshold = 3f;

        private SimpleInputMultiDragListener eventReceiver;

        public int Priority { get { return 1; } }

        private Vector2 m_value = Vector2.zero;
        public Vector2 Value { get { return m_value; } }

        private void Awake()
        {
            eventReceiver = GetComponent<SimpleInputMultiDragListener>();
        }

        private void OnEnable()
        {
            eventReceiver.AddListener( this );

            xAxis.StartTracking();
        }

        private void OnDisable()
        {
            eventReceiver.RemoveListener( this );

            xAxis.StopTracking();
        }

        public bool OnUpdate( List<PointerEventData> mousePointers, List<PointerEventData> touchPointers, ISimpleInputDraggableMultiTouch activeListener )
        {
            xAxis.value = 0f;

            if( activeListener != null && activeListener.Priority > Priority )
                return false;

            PointerEventData pointer = GetSatisfyingPointer( mousePointers, touchPointers );
            if( pointer == null )
                return false;

            Debug.Log(pointer.delta.x);
            
            if (Mathf.Abs(pointer.delta.x) < deltaTreshold)
            {
                return false;
            }

            var clumpedX = Mathf.Clamp(pointer.delta.x, -1, 1);
            m_value = new Vector2(clumpedX, 0);

            xAxis.value = invertHorizontal ? -m_value.x : m_value.x;

            return true;
        }
    }
}