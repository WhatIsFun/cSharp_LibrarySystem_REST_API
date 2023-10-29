using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using cSharp_LibrarySystemWebAPI.Model; // Replace with the correct namespace for your model classes

public class BorrowingTransactionsConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(List<BorrowingTransaction>);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        // Implement the logic to deserialize BorrowingTransactions if needed
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        // Exclude the BorrowingTransactions property from serialization
        writer.WriteNull();
    }
}