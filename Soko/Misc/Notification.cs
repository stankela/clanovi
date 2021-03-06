using System;
using System.Collections.Generic;

namespace Soko
{
    public class Notification
    {
        public static readonly string REQUIRED_FIELD = "Required Field";
        public static readonly string INVALID_FORMAT = "Invalid Format";

        private List<NotificationMessage> _list = new List<NotificationMessage>();

        public bool IsValid()
        {
            return _list.Count == 0;
        }

        public void RegisterMessage(string fieldName, string message)
        {
            _list.Add(new NotificationMessage(fieldName, message));
        }

        public string[] GetMessages(string fieldName)
        {
            List<NotificationMessage> messages = findAll(fieldName);
            string[] returnValue = new string[messages.Count];
            for (int i = 0; i < messages.Count; i++)
            {
                returnValue[i] = messages[i].Message;
            }

            return returnValue;
        }

        private List<NotificationMessage> findAll(string fieldName)
		{
            List<NotificationMessage> result = new List<NotificationMessage>();
			foreach (NotificationMessage m in _list)
			{
				if (m.FieldName == fieldName)
					result.Add(m);
			}
			return result;
		}

        public NotificationMessage[] AllMessages
        {
            get
            {
                _list.Sort();
                return _list.ToArray();
            }
        }

        // vraca prvu dodatu poruku
        public NotificationMessage FirstMessage
        {
            get
            {
                if (_list.Count > 0)
                    return _list[0];
                else
                    return null;
            }
        }

        public bool HasMessage(string fieldName, string messageText)
        {
            NotificationMessage message =
                    new NotificationMessage(fieldName, messageText);
            return _list.Contains(message);
        }
    }
}
