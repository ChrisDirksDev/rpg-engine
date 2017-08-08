namespace Rendering
{
    //public class SpriteFactory
    //{

    //    public SpriteFactory(string v)
    //    {
    //        SpriteDirectory = v;
    //    }

    //    public string SpriteDirectory { get; set; }

    //    public SpriteSheet ActiveSheet { get; private set; }

    //    public void LoadSpriteSheet(string identifier, Device device)
    //    {
    //        var directory = $"{Directory.GetCurrentDirectory()}{SpriteDirectory}\\{identifier}";
    //        var bitmap = Texture2D.FromFile(device, directory);

    //        ActiveSheet = new SpriteSheet(bitmap,identifier);
    //    }

    //    /// <summary>
    //    /// Generate a sprite array. Usefull for getting a row of sprites that are all the same size.
    //    /// </summary>
    //    /// <param name="startx">Starting X Coordinate</param>
    //    /// <param name="starty">Starting Y Coordinate</param>
    //    /// <param name="width">The Width of each sprite in the array</param>
    //    /// <param name="length">The Length of each sprite in the array</param>
    //    /// <param name="repeat">Amount of sprites to return</param>
    //    /// <returns></returns>
    //    //public Dictionary<string, Sprite> GenerateSpriteArray(int startx, int starty, int width, int length, int repeat )
    //    //{
    //    //    var sprites = new Dictionary<string,Sprite>();
    //    //    var currentY = starty;
    //    //    for (var i = 0; i < repeat; i++)
    //    //    {
    //    //        var currentX = startx + i*width;
    //    //        var sprite = ActiveSheet.GetSprite(currentX, currentY, width, length);
    //    //        sprites.Add(i.ToString(),sprite);
    //    //    }
    //    //    return sprites;
    //    //}

    //}
}