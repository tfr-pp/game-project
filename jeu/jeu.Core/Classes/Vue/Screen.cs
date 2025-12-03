using System;
using Microsoft.Xna.Framework.Graphics;

namespace jeu.Core.Classes;

public class Screen
{
    
    protected Texture2D bgTexture;
    
    public Texture2D getBgTexture()
    {
        if (bgTexture == null)
        {
            throw new NullReferenceException("Background Texture is null");
        }
        else
        {
            return bgTexture;
        }
    }
}