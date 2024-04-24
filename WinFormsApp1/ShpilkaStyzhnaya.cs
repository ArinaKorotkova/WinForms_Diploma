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
    internal class ShpilkaStyzhnaya : BasePart  
    {
        //Деталь 27 - Шпилька стяжная
        private readonly double diameter;

        public ShpilkaStyzhnaya(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string partName = null)
        {
            //if (File.Exists(Path.Combine(folderPath, "Шпилька Стяжная.m3d")))
            //{
            //    return Path.Combine(folderPath, "Шпилька Стяжная.m3d");
            //}
            CreateNew("Шпилька Стяжная");
            var radius = diameter / 2;

            //Эскиз 1 - основание 
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, -radius * 0.113, 0, -radius * 0.264, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(0, -radius * 0.264, 15, -radius * 0.3208, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(15, -radius * 0.3208, 25, -radius * 0.3208, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(25, -radius * 0.3208, 40, -radius * 0.377, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(40, -radius * 0.377, 355, -radius * 0.377, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(355, -radius * 0.377, 355, -radius * 0.3245, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(355, -radius * 0.3245, 455, -radius * 0.3245, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(455, -radius * 0.3245, 455, -radius * 0.377, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(455, -radius * 0.377, 4915, -radius * 0.377, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(4915, -radius * 0.377, 4915, -radius * 0.3245, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(4915, -radius * 0.3245, 5015, -radius * 0.3245, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(5015, -radius * 0.3245, 5015, -radius * 0.377, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(5015, -radius * 0.377, 5330, -radius * 0.377, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(5330, -radius * 0.377, 5345, -radius * 0.3208, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(5345, -radius * 0.3208, 5355, -radius * 0.3208, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(5355, -radius * 0.3208, 5370, -radius * 0.264, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(5370, -radius * 0.264, 5370, -radius * 0.113, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(5370, -radius * 0.113, 5334, -radius * 0.113, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(5334, -radius * 0.113, 5334, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(5334, 0, 36, 0, 3); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(36, 0, 36, -radius * 0.113, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(36, -radius * 0.113, 0, -radius * 0.113, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)


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

            ksEntityCollection ksEntityCollection1 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection1.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection1.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == RotatedBase1)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == radius * 0.377 && h1 == 4460)
                        {
                            part1.name = "CylinderShpilkaStyzh";
                            part1.Update();
                        }
                    }
                }
            }

            ksEntityCollection ksEntityCollection2 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection2.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection2.GetByIndex(q);
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

                                    if (Math.Abs(x1 - 4915) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 - radius * 0.377) <= 0.1)
                                    {
                                            part1.name = ("Plane1_ShpilkaStyzhnaya1");
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


            string path = Path.Combine(folderPath, "Шпилька стяжная.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
