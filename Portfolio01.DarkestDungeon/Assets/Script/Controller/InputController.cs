using System.Collections.Generic;

namespace Portfolio.Controller
{
    /// <summary>
    /// 입력 처리를 담당할 클래스.
    /// </summary>
    public class InputController
    {
        /// <summary>
        /// 버튼 타입의 키 입력 처리 시, 실행할 메서드를 대리할 대리자
        /// </summary>
        public delegate void InputButtonEvent();

        /// <summary>
        /// 축 타입의 키 입력 처리 시, 실행할 메서드를 대리할 대리자.
        /// </summary>
        /// <param name="value"></param>
        public delegate void InputAxisEvent(float value);
        
        /// <summary>
        /// string : 내가 등록한 키의 이름.
        /// Axis/ButtonHandler : 등록한 키를 누르거나 했을 때 같은 다양한 상황에서
        /// 발생시킬 기능을 가지고 있는 Handler
        /// </summary>
        public List<KeyValuePair<string, AxisHandler>> inputAxis = new List<KeyValuePair<string, AxisHandler>>();
        public List<KeyValuePair<string, ButtonHandler>> inputButton = new List<KeyValuePair<string, ButtonHandler>>();

        /// <summary>
        /// 축 타입의 키와 해당 키 상호작용 시, 실행 시킬 기능을 등록하는 기능.
        /// </summary>
        /// <param name="key">등록시키고자 하는 축타입의 키.</param>
        /// <param name="axisEvent">등록한 키와 상호작용 시, 실행할 기능</param>
        public void AddAxis(string key, InputAxisEvent axisEvent)
        {
            inputAxis.Add(new KeyValuePair<string, AxisHandler>(key, new AxisHandler(axisEvent)));
        }

        /// <summary>
        /// 버튼 타입의 키와 해당 키 상호작용 시, 실행 시킬 기능들을 등록하는 기능.
        /// </summary>
        /// <param name="key">등록시키고자 하는 버튼타입의 키.</param>
        /// <param name="buttonEvents">등록한 버튼과 상호작용 시, 실행할 기능</param>
        public void AddButton(string key, params InputButtonEvent[] buttonEvents)
        {
            inputButton.Add(new KeyValuePair<string, ButtonHandler>(key,
                new ButtonHandler( buttonEvents[0], buttonEvents[1], buttonEvents[2], buttonEvents[3])));
        }

        // 2가지 타입으로 입력 처리 클래스 만들기.
        //  -> 축 타입의 키.
        //  -> 버튼 타입의 키.

        public class AxisHandler
        {
            private InputAxisEvent axisEvent;
            
            public void GetAxisValue(float value)
            {
                axisEvent?.Invoke(value);
            }

            public AxisHandler(InputAxisEvent axisEvent) 
            {
                this.axisEvent = axisEvent;
            }
        }

        public class ButtonHandler
        {
            private InputButtonEvent downEvent;     // 등록된 버튼을 누른 순간(1번) 실행 시킬 기능.
            private InputButtonEvent upEvent;       // 등록된 버튼을 뗀 순간(1번) 실행 시킬 기능.
            private InputButtonEvent pressEvent;    // 등록된 버튼을 누르고 있는 동안(여러번) 실행 시킬 기능.
            private InputButtonEvent notPressEvent; // 등록된 버튼을 떼고 있는 동안(여러번) 실행 시킬 기능.

            public ButtonHandler(InputButtonEvent downEvent, InputButtonEvent upEvent,
                InputButtonEvent pressEvent, InputButtonEvent notPressEvent)
            {
                this.downEvent = downEvent;
                this.upEvent = upEvent;
                this.pressEvent = pressEvent;
                this.notPressEvent = notPressEvent;
            }

            public void OnDown()
            {
                downEvent?.Invoke();
            }

            public void OnUp()
            {
                upEvent?.Invoke();
            }

            public void OnPress() 
            {
                pressEvent?.Invoke();
            }

            public void OnNotPress()
            {
                notPressEvent?.Invoke();
            }
        }
    }
}