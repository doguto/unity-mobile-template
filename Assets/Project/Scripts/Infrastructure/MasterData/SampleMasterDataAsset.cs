using System;
using System.Collections.Generic;
using UnityEngine;
using Project.Scripts.Core.ValueObject;

namespace Project.Scripts.Infrastructure.MasterData
{
    // 実際のテーブルを1つ以上定義したらこのサンプルは削除して構わない
    [CreateAssetMenu(fileName = "Sample", menuName = "Project/MasterData/Sample")]
    public class SampleMasterDataAsset : MasterDataAssetBase<Sample>
    {
        [SerializeField] List<Record> elements = new();

        public override IEnumerable<Sample> GetAll()
        {
            foreach (var record in elements)
                yield return new Sample { Id = new SampleId(record.id), Name = record.name };
        }

        [Serializable]
        public class Record
        {
            public int id;
            public string name;
        }
    }
}
