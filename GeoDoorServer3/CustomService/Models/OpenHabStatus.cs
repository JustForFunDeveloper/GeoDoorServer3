using System;

namespace GeoDoorServer3.CustomService.Models
{
    public class OpenHabStatus
    {
        public TimeSpan OnlineTimeSpan
        {
            get
            {
                lock (_onlineTimeSpanLocker)
                {
                    return _onlineTimeSpan;
                }
            }
            set
            {
                lock (_onlineTimeSpanLocker)
                {
                    _onlineTimeSpan = value;
                }
            }
        }

        public DateTime StartTime
        {
            get
            {
                lock (_startTimeLocker)
                {
                    return _startTime;
                }
            }
            set
            {
                lock (_startTimeLocker)
                {
                    _startTime = value;
                }
            }
        }

        public GateStatus GateStatus
        {
            get
            {
                lock (_gateStatusLocker)
                {
                    return _gateStatus;
                }
            }
            set
            {
                lock (_gateStatusLocker)
                {
                    _gateStatus = value;
                }
            }
        }

        private TimeSpan _onlineTimeSpan;
        private DateTime _startTime;
        private GateStatus _gateStatus;

        private readonly object _onlineTimeSpanLocker = new object();
        private readonly object _startTimeLocker = new object();
        private readonly object _gateStatusLocker = new object();

        public OpenHabStatus()
        {
            _onlineTimeSpan = TimeSpan.Zero;
            _startTime = DateTime.Now;
            _gateStatus = GateStatus.GateOpen;
        }
    }

    public enum GateStatus
    {
        GateOpen,
        GateOpening,
        GateClosing,
        GateClosed
    }
}
