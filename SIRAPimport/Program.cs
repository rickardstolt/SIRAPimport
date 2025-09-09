using PowerArgs;

namespace SIRAPimport
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            Args.InvokeAction<ImportProgram>(args);
        }
    }
}
