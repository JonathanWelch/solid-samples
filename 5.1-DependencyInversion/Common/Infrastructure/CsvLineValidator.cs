using System;
using System.Collections.Generic;
using System.Linq;
using _5._1_DependencyInversion.Common.Extensions;

namespace _5._1_DependencyInversion.Common.Infrastructure
{
    public abstract class CsvLineValidator : ICsvLineValidator
    {
        private List<ValidationError> _errors;

        protected abstract int Fields { get; }

        public ICsvLine Validate(string[] line)
        {
            _errors = new List<ValidationError>();
            if (FieldCountIsCorrect(line))
            {
                RunValidation(line);
            }

            return new CsvLine(line, _errors);
        }

        protected abstract void RunValidation(string[] line);

        private bool FieldCountIsCorrect(string[] line)
        {
            if (!line.Any() || line.Length != Fields)
            {
                AddError(line, string.Format("Incorrect number of Fields in line. Expected {0}, encountered {1}", Fields, line.Length));
                return false;
            }

            return true;
        }

        protected void AddError(string[] line, string message)
        {
            _errors.Add(new ValidationError(Severity.Error, message));
        }

        protected void AddWarning(string[] line, string message)
        {
            _errors.Add(new ValidationError(Severity.Warning, message));
        }

        protected void ErrorOnNullOrEmptyString(string[] line, int fieldIndex, int maxLength, string fieldName)
        {
            if (line[fieldIndex].IsNullOrEmpty())
            {
                AddError(line, string.Format("{0} cannot be a null or empty value.", fieldName));
                return;
            }

            if (line[fieldIndex].Length > maxLength)
            {
                AddError(line, string.Format("{0} must be between 1 and {1} characters long. Current value is {2} characters long.", fieldName, maxLength, line[fieldIndex].Length));
            }
        }

        protected void WarnOnNullOrEmptyString(string[] line, int fieldIndex, int maxLength, string fieldName)
        {
            if (line[fieldIndex].IsNullOrEmpty())
            {
                AddWarning(line, string.Format("{0} cannot be a null or empty value.", fieldName));
                return;
            }

            if (line[fieldIndex].Length > maxLength)
            {
                AddWarning(line, string.Format("{0} must be between 1 and {1} characters long. Current value is {2} characters long.", fieldName, maxLength, line[fieldIndex].Length));
            }
        }

        protected void WarnIfNotInteger(string[] line, int fieldIndex, string fieldName)
        {
            ParseAndCheckInteger(line, fieldIndex, () => AddWarning(line, string.Format("{0} must be an integer value.", fieldName)));
        }

        protected void ErrorIfNotPercentage(string[] line, int fieldIndex, string fieldName)
        {
            ParseAndCheckPercentage(line, fieldIndex, () => AddError(line, string.Format("{0} must be a valid value.", fieldName)));
        }

        protected void ErrorIfNotInteger(string[] line, int fieldIndex, string fieldName)
        {
            ParseAndCheckInteger(line, fieldIndex, () => AddError(line, string.Format("{0} must be an integer value.", fieldName)));
        }

        private void ParseAndCheckInteger(string[] line, int fieldIndex, Action callback)
        {
            int quantity;
            if (!int.TryParse(line[fieldIndex], out quantity))
            {
                callback();
            }
        }

        private void ParseAndCheckPercentage(string[] line, int fieldIndex, Action callback)
        {
            byte byteValue;
            if (!byte.TryParse(line[fieldIndex], out byteValue))
            {
                callback();
            }

            if (byteValue < 1 || byteValue > 100)
            {
                callback();
            }
        }
    }
}
