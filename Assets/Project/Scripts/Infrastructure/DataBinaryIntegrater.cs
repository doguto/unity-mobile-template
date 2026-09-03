using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MasterMemory;

namespace Project.Scripts.Infrastructure
{
    public sealed class DataBinaryIntegrater
    {
        static readonly MethodInfo MergeTableMethod =
            typeof(DataBinaryIntegrater)
                .GetMethod(nameof(MergeTable), BindingFlags.NonPublic | BindingFlags.Static);

        // 後から渡したバイナリほど優先度が高い（DLCパッチが基本データを上書き）
        public byte[] Integrate(IReadOnlyList<byte[]> binaries)
        {
            var databases = binaries.Select(b => new MemoryDatabase(b)).ToList();
            var builder = new DatabaseBuilder();

            foreach (var tableType in DiscoverTableTypes())
                MergeTableMethod.MakeGenericMethod(tableType)
                    .Invoke(null, new object[] { databases, builder });

            return builder.Build();
        }

        static IEnumerable<Type> DiscoverTableTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(SafeGetTypes)
                .Where(t => t.GetCustomAttribute<MemoryTableAttribute>() != null);
        }

        static IEnumerable<Type> SafeGetTypes(Assembly assembly)
        {
            try { return assembly.GetTypes(); }
            catch { return Array.Empty<Type>(); }
        }

        static void MergeTable<T>(List<MemoryDatabase> databases, DatabaseBuilder builder) where T : class
        {
            var tableName = typeof(T).Name + "Table";
            var tableProp = typeof(MemoryDatabase).GetProperty(tableName)
                ?? throw new InvalidOperationException($"MemoryDatabase has no property '{tableName}'");
            var allProp = tableProp.PropertyType.GetProperty("All")
                ?? throw new InvalidOperationException($"{tableProp.PropertyType.Name} has no property 'All'");
            var keyProps = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null)
                .ToArray();

            var merged = new Dictionary<object, T>();
            foreach (var db in databases)
            {
                var table = tableProp.GetValue(db);
                foreach (var item in (IEnumerable<T>)allProp.GetValue(table))
                    merged[CompositeKey(item, keyProps)] = item;
            }

            // DatabaseBuilder.Append は型ごとに生成された非ジェネリックメソッドのためリフレクションで呼び出す
            var enumerableOfT = typeof(IEnumerable<>).MakeGenericType(typeof(T));
            var appendMethod = typeof(DatabaseBuilder).GetMethod("Append", new[] { enumerableOfT })
                ?? throw new InvalidOperationException($"DatabaseBuilder has no Append(IEnumerable<{typeof(T).Name}>) method");
            appendMethod.Invoke(builder, new object[] { merged.Values });
        }

        static object CompositeKey<T>(T item, PropertyInfo[] keyProps) =>
            keyProps.Length == 1
                ? keyProps[0].GetValue(item)!
                : string.Join("\0", keyProps.Select(p => p.GetValue(item)));
    }
}
