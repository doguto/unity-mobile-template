using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

namespace Project.Scripts.Infrastructure
{
    public sealed class UnityMessagePackResolver : IFormatterResolver
    {
        public static readonly UnityMessagePackResolver Instance = new();

        IMessagePackFormatter<T> IFormatterResolver.GetFormatter<T>()
            => FormatterCache<T>.Formatter;

        static class FormatterCache<T>
        {
            public static readonly IMessagePackFormatter<T> Formatter
                = (IMessagePackFormatter<T>)GetFormatterHelper();

            static object GetFormatterHelper()
            {
                if (typeof(T) == typeof(Vector2)) return Vector2Formatter.Instance;
                if (typeof(T) == typeof(Vector2Int)) return Vector2IntFormatter.Instance;
                if (typeof(T) == typeof(RectInt)) return RectIntFormatter.Instance;
                return null;
            }
        }
    }

    sealed class Vector2Formatter : IMessagePackFormatter<Vector2>
    {
        public static readonly Vector2Formatter Instance = new();

        public void Serialize(ref MessagePackWriter writer, Vector2 value, MessagePackSerializerOptions options)
        {
            writer.WriteArrayHeader(2);
            writer.Write(value.x);
            writer.Write(value.y);
        }

        public Vector2 Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            reader.ReadArrayHeader();
            var x = reader.ReadSingle();
            var y = reader.ReadSingle();
            return new Vector2(x, y);
        }
    }

    sealed class Vector2IntFormatter : IMessagePackFormatter<Vector2Int>
    {
        public static readonly Vector2IntFormatter Instance = new();

        public void Serialize(ref MessagePackWriter writer, Vector2Int value, MessagePackSerializerOptions options)
        {
            writer.WriteArrayHeader(2);
            writer.Write(value.x);
            writer.Write(value.y);
        }

        public Vector2Int Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            reader.ReadArrayHeader();
            var x = reader.ReadInt32();
            var y = reader.ReadInt32();
            return new Vector2Int(x, y);
        }
    }

    sealed class RectIntFormatter : IMessagePackFormatter<RectInt>
    {
        public static readonly RectIntFormatter Instance = new();

        public void Serialize(ref MessagePackWriter writer, RectInt value, MessagePackSerializerOptions options)
        {
            writer.WriteArrayHeader(4);
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.width);
            writer.Write(value.height);
        }

        public RectInt Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            reader.ReadArrayHeader();
            var x = reader.ReadInt32();
            var y = reader.ReadInt32();
            var width = reader.ReadInt32();
            var height = reader.ReadInt32();
            return new RectInt(x, y, width, height);
        }
    }
}
