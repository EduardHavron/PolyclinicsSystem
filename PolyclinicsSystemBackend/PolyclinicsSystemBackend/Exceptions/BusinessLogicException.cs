using System;

namespace PolyclinicsSystemBackend.Exceptions
{
  public class BusinessLogicException : Exception
  {
    public BusinessLogicException(string message) : base(message)
    {
      
    }
  }
}