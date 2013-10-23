using System;
using System.Collections.Generic;
using System.Linq;
using FeatureDealer.Models;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.DataProviders
{
    class BuhonlineDataReader : IDisposable
    {
        private readonly BuhonlineContext context = new BuhonlineContext();

        public void Dispose()
        {
            context.Dispose();
        }

        public IEnumerable<IDataItem> Read()
        {
            return context.yaf_Message;
        }

        public IQueryable<string> ReadMessageTexts()
        {
            return from m in context.yaf_Message select m.Message;
        }

        public IQueryable<yaf_Message> ReadMessageById(int messageId)
        {
            return from m in context.yaf_Message where m.MessageID == messageId select m;
        }

        public IQueryable<yaf_User> ReadUserWithId(int userId)
        {
            return from u in context.yaf_User where u.UserID == userId select u;
        }

        public IQueryable<UserPersonal> ReadUserPersonalWithId(int userId)
        {
            return from u in context.UserPersonals where u.UserPersonalId == userId select u;
        }

        public IQueryable<int> ReadTopicMessageIds(int topicId)
        {
            return from m in context.yaf_Message where m.TopicID == topicId select m.MessageID;
        }

        public IQueryable<int> ReadMessageIds()
        {
            return from m in context.yaf_Message select m.MessageID;
        }

        public IQueryable<int> ReadAnswerIds()
        {
            return from m in context.yaf_Message where m.Position != 0 select m.MessageID;
        }
    }
}
