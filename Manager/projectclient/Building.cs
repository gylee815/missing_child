using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShadowEngine;
using ShadowEngine.ContentLoading;
using Tao.OpenGl; 

namespace projectclient
{
    public class Building
    {
        ModelContainer m ;
        //Mesh aspa;
        public float posIX = 0, posIY = 0, posIZ = 0;
        int aa = 0;
        int bb = 0;

        public float rot = 2;

        public void Create(string Floor)
        {
            m = ContentManager.GetModelByName(Floor + ".3DS");
            m.RemoveMeshByName(Floor + ".3DS");
            m.CreateDisplayList();
        }
        //public void CreateCollisions()
        //{
        //    Collision.AddCollisionSegment(new Point3D(-24.4f, -14.1f, 0), new Point3D(18.9f, -14.1f, 0), 0.20f);
        //    Collision.AddCollisionSegment(new Point3D(-24.4f, -14.1f, 0), new Point3D(-24.4f, 13.2f, 0), 0.5f);
        //    Collision.AddCollisionSegment(new Point3D(-20.2f, -0.1f, 0), new Point3D(-4.8f, -0.1f, 0), 0.5f);
        //    Collision.AddCollisionSegment(new Point3D(-0.5f, 0.7f, 0), new Point3D(-0.5f, -8.7f, 0), 0.5f);
        //    Collision.AddCollisionSegment(new Point3D(19.4f, 14.4f, 0), new Point3D(-24.4f, 14.4f, 0), 0.5f);
        //    Collision.AddCollisionSegment(new Point3D(19.4f, 14.4f, 0), new Point3D(19.4f, -14.4f, 0), 0.5f);
        //    Collision.AddCollisionSegment(new Point3D(-17.5f, 0.5f, 0), new Point3D(-17.5f, 11, 0), 0.5f);
        //    Collision.AddCollisionSegment(new Point3D(13.4f, -0.15f, 0), new Point3D(18.74f, -0.15f, 0), 0.5f);
        //    Collision.AddCollisionSegment(new Point3D(-0.43f, -9.1f, 0), new Point3D(12.4f, -9.1f, 0), 0.5f);
        //    //Collision.GhostMode = true;   
        //}
        
        public void Dibujar(string floor)
        {

            Gl.glPushMatrix();
            Gl.glTranslatef(0, 0f, 0);

            if (floor == "ALL") { Gl.glScalef(0.1f, 0.1f, 0.1f); }
            else { Gl.glScalef(0.13f, 0.13f, 0.13f); }

            m.DrawWithTextures(); //???

            Gl.glPushMatrix();

            //bladeAngle += 25;

            //if (bladeAngle > 3600)
            //{
            //    bladeAngle = 0;
            //}

            Gl.glPopMatrix();
            
        }
       
        #region angle
        public void AngleChaneX()
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
        public void AngleChaneY()
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
        public void AngleChaneZ()
        {
            if (posIZ == -360 || posIZ == 360)
                posIZ = 0;

            if (posIZ == -270)
                posIZ = 90;

            if (posIZ == -180)
                posIZ = 180;

            if (posIX == -90)
                posIZ = 270;
        }
        #endregion

        #region 좌우마우스
        public void BuildingDrawX(bool movX)
        {
            if (movX == true)       //시계방향 회전 왼쪽 드래그
            {
                AngleChaneX();
                AngleChaneY();
                AngleChaneZ();

                #region x축회전
                if ((posIZ == 0 || posIZ == 180) && (posIY == 0 || posIY == 180))
                {
                    posIX -= rot;
                    Gl.glRotated(-rot, 0, 1, 0);

                    if (posIX >= 0 && posIX < 90)
                    {
                        //좌->우                       
                        aa += 7;
                        bb -= 0;
                    }
                    else if (posIX >= 90 && posIX < 180)
                    {
                        // MessageBox.Show(posIX.ToString());
                        //아->위
                        aa -= 1;
                        bb -= 5;
                    }
                    else if (posIX >= 180 && posIX < 270)
                    {
                        ////우->좌
                        aa -= 7;
                        bb += 0;
                    }
                    else
                    {
                        //위-->아
                        aa += 1;
                        bb += 5;
                    }

                }
                #endregion

                if ((posIZ == 75 || posIZ == 225))
                {
                    //Y축회전
                    if (posIX == 0 || posIX == 180)
                    {
                        posIY -= rot;
                        Gl.glRotated(-rot, 1, 0, 0);
                    }
                    //Z축회전
                    if (posIX == 90 || posIX == 270)
                    {
                        posIZ -= rot;
                        Gl.glRotated(-rot, 0, 0, 1);
                        aa -= 6;
                        bb -= 10;
                    }
                }

            }

            else if (movX == false) //반시계방향 오른쪽 드래그
            {
                AngleChaneX();
                AngleChaneY();
                AngleChaneZ();

                #region X축
                if ((posIZ == 0 || posIZ == 180) && (posIY == 0 || posIY == 180))
                {
                    posIX += rot;
                    Gl.glRotated(+rot, 0, 1, 0);

                    if (posIX >= 0 && posIX < 90)
                    {
                        aa -= 7;
                        bb -= 0;

                    }
                    else if (posIX >= 90 && posIX < 180)
                    {
                        aa -= 1;
                        bb += 5;

                    }
                    else if (posIX >= 180 && posIX < 270)
                    {

                        aa += 7;
                        bb += 01;
                    }
                    else
                    {
                        //위-->아
                        aa += 1;
                        bb -= 5;
                    }

                }
                #endregion

                if ((posIZ == 90 || posIZ == 270))
                {
                    //Y축회전
                    if (posIX == 0 || posIX == 180)
                    {
                        posIY += rot;
                        Gl.glRotated(+rot, 1, 0, 0);

                    }
                    //Z축회전
                    if (posIX == 90 || posIX == 270)
                    {
                        posIZ += rot;
                        Gl.glRotated(+rot, 0, 0, 1);
                        aa -= 6;
                        bb -= 1;
                    }
                }
                // rot -= 5;
            }
            Gl.glPopMatrix();
        }
        #endregion

        #region 위아래
        public void BuildingDrawY(bool Y)
        {
            if (Y == true)
            {
                AngleChaneX();
                AngleChaneY();
                AngleChaneZ();

                #region Z축회전
                //
                if ((posIX == 0 || posIX == 180) && (posIY == 0 || posIY == 180))
                {
                    if (posIX == 0)
                    {
                        if (posIZ >= 75)
                            Gl.glRotated(0, 0, 0, 1);
                        else
                        {
                            posIZ += rot;
                            Gl.glRotated(+rot, 0, 0, 1);
                        }
                    }
                    else if (posIX == 180)
                    {
                        if (posIZ >= 75)
                            Gl.glRotated(0, 0, 0, 1);
                        else
                        {
                            posIZ += rot;
                            Gl.glRotated(-rot, 0, 0, 1);
                        }
                    }

                }
                #endregion

                #region X축회전

                //X축회전
                if ((posIY == 90 || posIY == 270) && (posIZ == 90 || posIZ == 270))
                {
                    posIX += rot;
                    Gl.glRotated(+rot, 0, 1, 0);
                }
                #endregion

                #region Z축회전

                //Y축회전
                if (posIX == 270)
                {
                    if (posIY >= 75)
                        Gl.glRotated(0, 1, 0, 0);
                    else
                    {
                        posIY += rot;
                        Gl.glRotated(+rot, 1, 0, 0);
                    }
                }
                else if (posIX == 90)
                {
                    if (posIY >= 75)
                        Gl.glRotated(0, 1, 0, 0);
                    else
                    {
                        posIY += rot;
                        Gl.glRotated(-rot, 1, 0, 0);

                    }
                }

                #endregion

            }

            else
            {
                AngleChaneX();
                AngleChaneY();
                AngleChaneZ();

                #region Z축
                //Z축회전
                if ((posIX == 0 || posIX == 180) && (posIY == 0 || posIY == 180))
                {
                    if (posIX == 0)
                    {
                        if (posIZ <= -30)   //0
                            Gl.glRotated(0, 0, 0, 1);
                        else
                        {
                            posIZ -= rot;
                            Gl.glRotated(-rot, 0, 0, 1);
                        }
                    }
                    else if (posIX == 180)
                    {
                        if (posIZ <= -30)   //0
                            Gl.glRotated(0, 0, 0, 1);
                        else
                        {
                            posIZ -= rot;
                            Gl.glRotated(+rot, 0, 0, 1);
                        }
                    }
                }
                #endregion

                #region X축회전

                //X축회전
                if ((posIY == 90 || posIY == 270) && (posIZ == 90 || posIZ == 270))
                {
                    posIX -= rot;
                    Gl.glRotated(-rot, 0, 1, 0);
                }
                #endregion

                #region Y축회전

                if (posIX == 270)
                {
                    if (posIY <= -30)     //-30
                        Gl.glRotated(0, 1, 0, 0);
                    else
                    {
                        posIY -= rot;
                        Gl.glRotated(-rot, 1, 0, 0);
                    }
                }

                else if (posIX == 90)
                {
                    if (posIY <= -30)     //-30
                        Gl.glRotated(0, 1, 0, 0);
                    else
                    {
                        posIY -= rot;
                        Gl.glRotated(+rot, 1, 0, 0);
                    }
                }
                #endregion

            }

            Gl.glPopMatrix();
        }
        #endregion
    }
}
