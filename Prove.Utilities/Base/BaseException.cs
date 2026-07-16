using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Prove.Utilities.Base
{
    public class ParameterNotMatchException : Exception
    {
        public string showMessage;
        public bool useShowMessage = false;
        public ParameterNotMatchException()
        {
        }
        public ParameterNotMatchException(string showMessage, bool useShowMessage = false)
        {
            this.showMessage = showMessage;
            this.useShowMessage = useShowMessage || !string.IsNullOrWhiteSpace(showMessage);
        }

        public ParameterNotMatchException(string message) : base(message)
        {
        }

        public ParameterNotMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParameterNotMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class DomainLayerException : Exception
    {
        public string showMessage;
        public bool useShowMessage = false;
        public DomainLayerException()
        {
        }
        public DomainLayerException(string showMessage, bool useShowMessage = false)
        {
            this.showMessage = showMessage;
            this.useShowMessage = useShowMessage || !string.IsNullOrWhiteSpace(showMessage);
        }

        public DomainLayerException(string message) : base(message)
        {
        }

        public DomainLayerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DomainLayerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class DataNotFoundException : Exception
    {
        public string showMessage;
        public bool useShowMessage = false;
        public DataNotFoundException()
        {
        }
        public DataNotFoundException(string showMessage, bool useShowMessage = false)
        {
            this.showMessage = showMessage;
            this.useShowMessage = useShowMessage;
        }

        public DataNotFoundException(string message) : base(message)
        {
        }

        public DataNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class SendSMSFailedException : Exception
    {
        public string showMessage;
        public bool useShowMessage = false;
        public SendSMSFailedException()
        {
        }
        public SendSMSFailedException(string showMessage, bool useShowMessage = false)
        {
            this.showMessage = showMessage;
            this.useShowMessage = useShowMessage;
        }

        public SendSMSFailedException(string message) : base(message)
        {
        }

        public SendSMSFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SendSMSFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class SendMailFailedException : Exception
    {
        public string showMessage;
        public bool useShowMessage = false;
        public SendMailFailedException()
        {
        }
        public SendMailFailedException(string showMessage, bool useShowMessage = false)
        {
            this.showMessage = showMessage;
            this.useShowMessage = useShowMessage;
        }

        public SendMailFailedException(string message) : base(message)
        {
        }

        public SendMailFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SendMailFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
