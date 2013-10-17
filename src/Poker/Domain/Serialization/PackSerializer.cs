using System;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Poker.Domain.Data;

namespace Poker.Domain.Serialization
{
    public class PackSerializer : BsonBaseSerializer
    {
        public override object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
        {
            var ser = new ArraySerializer<Card>();
            var cards = (Card[]) ser.Deserialize(bsonReader, typeof (Card[]), options);
            return cards == null ? null : new Pack(cards);
        }

        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            if (value == null)
            {
                bsonWriter.WriteNull();
            }
            else
            {
                var pack = (Pack) value;
                var ser = new ArraySerializer<Card>();
                ser.Serialize(bsonWriter, typeof(Card[]), pack.GetAllCards(),options);
            }
        }
    }
}