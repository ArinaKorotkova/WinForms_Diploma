using Kompas6API5;
using Kompas6Constants3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseWork
{
    internal class Stakan1 : BasePart
    {
        //Деталь 16 - Стакан 1
        private readonly double diameter;

        public Stakan1(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string partName = null)
        {
            //if (File.Exists(Path.Combine(folderPath, "Стакан1_022.m3d")))
            //{
            //    return Path.Combine(folderPath, "Стакан1_022.m3d");
            //}

            CreateNew("Стакан1_022");
            var radius = diameter / 2;

            //Эскиз 1 - основание 
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, 0, 0, 10, 3); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(radius * 0.45, 0, radius * 0.49, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.49, 0, radius * 0.49, 80, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.49, 80, radius * 0.604, 80, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.604, 80, radius * 0.604, 110, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.604, 110, radius * 0.49, 110, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.49, 110, radius * 0.49, 190, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.49, 190, radius * 0.45, 190, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.45, 190, radius * 0.45, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            ksScetchDef1.EndEdit();

            ksEntity RotatedBase1 = part.NewEntity((int)Obj3dType.o3d_bossRotated);
            // получаем интерфейс операции вращения
            ksBossRotatedDefinition RotateDef1 = RotatedBase1.GetDefinition();
            RotateDef1.directionType = (short)Direction_Type.dtNormal;
            // настройки вращения (направление вращения, угол вращения) true - прямое, false - обратное
            RotateDef1.SetSideParam(true, 360);
            // устанавливаем эскиз вращения
            RotateDef1.SetSketch(ksScetch1Entity);
            RotatedBase1.Create(); // создаём операцию


            ksEntityCollection ksEntityCollection161 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection161.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection161.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == RotatedBase1)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == radius * 0.45)
                        {
                            part1.name = "CylinderCenterOtv_Stakan1";
                            part1.Update();
                        }
                    }
                }
            }

            ksEntityCollection ksEntityCollection162 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection162.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection162.GetByIndex(q);
                ksFaceDefinition def1 = part1.GetDefinition();

                if (def1.GetOwnerEntity() == RotatedBase1)
                {
                    if (def1.IsPlanar())
                    {
                        ksEdgeCollection col1 = def1.EdgeCollection();
                        ksEdgeCollection col2 = def1.EdgeCollection();

                        for (int k = 0; k < col1.GetCount(); k++)
                        {
                            for (int k1 = 0; k1 < col2.GetCount(); k1++)
                            {

                                ksEdgeDefinition d1 = col1.GetByIndex(k);
                                ksEdgeDefinition d2 = col2.GetByIndex(k1);

                                if (d1.IsCircle() && d2.IsCircle())
                                {


                                    ksVertexDefinition p1 = d1.GetVertex(true);
                                    ksVertexDefinition p2 = d2.GetVertex(true);

                                    double x1, y1, z1;
                                    double x2, y2, z2;

                                    p1.GetPoint(out x1, out y1, out z1);
                                    p2.GetPoint(out x2, out y2, out z2);

                                    if (Math.Abs(x2 - radius * 0.604) <= 0.1 && Math.Abs(y2) <= 0.1 && Math.Abs(z2 + 80) <= 0.1)
                                    {
                                            part1.name = ("Plane1_Stakan1");
                                            part1.Update();
                                            break;
                                    }
                                }

                            }
                        }
                    }
                }
            }

            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Стакан1_022.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
