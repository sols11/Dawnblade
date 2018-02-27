using System;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using Object = UnityEngine.Object;

namespace SFramework{
    /// <summary>
    /// 场景内存回收。功能待完善
    /// </summary>
    public class ClearSceneData
    {
        public static void GarbageCollect()
        {
            // 回收Material
            Object[] objAry = Resources.FindObjectsOfTypeAll<Material>();
            // 解除资源的引用
            for (int i = 0; i < objAry.Length; ++i)
                objAry[i] = null;

            // 回收Texture
            Object[] objAry2 = Resources.FindObjectsOfTypeAll<Texture>();
            for (int i = 0; i < objAry2.Length; ++i)
                objAry2[i] = null;

            // 释放所有没有引用的Asset资源
            Resources.UnloadUnusedAssets();

            // 立即进行垃圾回收
            GC.Collect();
            // 挂起当前线程，直到处理终结器队列的线程清空该队列为止
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}