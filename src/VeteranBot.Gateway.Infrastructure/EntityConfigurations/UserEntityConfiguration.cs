using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using VeteranBot.Gateway.Domain.Entities;
using VeteranBot.Gateway.Domain.Enums;

namespace VeteranBot.Gateway.Infrastructure.EntityConfigurations;

public static class UserEntityConfiguration
{
    public static void Configure()
    {
        BsonClassMap.RegisterClassMap<UserEntity>(classMap => 
        {
            classMap.AutoMap();
            classMap.MapIdProperty(entity => entity.PhoneNumber);
            classMap.MapMember(entity => entity.Type).SetSerializer(new EnumSerializer<UserType>(BsonType.String));
        });
    }
}