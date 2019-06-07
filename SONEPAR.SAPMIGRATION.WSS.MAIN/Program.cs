using SONEPAR.SAPMIGRATION.WSS.LOGIC;

namespace SONEPAR.SAPMIGRATION.WSS.MAIN
{
    class Program
    {
        static void Main(string[] args)
        {
            new ApplicationLogic().Initizalize(); 
            new ApplicationLogic().RunProccess();  
        }
    }
}
