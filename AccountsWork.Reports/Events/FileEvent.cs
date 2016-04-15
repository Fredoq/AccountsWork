using Prism.Events;

namespace AccountsWork.Reports.Events
{
    public class SaveFileEvent : PubSubEvent<string>
    {
    }
    public class OpenFileEvent : PubSubEvent<string>
    {
    }
}
