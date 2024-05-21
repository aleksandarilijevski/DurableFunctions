using System;

namespace DurableFunctions.Exceptions
{
    public class EntityAlreadyExistsInTableStorageException : Exception
    {
        public EntityAlreadyExistsInTableStorageException() : base() { }

        public EntityAlreadyExistsInTableStorageException(string message) : base(message)
        {

        }
    }
}
