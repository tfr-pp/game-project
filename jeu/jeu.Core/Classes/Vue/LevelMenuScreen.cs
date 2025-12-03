using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes;

public class LevelMenuScreen : Screen
{
    public enum LevelEntry
    {
        Level1,
        Level2
    }
    
    private LevelEntry levelEntry;
    public LevelMenuScreen()
    {
        levelEntry = LevelEntry.Level1;
    }
    
    public void LoadContent(Texture2D bgTexture2D)
    {
        bgTexture = bgTexture2D;
        Microsoft.Xna.Framework.Point p = new Microsoft.Xna.Framework.Point(0, 0);
        Microsoft.Xna.Framework.Point s = new Microsoft.Xna.Framework.Point(40, 40);
        LevelButton b1 = new LevelButton(1, new Rectangle(p,s),true);
        LevelButton b2 = new LevelButton(2, new Rectangle(p,s),true);
        /*
        LevelButton b3 = new LevelButton();
        LevelButton b4 = new LevelButton();
        */
    }
    
    public void menuDown()
    {
        levelEntry = (LevelEntry)(((int)levelEntry + 1) % Enum.GetNames(typeof(LevelEntry)).Length);
    }
    
    public void menuUp()
    {
        levelEntry = (LevelEntry)(((int)levelEntry - 1) % Enum.GetNames(typeof(LevelEntry)).Length);
    }
    
    public void selectOpt()
    {
        //mystere
    }
    
    public void unselectOpt()
    {
    }

    public void Update(int dt)
    {
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        
    }
}