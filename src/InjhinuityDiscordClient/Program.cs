namespace InjhinuityDiscordClient
{
    class Program
    {
        public static void Main(string[] args) =>
           new InjhinuityInstance()
            .RunAndBlockAsync(args)
            .GetAwaiter()
            .GetResult();
    }
}