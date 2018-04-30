using System;
using System.Collections.Generic;
using SimpleSettings;

namespace ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Create new simple settings & fill them.
            Console.WriteLine("Create new simple settings.");

            SimpleSetting intSetting = SimpleSetting.CreateSetting("IntOnlySetting", 5, true);
            Console.WriteLine("Int only done.");

            SimpleSetting stringSetting = SimpleSetting.CreateSetting("StringOnlySetting", "Hello World", true);
            Console.WriteLine("String only done.");

            SimpleSetting mixedSetting = SimpleSetting.CreateSetting("MixedSetting", 10);
            mixedSetting.SetValue("Mixed setting String value");
            Console.WriteLine("Mixed done.");

            SimpleSetting emptySetting = new SimpleSetting("EmptySetting", false);
            Console.WriteLine("Empty done.");


            // Get value of settings correctly
            Console.WriteLine($"Int Only[{intSetting.Key}]: {intSetting.GetValue<int>()}");
            Console.WriteLine($"String Only[{stringSetting.Key}]: {stringSetting.GetValue<string>()}");
            Console.WriteLine($"MixedSetting[{mixedSetting.Key}]: {mixedSetting.GetValue<int>()} - {mixedSetting.GetValue<string>()}");

            Console.WriteLine("\n\n");
            
            // Test possible exceptions
            Console.WriteLine($"Set wrong type value in TypeLock setting: \n {RunAndGiveExceptionMessage(() => intSetting.SetValue("Not allowed string."))}");
            Console.Write("\n");
            Console.WriteLine($"Get wrong type value in TypeLock setting: \n {RunAndGiveExceptionMessage(() => intSetting.GetValue<string>())}");
            Console.Write("\n");
            Console.WriteLine($"Set invalid type: \n {RunAndGiveExceptionMessage(() => mixedSetting.SetValue<Action>(() => { }))}");
            Console.Write("\n");
            Console.WriteLine($"Get invalid type: \n {RunAndGiveExceptionMessage(() => mixedSetting.GetValue<Action>())}");
            Console.Write("\n");
            Console.WriteLine($"Get value from empty Setting: \n {RunAndGiveExceptionMessage(() => emptySetting.GetValue<string>())}");

            // Wait for input.
            Console.ReadLine();
        }

        private static string RunAndGiveExceptionMessage(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "No exception thrown.";
        }
    }
}
