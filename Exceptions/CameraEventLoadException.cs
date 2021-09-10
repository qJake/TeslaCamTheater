using System;

namespace TeslaCamTheater.Exceptions
{
    [Serializable]
    public class CameraEventLoadException : Exception
    {
        public CameraEventLoadException() { }
        public CameraEventLoadException(string message) : base(message) { }
        public CameraEventLoadException(string message, Exception inner) : base(message, inner) { }
        protected CameraEventLoadException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
