using System;
using System.Collections.Generic;
using System.Reflection;
using MasterMemory;
using UnityEngine;

namespace Project.Scripts.Infrastructure.MasterData
{
    // 型引数を問わず AssetDatabase.FindAssets(t:MasterDataAssetBase) で一括検索できるようにするための
    // 非ジェネリックな中間基底。直接継承せず MasterDataAssetBase<T> を使う
    public abstract class MasterDataAssetBase : ScriptableObject, IMasterDataAsset
    {
        public abstract void AppendTo(DatabaseBuilder builder);
    }

    public abstract class MasterDataAssetBase<T> : MasterDataAssetBase where T : class
    {
        // DatabaseBuilder.Append(IEnumerable<T>) は MasterMemory の Source Generator が
        // 具体的なテーブル型ごとに生成する専用オーバーロードのため、ジェネリックな T のままでは
        // コンパイル時にオーバーロード解決できずリフレクション経由で呼び出す
        static readonly MethodInfo AppendMethod =
            typeof(DatabaseBuilder).GetMethod("Append", new[] { typeof(IEnumerable<T>) })
                ?? throw new InvalidOperationException($"DatabaseBuilder has no Append(IEnumerable<{typeof(T).Name}>) method");

        public abstract IEnumerable<T> GetAll();

        public override void AppendTo(DatabaseBuilder builder) => AppendMethod.Invoke(builder, new object[] { GetAll() });
    }
}
