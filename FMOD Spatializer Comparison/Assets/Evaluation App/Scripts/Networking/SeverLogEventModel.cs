using Normal.Realtime;
using Normal.Realtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[RealtimeModel]
public partial class ServerLogEventModel
{
    [RealtimeProperty(1, true)] private int _trigger;
    [RealtimeProperty(2, true)] private int _senderID;
    [RealtimeProperty(3, true)] private int _pageNum;
    [RealtimeProperty(4, true)] private string _eventLog;

    // Used to fire an event on all clients
    public void FireEvent(int senderID, int pageNum, string eventLog)
    {
        this.trigger++;
        this.senderID = senderID;
        this._pageNum = pageNum;
        this._eventLog = eventLog;
        Debug.Log("Page Fire: " + pageNum);
    }

    // An event that consumers of this model can subscribe to in order to respond to the event
    public delegate void EventHandler(int senderID, int pageNum, string eventLog);
    public event EventHandler eventDidFire;

    // A RealtimeCallback method that fires whenever we read any values from the server
    [RealtimeCallback(RealtimeModelEvent.OnDidRead)]
    private void DidRead()
    {
        Debug.Log("Read "+pageNum);
        if (eventDidFire != null && trigger != 0)
            eventDidFire(senderID, pageNum, eventLog);
    }

}
*/

[RealtimeModel]
public partial class ServerLogEventModel
{
    [RealtimeProperty(1, true)] private int _trigger;
    [RealtimeProperty(2, true)] private int _senderID;
    [RealtimeProperty(3, true)] private int _pageNum;
    [RealtimeProperty(4, true)] private string _eventLog;
    [RealtimeProperty(5, true)] private int _nextPage;


}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class ServerLogEventModel : RealtimeModel
{
    public int trigger
    {
        get
        {
            return _triggerProperty.value;
        }
        set
        {
            if (_triggerProperty.value == value) return;
            _triggerProperty.value = value;
            InvalidateReliableLength();
        }
    }

    public int senderID
    {
        get
        {
            return _senderIDProperty.value;
        }
        set
        {
            if (_senderIDProperty.value == value) return;
            _senderIDProperty.value = value;
            InvalidateReliableLength();
        }
    }

    public int pageNum
    {
        get
        {
            return _pageNumProperty.value;
        }
        set
        {
            if (_pageNumProperty.value == value) return;
            _pageNumProperty.value = value;
            InvalidateReliableLength();
        }
    }

    public string eventLog
    {
        get
        {
            return _eventLogProperty.value;
        }
        set
        {
            if (_eventLogProperty.value == value) return;
            _eventLogProperty.value = value;
            InvalidateReliableLength();
        }
    }

    public int nextPage
    {
        get
        {
            return _nextPageProperty.value;
        }
        set
        {
            if (_nextPageProperty.value == value) return;
            _nextPageProperty.value = value;
            InvalidateReliableLength();
        }
    }

    public enum PropertyID : uint
    {
        Trigger = 1,
        SenderID = 2,
        PageNum = 3,
        EventLog = 4,
        NextPage,

    }

    #region Properties

    private ReliableProperty<int> _triggerProperty;

    private ReliableProperty<int> _senderIDProperty;

    private ReliableProperty<int> _pageNumProperty;

    private ReliableProperty<string> _eventLogProperty;

    private ReliableProperty<int> _nextPageProperty;

    #endregion

    public ServerLogEventModel() : base(null)
    {
        _triggerProperty = new ReliableProperty<int>(1, _trigger);
        _senderIDProperty = new ReliableProperty<int>(2, _senderID);
        _pageNumProperty = new ReliableProperty<int>(3, _pageNum);
        _eventLogProperty = new ReliableProperty<string>(4, _eventLog);
        _nextPageProperty = new ReliableProperty<int>(5, _nextPage);

        SubscribeEventCallback(Normal.Realtime.RealtimeModelEvent.OnDidRead, DidRead);
    }

    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent)
    {
        _triggerProperty.UnsubscribeCallback();
        _senderIDProperty.UnsubscribeCallback();
        _pageNumProperty.UnsubscribeCallback();
        _eventLogProperty.UnsubscribeCallback();
        _nextPageProperty.UnsubscribeCallback();
    }

    protected override int WriteLength(StreamContext context)
    {
        var length = 0;
        length += _triggerProperty.WriteLength(context);
        length += _senderIDProperty.WriteLength(context);
        length += _pageNumProperty.WriteLength(context);
        length += _eventLogProperty.WriteLength(context);
        length += _nextPageProperty.WriteLength(context);
        return length;
    }

    protected override void Write(WriteStream stream, StreamContext context)
    {
        var writes = false;
        writes |= _triggerProperty.Write(stream, context);
        writes |= _senderIDProperty.Write(stream, context);
        writes |= _pageNumProperty.Write(stream, context);
        writes |= _eventLogProperty.Write(stream, context);
        writes |= _nextPageProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }

    protected override void Read(ReadStream stream, StreamContext context)
    {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID))
        {
            var changed = false;
            switch (propertyID)
            {
                case (uint)PropertyID.Trigger:
                    {
                        changed = _triggerProperty.Read(stream, context);
                        break;
                    }
                case (uint)PropertyID.SenderID:
                    {
                        changed = _senderIDProperty.Read(stream, context);
                        break;
                    }
                case (uint)PropertyID.PageNum:
                    {
                        changed = _pageNumProperty.Read(stream, context);
                        break;
                    }
                case (uint)PropertyID.EventLog:
                    {
                        changed = _eventLogProperty.Read(stream, context);
                        break;
                    }
                case (uint)PropertyID.NextPage:
                    {
                        changed = _nextPageProperty.Read(stream, context);
                        break;
                    }
                default:
                    {
                        stream.SkipProperty();
                        break;
                    }
            }
            anyPropertiesChanged |= changed;
        }
        if (anyPropertiesChanged)
        {
            UpdateBackingFields();
        }
    }

    private void UpdateBackingFields()
    {
        _trigger = trigger;
        _senderID = senderID;
        _pageNum = pageNum;
        _eventLog = eventLog;
        _nextPage = nextPage;
    }
    public void FireEvent(int senderID, int pageNum, string eventLog, int nextPage)
    {
        trigger = 1;
        this.senderID = senderID;
        this.pageNum = pageNum;
        this.eventLog = eventLog;
        this.nextPage = nextPage;
        //Debug.Log("Fire: "+pageNum + "next: "+nextPage);
        //eventDidFire(senderID);
    }

    // An event that consumers of this model can subscribe to in order to respond to the event
    public delegate void EventHandler(int senderID, int pageNum, string eventLog, int nextPage);
    public event EventHandler eventDidFire;

    // A RealtimeCallback method that fires whenever we read any values from the server
    [RealtimeCallback(RealtimeModelEvent.OnDidRead)]
    private void DidRead()
    {
        if (eventDidFire != null && trigger == 1)
        {
            eventDidFire(senderID, pageNum, eventLog, nextPage);
            trigger = 0;
        }
    }
}