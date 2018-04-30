using System;
using System.Collections.Generic;
using System.Linq;
using SimpleSettings.Exceptions;

namespace SimpleSettings
{
    /// <summary>
    ///     A setting is stored by a specific key. However, a setting key can hide several types of unequal settings if
    ///     TypeLock has not been activated.
    /// </summary>
    public class SimpleSetting
    {
        /// <summary>
        ///     Creates a new Setting with no value.
        /// </summary>
        /// <param name="key">
        ///     The key of the setting.
        ///     <remarks>Converted to upper case.</remarks>
        /// </param>
        /// <param name="typeLock">If typeLock is enabled, the setting can store only one type of value.</param>
        public SimpleSetting(string key, bool typeLock)
        {
            Key = key.ToUpper();
            TypeLock = typeLock;
        }

        /// <summary>
        ///     Key of the Setting
        /// </summary>
        public string Key { get; }

        /// <summary>
        ///     The TypeLock property of the setting.
        ///     If TypeLock is enabled, the setting can store only one type.
        ///     Otherwise the setting can store all valid types.
        /// </summary>
        public bool TypeLock { get; set; }

        /// <summary>
        ///     A list of all value types set so far. If TypeLock is active, this list contains only one entry.
        /// </summary>
        public List<Type> SettingTypes { get; } = new List<Type>();

        /// <summary>
        ///     Stores the int value.
        /// </summary>
        private int ValueInt { get; set; }

        /// <summary>
        ///     Stores the double value.
        /// </summary>
        private double ValueDouble { get; set; }

        /// <summary>
        ///     Stores the string value.
        /// </summary>
        private string ValueString { get; set; }

        /// <summary>
        ///     Stores the bool value.
        /// </summary>
        private bool ValueBool { get; set; }

        /// <summary>
        ///     Stores the datetime value.
        /// </summary>
        private DateTime ValueDateTime { get; set; }

        /// <summary>
        ///     Creates a new setting and fills it directly with the specified value.
        /// </summary>
        /// <typeparam name="T">Type of the specified value.</typeparam>
        /// <param name="key">
        ///     The key of the setting.
        ///     <remarks>Converted to upper case.</remarks>
        /// </param>
        /// <param name="value">First value for the setting</param>
        /// <param name="typeLock">If typeLock is enabled, the setting can store only one type of value.</param>
        /// <returns>Fresh new setting, already filled with the specified value.</returns>
        public static SimpleSetting CreateSetting<T>(string key, T value, bool typeLock = false)
        {
            SimpleSetting newSetting = new SimpleSetting(key, typeLock);
            newSetting.SetValue(value);
            return newSetting;
        }

        /// <summary>
        ///     Returns the wanted value of the setting.
        /// </summary>
        /// <typeparam name="T">Type of the wanted setting value.</typeparam>
        /// <exception cref="EmptySettingException">Thrown if the setting is completly empty.</exception>
        /// <exception cref="ActiveTypeLockException">Thrown if TypeLock is enabled and an invalid type was given.</exception>
        /// <exception cref="InvalidSettingTypeException">Thrown if value is an unsupported setting type.</exception>
        /// <returns>Value of the wanted type from the setting.</returns>
        public T GetValue<T>()
        {
            // Setting.SetValue never called -> exception;
            if (!SettingTypes.Any())
                throw new EmptySettingException($"The setting \"{Key}\" does not have any values at this time.");


            // TypeLock enabled, type already set and didn’t match -> exception;
            if (TypeLock && SettingTypes.Any() && SettingTypes.First() != typeof(T))
                throw new ActiveTypeLockException(
                    $"The setting \"{Key}\" has activated TypeLock. " +
                    "When TypeLock is enabled, a setting can only store and return one data type. " +
                    $"The data type already used is: \"{SettingTypes.First()}\".");

            // Wanted type is never setten -> exception;
            if (!SettingTypes.Contains(typeof(T)))
                throw new EmptySettingException($"The setting \"{Key}\" does not store a value of this type.");

            // Return value for existing type...
            if (typeof(T) == typeof(int))
                return (T)Convert.ChangeType(ValueInt, typeof(T));
            if (typeof(T) == typeof(double))
                return (T)Convert.ChangeType(ValueDouble, typeof(T));
            if (typeof(T) == typeof(string))
                return (T)Convert.ChangeType(ValueString, typeof(T));
            if (typeof(T) == typeof(bool))
                return (T)Convert.ChangeType(ValueBool, typeof(T));
            if (typeof(T) == typeof(DateTime))
                return (T)Convert.ChangeType(ValueDateTime, typeof(T));

            // or thro exception and return list of valid types.
            throw new InvalidSettingTypeException(
                $"The type {typeof(T)} is not a valid setting type. Currently valid setting types are: \n" +
                GetType().GetProperties()
                    .Where(propertyInfo => propertyInfo.Name.StartsWith("Value")).Aggregate("",
                        (current, propertyInfo) => current + $"{propertyInfo.PropertyType}, ")
                    .TrimEnd(','));
        }

        /// <summary>
        ///     Set a value of the setting, or override it.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="value">New value itself.</param>
        /// <exception cref="ActiveTypeLockException">Thrown if TypeLock is enabled and an invalid type was given.</exception>
        /// <exception cref="InvalidSettingTypeException">Thrown if value is an unsupported setting type.</exception>
        public void SetValue<T>(T value)
        {
            // TypeLock enabled, type already set and didn’t match -> exception;
            if (TypeLock && SettingTypes.Any() && SettingTypes.First() != typeof(T))
                throw new ActiveTypeLockException(
                    $"The setting \"{Key}\" has activated TypeLock. " +
                    "When TypeLock is enabled, a setting can only store and return one data type. " +
                    $"The data type already used is: \"{SettingTypes.First()}\".");

            // Set value for existing type...
            if (typeof(T) == typeof(int))
                ValueInt = Convert.ToInt32(value);
            else if (typeof(T) == typeof(double))
                ValueDouble = Convert.ToDouble(value);
            else if (typeof(T) == typeof(string))
                ValueString = Convert.ToString(value);
            else if (typeof(T) == typeof(bool))
                ValueBool = Convert.ToBoolean(value);
            else if (typeof(T) == typeof(DateTime))
                ValueDateTime = Convert.ToDateTime(value);
            else // ...or throw exception and return list of valid types.
                throw new InvalidSettingTypeException(
                    $"The type {typeof(T)} is not a valid setting type. Currently valid setting types are: \n" +
                    GetType().GetProperties()
                        .Where(propertyInfo => propertyInfo.Name.StartsWith("Value")).Aggregate("",
                            (current, propertyInfo) => current + $"{propertyInfo.PropertyType}, ")
                        .TrimEnd(','));

            // No exception thrown -> store setting type if not already stored.
            if (!SettingTypes.Contains(typeof(T)))
                SettingTypes.Add(typeof(T));
        }
    }
}