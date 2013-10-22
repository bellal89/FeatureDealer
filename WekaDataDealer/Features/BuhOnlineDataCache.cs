using System.Collections.Generic;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Features
{
    public class BuhOnlineDataCache
    {
        IDictionary<int, yaf_Message> messages = new Dictionary<int, yaf_Message>();
        IDictionary<int, yaf_User> users = new Dictionary<int, yaf_User>();
        IDictionary<int, UserPersonal> personals = new Dictionary<int, UserPersonal>();
        IDictionary<int, IEnumerable<int>> topics = new Dictionary<int, IEnumerable<int>>();
        private const int keepMaxMessages = 10000;
        private const int keepMaxUsers = 100000;
        private const int keepMaxPersonals = 20000;

        public void AddTopic(int topicId, IEnumerable<int> messageIds)
        {
            topics.Add(topicId, messageIds);
        }

        public bool ContainsTopic(int topicId)
        {
            return topics.ContainsKey(topicId);
        }

        public IEnumerable<int> GetTopicMessageIds(int topicId)
        {
            return topics[topicId];
        }

        public void AddPersonal(UserPersonal personal)
        {
            if (personals.Count == keepMaxPersonals)
                personals = new Dictionary<int, UserPersonal>();
            personals.Add(personal.UserPersonalId, personal);
        }

        public bool ContainsPersonal(int id)
        {
            return personals.ContainsKey(id);
        }

        public UserPersonal GetPersonal(int id)
        {
            return personals[id];
        }

        public void AddUser(yaf_User user)
        {
            if (users.Count == keepMaxUsers)
                users = new Dictionary<int, yaf_User>();
            if (user != null)
                users.Add(user.UserID, user);
        }

        public bool ContainsUser(int id)
        {
            return users.ContainsKey(id);
        }

        public yaf_User GetUser(int id)
        {
            return users[id];
        }

        public void AddMessage(yaf_Message message)
        {
            if(messages.Count == keepMaxMessages)
                messages = new Dictionary<int, yaf_Message>();
            messages.Add(message.MessageID, message);
        }

        public bool ContainsMessage(int id)
        {
            return messages.ContainsKey(id);
        }

        public yaf_Message GetMessage(int id)
        {
            return messages[id];
        }
    }
}