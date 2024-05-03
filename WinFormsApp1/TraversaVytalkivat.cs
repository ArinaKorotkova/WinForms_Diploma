using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurseWork
{
    internal class TraversaVytalkivat : BasePart
    {
        //Деталь 10 - Траверса выталкивателя
        private readonly double diameter;

        public TraversaVytalkivat(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string partName = null)
        {

            //if (File.Exists(Path.Combine(folderPath, "Траверса выталкивателя.m3d")))
            //{
            //    return Path.Combine(folderPath, "Траверса выталкивателя.m3d");
            //}
            CreateNew("Траверса выталкивателя");
            var radius = diameter / 2;

            //Эскиз 1 - первая половина поперечины
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза


            Scetch12D.ksLineSeg(0, 0, radius * 1.132 / 2, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 1.132 / 2, 0, radius * 3.775, 250, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 3.775, 250, radius * 4.45, 250, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(radius * 4.45, 250, radius * 4.45, 450, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 4.45, 450, radius * 1.132 / 2, 450, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 1.132 / 2, 450, radius * 1.132 / 2, 480, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 1.132 / 2, 480, 0, 480, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(0, 480, 0, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef1.EndEdit(); // заканчиваем редактирование эскиза

            //Выдавливание
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
                extrProp1.depthNormal = 40; // глубина выдавливания
                bossExtr1.Create(); // создадим операцию
            }


            //Зеркальный массив относительно плоскости ZOY
            ksEntity MirrorCopyPart1 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart1Def = MirrorCopyPart1.GetDefinition();

            if (MirrorCopyPart1Def != null)
            {
                ksEntityCollection EntityCollection1 = MirrorCopyPart1Def.GetOperationArray();
                EntityCollection1.Clear();
                EntityCollection1.Add(bossExtr1);

                MirrorCopyPart1Def.SetPlane(basePlaneZOY);
                MirrorCopyPart1.Create();
            }


            ksEntityCollection ksEntityCollection3 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection3.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection3.GetByIndex(q);
                ksFaceDefinition def1 = part1.GetDefinition();

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

                            if (Math.Abs(x1 - radius * 4.45) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 + 450) <= 0.1)
                            {
                                if (Math.Abs(x2 + radius * 1.132 / 2) <= 0.1 && Math.Abs(y2) <= 0.1 && Math.Abs(z2) <= 0.1)
                                {
                                    part1.name = ("Plane2_Bok_traversaVytalk");
                                    part1.Update();
                                    break;
                                }

                            }

                        }
                    }
                }
            }

            //Эскиз 2 - центральная баклуша траверсы
            ksEntity ksRect1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. 
            SketchDefinition ksRect1ScetchDef = ksRect1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. 
            ksRect1ScetchDef.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksRect1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect1Sketch = (ksDocument2D)ksRect1ScetchDef.BeginEdit(); // начинаем редактирование эскиза. 
            ksRectangleParam Rect1Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            Rect1Param.x = -radius * 1.132 / 2; // координата х одной из вершин прямоугольника
            Rect1Param.y = 0; // координата y одной из вершин прямоугольника
            Rect1Param.height = 480; // высота
            Rect1Param.width = radius * 1.132; // ширина
            Rect1Param.ang = 0; // угол поворота
            Rect1Param.style = 1; // стиль линии
                                  // отправляем интерфейс параметров в функцию создания

            Rect1Sketch.ksRectangle(Rect1Param);
            ksRect1ScetchDef.EndEdit(); // заканчиваем редактирование эскиза


            ksEntity bossExtr2 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef2 = bossExtr2.GetDefinition();
            ksExtrusionParam extrProp2 = (ksExtrusionParam)ExtrDef2.ExtrusionParam();

            if (extrProp2 != null)
            {
                ExtrDef2.SetSketch(ksRect1ScetchEntity); // эскиз операции выдавливания
                                                         // направление выдавливания (обычное)
                extrProp2.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                extrProp2.typeNormal = (short)End_Type.etBlind;
                extrProp2.depthNormal = radius * 0.717; // глубина выдавливания
                bossExtr2.Create(); // создадим операцию
            }

            ksEntityCollection ksEntityCollection2 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection2.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection2.GetByIndex(q);
                ksFaceDefinition def1 = part1.GetDefinition();

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

                                    if (Math.Abs(x1 + radius * 1.132 / 2) <= 0.1 && Math.Abs(y1 - radius * 0.717) <= 0.1 && Math.Abs(z1) <= 0.1)
                                    {
                                        if (Math.Abs(x2 - radius * 1.132 / 2) <= 0.1 && Math.Abs(y2) <= 0.1 && Math.Abs(z2) <= 0.1)
                                        {
                                            part1.name = ("Plane1_traversaVytalk");
                                            part1.Update();
                                            break;
                                        }

                                    }

                            }
                        }
                    }
            }

            //смещаем плоскость XOY на 450 по оси Z
            ksEntity basePlane1Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef1 = basePlane1Offset.GetDefinition();
            offsetPlaneDef1.direction = false;
            offsetPlaneDef1.offset = 480;
            offsetPlaneDef1.SetPlane(basePlaneXOY);
            basePlane1Offset.Create();



            //Эскиз 3 - отверстие на центральной баклуши под шток
            ksEntity ksCircle1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle1ScetchDef = ksCircle1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle1ScetchDef.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle1Sketch = (ksDocument2D)ksCircle1ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle1Sketch.ksCircle(0, radius * 0.717, radius * 1.132 / 4, 1);


            ksCircle1ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle1 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefCircle1 = CutCircle1.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropCircle1 = (ksExtrusionParam)CutDefCircle1.ExtrusionParam();

            if (CutPropCircle1 != null)
            {
                // эскиз для вырезания
                CutDefCircle1.SetSketch(ksCircle1ScetchDef);
                // направление вырезания (обратное)
                CutPropCircle1.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropCircle1.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle1.depthReverse = 305;
                // создадим операцию

                CutCircle1.Create();
            }



            //смещаем плоскость XOZ на 450 по оси Z
            ksEntity basePlane2Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef2 = basePlane2Offset.GetDefinition();
            offsetPlaneDef2.direction = true;
            offsetPlaneDef2.offset = radius * 0.717;
            offsetPlaneDef2.SetPlane(basePlaneXOZ);
            basePlane2Offset.Create();



            //Эскиз 4 - вырезание вращением 
            ksEntity ksScetch2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef2 = ksScetch2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef2.SetPlane(basePlane2Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch2Entity.Create(); // создадим эскиз
            ksDocument2D Scetch22D = (ksDocument2D)ksScetchDef2.BeginEdit(); // начинаем редактирование эскиза


            Scetch22D.ksLineSeg(0, 100, 0, 175, 3); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(0, 175, radius * 1.132 / 4, 175, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksArcBy3Points(radius * 1.132 / 4, 175, radius * 0.2, 122, 0, 100, 1);

            ksScetchDef2.EndEdit(); // заканчиваем редактирование эскиза


            ksEntity Rotate1Extr = part.NewEntity((short)Obj3dType.o3d_cutRotated); //создание интерфейса объекта вращения
            ksCutRotatedDefinition Rotated1Def = Rotate1Extr.GetDefinition();// получаем интерфейс операции вращения
            Rotated1Def.directionType = (short)Direction_Type.dtNormal; //устанавливаем направление вращения - прямо
            Rotated1Def.SetSideParam(false, 360);// настройки вращения (направление вращения, угол вращения) 
            Rotated1Def.SetSketch(ksScetch2Entity);// устанавливаем эскиз вращения
            Rotate1Extr.Create();// создаём операцию


            //Эскиз 5 - боковая баклуша
            ksEntity ksRect2ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. 
            SketchDefinition ksRect2ScetchDef = ksRect2ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. 
            ksRect2ScetchDef.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksRect2ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect2Sketch = (ksDocument2D)ksRect2ScetchDef.BeginEdit(); // начинаем редактирование эскиза. 
            ksRectangleParam Rect2Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            Rect2Param.x = -radius * 4.45; // координата х одной из вершин прямоугольника
            Rect2Param.y = 250; // координата y одной из вершин прямоугольника
            Rect2Param.height = 200; // высота
            Rect2Param.width = radius * 0.68; // ширина
            Rect2Param.ang = 0; // угол поворота
            Rect2Param.style = 1; // стиль линии
                                  // отправляем интерфейс параметров в функцию создания

            Rect2Sketch.ksRectangle(Rect2Param);
            ksRect2ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            //Выдавливание базовой части корпуса
            ksEntity bossExtr3 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef3 = bossExtr3.GetDefinition();
            ksExtrusionParam extrProp3 = (ksExtrusionParam)ExtrDef3.ExtrusionParam();

            if (extrProp3 != null)
            {
                ExtrDef3.SetSketch(ksRect2ScetchEntity); // эскиз операции выдавливания
                                                         // направление выдавливания (обычное)
                extrProp3.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                extrProp3.typeNormal = (short)End_Type.etBlind;
                extrProp3.depthNormal = radius * 0.717; // глубина выдавливания
                bossExtr3.Create(); // создадим операцию
            }


            //смещаем плоскость XOY на 450 по оси Z
            ksEntity basePlane3Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef3 = basePlane3Offset.GetDefinition();
            offsetPlaneDef3.direction = false;
            offsetPlaneDef3.offset = 450;
            offsetPlaneDef3.SetPlane(basePlaneXOY);
            basePlane3Offset.Create();


            //Эскиз 6 - отверстие под направляющую и задвижку
            ksEntity ksScetch3Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef3 = ksScetch3Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef3.SetPlane(basePlane3Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch3Entity.Create(); // создадим эскиз
            ksDocument2D Scetch32D = (ksDocument2D)ksScetchDef3.BeginEdit(); // начинаем редактирование эскиза


            Scetch32D.ksLineSeg(-radius * 4.45, radius * 0.415, -radius * 4.377, radius * 0.415, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch32D.ksArcBy3Points(-radius * 4.377, radius * 0.415, -radius * 4.325, radius * 0.438, -radius * 4.302, radius * 0.49, 1);
            Scetch32D.ksLineSeg(-radius * 4.302, radius * 0.49, -radius * 4.45, radius * 0.581, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch32D.ksLineSeg(-radius * 4.45, radius * 0.581, -radius * 4.45, radius * 0.415, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef3.EndEdit(); // заканчиваем редактирование эскиза


            ksEntity CutScetch3 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch3 = CutScetch3.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch3 = (ksExtrusionParam)CutDefScetch3.ExtrusionParam();

            if (CutPropScetch3 != null)
            {
                // эскиз для вырезания
                CutDefScetch3.SetSketch(ksScetchDef3);
                // направление вырезания (обратное)
                CutPropScetch3.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch3.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch3.depthReverse = 40;
                // создадим операцию

                CutScetch3.Create();
            }



            //Эскиз 7 - отверстие под направляющую и задвижку 2
            ksEntity ksScetch4Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef4 = ksScetch4Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef4.SetPlane(basePlane3Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch4Entity.Create(); // создадим эскиз
            ksDocument2D Scetch42D = (ksDocument2D)ksScetchDef4.BeginEdit(); // начинаем редактирование эскиза

            Scetch42D.ksLineSeg(-radius * 4.302, radius * 0.49, -radius * 4.45, radius * 0.581, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch42D.ksLineSeg(-radius * 4.45, radius * 0.581, -radius * 4.45, radius * 0.717, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch42D.ksLineSeg(-radius * 4.45, radius * 0.717, -radius * 4.03, radius * 0.717, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch42D.ksArcBy3Points(-radius * 4.03, radius * 0.717, -radius * 4.068, radius * 0.619, -radius * 4.166, radius * 0.581, 1);

            Scetch42D.ksLineSeg(-radius * 4.166, radius * 0.581, -radius * 4.302, radius * 0.581, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch42D.ksLineSeg(-radius * 4.302, radius * 0.581, -radius * 4.302, radius * 0.49, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef4.EndEdit(); // заканчиваем редактирование эскиза


            ksEntity CutScetch4 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch4 = CutScetch4.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch4 = (ksExtrusionParam)CutDefScetch4.ExtrusionParam();

            if (CutPropScetch4 != null)
            {
                // эскиз для вырезания
                CutDefScetch4.SetSketch(ksScetchDef4);
                // направление вырезания (обратное)
                CutPropScetch4.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch4.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch4.depthReverse = 200;
                // создадим операцию

                CutScetch4.Create();
            }

            //Зеркальный массив относительно плоскости ZOY зеркалим отверстия под направляющую
            ksEntity MirrorCopyPart2 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart2Def = MirrorCopyPart2.GetDefinition();

            if (MirrorCopyPart2Def != null)
            {
                ksEntityCollection EntityCollection2 = MirrorCopyPart2Def.GetOperationArray();
                EntityCollection2.Clear();
                EntityCollection2.Add(bossExtr3);
                EntityCollection2.Add(CutScetch3);
                EntityCollection2.Add(CutScetch4);

                MirrorCopyPart2Def.SetPlane(basePlaneZOY);
                MirrorCopyPart2.Create();
            }



            //Зеркальный массив относительно плоскости ZOY зеркалим всю половину детали
            ksEntity MirrorCopyPart3 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart3Def = MirrorCopyPart3.GetDefinition();

            if (MirrorCopyPart3Def != null)
            {
                ksEntityCollection EntityCollection3 = MirrorCopyPart3Def.GetOperationArray();
                EntityCollection3.Clear();
                EntityCollection3.Add(bossExtr1);
                EntityCollection3.Add(MirrorCopyPart1);
                EntityCollection3.Add(bossExtr2);
                EntityCollection3.Add(CutCircle1);
                EntityCollection3.Add(Rotate1Extr);
                EntityCollection3.Add(bossExtr3);
                EntityCollection3.Add(CutScetch3);
                EntityCollection3.Add(CutScetch4);
                EntityCollection3.Add(MirrorCopyPart2);

                MirrorCopyPart3Def.SetPlane(basePlane2Offset);
                MirrorCopyPart3.Create();
            }

            ksEntityCollection ksEntityCollection1 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection1.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection1.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == radius * 1.132 / 4)
                        {
                            part1.name = "CylinderCentre_traversaVytalk";
                            part1.Update();
                        }
                    }
            }

            //Эскиз 8 - отверстие под крепление штока, диаметром М30 ???? (уточнить ибо подобрать болт и от него поставить диаметр)
            ksEntity ksCircle2ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle2ScetchDef = ksCircle2ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle2ScetchDef.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksCircle2ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle2Sketch = (ksDocument2D)ksCircle2ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle2Sketch.ksCircle(0, 135, 15, 1);

            ksCircle2ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle2 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefCircle2 = CutCircle2.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropCircle2 = (ksExtrusionParam)CutDefCircle2.ExtrusionParam();

            if (CutPropCircle2 != null)
            {
                // эскиз для вырезания
                CutDefCircle2.SetSketch(ksCircle2ScetchDef);
                // направление вырезания (обратное)
                CutPropCircle2.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropCircle2.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle2.depthReverse = radius * 1.434;
                // создадим операцию

                CutCircle2.Create();
            }

            ksEntityCollection ksEntityCollection5 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection5.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection5.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == CutCircle2)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == 15 && h1 == radius * 1.434)
                        {
                            part1.name = "Cylinder2_PodShpilky";
                            part1.Update();
                        }
                    }
                }

            }


            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Траверса выталкивателя.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
