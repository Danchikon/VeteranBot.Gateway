using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using VeteranBot.Gateway.Domain.Entities;
using VeteranBot.Gateway.Domain.Enums;

namespace VeteranBot.Gateway.Infrastructure.EntityConfigurations;

public static class UserGroupEntityConfiguration
{
    public static void Configure()
    {
        BsonClassMap.RegisterClassMap<UserGroupEntity>(classMap => 
        {
            classMap.AutoMap();
            classMap.MapIdProperty(entity => entity.Id);
        });
    }
}