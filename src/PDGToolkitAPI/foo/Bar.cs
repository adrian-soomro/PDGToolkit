using System;

namespace PDGToolkitAPI.foo
{
    public class Bar : IBar
    {
        private readonly Settings settings;
        
        public Bar(Settings settings)
        {
            this.settings = settings;
        }
        
        public void DoSomething()
        {
            Console.WriteLine($"The value for key foo is {settings.Foo}.");
        }
    }
}