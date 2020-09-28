// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.StandaloneInputModule
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2216A18B-AF52-44A5-85A0-A1CAA19C1090
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEngine.UI.dll

using System;
using UnityEngine.Serialization;

namespace UnityEngine.EventSystems
{
    /// <summary>
    ///   <para>A BaseInputModule designed for mouse  keyboard  controller input.</para>
    /// </summary>
    [AddComponentMenu("Event/Custom Input Module")]
    public class CustomInputModule : PointerInputModule
    {
        [SerializeField]
        private string m_HorizontalAxis = "Horizontal";
        [SerializeField]
        private string m_VerticalAxis = "Vertical";
        [SerializeField]
        private string m_SubmitButton = "Submit";
        [SerializeField]
        private string m_CancelButton = "Cancel";
        [SerializeField]
        private float m_InputActionsPerSecond = 10f;
        [SerializeField]
        private float m_RepeatDelay = 0.5f;
        private float m_PrevActionTime;
        private Vector2 m_LastMoveVector;
        private int m_ConsecutiveMoveCount;
        private Vector2 m_LastMousePosition;
        private Vector2 m_MousePosition;
        [SerializeField]
        [FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
        private bool m_ForceModuleActive;

        [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
        public CustomInputModule.InputMode inputMode
        {
            get
            {
                return CustomInputModule.InputMode.Mouse;
            }
        }

        /// <summary>
        ///   <para>Is this module allowed to be activated if you are on mobile.</para>
        /// </summary>
        [Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
        public bool allowActivationOnMobileDevice
        {
            get
            {
                return this.m_ForceModuleActive;
            }
            set
            {
                this.m_ForceModuleActive = value;
            }
        }

        /// <summary>
        ///   <para>Force this module to be active.</para>
        /// </summary>
        public bool forceModuleActive
        {
            get
            {
                return this.m_ForceModuleActive;
            }
            set
            {
                this.m_ForceModuleActive = value;
            }
        }

        /// <summary>
        ///   <para>Number of keyboard / controller inputs allowed per second.</para>
        /// </summary>
        public float inputActionsPerSecond
        {
            get
            {
                return this.m_InputActionsPerSecond;
            }
            set
            {
                this.m_InputActionsPerSecond = value;
            }
        }

        /// <summary>
        ///   <para>Delay in seconds before the input actions per second repeat rate takes effect.</para>
        /// </summary>
        public float repeatDelay
        {
            get
            {
                return this.m_RepeatDelay;
            }
            set
            {
                this.m_RepeatDelay = value;
            }
        }

        /// <summary>
        ///   <para>Input manager name for the horizontal axis button.</para>
        /// </summary>
        public string horizontalAxis
        {
            get
            {
                return this.m_HorizontalAxis;
            }
            set
            {
                this.m_HorizontalAxis = value;
            }
        }

        /// <summary>
        ///   <para>Input manager name for the vertical axis.</para>
        /// </summary>
        public string verticalAxis
        {
            get
            {
                return this.m_VerticalAxis;
            }
            set
            {
                this.m_VerticalAxis = value;
            }
        }

        /// <summary>
        ///   <para>Maximum number of input events handled per second.</para>
        /// </summary>
        public string submitButton
        {
            get
            {
                return this.m_SubmitButton;
            }
            set
            {
                this.m_SubmitButton = value;
            }
        }

        /// <summary>
        ///   <para>Input manager name for the 'cancel' button.</para>
        /// </summary>
        public string cancelButton
        {
            get
            {
                return this.m_CancelButton;
            }
            set
            {
                this.m_CancelButton = value;
            }
        }

        protected CustomInputModule()
        {
        }

        /// <summary>
        ///   <para>See BaseInputModule.</para>
        /// </summary>
        public override void UpdateModule()
        {
            this.m_LastMousePosition = this.m_MousePosition;
            this.m_MousePosition = (Vector2)Input.mousePosition;
        }

        /// <summary>
        ///   <para>See BaseInputModule.</para>
        /// </summary>
        /// <returns>
        ///   <para>Supported.</para>
        /// </returns>
        public override bool IsModuleSupported()
        {
            if (!this.m_ForceModuleActive && !Input.mousePresent)
                return Input.touchSupported;
            return true;
        }

        /// <summary>
        ///   <para>See BaseInputModule.</para>
        /// </summary>
        /// <returns>
        ///   <para>Should activate.</para>
        /// </returns>
        public override bool ShouldActivateModule()
        {
            if (!base.ShouldActivateModule())
                return false;
            bool flag = this.m_ForceModuleActive | Input.GetButtonDown(this.m_SubmitButton) | Input.GetButtonDown(this.m_CancelButton) | !Mathf.Approximately(Input.GetAxisRaw(this.m_HorizontalAxis), 0.0f) | !Mathf.Approximately(Input.GetAxisRaw(this.m_VerticalAxis), 0.0f) | (double)(this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0.0 | Input.GetMouseButtonDown(0);
            if (Input.touchCount > 0)
                flag = true;
            return flag;
        }

        /// <summary>
        ///   <para>See BaseInputModule.</para>
        /// </summary>
        public override void ActivateModule()
        {
            base.ActivateModule();
            this.m_MousePosition = (Vector2)Input.mousePosition;
            this.m_LastMousePosition = (Vector2)Input.mousePosition;
            GameObject selectedGameObject = this.eventSystem.currentSelectedGameObject;
            if ((UnityEngine.Object)selectedGameObject == (UnityEngine.Object)null)
                selectedGameObject = this.eventSystem.firstSelectedGameObject;
            this.eventSystem.SetSelectedGameObject(selectedGameObject, this.GetBaseEventData());
        }

        /// <summary>
        ///   <para>See BaseInputModule.</para>
        /// </summary>
        public override void DeactivateModule()
        {
            base.DeactivateModule();
            this.ClearSelection();
        }

        /// <summary>
        ///   <para>See BaseInputModule.</para>
        /// </summary>
        public override void Process()
        {
            bool selectedObject = this.SendUpdateEventToSelectedObject();
            if (this.eventSystem.sendNavigationEvents)
            {
                if (!selectedObject)
                    selectedObject |= this.SendMoveEventToSelectedObject();
                if (!selectedObject)
                    this.SendSubmitEventToSelectedObject();
            }
            if (this.ProcessTouchEvents())
                return;
            this.ProcessMouseEvent();
        }

        private bool ProcessTouchEvents()
        {
            for (int index = 0; index < Input.touchCount; ++index)
            {
                Touch touch = Input.GetTouch(index);
                if (touch.type != TouchType.Indirect)
                {
                    bool pressed;
                    bool released;
                    PointerEventData pointerEventData = this.GetTouchPointerEventData(touch, out pressed, out released);
                    this.ProcessTouchPress(pointerEventData, pressed, released);
                    if (!released)
                    {
                        this.ProcessMove(pointerEventData);
                        this.ProcessDrag(pointerEventData);
                    }
                    else
                        this.RemovePointerData(pointerEventData);
                }
            }
            return Input.touchCount > 0;
        }

        private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
        {
            GameObject gameObject1 = pointerEvent.pointerCurrentRaycast.gameObject;
            if (pressed)
            {
                pointerEvent.eligibleForClick = true;
                pointerEvent.delta = Vector2.zero;
                pointerEvent.dragging = false;
                pointerEvent.useDragThreshold = true;
                pointerEvent.pressPosition = pointerEvent.position;
                pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
                this.DeselectIfSelectionChanged(gameObject1, (BaseEventData)pointerEvent);
                if ((UnityEngine.Object)pointerEvent.pointerEnter != (UnityEngine.Object)gameObject1)
                {
                    this.HandlePointerExitAndEnter(pointerEvent, gameObject1);
                    pointerEvent.pointerEnter = gameObject1;
                }
                GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData)pointerEvent, ExecuteEvents.pointerDownHandler);
                if ((UnityEngine.Object)gameObject2 == (UnityEngine.Object)null)
                    gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
                float unscaledTime = Time.unscaledTime;
                if ((UnityEngine.Object)gameObject2 == (UnityEngine.Object)pointerEvent.lastPress)
                {
                    if ((double)(unscaledTime - pointerEvent.clickTime) < 0.300000011920929)
                        ++pointerEvent.clickCount;
                    else
                        pointerEvent.clickCount = 1;
                    pointerEvent.clickTime = unscaledTime;
                }
                else
                    pointerEvent.clickCount = 1;
                pointerEvent.pointerPress = gameObject2;
                pointerEvent.rawPointerPress = gameObject1;
                pointerEvent.clickTime = unscaledTime;
                pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1);
                if ((UnityEngine.Object)pointerEvent.pointerDrag != (UnityEngine.Object)null)
                    ExecuteEvents.Execute<IInitializePotentialDragHandler>(pointerEvent.pointerDrag, (BaseEventData)pointerEvent, ExecuteEvents.initializePotentialDrag);
            }
            if (!released)
                return;
            ExecuteEvents.Execute<IPointerUpHandler>(pointerEvent.pointerPress, (BaseEventData)pointerEvent, ExecuteEvents.pointerUpHandler);
            GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
            if ((UnityEngine.Object)pointerEvent.pointerPress == (UnityEngine.Object)eventHandler && pointerEvent.eligibleForClick)
                ExecuteEvents.Execute<IPointerClickHandler>(pointerEvent.pointerPress, (BaseEventData)pointerEvent, ExecuteEvents.pointerClickHandler);
            else if ((UnityEngine.Object)pointerEvent.pointerDrag != (UnityEngine.Object)null && pointerEvent.dragging)
                ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject1, (BaseEventData)pointerEvent, ExecuteEvents.dropHandler);
            pointerEvent.eligibleForClick = false;
            pointerEvent.pointerPress = (GameObject)null;
            pointerEvent.rawPointerPress = (GameObject)null;
            if ((UnityEngine.Object)pointerEvent.pointerDrag != (UnityEngine.Object)null && pointerEvent.dragging)
                ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, (BaseEventData)pointerEvent, ExecuteEvents.endDragHandler);
            pointerEvent.dragging = false;
            pointerEvent.pointerDrag = (GameObject)null;
            if ((UnityEngine.Object)pointerEvent.pointerDrag != (UnityEngine.Object)null)
                ExecuteEvents.Execute<IEndDragHandler>(pointerEvent.pointerDrag, (BaseEventData)pointerEvent, ExecuteEvents.endDragHandler);
            pointerEvent.pointerDrag = (GameObject)null;
            ExecuteEvents.ExecuteHierarchy<IPointerExitHandler>(pointerEvent.pointerEnter, (BaseEventData)pointerEvent, ExecuteEvents.pointerExitHandler);
            pointerEvent.pointerEnter = (GameObject)null;
        }

        /// <summary>
        ///   <para>Calculate and send a submit event to the current selected object.</para>
        /// </summary>
        /// <returns>
        ///   <para>If the submit event was used by the selected object.</para>
        /// </returns>
        protected bool SendSubmitEventToSelectedObject()
        {
            if ((UnityEngine.Object)this.eventSystem.currentSelectedGameObject == (UnityEngine.Object)null)
                return false;
            BaseEventData baseEventData = this.GetBaseEventData();
            if (Input.GetButtonDown(this.m_SubmitButton))
                ExecuteEvents.Execute<ISubmitHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
            if (Input.GetButtonDown(this.m_CancelButton))
                ExecuteEvents.Execute<ICancelHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
            return baseEventData.used;
        }

        private Vector2 GetRawMoveVector()
        {
            Vector2 zero = Vector2.zero;
            zero.x = Input.GetAxisRaw(this.m_HorizontalAxis);
            zero.y = Input.GetAxisRaw(this.m_VerticalAxis);
            if (Input.GetButtonDown(this.m_HorizontalAxis))
            {
                if ((double)zero.x < 0.0)
                    zero.x = -1f;
                if ((double)zero.x > 0.0)
                    zero.x = 1f;
            }
            if (Input.GetButtonDown(this.m_VerticalAxis))
            {
                if ((double)zero.y < 0.0)
                    zero.y = -1f;
                if ((double)zero.y > 0.0)
                    zero.y = 1f;
            }
            return zero;
        }

        /// <summary>
        ///   <para>Calculate and send a move event to the current selected object.</para>
        /// </summary>
        /// <returns>
        ///   <para>If the move event was used by the selected object.</para>
        /// </returns>
        protected bool SendMoveEventToSelectedObject()
        {
            float unscaledTime = Time.unscaledTime;
            Vector2 rawMoveVector = this.GetRawMoveVector();
            if (Mathf.Approximately(rawMoveVector.x, 0.0f) && Mathf.Approximately(rawMoveVector.y, 0.0f))
            {
                this.m_ConsecutiveMoveCount = 0;
                return false;
            }
            bool flag1 = Input.GetButtonDown(this.m_HorizontalAxis) || Input.GetButtonDown(this.m_VerticalAxis);
            bool flag2 = (double)Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0.0;
            if (!flag1)
                flag1 = !flag2 || this.m_ConsecutiveMoveCount != 1 ? (double)unscaledTime > (double)this.m_PrevActionTime + 1.0 / (double)this.m_InputActionsPerSecond : (double)unscaledTime > (double)this.m_PrevActionTime + (double)this.m_RepeatDelay;
            if (!flag1)
                return false;
            AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
            if (axisEventData.moveDir != MoveDirection.None)
            {
                ExecuteEvents.Execute<IMoveHandler>(this.eventSystem.currentSelectedGameObject, (BaseEventData)axisEventData, ExecuteEvents.moveHandler);
                if (!flag2)
                    this.m_ConsecutiveMoveCount = 0;
                ++this.m_ConsecutiveMoveCount;
                this.m_PrevActionTime = unscaledTime;
                this.m_LastMoveVector = rawMoveVector;
            }
            else
                this.m_ConsecutiveMoveCount = 0;
            return axisEventData.used;
        }

        /// <summary>
        ///   <para>Iterate through all the different mouse events.</para>
        /// </summary>
        /// <param name="id">The mouse pointer Event data id to get.</param>
        protected void ProcessMouseEvent()
        {
            this.ProcessMouseEvent(0);
        }

        /// <summary>
        ///   <para>Iterate through all the different mouse events.</para>
        /// </summary>
        /// <param name="id">The mouse pointer Event data id to get.</param>
        protected void ProcessMouseEvent(int id)
        {
            PointerInputModule.MouseState pointerEventData = this.GetMousePointerEventData(id);
            PointerInputModule.MouseButtonEventData eventData = pointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
            this.ProcessMousePress(eventData);
            this.ProcessMove(eventData.buttonData);
            this.ProcessDrag(eventData.buttonData);
            this.ProcessMousePress(pointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData);
            this.ProcessDrag(pointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
            this.ProcessMousePress(pointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
            this.ProcessDrag(pointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
            if (Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0.0f))
                return;
            ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), (BaseEventData)eventData.buttonData, ExecuteEvents.scrollHandler);
        }

        /// <summary>
        ///   <para>Send a update event to the currently selected object.</para>
        /// </summary>
        /// <returns>
        ///   <para>If the update event was used by the selected object.</para>
        /// </returns>
        protected bool SendUpdateEventToSelectedObject()
        {
            if ((UnityEngine.Object)this.eventSystem.currentSelectedGameObject == (UnityEngine.Object)null)
                return false;
            BaseEventData baseEventData = this.GetBaseEventData();
            ExecuteEvents.Execute<IUpdateSelectedHandler>(this.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
            return baseEventData.used;
        }

        protected void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
        {
            PointerEventData buttonData = data.buttonData;
            GameObject gameObject1 = buttonData.pointerCurrentRaycast.gameObject;
            if (data.PressedThisFrame())
            {
                buttonData.eligibleForClick = true;
                buttonData.delta = Vector2.zero;
                buttonData.dragging = false;
                buttonData.useDragThreshold = true;
                buttonData.pressPosition = buttonData.position;
                buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
                //this.DeselectIfSelectionChanged(gameObject1, (BaseEventData)buttonData);
                GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject1, (BaseEventData)buttonData, ExecuteEvents.pointerDownHandler);
                if ((UnityEngine.Object)gameObject2 == (UnityEngine.Object)null)
                    gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
                float unscaledTime = Time.unscaledTime;
                if ((UnityEngine.Object)gameObject2 == (UnityEngine.Object)buttonData.lastPress)
                {
                    if ((double)(unscaledTime - buttonData.clickTime) < 0.300000011920929)
                        ++buttonData.clickCount;
                    else
                        buttonData.clickCount = 1;
                    buttonData.clickTime = unscaledTime;
                }
                else
                    buttonData.clickCount = 1;
                buttonData.pointerPress = gameObject2;
                buttonData.rawPointerPress = gameObject1;
                buttonData.clickTime = unscaledTime;
                buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject1);
                if ((UnityEngine.Object)buttonData.pointerDrag != (UnityEngine.Object)null)
                    ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, (BaseEventData)buttonData, ExecuteEvents.initializePotentialDrag);
            }
            if (!data.ReleasedThisFrame())
                return;
            ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, (BaseEventData)buttonData, ExecuteEvents.pointerUpHandler);
            GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject1);
            if ((UnityEngine.Object)buttonData.pointerPress == (UnityEngine.Object)eventHandler && buttonData.eligibleForClick)
                ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, (BaseEventData)buttonData, ExecuteEvents.pointerClickHandler);
            else if ((UnityEngine.Object)buttonData.pointerDrag != (UnityEngine.Object)null && buttonData.dragging)
                ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject1, (BaseEventData)buttonData, ExecuteEvents.dropHandler);
            buttonData.eligibleForClick = false;
            buttonData.pointerPress = (GameObject)null;
            buttonData.rawPointerPress = (GameObject)null;
            if ((UnityEngine.Object)buttonData.pointerDrag != (UnityEngine.Object)null && buttonData.dragging)
                ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, (BaseEventData)buttonData, ExecuteEvents.endDragHandler);
            buttonData.dragging = false;
            buttonData.pointerDrag = (GameObject)null;
            if (!((UnityEngine.Object)gameObject1 != (UnityEngine.Object)buttonData.pointerEnter))
                return;
            this.HandlePointerExitAndEnter(buttonData, (GameObject)null);
            this.HandlePointerExitAndEnter(buttonData, gameObject1);
        }

        [Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
        public enum InputMode
        {
            Mouse,
            Buttons,
        }
    }
}