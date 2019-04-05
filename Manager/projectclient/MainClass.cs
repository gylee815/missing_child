using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShadowEngine;
using Tao.OpenGl;
using Tao.FreeGlut;
using Tao.Platform;
using System.Windows.Forms;

namespace projectclient
{
    public class MainClass
    {
        public Building building;
        public Camara camara;

        public MainClass(ListView list)
        {
            building = new Building();
            camara = new Camara(list);
        }

        public void CrearObjetos(string Floor)
        {
            building.Create(Floor);
            camara.Floor_num = Floor;
            Sprite.Create();
        }

        public Camara Camara
        {
            get { return camara; }
        }

        public void DibujarEscena()
        {
            //DebugMode.WriteCamaraPos(200, 200);
            Collision.DrawColissions();
        }
    }
}
