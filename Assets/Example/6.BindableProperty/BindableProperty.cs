using System;
using UnityEngine;

namespace QFramework.Example
{
    [Serializable]
    public class IntProperty : BindableProperty<int>
    {
        public IntProperty(int value = default) : base(value)
        {
        }
    }
    // 展示了BindableProperty 可以通过Register函数来实现数值改变时的链式事件调用
    // BindableProperty的实现是靠 多播委托 + 重写setter函数实现的
    // setter时会触发多播委托
    
    
    // delegate 是C#语核，类似于函数指针
    // Action/Function 是C#标准库一部分，实现了多播委托，利用了delegate特性
    public class BindableProperty : MonoBehaviour
    {
        public  IntProperty Age = new IntProperty(10);
        public  IntProperty Counter = new IntProperty();
        private void Start()
        {
            // 1. Age值改变会改变Counter的值，Counter的值改变会打印Counter
            Age.Register(age =>
            {
                Debug.Log($"{nameof(age)}: {age.ToString()}");
                Counter.Value = 10 * age;
            }).CancelOnDestroy(gameObject);
            Counter.RegisterWithInitValue(counter => Debug.Log($"{nameof(counter)}: {counter.ToString()}")).CancelOnDestroy(gameObject);
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Age.Value++;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Age.Value--;
            }
        }
    }
}