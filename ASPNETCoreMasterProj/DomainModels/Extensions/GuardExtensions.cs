using System;
using System.Collections.Generic;
using DomainModels.Exceptions;
using DomainModels.Helpers;

namespace DomainModels.Extensions
{
    public static class GuardExtensions
    {
        /// <summary>
        /// Returns the current value if it is greater than or equal to zero.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public static TValue MustBePositive<TValue>(this TValue @value, string errorMessage) where TValue : IComparable<TValue>
        {
            Throw<BadRequestException>.IfFalse(@value.CompareTo(default) >= 0, errorMessage);

            return @value;
        }

        /// <summary>
        /// Returns the current value if it is greater than zero.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public static TValue MustBeGreaterThanZero<TValue>(this TValue @value, string errorMessage) where TValue : IComparable<TValue>
        {
            return MustBeGreaterThan(value, default, errorMessage);
        }

        /// <summary>
        /// Returns the current value if it is greater than a number.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="greaterThan"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public static TValue MustBeGreaterThan<TValue>(this TValue @value, TValue greaterThan, string errorMessage) where TValue : IComparable<TValue>
        {
            Throw<BadRequestException>.IfFalse(@value.CompareTo(greaterThan) > 0, errorMessage);

            return @value;
        }

        /// <summary>
        /// Returns the value if it is less than a number.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="lessThan"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public static TValue MustBeLessThan<TValue>(this TValue @value, TValue lessThan, string errorMessage) where TValue : IComparable<TValue>
        {
            Throw<BadRequestException>.IfFalse(@value.CompareTo(lessThan) < 0, errorMessage);

            return @value;
        }

        /// <summary>
        /// Returns the current value if it is not null or empty.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public static string MustNotBeEmpty(this string @value, string errorMessage)
        {
            Throw<BadRequestException>.IfTrue(string.IsNullOrEmpty(@value), errorMessage);

            return @value;
        }

        /// <summary>
        /// Returns the enum value if it belongs in the list.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public static TEnum MustBeValid<TEnum>(this TEnum @value, string errorMessage) where TEnum : System.Enum, IConvertible
        {
            Throw<BadRequestException>.IfNotDefined(value, errorMessage);

            return @value;
        }

        /// <summary>
        /// Returns the current value if the class is not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="GuardException"></exception>
        public static T MustBeImplemented<T>(this T @value) where T : class
        {
            Throw<GuardException>.IfNull(@value, $"Unable to resolve dependency for {typeof(T)}");

            return @value;
        }

        /// <summary>
        /// Returns the current value if the class is not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public static T MustNotBeNull<T>(this T @value, string errorMessage) where T : class
        {
            Throw<NotFoundException>.IfNull(@value, errorMessage);

            return @value;
        }

        /// <summary>
        /// Throws an exception if the list is null or empty.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        /// <exception cref="BadRequestException"></exception>
        public static IEnumerable<TValue> MustNotBeNullOrEmpty<TValue>(this IEnumerable<TValue> @value, string errorMessage) where TValue : class
        {
            Throw<NotFoundException>.IfNullOrEmpty(@value, errorMessage);

            return @value;
        }
    }
}
