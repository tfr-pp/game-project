using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes;

public class LevelMenuScreen : Screen
{
    public enum LevelEntry
    {
        Level1,
        Level2,
        Level3,
        Level4
    }
    
    private LevelEntry levelEntry;
    public LevelMenuScreen()
    {
        levelEntry = LevelEntry.Level1;
    }
    
    public void LoadContent(Texture2D bgTexture2D)
    {
        bgTexture = bgTexture2D;
        Microsoft.Xna.Framework.Point p = new Microsoft.Xna.Framework.Point(200, 20);
        Microsoft.Xna.Framework.Point s = new Microsoft.Xna.Framework.Point(50, 20);
        LevelButton b1 = new LevelButton(1, new Rectangle(p,s),true);
        p.Y += 30;
        LevelButton b2 = new LevelButton(2, new Rectangle(p,s),true);
        p.Y += 30;
        LevelButton b3 = new LevelButton(3, new Rectangle(p,s),true);
        p.Y += 30;
        LevelButton b4 = new LevelButton(4, new Rectangle(p,s),true);
    }
    
    public void menuDown()
    {
        levelEntry = (LevelEntry)(((int)levelEntry + 1) % Enum.GetNames(typeof(LevelEntry)).Length);
    }
    
    public void menuUp()
    {
        levelEntry = (LevelEntry)(((int)levelEntry - 1) % Enum.GetNames(typeof(LevelEntry)).Length);
    }
    
    public void selectOpt(JeuGame game)
    {
        int level = (int)levelEntry;
        game.gameManager.LoadFirstLevel(level);
        //game.setState(GameState.Playing);
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