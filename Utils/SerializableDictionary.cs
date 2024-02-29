using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MaaPiAvaGui.Utils;

public class SerializableDictionary<TKey, TValue> where TKey : notnull
{
    [JsonProperty("dictionary")]
    public Dictionary<TKey, TValue> Dictionary { get; set; }

    public SerializableDictionary()
    {
        Dictionary = new Dictionary<TKey, TValue>();
    }
}
