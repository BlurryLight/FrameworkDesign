using System;
using UnityEngine;
namespace QFramework.Example
{
    public class TypeEventSystem : MonoBehaviour
    {
        public struct EventA
        {
        }
        public struct EventB
        {
            public int ParamB;
        }
        // 通过继承 + 子类可以做出Group的效果
        public interface IEventGroup
        {
        }
        public struct EventC : IEventGroup
        {
        }

        public struct EventD : IEventGroup
        {
        }
        private QFramework.TypeEventSystem typeEventSystem = new QFramework.TypeEventSystem();
        private GameObject TransientObject = null;
        private void Start()
        {
            TransientObject = GameObject.Find("Wait5SecToDie");
            typeEventSystem.Register<EventA>(new Action<EventA>(OnEventA));
            // 这个Cancel不是终止某个Function执行的意思，是指这里注册了一个对EventA事件的响应，如果绑定的object died了就unregister这个事件。
            typeEventSystem.Register<EventA>(onEventA => { Debug.Log($"OnEventA bind on TrasientObject"); }).CancelOnDestroy(TransientObject);
            typeEventSystem.Register<EventB>(onEventB => { Debug.Log($"onEventB {onEventB.ParamB}"); }).CancelOnDestroy(TransientObject);
            typeEventSystem.Register<EventB>(OnEventB).CancelOnDestroy(TransientObject);
            typeEventSystem.Register<IEventGroup>(e => { Debug.Log($"{e.GetType()}"); }).CancelOnDestroy(TransientObject);

        }
        private void Update()
        {
            if (TransientObject != null)
            {
                Destroy(TransientObject,5);
                TransientObject = null;
            }
            if (Input.GetMouseButtonDown(0)) typeEventSystem.Send<EventA>();
            if (Input.GetMouseButtonDown(1)) typeEventSystem.Send<EventB>(new EventB {ParamB = 123});
            if (Input.GetKeyDown(KeyCode.Space))
            {
                typeEventSystem.Send<IEventGroup>(new EventC());
                typeEventSystem.Send<IEventGroup>(new EventD());
            }
        }
        private void OnEventA(EventA obj) => Debug.Log("OnEventA No Bind");
        private void OnEventB(EventB obj) => Debug.Log($"OnEventB {obj.ParamB}");
        private void OnDestroy() => typeEventSystem.Cancel<EventA>(OnEventA);
    }
}