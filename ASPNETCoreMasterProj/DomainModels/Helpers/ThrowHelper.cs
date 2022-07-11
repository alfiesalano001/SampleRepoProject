using System;
using System.Collections.Generic;
using System.Linq;
using DomainModels.Exceptions;

namespace DomainModels.Helpers
{
    /// <summary>
    /// Throw an error based on given type TException.
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    public static class Throw<TException> where TException : GuardException
    {
        /// <summary>
        /// Throws an error if value is true
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        public static void IfTrue(bool @value, string errorMessage)
        {
            if (@value) ThrowException(errorMessage);
        }

        /// <summary>
        /// Throws an error if value is false
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        public static void IfFalse(bool @value, string errorMessage)
        {
            if (!@value) ThrowException(errorMessage);
        }

        /// <summary>
        /// Throws an error if value does not exist in enum
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        public static void IfNotDefined<TEnum>(TEnum @value, string errorMessage) where TEnum : System.Enum, IConvertible
        {
            if (!System.Enum.IsDefined(typeof(TEnum), @value)) ThrowException(errorMessage);
        }

        /// <summary>
        /// Throws an error if value is not greater than zero.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        public static void IfNotGreaterThanZero<TValue>(TValue @value, string errorMessage)
            where TValue : IComparable<TValue>
        {
            if (@value.CompareTo(default) <= 0) ThrowException(errorMessage);
        }

        /// <summary>
        /// Throws an error if value is null or empty
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        public static void IfNullOrEmpty(string @value, string errorMessage)
        {
            if (string.IsNullOrEmpty(@value.Trim())) ThrowException(errorMessage);
        }

        /// <summary>
        /// Throws an error if the value is null.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        public static void IfNull<TValue>(TValue @value, string errorMessage) where TValue : class
        {
            if (@value is null) ThrowException(errorMessage);
        }

        /// <summary>
        /// Throws an exception if the list is null or empty.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="errorMessage"></param>
        public static void IfNullOrEmpty<TValue>(IEnumerable<TValue> @value, string errorMessage) where TValue : class
        {
            if (@value is null) ThrowException(errorMessage);
            if (!@value.Any()) ThrowException(errorMessage);
        }

        /// <summary>
        /// Throw an exception based on given TException type
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="GuardException"></exception>
        private static void ThrowException(string errorMessage)
        {
            var type = typeof(TException);

            if (type == typeof(NotFoundException)) throw new NotFoundException(errorMessage);
            if (type == typeof(BadRequestException)) throw new BadRequestException(errorMessage);

            throw new GuardException(errorMessage);
        }
    }
}
