using ShadowEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.OpenGl;

namespace projectclient
{
    public class r_Rectangle
    {
        #region 사람
        public void rectangleDraw(float personx, float persony, float personz)
        {
            // size height and length
            int width = 10;
            int height = 25;
            int length = 10;

            // begins at these coordinates
            float x = personx;      //10
            float y = persony;      //-3
            float z = personz;      //7

            //encentrar the square
            x = x - width / 2;
            y = y - height / 2;
            z = z - length / 2;
            Gl.glEnable(Gl.GL_TEXTURE_2D);

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("yanet1.jpg"));//백

            //begins to draw squares
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(-1, 1, 1);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x + width, y, z);
            Gl.glNormal3d(-1, -1, 1);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x + width, y + height, z);
            Gl.glNormal3d(1, -1, 1);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x, y + height, z);
            Gl.glNormal3d(1, 1, 1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x, y, z);
            Gl.glEnd();

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("yanet1.jpg"));//아ㅍ
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(1, 1, -1);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x, y, z + length);
            Gl.glNormal3d(1, -1, -1);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x, y + height, z + length);
            Gl.glNormal3d(-1, -1, -1);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x + width, y + height, z + length);
            Gl.glNormal3d(-1, 1, -1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, y, z + length);
            Gl.glEnd();

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("yanet1.jpg"));//위
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(-1, -1, 1);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x + width, y + height, z);
            Gl.glNormal3d(-1, -1, -1);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x + width, y + height, z + length);
            Gl.glNormal3d(1, -1, -1);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x, y + height, z + length);
            Gl.glNormal3d(1, -1, 1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x, y + height, z);
            Gl.glEnd();

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("yanet1.jpg"));//ㅇㅚㄴ
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(1, -1, 1);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x, y + height, z);
            Gl.glNormal3d(1, -1, -1);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x, y + height, z + length);
            Gl.glNormal3d(1, 1, -1);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x, y, z + length);
            Gl.glNormal3d(1, 1, 1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x, y, z);
            Gl.glEnd();

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("yanet1.jpg"));//오
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(-1, 1, 1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, y, z);
            Gl.glNormal3d(-1, 1, -1);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x + width, y, z + length);
            Gl.glNormal3d(-1, -1, -1);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x + width, y + height, z + length);
            Gl.glNormal3d(-1, -1, 1);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x + width, y + height, z);
            Gl.glEnd();

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("floor.jpg"));//바닥
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(1, 1, 1);
            Gl.glTexCoord2f(8.0f, 0.0f); Gl.glVertex3d(x, y, z);
            Gl.glNormal3d(1, 1, -1);
            Gl.glTexCoord2f(8.0f, 8.0f); Gl.glVertex3d(x, y, z + length);
            Gl.glNormal3d(-1, 1, -1);
            Gl.glTexCoord2f(0.0f, 8.0f); Gl.glVertex3d(x + width, y, z + length);
            Gl.glNormal3d(-1, 1, 1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, y, z);
            Gl.glEnd();
        }
        #endregion

        #region 미아
        public void missingrectangleDraw(float personx, float persony, float personz)
        {
            // size height and length
            int width = 10;
            int height = 25;
            int length = 10;

            // begins at these coordinates
            float x = personx;      //10
            float y = persony;      //-3
            float z = personz;      //7

            //encentrar the square
            x = x - width / 2;
            y = y - height / 2;
            z = z - length / 2;
            Gl.glEnable(Gl.GL_TEXTURE_2D);

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("red.jpg"));//백

            //begins to draw squares
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(-1, 1, 1);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x + width, y, z);
            Gl.glNormal3d(-1, -1, 1);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x + width, y + height, z);
            Gl.glNormal3d(1, -1, 1);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x, y + height, z);
            Gl.glNormal3d(1, 1, 1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x, y, z);
            Gl.glEnd();

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("red.jpg"));//아ㅍ
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(1, 1, -1);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x, y, z + length);
            Gl.glNormal3d(1, -1, -1);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x, y + height, z + length);
            Gl.glNormal3d(-1, -1, -1);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x + width, y + height, z + length);
            Gl.glNormal3d(-1, 1, -1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, y, z + length);
            Gl.glEnd();

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("red.jpg"));//위
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(-1, -1, 1);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x + width, y + height, z);
            Gl.glNormal3d(-1, -1, -1);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x + width, y + height, z + length);
            Gl.glNormal3d(1, -1, -1);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x, y + height, z + length);
            Gl.glNormal3d(1, -1, 1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x, y + height, z);
            Gl.glEnd();

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("red.jpg"));//ㅇㅚㄴ
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(1, -1, 1);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x, y + height, z);
            Gl.glNormal3d(1, -1, -1);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x, y + height, z + length);
            Gl.glNormal3d(1, 1, -1);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x, y, z + length);
            Gl.glNormal3d(1, 1, 1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x, y, z);
            Gl.glEnd();

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("red.jpg"));//오
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(-1, 1, 1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, y, z);
            Gl.glNormal3d(-1, 1, -1);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x + width, y, z + length);
            Gl.glNormal3d(-1, -1, -1);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x + width, y + height, z + length);
            Gl.glNormal3d(-1, -1, 1);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x + width, y + height, z);
            Gl.glEnd();

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("red.jpg"));//바닥
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(1, 1, 1);
            Gl.glTexCoord2f(8.0f, 0.0f); Gl.glVertex3d(x, y, z);
            Gl.glNormal3d(1, 1, -1);
            Gl.glTexCoord2f(8.0f, 8.0f); Gl.glVertex3d(x, y, z + length);
            Gl.glNormal3d(-1, 1, -1);
            Gl.glTexCoord2f(0.0f, 8.0f); Gl.glVertex3d(x + width, y, z + length);
            Gl.glNormal3d(-1, 1, 1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, y, z);
            Gl.glEnd();
        }
        #endregion

        #region angle
        public void AngleChaneX(float posIX)
        {
            if (posIX == -360 || posIX == 360)
                posIX = 0;

            if (posIX == -270)
                posIX = 90;

            if (posIX == -180)
                posIX = 180;

            if (posIX == -90)
                posIX = 270;
        }

        public void AngleChaneY(float posIY)
        {

            if (posIY == -360 || posIY == 360)
                posIY = 0;

            if (posIY == -270)
                posIY = 90;

            if (posIY == -180)
                posIY = 180;

            if (posIY == -90)
                posIY = 270;
        }

        public void AngleChaneZ(float posIZ)
        {
            if (posIZ == -360 || posIZ == 360)
                posIZ = 0;

            if (posIZ == -270)
                posIZ = 90;

            if (posIZ == -180)
                posIZ = 180;

            if (posIZ == -90)
                posIZ = 270;
        }
        #endregion

        #region 이름판
        //이름판
        public void rectangle_Text(string t_name, float posix, float posiy, float posiz, float personx, float persony, float personz)
        {
            // size height and length
            int width = 50;
            int height = 25;
            int length = 50;

            // begins at these coordinates
            float x = personx;      //10
            float y = persony + 30;           //-3        //높이
            float z = personz;      //7

            //encentrar the square
            x = x - width / 2;
            y = y - height / 2;
            z = z - length / 2;


            Gl.glEnable(Gl.GL_TEXTURE_2D);

            AngleChaneX(posix);
            //AngleChaneY(posiy);
            AngleChaneZ(posiz);

            // if ((posix >= 0 && posix < 45) || (posix >= 135 && posix < 225))


            #region Y축이동
            if (posiy >= 15)
            {
                if (posix == 90)
                {
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName(t_name + ".jpg"));//_u
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glNormal3d(1, 1, 1);
                    Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x, y, z);
                    Gl.glNormal3d(1, 1, -1);
                    Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x, y, z + (length / 2));
                    Gl.glNormal3d(-1, 1, -1);
                    Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x + width, y, z + (length / 2));
                    Gl.glNormal3d(-1, 1, 1);
                    Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, y, z);
                    Gl.glEnd();
                }
                else
                {
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName(t_name + "_1.jpg"));//_u1
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glNormal3d(1, 1, 1);
                    Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x, y, z);
                    Gl.glNormal3d(1, 1, -1);
                    Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x, y, z + (length / 2));
                    Gl.glNormal3d(-1, 1, -1);
                    Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x + width, y, z + (length / 2));
                    Gl.glNormal3d(-1, 1, 1);
                    Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, y, z);
                    Gl.glEnd();
                }

            }
            #endregion

            #region Z
            else if (posiz >= 15)
            {
                if (posix == 0)
                {
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName(t_name + "_2.jpg"));//_u2
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glNormal3d(-1, -1, 1);
                    Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x, y + height, z);
                    Gl.glNormal3d(-1, -1, -1);
                    Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x, y + height, z + length);
                    Gl.glNormal3d(1, -1, -1);
                    Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x + (width / 2), y + height, z + length);
                    Gl.glNormal3d(1, -1, 1);
                    Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + (width / 2), y + height, z);

                    Gl.glEnd();
                }
                else
                {
                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName(t_name + "_3.jpg"));//_u3
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glNormal3d(-1, -1, 1);
                    Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x, y + height, z);
                    Gl.glNormal3d(-1, -1, -1);
                    Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x, y + height, z + length);
                    Gl.glNormal3d(1, -1, -1);
                    Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x + (width / 2), y + height, z + length);
                    Gl.glNormal3d(1, -1, 1);
                    Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + (width / 2), y + height, z);

                    Gl.glEnd();
                }

            }

            #endregion

            #region X
            else if ((posix >= 45 && posix < 135) || (posix >= 225 && posix < 315))
            {
                #region x가 90도 일때 스크린상 앞뒤

                Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName(t_name + ".jpg"));//_u
                //begins to draw squares
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glNormal3d(-1, 1, 1);
                Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x + width, y, z + 1 + (length / 2));
                Gl.glNormal3d(-1, -1, 1);
                Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x + width, y + height, z + 1 + (length / 2));
                Gl.glNormal3d(1, -1, 1);
                Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x, y + height, z + 1 + (length / 2));
                Gl.glNormal3d(1, 1, 1);
                Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x, y, z + 1 + (length / 2));
                Gl.glEnd();

                Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName(t_name + ".jpg"));//_u
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glNormal3d(1, 1, -1);
                Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x, y, z + (length / 2));
                Gl.glNormal3d(1, -1, -1);
                Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x, y + height, z + (length / 2));
                Gl.glNormal3d(-1, -1, -1);
                Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x + width, y + height, z + (length / 2));
                Gl.glNormal3d(-1, 1, -1);
                Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, y, z + (length / 2));
                Gl.glEnd();

                #endregion
            }

            else
            {
                #region x가 0도 일때 스크린상 좌우

                Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName(t_name + ".jpg"));//_u
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glNormal3d(-1, 1, 1);
                Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + (width / 2), y, z);
                Gl.glNormal3d(-1, 1, -1);
                Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x + (width / 2), y, z + length);
                Gl.glNormal3d(-1, -1, -1);
                Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x + (width / 2), y + height, z + length);
                Gl.glNormal3d(-1, -1, 1);
                Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x + (width / 2), y + height, z);
                Gl.glEnd();

                Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName(t_name + "_0.jpg"));//_u0
                Gl.glBegin(Gl.GL_QUADS);
                Gl.glNormal3d(1, -1, 1);
                Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x + 1 + (width / 2), y + height, z); //우위
                Gl.glNormal3d(1, -1, -1);
                Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x + 1 + (width / 2), y + height, z + length);
                Gl.glNormal3d(1, 1, -1);
                Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x + 1 + (width / 2), y, z + length);
                Gl.glNormal3d(1, 1, 1);
                Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + 1 + (width / 2), y, z);
                Gl.glEnd();


                #endregion

            }

            #endregion
        }
        #endregion

        #region 바닥
        //바닥
        public void Un_Floor(string F)
        {
            int width = 450;
            int length = 570;

            // begins at these coordinates
            float x = -10;      //10
            float z = 1000;      //7

            switch (F)
            {
                case "ALL":
                    // size height and length                    
                    length = 598; //Y축 길이

                    // begins at these coordinates
                    x = -10;      //10
                    z = 28;      //7

                    //encentrar the square
                    x = x - width / 2;
                    z = z - length / 2;

                    Gl.glEnable(Gl.GL_TEXTURE_2D);

                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("floor1.jpg"));
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glNormal3d(1, 1, 1);
                    Gl.glTexCoord2f(8.0f, 0.0f); Gl.glVertex3d(x, -15, z);
                    Gl.glNormal3d(1, 1, -1);
                    Gl.glTexCoord2f(8.0f, 8.0f); Gl.glVertex3d(x, -15, z + length);
                    Gl.glNormal3d(-1, 1, -1);
                    Gl.glTexCoord2f(0.0f, 8.0f); Gl.glVertex3d(x + width, -15, z + length);
                    Gl.glNormal3d(-1, 1, 1);
                    Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, -15, z);
                    Gl.glEnd();

                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("floor1.jpg"));
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glNormal3d(1, 1, 1);
                    Gl.glTexCoord2f(8.0f, 0.0f); Gl.glVertex3d(x, 120, z);
                    Gl.glNormal3d(1, 1, -1);
                    Gl.glTexCoord2f(8.0f, 8.0f); Gl.glVertex3d(x, 120, z + length);
                    Gl.glNormal3d(-1, 1, -1);
                    Gl.glTexCoord2f(0.0f, 8.0f); Gl.glVertex3d(x + width, 120, z + length);
                    Gl.glNormal3d(-1, 1, 1);
                    Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, 120, z);
                    Gl.glEnd();

                    Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("floor1.jpg"));
                    Gl.glBegin(Gl.GL_QUADS);
                    Gl.glNormal3d(1, 1, 1);
                    Gl.glTexCoord2f(8.0f, 0.0f); Gl.glVertex3d(x, 242, z);
                    Gl.glNormal3d(1, 1, -1);
                    Gl.glTexCoord2f(8.0f, 8.0f); Gl.glVertex3d(x, 242, z + length);
                    Gl.glNormal3d(-1, 1, -1);
                    Gl.glTexCoord2f(0.0f, 8.0f); Gl.glVertex3d(x + width, 242, z + length);
                    Gl.glNormal3d(-1, 1, 1);
                    Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, 242, z);
                    Gl.glEnd();

                    break;
            }
        }
        #endregion

        #region 강의실 이름
        //강의실 이름.
        public void rectangle_classText(string t_name, float posix, float posiy, float posiz, float personx, float persony, float personz)
        {
            // size height and length
            int width = 50;
            int height = 38;
            int length = 80;

            // begins at these coordinates
            float x = personx-50;      //10
            float y = persony + 30;           //-3        //높이
            float z = personz;      //7

            //encentrar the square
            x = x - width / 2;
            y = y - height / 2;
            z = z - length / 2;


            Gl.glEnable(Gl.GL_TEXTURE_2D);

            AngleChaneX(posix);
            //AngleChaneY(posiy);
            AngleChaneZ(posiz);

            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ContentManager.GetTextureByName("H" + t_name + "n_2.JPG"));//_u
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glNormal3d(1, 1, 1);
            Gl.glTexCoord2f(1.0f, 0.0f); Gl.glVertex3d(x, y, z);
            Gl.glNormal3d(1, 1, -1);
            Gl.glTexCoord2f(1.0f, 1.0f); Gl.glVertex3d(x, y, z + (length / 2));
            Gl.glNormal3d(-1, 1, -1);
            Gl.glTexCoord2f(0.0f, 1.0f); Gl.glVertex3d(x + width, y, z + (length / 2));
            Gl.glNormal3d(-1, 1, 1);
            Gl.glTexCoord2f(0.0f, 0.0f); Gl.glVertex3d(x + width, y, z);
            Gl.glEnd();
        }
        #endregion
    }
}
