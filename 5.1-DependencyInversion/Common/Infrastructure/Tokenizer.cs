using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace _5._1_DependencyInversion.Common.Infrastructure
{
    public class Tokenizer : IDisposable
    {
        #region Fields

        private readonly TextReader _reader;
        private char _current;
        private int _position;
        private int _line = 1;
        private Token _token;

        #endregion

        #region Constructors / Finalizer

        public Tokenizer(TextReader reader)
        {
            if (null == reader)
            {
                throw new ArgumentNullException("reader");
            }

            _reader = reader;
            ReadNextChar();
            ReadNextToken();
        }

        ~Tokenizer()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        public Token Current
        {
            get { return _token; }
        }

        private bool EndOfFile
        {
            get { return Characters.Null == _current; }
        }

        private bool IsWhiteSpace
        {
            get { return _current == Characters.Space || _current == Characters.Tab; }
        }

        private bool IsSymbol
        {
            get { return !IsWhiteSpace && !IsLetterOrDigit && !EndOfFile; }
        }

        private bool IsLetterOrDigit
        {
            get { return char.IsLetterOrDigit(_current); }
        }

        #endregion

        #region Methods

        public void ReadNextToken()
        {
            _token = ReadNext();
        }

        public void ReadNextToken(bool skipWhiteSpace)
        {
            ReadNextToken();
            if (skipWhiteSpace)
            {
                SkipWhiteSpace();
            }
        }

        public void SkipWhiteSpace()
        {
            while (_token.Type == TokenType.WhiteSpace)
            {
                ReadNextToken();
            }
        }

        private Token ReadNext()
        {
            if (EndOfFile)
            {
                return new Token(_line, _position, TokenType.EndOfFile);
            }

            if (IsWhiteSpace)
            {
                return ReadWhiteSpace();
            }

            if (_current == Characters.CarriageReturn || _current == Characters.LineFeed)
            {
                return ReadEndOfLine();
            }

            if (IsSymbol)
            {
                return ReadSymbol();
            }

            return ReadWord();
        }

        private Token ReadWord()
        {
            var position = _position;
            var line = _line;

            var value = new StringBuilder();
            while (IsLetterOrDigit || _current == Characters.Underscore)
            {
                value.Append(GetCurrentAndReadNext());
            }

            return new Token(line, position, TokenType.Word, value.ToString());
        }

        private Token ReadSymbol()
        {
            return new Token(_line, _position, TokenType.Symbol, GetCurrentAndReadNext().ToString(CultureInfo.InvariantCulture));
        }

        private Token ReadEndOfLine()
        {
            var position = _position;
            var line = _line;

            var value = new StringBuilder();
            if (_current == Characters.CarriageReturn)
            {
                value.Append(GetCurrentAndReadNext());
            }

            if (_current == Characters.LineFeed)
            {
                value.Append(GetCurrentAndReadNext());
            }

            _line++;
            _position = 0;

            return new Token(line, position, TokenType.EndOfLine, value.ToString());
        }

        private Token ReadWhiteSpace()
        {
            var position = _position;
            var line = _line;

            var value = new StringBuilder();

            while (IsWhiteSpace)
            {
                value.Append(GetCurrentAndReadNext());
            }

            return new Token(line, position, TokenType.WhiteSpace, value.ToString());
        }

        private char GetCurrentAndReadNext()
        {
            var c = _current;
            ReadNextChar();
            return c;
        }

        private void ReadNextChar()
        {
            var read = _reader.Read();
            if (0 > read)
            {
                _current = Characters.Null;
            }
            else
            {
                _position++;
                _current = (char)read;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && null != _reader)
            {
                _reader.Dispose();
            }
        }

        #endregion
    }
}
