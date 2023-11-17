using System;

namespace ExampleApiGameCatalog.Exceptions
{
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException()
            : base("This game is not registered")
        { }
    }
}
