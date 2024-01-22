using System;

namespace Apphr.Domain.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LogIgnoreAttribute : Attribute
    {         
    }
}