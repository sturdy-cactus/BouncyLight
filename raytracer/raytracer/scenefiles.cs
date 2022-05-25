using System.Diagnostics;

namespace scenefiles;

public class Token
{
    public SourceLocation Location;
    public string Member;
    
    public Token(string member, SourceLocation location)
    {
        Member = member;
        Location = location;
    }

    public virtual string ToString()
    {
        return Member;
    }
}

public class IdentifierToken : Token
{
    public IdentifierToken(string member, SourceLocation location) : base(member, location){}
}

public class KeywordToken : Token
{
    public KeywordToken(string member, SourceLocation location) : base(member, location){}
}

public class NumberToken : Token
{
    public float Float;

    public NumberToken(string member, SourceLocation location) : base(member, location)
    {
        Float = Convert.ToSingle(member);
    }
}

public class StopToken : Token
{
    public StopToken(string member, SourceLocation location) : base(member, location){}
}

public class StringToken : Token
{
    public StringToken(string member, SourceLocation location) : base(member, location){}
}

public class SymbolToken : Token
{
    public SymbolToken(string member, SourceLocation location) : base(member, location){}
}



public enum KeywordList
{
    NEW,
    MATERIAL,
    PLANE,
    SPHERE,
    DIFFUSE,
    SPECULAR,
    UNIFORM,
    CHECKERED,
    IMAGE,
    IDENTITY,
    TRANSLATION,
    ROTATION_X,
    ROTATION_Y,
    ROTATION_Z,
    SCALING,
    CAMERA,
    ORTHOGONAL,
    PERSPECTIVE,
    FLOAT
}

public struct InputStream
{
    public StreamReader InputFile;
    public SourceLocation Location;
    public int Tabs;

    public char SavedChar;
    public SourceLocation SavedLocation;

    public InputStream(StreamReader inputFile, SourceLocation location, int tabs)
    {
        this.InputFile = inputFile;
        this.Location = location;
        this.Tabs = tabs;

        this.SavedChar = '\0';
        this.SavedLocation = this.Location;
    }

    public void UpdatePosition(char character)
    {
        switch (character)
        {
            case '\0':
                return;

            case '\n':
            {
                Location.LineNum += 1;
                Location.ColNum = 1;
                return;
            }

            case '\t':
            {
                Location.ColNum += Tabs;
                return;
            }

            default:
            {
                Location.ColNum++;
                return;
            }
        }
    }

    public char ReadChar()
    {
        char character;
        
        if (this.SavedChar != '\0')
        {
            character = SavedChar;
            this.SavedChar = '\0';
        }

        else
        {
            character = (char)InputFile.Read();
        }

        SavedLocation = Location;
        UpdatePosition(character);

        return character;
    }

    public void UnreadChar(char character)
    {
        Debug.Assert(SavedChar == '\0');
        this.SavedChar = character;
        this.Location = SavedLocation;
    }

    public void SkipWhitespace()
    {
        string Whitespace = " \n\r\t";

        char character = (char)InputFile.Read();
        while (Whitespace.Contains(character) || character == '#')
        {
            if (character == '#')
                InputFile.ReadLine();
            character = (char) InputFile.Read();
            if (character.ToString() == "")
                return;
        }
        UnreadChar(character);
    }

    public Token ReadToken()
    {
        SkipWhitespace();
        string Symbols = "()[]<>,*";
        
        char character = (char) InputFile.Read();
        
        if (character.ToString() == "")
            return new StopToken(character.ToString(),Location);

        if (Symbols.Contains(character))
            return new SymbolToken(character.ToString(), Location);

        if (character == '"')
        {
            string myString = "";
            while (character!='"')
            {
                myString += character;
                character = (char) InputFile.Read();
            }

            return new StringToken(myString,Location);
        }
        
        if (char.IsDigit(character)){}
    }
    
    
}

public struct SourceLocation
{
    public string Filename;
    public int LineNum;
    public int ColNum;

    public SourceLocation()
    {
        Filename = "";
        LineNum = 0;
        ColNum = 0;
    }
}

public struct GrammarError
{
    public string Message;
    public SourceLocation Location;

    public GrammarError(SourceLocation Location, string Message)
    {
        this.Location = Location;
        this.Message = Message;
    }
}

