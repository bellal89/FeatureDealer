using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FeatureDealer.DataProviders;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Features
{
    public class BuhOnlineDataProvider
    {
        private readonly BuhonlineDataReader buhonlineDataReader;
        private readonly BuhOnlineDataCache buhOnlineDataCache;

        public BuhOnlineDataProvider()
        {
            buhonlineDataReader = new BuhonlineDataReader();
            buhOnlineDataCache = new BuhOnlineDataCache();
        }

        public yaf_Message ReadMessage(int id)
        {
            yaf_Message message;
            if(!buhOnlineDataCache.ContainsMessage(id))
            {
                message = buhonlineDataReader.ReadMessageById(id).FirstOrDefault();
                buhOnlineDataCache.AddMessage(message);
            }
            else
                message = buhOnlineDataCache.GetMessage(id);
            return message;
        }

        public yaf_User ReadUser(int id)
        {
            yaf_User user;
            if (!buhOnlineDataCache.ContainsUser(id))
            {
                user = buhonlineDataReader.ReadUserWithId(id).FirstOrDefault();
                buhOnlineDataCache.AddUser(user);
            }
            else
                user = buhOnlineDataCache.GetUser(id);
            return user;
        }

        public UserPersonal ReadUserPersonal(int id)
        {
            var userPersonal = buhonlineDataReader.ReadUserPersonalWithId(id).FirstOrDefault();
            return userPersonal;
        }

        public IEnumerable<yaf_Message> ReadTopicMessages(int topicId)
        {
            IEnumerable<int> messageIds;
            if (!buhOnlineDataCache.ContainsTopic(topicId))
            {
                messageIds = buhonlineDataReader.ReadTopicMessageIds(topicId);
                buhOnlineDataCache.AddTopic(topicId, messageIds);
            }
            else
            {
                messageIds = buhOnlineDataCache.GetTopicMessageIds(topicId);
            }
            var messages = messageIds.Select(ReadMessage);
            return messages;
        }

        public yaf_Message ReadMessageInPosition(int topicId, int n)
        {
            var messages = ReadTopicMessages(topicId);
            return messages.FirstOrDefault(m => m.Position == n);
        }

        public yaf_Message ReadTopicStarter(int topicId)
        {
            return ReadMessageInPosition(topicId, 0);
        }
    }
}