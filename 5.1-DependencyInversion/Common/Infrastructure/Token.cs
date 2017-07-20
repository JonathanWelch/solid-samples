namespace _5._1_DependencyInversion.Common.Infrastructure
{
    public class Token
    {
        #region Constructors

        internal Token(int line, int position, TokenType type, string value)
        {
            Type = type;
            Value = value;
            Line = line;
            Position = position;
        }

        internal Token(int line, int position, TokenType type) : this(line, position, type, null)
        {
        }

        #endregion

        #region Properties

        public int Line { get; private set; }

        public int Position { get; private set; }

        public TokenType Type { get; private set; }

        public string Value { get; private set; }

        #endregion

        #region Methods

        public bool Equals(TokenType type, char value)
        {
            return Equals(new Token(Line, Position, type, new string(new[] { value })));
        }

        public bool Equals(Token token)
        {
            return token.Type.Equals(Type) &&
                token.Value == Value &&
                token.Position == Position &&
                token.Line == Line;
        }

        #endregion
    }
}
