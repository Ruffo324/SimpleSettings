# SimpleSettings
Libary wich can simple store and read settings by key. Allows multiple types per setting key, if enabled.


ConsoleTest.Program.cs example:
```CSharp
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
```

Output:
```Text
Create new simple settings.
Int only done.
String only done.
Mixed done.
Empty done.
Int Only[INTONLYSETTING]: 5
String Only[STRINGONLYSETTING]: Hello World
MixedSetting[MIXEDSETTING]: 10 - Mixed setting String value



Set wrong type value in TypeLock setting:
 Active TypeLock: The setting "INTONLYSETTING" has activated TypeLock. When TypeLock is enabled, a setting can only store and return one data type. The data type already used is: "System.Int32".

Get wrong type value in TypeLock setting:
 Active TypeLock: The setting "INTONLYSETTING" has activated TypeLock. When TypeLock is enabled, a setting can only store and return one data type. The data type already used is: "System.Int32".

Set invalid type:
 Invalid setting type: The type System.Action is not a valid setting type. Currently valid setting types are:


Get invalid type:
 Empty setting: The setting "MIXEDSETTING" does not store a value of this type.

Get value from empty Setting:
 Empty setting: The setting "EMPTYSETTING" does not have any values at this time.
```

## Planned
Save the settings in localDb with the entity framework.
