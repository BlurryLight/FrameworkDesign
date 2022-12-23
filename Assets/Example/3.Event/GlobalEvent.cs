using UnityEngine;
namespace QFramework.Example
{
    public struct GlobalEventA
    {
    }
    public struct GlobalEventB
    {
    }

    public class HandleEventB: IOnEvent<GlobalEventB>
    {
        public void OnEvent(GlobalEventB e)
        {
            Debug.Log(e.ToString());
        }
    }
    public class GlobalEvent : MonoBehaviour
    {
        private HandleEventB _handleEventB = null;
        private void Start()
        {
            //注册方式和注销事件方式1
            // 1. 注册方式1，手动注册
            QFramework.TypeEventSystem.Global.Register<GlobalEventA>(OnGlobalEventA).CancelOnDestroy(gameObject);
            // 2. 注册方式2，通过IOnEvent接口注册事件
            _handleEventB = new HandleEventB();
            _handleEventB.Register();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                QFramework.TypeEventSystem.Global.Send<GlobalEventA>();
            }
            if (Input.GetMouseButtonDown(1))
            {
                QFramework.TypeEventSystem.Global.Send<GlobalEventB>();
            }
        }
        private void OnDestroy()
        {
        //     // 注销事件方式2：
        //     QFramework.TypeEventSystem.Global.Cancel<GlobalEventA>(OnGlobalEventA);
                // 使用IOnEvent接口监听注销事件
            _handleEventB.Cancel();
        }
        private void OnGlobalEventA(GlobalEventA obj) { Debug.Log(obj.ToString()); }
    }
}