namespace scenefiles;

public struct Token
{
    public SourceLocation Location;
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

    public char ReadCharacter()
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
        UpdatePosition();
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

