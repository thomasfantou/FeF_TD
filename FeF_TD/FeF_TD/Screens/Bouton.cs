using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FeF_TD
{
    public class Bouton
    {
        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private Texture2D texture;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        private Rectangle rect;

        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }


        private Texture2D textureOver;

        public Texture2D TextureOver
        {
            get { return textureOver; }
            set { textureOver = value; }
        }


        private Vector2 positionOver;

        public Vector2 PositionOver
        {
            get { return positionOver; }
            set { positionOver = value; }
        }

        private Texture2D textureSelected;

        public Texture2D TextureSelected
        {
            get { return textureSelected; }
            set { textureSelected = value; }
        }

        private Texture2D textureSelectedOver;

        public Texture2D TextureSelectedOver
        {
            get { return textureSelectedOver; }
            set { textureSelectedOver = value; }
        }

        private bool selected;

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public Bouton(Texture2D t, Vector2 p, String n = "")
        {
            this.texture = t;
            this.position = p;
            this.rect = new Rectangle((int)this.position.X, (int)this.position.Y, this.texture.Width, this.texture.Height);
            this.name = n;
            this.enabled = true;
        }

        public Bouton(Texture2D t, Vector2 p, Texture2D to, Vector2 po, String n = "")
        {
            this.texture = t;
            this.position = p;
            this.rect = new Rectangle((int)this.position.X, (int)this.position.Y, this.texture.Width, this.texture.Height);
            this.textureOver = to;
            this.positionOver = this.position + po;
            this.name = n;
            this.selected = false;
            this.enabled = true;
            
        }

        public Bouton(Texture2D t, Vector2 p, Texture2D to, Vector2 po, Texture2D ts, Texture2D tso, String n = "")
        {
            this.texture = t;
            this.position = p;
            this.rect = new Rectangle((int)this.position.X, (int)this.position.Y, this.texture.Width, this.texture.Height);
            this.textureOver = to;
            this.positionOver = this.position + po;
            this.textureSelected = ts;
            this.textureSelectedOver = tso;
            this.name = n;
            this.enabled = true;
        }

        public bool IsMouseOver(Vector2 mousePosition)
        {
            if (this.enabled)
            {
                if (this.Rect.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }
    }
}
