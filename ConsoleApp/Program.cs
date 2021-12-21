using System;
using ControllerLib;


namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller controller = new Controller();
            GUI.ProcessUserInterface(controller);
        }
    }
}
