using System;

namespace HCI
{
    class Program
    {
        static void Main(string[] args)
        {
            Robot robot = new Robot();
            
            robot.accept("今天上海天气如何");
            if (!robot.ok()) {
                Console.WriteLine("Error: " + robot.getErrorMessage());
            } else {
                Console.WriteLine("response: " + robot.getResponse());
            }
        }
    }
}
