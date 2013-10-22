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

        public IEnumerable<IDataItem> ReadUselessTopics(IEnumerable<int> uselessForumIds)
        {
            return context.yaf_Topic.Where(t => uselessForumIds.Contains(t.ForumID));
        }

        public IEnumerable<IDataItem> ReadDeletedMessages()
        {
            return context.yaf_Message.Where(p => p.IsDeleted == true);
        }

        public IEnumerable<IDataItem> Read(IEnumerable<int> messageIds)
        {
            return context.yaf_Message.Where(p => p.IsDeleted == false && messageIds.Contains(p.MessageID));
        }

        public IEnumerable<IDataItem> Read()
        {
            return context.yaf_Message;
        }

        public IEnumerable<int> ReadIds()
        {
            return context.yaf_Message.Select(m => m.MessageID);
        }

        public IEnumerable<yaf_Message> ReadTopicStarters(IEnumerable<int> topicIds)
        {
            return context.yaf_Message.Where(m => topicIds.Contains(m.TopicID) && m.IsDeleted == false && m.Position == 0);
        }

        public IEnumerable<yaf_Message> ReadTopicStarters()
        {
            return context.yaf_Message.Where(m => m.Position == 0);
        }

        public IEnumerable<UserPersonal> ReadUserPersonals()
        {
            return context.UserPersonals;
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
