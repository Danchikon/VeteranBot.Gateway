using MongoDB.Driver;
using VeteranBot.Gateway.Domain.Entities;

namespace VeteranBot.Gateway.Api.GraphQl;

public class Query 
{
    [UseOffsetPaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IExecutable<UserEntity> GetPagedUsers([Service] IMongoCollection<UserEntity> usersCollection)
    {
        return usersCollection.AsExecutable();
    }
    
    [UseOffsetPaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IExecutable<ScheduledMessageEntity> GetPagedScheduledMessages([Service] IMongoCollection<ScheduledMessageEntity> scheduledMessagesCollection)
    {
        return scheduledMessagesCollection.AsExecutable();
    }
    
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IExecutable<ScheduledMessageEntity> GetScheduledMessages([Service] IMongoCollection<ScheduledMessageEntity> scheduledMessagesCollection)
    {
        return scheduledMessagesCollection.AsExecutable();
    }
    
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IExecutable<UserEntity> GetUsers([Service] IMongoCollection<UserEntity> usersCollection)
    {
        return usersCollection.AsExecutable();
    }
    
    [UseOffsetPaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IExecutable<UserGroupEntity> GetPagedUserGroups([Service] IMongoCollection<UserGroupEntity> userGroupsCollection)
    {
        return userGroupsCollection.AsExecutable();
    }
   
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IExecutable<UserGroupEntity> GetUserGroups([Service] IMongoCollection<UserGroupEntity> userGroupsCollection)
    {
        return userGroupsCollection.AsExecutable();
    }
    
    [UseOffsetPaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IExecutable<UserMessageEntity> GetPagedUserMessages([Service] IMongoCollection<UserMessageEntity> userMessagesCollection)
    {
        return userMessagesCollection.AsExecutable();
    }
}