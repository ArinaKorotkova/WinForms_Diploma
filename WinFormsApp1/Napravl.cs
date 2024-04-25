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
    internal class Napravl : BasePart
    {
        //Деталь 11 - Направляющая на раме пресса
        private readonly double diameter;

        public Napravl(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string name)
        {

            //if (File.Exists(Path.Combine(folderPath, $"{name}.m3d")))
            //{
            //    return Path.Combine(folderPath, $"{name}.m3d");
            //}
            CreateNew("Направляющая");
            var radius = diameter / 2;

            //Эскиз 1 - основание направляющей
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, 0, 0, radius * 0.189, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(0, radius * 0.189, radius * 0.619, radius * 0.189, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.619, radius * 0.189, radius * 0.717, radius * 0.09, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(radius * 0.717, radius * 0.09, radius * 0.717, -radius * 0.679, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.717, -radius * 0.679, radius * 1.034, -radius * 0.679, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 1.034, -radius * 0.679, radius * 1.079, -radius * 0.725, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 1.079, -radius * 0.725, radius * 0.608, -radius * 1.196, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.608, -radius * 1.196, radius * 0.528, -radius * 1.117, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.528, -radius * 1.117, radius * 0.528, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.528, 0, 0, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef1.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity bossExtr1 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef1 = bossExtr1.GetDefinition();
            ksExtrusionParam extrProp1 = (ksExtrusionParam)ExtrDef1.ExtrusionParam();

            if (extrProp1 != null)
            {
                ExtrDef1.SetSketch(ksScetch1Entity); // эскиз операции выдавливания
                                                     // направление выдавливания (обычное)
                extrProp1.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                extrProp1.typeNormal = (short)End_Type.etBlind;
                extrProp1.depthNormal = 2400; // глубина выдавливания
                bossExtr1.Create(); // создадим операцию
            }

            ksEntityCollection ksEntityCollection1 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection1.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection1.GetByIndex(q);
                ksFaceDefinition def1 = part1.GetDefinition();

                if (def1.GetOwnerEntity() == bossExtr1)
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


                                    ksVertexDefinition p1 = d1.GetVertex(true);
                                    ksVertexDefinition p2 = d2.GetVertex(true);

                                    double x1, y1, z1;
                                    double x2, y2, z2;

                                    p1.GetPoint(out x1, out y1, out z1);
                                    p2.GetPoint(out x2, out y2, out z2);

                                    if (Math.Abs(x1 - radius * 0.528) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 - radius * 1.117) <= 0.1)
                                    {
                                        if (Math.Abs(x2 - radius * 0.528) <= 0.1 && Math.Abs(y2 - 2400) <= 0.1 && Math.Abs(z2) <= 0.1)
                                        {
                                            part1.name = ("Plane1_Zad_Napr");
                                            part1.Update();
                                            break;
                                        }
                                    }

                            }
                        }
                    }
                }
            }

            ksEntityCollection ksEntityCollection2 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection2.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection2.GetByIndex(q);
                ksFaceDefinition def1 = part1.GetDefinition();

                if (def1.GetOwnerEntity() == bossExtr1)
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

                                    ksVertexDefinition p1 = d1.GetVertex(true);
                                    ksVertexDefinition p2 = d2.GetVertex(true);

                                    double x1, y1, z1;
                                    double x2, y2, z2;

                                    p1.GetPoint(out x1, out y1, out z1);
                                    p2.GetPoint(out x2, out y2, out z2);

                                    if (Math.Abs(x1 - radius * 0.528) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1) <= 0.1)
                                    {
                                        if (Math.Abs(x2) <= 0.1 && Math.Abs(y2 - 2400) <= 0.1 && Math.Abs(z2) <= 0.1)
                                        {
                                            part1.name = ("Plane2_Bok_Napr");
                                            part1.Update();
                                            break;
                                        }
                                    }
                            }
                        }
                    }
                }
            }
            ksEntityCollection ksEntityCollection3 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection3.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection3.GetByIndex(q);
                ksFaceDefinition def1 = part1.GetDefinition();

                if (def1.GetOwnerEntity() == bossExtr1)
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

                                    ksVertexDefinition p1 = d1.GetVertex(true);
                                    ksVertexDefinition p2 = d2.GetVertex(true);

                                    double x1, y1, z1;
                                    double x2, y2, z2;

                                    p1.GetPoint(out x1, out y1, out z1);
                                    p2.GetPoint(out x2, out y2, out z2);

                                    if (Math.Abs(x1 - radius * 0.608) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 - radius * 1.196) <= 0.1)
                                    {
                                        if (Math.Abs(x2 - radius * 0.619) <= 0.1 && Math.Abs(y2) <= 0.1 && Math.Abs(z2 + radius * 0.189) <= 0.1)
                                        {
                                            part1.name = ("Plane3_Verh_Napr");
                                            part1.Update();
                                            break;
                                        }
                                    }

                            }
                        }
                    }
                }
            }
            //смещаем плоскость ZOY на 450 по оси Z
            ksEntity basePlane1Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef1 = basePlane1Offset.GetDefinition();
            offsetPlaneDef1.direction = false;
            offsetPlaneDef1.offset = radius * 0.528;
            offsetPlaneDef1.SetPlane(basePlaneZOY);
            basePlane1Offset.Create();

            //Эскиз 2 - пазы под крепление к раме пресса
            ksEntity ksScetch2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef2 = ksScetch2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef2.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch2Entity.Create(); // создадим эскиз
            ksDocument2D Scetch22D = (ksDocument2D)ksScetchDef2.BeginEdit(); // начинаем редактирование эскиза

            Scetch22D.ksLineSeg(-radius * 0.43, -2222, -radius * 0.55, -2222, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksArcBy3Points(-radius * 0.55, -2222, -radius * 0.604, -2236, -radius * 0.55, -2250, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(-radius * 0.55, -2250, -radius * 0.43, -2250, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksArcBy3Points(-radius * 0.43, -2250, -radius * 0.377, -2236, -radius * 0.43, -2222, 1);

            ksScetchDef2.EndEdit(); // заканчиваем редактирование эскиза


            ksEntity CutScetch1 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            ksCutExtrusionDefinition CutDefScetch1 = CutScetch1.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch1 = (ksExtrusionParam)CutDefScetch1.ExtrusionParam();

            if (CutPropScetch1 != null)
            {
                // эскиз для вырезания
                CutDefScetch1.SetSketch(ksScetch2Entity);
                // направление вырезания (обратное)
                CutPropScetch1.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropScetch1.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch1.depthNormal = radius * 0.189;
                // создадим операцию

                CutScetch1.Create();
            }

            ksEntity MeshCopy1 = part.NewEntity((short)Obj3dType.o3d_meshCopy);
            //создаём интерфейс свойств линейного массива
            MeshCopyDefinition MeshCopyDef1 = MeshCopy1.GetDefinition();
            //создаём ось линейного массива на основе базовой Y
            ksEntity baseAxisY1 = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOY);
            //выставляем базовую ось
            MeshCopyDef1.SetAxis1(baseAxisY1);
            // количество элементов копирования вдоль первой оси
            MeshCopyDef1.count1 = 5;
            // шаг копирования вдоль первой оси
            MeshCopyDef1.step1 = -500;
            //создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection1 = MeshCopyDef1.OperationArray();
            EntityCollection1.Clear(); // очищаем её
                                       //добавляем элемент выдавливания в коллекци.
            EntityCollection1.Add(CutScetch1);
            //добавляем элемент выдавливания в коллекци.
            MeshCopy1.Create(); // создаём массив


            ksEntity PlaneAngle1 = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeAngle);
            // получаем интерфейс параметров плоскости под углом
            ksPlaneAngleDefinition PlaneAngleDef1 = PlaneAngle1.GetDefinition();
            ksEntity Axis1 = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOY);
            // настраиваем параметры
            // устанавливаем прямую или ось, через которую проходит плоскость
            PlaneAngleDef1.SetAxis(Axis1);
            // устанавливаем базовую плоскость
            PlaneAngleDef1.SetPlane(basePlaneZOY);
            // устанавливаем угол наклона плоскости
            PlaneAngleDef1.angle = -45;
            // создаём плоскость
            PlaneAngle1.Create();

            ksEntity basePlane2Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef2 = basePlane2Offset.GetDefinition();
            offsetPlaneDef2.direction = false;
            offsetPlaneDef2.offset = radius * 1.275;
            offsetPlaneDef2.SetPlane(PlaneAngle1);
            basePlane2Offset.Create();


            //Эскиз 3 - отверстия под крепление бронзовых накладок
            ksEntity ksScetch3Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef3 = ksScetch3Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef3.SetPlane(basePlane2Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch3Entity.Create(); // создадим эскиз
            ksDocument2D Scetch32D = (ksDocument2D)ksScetchDef3.BeginEdit(); // начинаем редактирование эскиза

            Scetch32D.ksCircle(radius * 0.249, 2340, 8, 1);
            Scetch32D.ksCircle(-radius * 0.083, 2260, 8, 1);

            ksScetchDef3.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutScetch2 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            ksCutExtrusionDefinition CutDefScetch2 = CutScetch2.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch2 = (ksExtrusionParam)CutDefScetch2.ExtrusionParam();

            if (CutPropScetch2 != null)
            {
                // эскиз для вырезания
                CutDefScetch2.SetSketch(ksScetch3Entity);
                // направление вырезания (обратное)
                CutPropScetch2.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch2.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch2.depthReverse = 30;
                // создадим операцию

                CutScetch2.Create();
            }

            ksEntity MeshCopy2 = part.NewEntity((short)Obj3dType.o3d_meshCopy);
            //создаём интерфейс свойств линейного массива
            MeshCopyDefinition MeshCopyDef2 = MeshCopy2.GetDefinition();
            //создаём ось линейного массива на основе базовой Y
            ksEntity baseAxisY2 = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOY);
            //выставляем базовую ось
            MeshCopyDef2.SetAxis1(baseAxisY2);
            // количество элементов копирования вдоль первой оси
            MeshCopyDef2.count1 = 15;
            // шаг копирования вдоль первой оси
            MeshCopyDef2.step1 = -157;
            //создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection2 = MeshCopyDef2.OperationArray();
            EntityCollection2.Clear(); // очищаем её
                                       //добавляем элемент выдавливания в коллекци.
            EntityCollection2.Add(CutScetch2);
            //добавляем элемент выдавливания в коллекци.
            MeshCopy2.Create(); // создаём массив


            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси
            //ksDoc3d.SaveAs("C:\\Users\\arina\\Documents\\Институт\\ИнжПроект_Диплом\\Гидравлический пресс\\Сборка1\\Направляющая.m3d");

            string path = Path.Combine(folderPath, $"{name}.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
