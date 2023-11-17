using System;

namespace ExampleApiGameCatalog.Exceptions
{
    public class GameAlreadyRegisteredException : Exception
    {
        public GameAlreadyRegisteredException()
            : base("This game is already registered")
        { }
    }
}
