using System;
using QFramework;
using UnityEngine;
namespace CounterApp
{
    public interface IAchievementSystem : ISystem
    {
    }
    public class AchievementSystem : AbstractSystem, IAchievementSystem
    {
        protected override void OnInit()
        {
            var counterModel = this.GetModel<ICounterModel>();
            var previousCount = counterModel.Count.Value;
            // 将bool值存储
            // var storage = Architecture.GetUtility<IStorage>();
            // storage.SaveInt();
            // bool count18Unlock = false;
            
            var storage = this.GetUtility<IStorage>();
            bool count9Unlock = storage.LoadInt("Count9Unlock",0) != 0;
            bool count18Unlock = storage.LoadInt("Count18Unlock",0) != 0;
            Debug.Log($"点击十次成就解锁: {(count9Unlock ? "是" : "否")}");
            Debug.Log($"点击十八次成就解锁: {(count18Unlock ? "是" : "否")}");
            
            counterModel.Count.Register(newCount =>
            {
                if (previousCount < 9 && newCount >= 9 && !count9Unlock)
                {
                    count9Unlock = true;
                    storage.SaveInt("Count9Unlock",1);
                    Debug.Log("解锁：点击10次成就");
                } else if (previousCount < 18 && newCount >= 18 && count9Unlock && !count18Unlock)
                {
                    count18Unlock = true;
                    storage.SaveInt("Count18Unlock",1);
                    Debug.Log("解锁：点击18次成就");
                }
                previousCount = newCount;
            });
        }
    }
}