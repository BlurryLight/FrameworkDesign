#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
namespace QFramework.Example
{
    // 所谓的DIP(依赖倒转)就是需要面向接口编程，不关心具体的实现。
    public class DIPExample : MonoBehaviour
    {
        private IOCContainer mContainer;
        private IStorage mStorage;
        private void Awake()
        {
            //创建一个IOC容器
            mContainer = new IOCContainer();
            
            // 控制反转、依赖注入这些名词都太高大上了。
            // 实质上就是Class A要用到Class B，但是不想在Class A内部写一大串初始化Class B的逻辑
            // Class A需要暴露某个Register的接口，最简单的像C++提供一个Public的clas B*
            // 由外部代码填充这个指针
            // 然后Class A就可以用上Class B提供的功能了
            
            // 这个设计模式都不一定需要是强类型的，可以是鸭子类型
        }
        private void OnEnable()
        {
            //注册运行时模块
            mContainer.Register<IStorage>(new PlayerPrefsStorage());
            mStorage = mContainer.Get<IStorage>();
            mStorage.SaveString("name", "name运行时存储");
            Debug.Log($"Player mStorage.LoadString(\"name\") {mStorage.LoadString("name")}");
            Debug.Log($"Player mStorage.LoadString(\"Name\") {mStorage.LoadString("Name")}");
            //切换实现
            mContainer.Register<IStorage>(new EditorPrefsStorage());
            mStorage = mContainer.Get<IStorage>();
            mStorage.SaveString("name2", "name2编辑器存储");
            Debug.Log($"Editor mStorage.LoadString(\"name\") {mStorage.LoadString("name")}");
            Debug.Log($"Editor mStorage.LoadString(\"Name\") {mStorage.LoadString("Name")}");
            Debug.Log($"Editor mStorage.LoadString(\"name2\") {mStorage.LoadString("name2")}");
            Debug.Log($"Editor mStorage.LoadString(\"Name2\") {mStorage.LoadString("Name2")}");
        }

        /// <summary>
        /// 存取接口
        /// </summary>
        public interface IStorage
        {
            void SaveString(string key, string value);
            string LoadString(string key, string value = default);
        }
        /// <summary>
        /// 实现接口
        /// 运行时存储
        /// </summary>
        public class PlayerPrefsStorage : IStorage
        {
            public void SaveString(string key, string value) { PlayerPrefs.SetString(key, value); }
            public string LoadString(string key, string value = default) { return PlayerPrefs.GetString(key, value); }
        }
        /// <summary>
        /// 实现接口
        /// 编辑器存储
        /// </summary>
        public class EditorPrefsStorage : IStorage
        {
            public void SaveString(string key, string value)
            {
#if UNITY_EDITOR
                EditorPrefs.SetString(key, value);
#endif
            }
            public string LoadString(string key, string value = default)
            {
#if UNITY_EDITOR
                return EditorPrefs.GetString(key, value);
#else
                return String.Empty;
#endif
            }
        }
    }
}