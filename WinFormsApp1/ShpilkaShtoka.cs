﻿using Kompas6API5;
using Kompas6Constants3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseWork
{
    internal class ShpilkaShtoka : BasePart
    {
        //Деталь 22 - Шпилька под шток
        private readonly double diameter;

        public ShpilkaShtoka(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string partName = null)
        {
            //if (File.Exists(Path.Combine(folderPath, "Шпилька под шток.m3d")))
            //{
            //    return Path.Combine(folderPath, "Шпилька под шток.m3d");
            //}
            CreateNew("Шпилька под шток");
            var radius = diameter / 2;

            //Эскиз 1 - основание 
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, 0, radius * 1.66, 0, 3); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 1.66, 0, radius * 1.66, 15, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 1.66, 15, 0, 15, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(0, 15, 0, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

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

            ksEntityCollection ksEntityCollection1 = 
             (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
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

                        if (r == 15)
                        {
                            part1.name = "Cylinder_ShpilkaSht";
                            part1.Update();
                        }
                    }
                }
            }


            ksEntityCollection ksEntityCollection2 = 
                (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
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
                            if (Math.Abs(x1) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 + 15) <= 0.1)
                            {
                                part.name = ("Plane1_Dno_ShpilkaSht");
                                part.Update();
                                break;
                            }
                        }
                    }
                }
            }

            //Условное обозначение резьбы
            ksEntity Thread = part.NewEntity((short)Obj3dType.o3d_thread);
            // получаем интерфейс настроек резьбы
            ksThreadDefinition ThreadDef = Thread.GetDefinition();
            ThreadDef.allLength = false; // признак полной длины
            ThreadDef.autoDefinDr = false;// признак автоопределения диаметра
            ThreadDef.dr = 30; // номинальный диаметр резьбы
            ThreadDef.length = radius * 0.113; // длина резьбы
            ThreadDef.faceValue = true; // направление построения резьбы
            ThreadDef.p = 3.5; // шаг резьбы
                               // получаем коллекцию рёбер детали
            ksEntityCollection EdgeECol = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_edge);
            // оставляем в массиве только ребро, проходящее через точку (x,y,z)
            EdgeECol.SelectByPoint(0, -15, 0);
            ThreadDef.SetBaseObject(EdgeECol.First()); // устанавливаем ребро в параметры резьбы
            // создаём резьбу
            Thread.Create();


            //Условное обозначение резьбы
            ksEntity Thread1 = part.NewEntity((short)Obj3dType.o3d_thread);
            // получаем интерфейс настроек резьбы
            ksThreadDefinition ThreadDef1 = Thread1.GetDefinition();
            ThreadDef1.allLength = false; // признак полной длины
            ThreadDef1.autoDefinDr = false;// признак автоопределения диаметра
            ThreadDef1.dr = 30; // номинальный диаметр резьбы
            ThreadDef1.length = radius * 0.113; // длина резьбы
            ThreadDef1.faceValue = true; // направление построения резьбы
            ThreadDef1.p = 3.5; // шаг резьбы
                                // получаем коллекцию рёбер детали
            ksEntityCollection EdgeECol1 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_edge);
            // оставляем в массиве только ребро, проходящее через точку (x,y,z)
            EdgeECol1.SelectByPoint(radius * 1.66, -15, 0);
            ThreadDef1.SetBaseObject(EdgeECol1.First()); // устанавливаем ребро в параметры резьбы
            // создаём резьбу
            Thread1.Create();

            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Шпилька под шток.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;

        }
    }
}
