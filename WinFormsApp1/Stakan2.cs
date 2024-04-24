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
    internal class Stakan2 : BasePart
    {
        //Деталь 17 - Стакан 2
        private readonly double diameter;

        public Stakan2(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string partName = null)
        {
            //if (File.Exists(Path.Combine(folderPath, "Стакан2_023.m3d")))
            //{
            //    return Path.Combine(folderPath, "Стакан2_023.m3d");
            //}
            CreateNew("Стакан2_023");

            var radius = diameter / 2;

            //Эскиз 1 - основание 
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, 0, 0, 10, 3); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(radius * 0.415, 0, radius * 0.717, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.717, 0, radius * 0.717, 40, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.717, 40, radius * 0.45, 40, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.45, 40, radius * 0.45, 200, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.45, 200, radius * 0.377, 300, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.377, 300, radius * 0.339, 300, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.339, 300, radius * 0.339, 20, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.339, 20, radius * 0.415, 20, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.415, 20, radius * 0.415, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
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

                        if (r == radius * 0.339)
                        {
                            part1.name = "CylinderMain_Stakan2";
                            part1.Update();
                        }
                    }
                }
            }

            ksEntityCollection ksEntityCollection2 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection2.GetCount(); i++)
            {
                ksEntity part = ksEntityCollection2.GetByIndex(i);
                ksFaceDefinition def = part.GetDefinition();
                if (def.IsPlanar())
                {
                    ksEdgeCollection col = def.EdgeCollection();
                    for (int k = 0; k < col.GetCount(); k++)
                    {
                        ksEdgeDefinition d = col.GetByIndex(k);
                        if (d.IsCircle())
                        {
                            ksVertexDefinition p = d.GetVertex(true);
                            double x1, y1, z1;
                            p.GetPoint(out x1, out y1, out z1);
                            if (Math.Abs(x1 - radius * 0.717) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1) <= 0.1)
                            {
                                part.name = ("Plane1_Dno_Stakan2");
                                part.Update();
                                break;
                            }
                        }
                    }
                }
            }


            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Стакан2_023.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }

    }
}
