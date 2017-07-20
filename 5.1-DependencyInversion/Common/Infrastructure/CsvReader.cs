using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _5._1_DependencyInversion.Common.Infrastructure
{
    public class CsvReader : IDisposable
    {
        #region Fields

        readonly Tokenizer _tokenizer;
        string[] _values;

        #endregion

        #region Constructors / Finalizer

        public CsvReader(Stream stream) : this(new StreamReader(stream ?? Stream.Null))
        {
        }

        public CsvReader(TextReader reader) : this(new Tokenizer(reader))
        {
        }

        public CsvReader(Tokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        ~CsvReader()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        public string this[int index]
        {
            get
            {
                CheckIndex(index);
                return Current[index];
            }
        }

        public int Length
        {
            get { return Current.Length; }
        }

        public string[] Current
        {
            get
            {
                CheckPositionedOnRow();
                return _values;
            }
        }

        #endregion

        #region Methods

        private void CheckIndex(int index)
        {
            if (index >= _values.Length)
            {
                throw new IndexOutOfRangeException();
            }
        }

        public bool ReadNext()
        {
            if (TokenType.EndOfFile == _tokenizer.Current.Type)
            {
                return false;
            }

            var values = new List<string>();

            while (true)
            {
                if (TokenType.EndOfFile == _tokenizer.Current.Type)
                {
                    _values = values.ToArray();
                    return true;
                }

                if (TokenType.EndOfLine == _tokenizer.Current.Type)
                {
                    _values = values.ToArray();
                    _tokenizer.ReadNextToken(true);
                    return true;
                }

                values.Add(ParseValue());
            }
        }

        public string ParseValue()
        {
            if (_tokenizer.Current.Equals(TokenType.Symbol, Characters.Comma))
            {
                _tokenizer.ReadNextToken(true);
            }

            if (TokenType.EndOfFile == _tokenizer.Current.Type || TokenType.EndOfLine == _tokenizer.Current.Type)
            {
                return null;
            }

            return _tokenizer.Current.Equals(
                TokenType.Symbol, Characters.Quote)
                       ? ParseQuotedString()
                       : ParseUnquotedString();
        }

        public string ParseUnquotedString()
        {
            var buffer = new StringBuilder();
            while (true)
            {
                if (TokenType.EndOfFile == _tokenizer.Current.Type || TokenType.EndOfLine == _tokenizer.Current.Type ||
                    _tokenizer.Current.Equals(TokenType.Symbol, Characters.Comma))
                {
                    return EmptyToNull(buffer.ToString().Trim());
                }

                buffer.Append(_tokenizer.Current.Value);
                _tokenizer.ReadNextToken();
            }
        }

        public string ParseQuotedString()
        {
            _tokenizer.ReadNextToken(false);

            var buffer = new StringBuilder();
            while (true)
            {
                if (TokenType.EndOfFile == _tokenizer.Current.Type || TokenType.EndOfLine == _tokenizer.Current.Type)
                {
                    return buffer.ToString();
                }

                if (_tokenizer.Current.Equals(TokenType.Symbol, Characters.Quote))
                {
                    _tokenizer.ReadNextToken(true);
                    return buffer.ToString();
                }

                buffer.Append(_tokenizer.Current.Value);
                _tokenizer.ReadNextToken();
            }
        }

        private static string EmptyToNull(string s)
        {
            return string.IsNullOrEmpty(s) ? null : s;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tokenizer.Dispose();
            }
        }

        public void CheckPositionedOnRow()
        {
            if (null == _values)
            {
                throw new InvalidOperationException("Reader is not positioned on a row.");
            }
        }

        #endregion
    }
}
