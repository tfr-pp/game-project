using System;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes;

public class StartScreen : Screen
{
    public enum MenuEntry
    {
        PlayMenu,
        HighScoresMenu,
        LevelMenu,
        OptionsMenu,
        Quit
    }
    
    private MenuEntry _menuEntry;
    public StartScreen()
    {
        _menuEntry = MenuEntry.PlayMenu;
    }
    
    public void LoadContent(Texture2D bgTexture2D)
    {
        bgTexture = bgTexture2D;
    }
    
    public void menuDown()
    {
        _menuEntry = (MenuEntry)(((int)_menuEntry + 1) % Enum.GetNames(typeof(MenuEntry)).Length);
    }
    
    public void menuUp()
    {
        _menuEntry = (MenuEntry)(((int)_menuEntry - 1) % Enum.GetNames(typeof(MenuEntry)).Length);
    }
    
    public void selectOpt(JeuGame game)
    {
        //OPTION: LANCER LE JEU (DEFAUT)
        game.setState(GameState.Playing);
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