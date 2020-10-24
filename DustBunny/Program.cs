namespace DustBunny
{
    class Program
    {
        static void Main(string[] args)
            => Startup.RunAsync(args).GetAwaiter().GetResult();
    }
}
