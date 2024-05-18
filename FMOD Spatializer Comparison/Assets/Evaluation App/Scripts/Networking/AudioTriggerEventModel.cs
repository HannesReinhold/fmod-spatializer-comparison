using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;
using System;

[RealtimeModel]
public partial class AudioTriggerEventModel
{
    [RealtimeProperty(5, true)] private int _start;
    [RealtimeProperty(6, true)] private int _fileID;
    [RealtimeProperty(7, true)] private int _senderID;


}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class AudioTriggerEventModel : RealtimeModel
{
    public int start
    {
        get
        {
            return _startProperty.value;
        }
        set
        {
            if (_startProperty.value == value) return;
            _startProperty.value = value;
            InvalidateReliableLength();
        }
    }

    public int fileID
    {
        get
        {
            return _fileIDProperty.value;
        }
        set
        {
            if (_fileIDProperty.value == value) return;
            _fileIDProperty.value = value;
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

    public enum PropertyID : uint
    {
        Start = 5,
        FileID = 6,
        SenderID = 7,
        
    }

    #region Properties

    private ReliableProperty<int> _startProperty;

    private ReliableProperty<int> _fileIDProperty;

    private ReliableProperty<int> _senderIDProperty;

    #endregion

    public AudioTriggerEventModel() : base(null)
    {
        _startProperty = new ReliableProperty<int>(5, _start);
        _fileIDProperty = new ReliableProperty<int>(6, _fileID);
        _senderIDProperty = new ReliableProperty<int>(7, _senderID);

        SubscribeEventCallback(Normal.Realtime.RealtimeModelEvent.OnDidRead, DidRead);
    }

    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent)
    {
        _startProperty.UnsubscribeCallback();
        _fileIDProperty.UnsubscribeCallback();
        _senderIDProperty.UnsubscribeCallback();
    }

    protected override int WriteLength(StreamContext context)
    {
        var length = 0;
        length += _startProperty.WriteLength(context);
        length += _fileIDProperty.WriteLength(context);
        length += _senderIDProperty.WriteLength(context);
        return length;
    }

    protected override void Write(WriteStream stream, StreamContext context)
    {
        var writes = false;
        writes |= _startProperty.Write(stream, context);
        writes |= _fileIDProperty.Write(stream, context);
        writes |= _senderIDProperty.Write(stream, context);
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
                case (uint)PropertyID.Start:
                    {
                        changed = _startProperty.Read(stream, context);
                        break;
                    }
                case (uint)PropertyID.FileID:
                    {
                        changed = _fileIDProperty.Read(stream, context);
                        break;
                    }
                case (uint)PropertyID.SenderID:
                    {
                        changed = _senderIDProperty.Read(stream, context);
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
        _start = start;
        _fileID = fileID;

        _senderID = senderID;
    }
    public void FireEvent(int senderID, int s, int fileID)
    {
        this.start = s;
        this.senderID = senderID;
        this.fileID = fileID;
        //eventDidFire(senderID);
    }

    // An event that consumers of this model can subscribe to in order to respond to the event
    public delegate void EventHandler(int senderID, int start, int fileID);
    public event EventHandler eventDidFire1;

    // A RealtimeCallback method that fires whenever we read any values from the server
    [RealtimeCallback(RealtimeModelEvent.OnDidRead)]
    private void DidRead()
    {
        if (eventDidFire1 != null && start==1)
        {
            eventDidFire1(senderID, start, fileID);
            start = 0;
        }
    }
}
/* ----- End Normal Autogenerated Code ----- */