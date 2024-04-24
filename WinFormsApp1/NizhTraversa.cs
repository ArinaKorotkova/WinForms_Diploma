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
    internal class NizhTraversa : BasePart
    {
        //Деталь 5 - Нижняя траверса
        private readonly double diameter;

        public NizhTraversa(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string partName = null)
        {

            //if (File.Exists(Path.Combine(folderPath, "Нижняя траверса.m3d")))
            //{
            //    return Path.Combine(folderPath, "Нижняя траверса.m3d");
            //}

            var radius = diameter / 2;

            CreateNew("Нижняя траверса");

            //Эскиз 1 - первая четвертина базы
            ksEntity ksRect1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. 
            SketchDefinition ksRect1ScetchDef = ksRect1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. 
            ksRect1ScetchDef.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksRect1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect1Sketch = (ksDocument2D)ksRect1ScetchDef.BeginEdit(); // начинаем редактирование эскиза. 
            ksRectangleParam Rect1Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            Rect1Param.x = 0; // координата х одной из вершин прямоугольника
            Rect1Param.y = 0; // координата y одной из вершин прямоугольника
            Rect1Param.height = diameter; // высота
            Rect1Param.width = radius * 3.775; // ширина
            Rect1Param.ang = 0; // угол поворота
            Rect1Param.style = 1; // стиль линии
                                  // отправляем интерфейс параметров в функцию создания

            Rect1Sketch.ksRectangle(Rect1Param);
            ksRect1ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            //Выдавливание базовой части корпуса
            ksEntity bossExtr1 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef1 = bossExtr1.GetDefinition();
            ksExtrusionParam extrProp1 = (ksExtrusionParam)ExtrDef1.ExtrusionParam();

            if (extrProp1 != null)
            {
                ExtrDef1.SetSketch(ksRect1ScetchEntity); // эскиз операции выдавливания
                                                         // направление выдавливания (обычное)
                extrProp1.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                extrProp1.typeNormal = (short)End_Type.etBlind;
                extrProp1.depthNormal = 300; // глубина выдавливания
                bossExtr1.Create(); // создадим операцию
            }


            //Эскиз 2
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Base1Scetch2D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Base1Scetch2D.ksLineSeg(0, -300, 0, -500, 1);
            Base1Scetch2D.ksLineSeg(0, -500, radius * 1.132, -500, 1);
            Base1Scetch2D.ksLineSeg(radius * 1.132, -500, radius * 2.377, -300, 1);
            Base1Scetch2D.ksLineSeg(radius * 2.377, -300, 0, -300, 1);

            ksScetchDef1.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity bossExtr2 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef2 = bossExtr2.GetDefinition();
            ksExtrusionParam extrProp2 = (ksExtrusionParam)ExtrDef2.ExtrusionParam();

            if (extrProp2 != null)
            {
                ExtrDef2.SetSketch(ksScetch1Entity); // эскиз операции выдавливания
                                                     // направление выдавливания (обычное)
                extrProp2.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                extrProp2.typeNormal = (short)End_Type.etBlind;
                extrProp2.depthNormal = diameter; // глубина выдавливания
                bossExtr2.Create(); // создадим операцию
            }


            //Эскиз 3 - крыло p.1
            ksEntity ksRect2ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksRect2ScetchDef = ksRect2ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksRect2ScetchDef.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksRect2ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect2Sketch = (ksDocument2D)ksRect2ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы
            ksRectangleParam Rect2Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            Rect2Param.x = radius * 1.887; // координата х одной из вершин прямоугольника
            Rect2Param.y = -300; // координата y одной из вершин прямоугольника
            Rect2Param.height = 200; // высота
            Rect2Param.width = radius * 2.42; // ширина
            Rect2Param.ang = 0; // угол поворота
            Rect2Param.style = 1; // стиль линии
                                  // отправляем интерфейс параметров в функцию создания

            Rect2Sketch.ksRectangle(Rect2Param);
            ksRect2ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity bossExtr3 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef3 = bossExtr3.GetDefinition();
            ksExtrusionParam extrProp3 = (ksExtrusionParam)ExtrDef3.ExtrusionParam();

            if (extrProp3 != null)
            {
                ExtrDef3.SetSketch(ksRect2ScetchEntity); // эскиз операции выдавливания
                                                         // направление выдавливания (обычное)
                extrProp3.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                extrProp3.typeReverse = (short)End_Type.etBlind;
                extrProp3.depthReverse = radius * 1.396; // глубина выдавливания
                bossExtr3.Create(); // создадим операцию
            }
            ksEntity bossExtr4 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef4 = bossExtr4.GetDefinition();
            ksExtrusionParam extrProp4 = (ksExtrusionParam)ExtrDef4.ExtrusionParam();

            if (extrProp4 != null)
            {
                ExtrDef4.SetSketch(ksRect2ScetchEntity); // эскиз операции выдавливания
                                                         // направление выдавливания (обычное)
                extrProp4.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                extrProp4.typeNormal = (short)End_Type.etBlind;
                extrProp4.depthNormal = radius * 0.528; // глубина выдавливания
                bossExtr4.Create(); // создадим операцию
            }

            //Эскиз 4 - отверстие диаметром 204
            ksEntity ksCircle1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle1ScetchDef = ksCircle1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle1ScetchDef.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksCircle1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle1Sketch = (ksDocument2D)ksCircle1ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle1Sketch.ksCircle(radius * 3, radius * 0.68, radius * 0.38, 1);


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
                CutPropCircle1.depthReverse = 640;
                // создадим операцию

                CutCircle1.Create();
            }


            //Эскиз 5 - отверстие диаметром 240
            ksEntity ksCircle2ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle2ScetchDef = ksCircle2ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle2ScetchDef.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksCircle2ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle2Sketch = (ksDocument2D)ksCircle2ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle2Sketch.ksCircle(radius * 3, radius * 0.68, radius * 0.45, 1);

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
                CutPropCircle2.depthReverse = 160;
                // создадим операцию

                CutCircle2.Create();
            }



            //смещаем плоскость XOY на 100 по оси Z
            ksEntity basePlane2Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef2 = basePlane2Offset.GetDefinition();
            offsetPlaneDef2.direction = true;
            offsetPlaneDef2.offset = 100;
            offsetPlaneDef2.SetPlane(basePlaneXOY);
            basePlane2Offset.Create();


            //Эскиз 6 - маленькие отверстия для крепления диаметром 54
            ksEntity ksCircle3ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle3ScetchDef = ksCircle3ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle3ScetchDef.SetPlane(basePlane2Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle3ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle3Sketch = (ksDocument2D)ksCircle3ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle3Sketch.ksCircle(radius * 2.7, -radius * 0.8, radius * 0.102, 1);
            Circle3Sketch.ksCircle(radius * 3.7, -radius * 0.8, radius * 0.102, 1);

            ksCircle3ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle3 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefCircle3 = CutCircle3.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropCircle3 = (ksExtrusionParam)CutDefCircle3.ExtrusionParam();

            if (CutPropCircle3 != null)
            {
                // эскиз для вырезания
                CutDefCircle3.SetSketch(ksCircle3ScetchDef);
                // направление вырезания (обратное)
                CutPropCircle3.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropCircle3.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle3.depthReverse = 200;
                // создадим операцию

                CutCircle3.Create();
            }


            //Эскиз 6 - маленькие отверстия для крепления диаметром 64
            ksEntity ksCircle4ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle4ScetchDef = ksCircle4ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle4ScetchDef.SetPlane(basePlane2Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle4ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle4Sketch = (ksDocument2D)ksCircle4ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle4Sketch.ksCircle(radius * 2.7, -radius * 0.8, radius * 0.121, 1);
            Circle4Sketch.ksCircle(radius * 3.7, -radius * 0.8, radius * 0.121, 1);

            ksCircle4ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle4 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefCircle4 = CutCircle4.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropCircle4 = (ksExtrusionParam)CutDefCircle4.ExtrusionParam();

            if (CutPropCircle4 != null)
            {
                // эскиз для вырезания
                CutDefCircle4.SetSketch(ksCircle4ScetchDef);
                // направление вырезания (обратное)
                CutPropCircle4.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropCircle4.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle4.depthReverse = 10;
                // создадим операцию

                CutCircle4.Create();
            }

            //Зеркалирование первой половины относительно ZOY
            ksEntity MirrorCopyPart1 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart1Def = MirrorCopyPart1.GetDefinition();
            if (MirrorCopyPart1Def != null)
            {
                ksEntityCollection EntityCollection = MirrorCopyPart1Def.GetOperationArray();
                EntityCollection.Clear();
                EntityCollection.Add(bossExtr1);
                EntityCollection.Add(bossExtr2);
                EntityCollection.Add(bossExtr3);
                EntityCollection.Add(bossExtr4);

                EntityCollection.Add(CutCircle1);
                EntityCollection.Add(CutCircle2);
                EntityCollection.Add(CutCircle3);
                EntityCollection.Add(CutCircle4);

                MirrorCopyPart1Def.SetPlane(basePlaneZOY);
                MirrorCopyPart1.Create();
            }

            ksEntityCollection ksEntityCollection52 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection52.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection52.GetByIndex(q);
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


                            if (Math.Abs(x1 - radius * 3.775) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1) <= 0.1)
                            {
                                if (Math.Abs(x2 - radius * 1.132) <= 0.1 && Math.Abs(y2) <= 0.1 && Math.Abs(z2 - 500) <= 0.1)
                                {
                                    part1.name = ("PanelBok_3_NizhTr");
                                    part1.Update();
                                    break;
                                }

                            }
                        }

                    }
                }
            }


            //смещаем плоскость XOZ на 530 по оси Y
            ksEntity basePlane3Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef3 = basePlane3Offset.GetDefinition();
            offsetPlaneDef3.direction = true;
            offsetPlaneDef3.offset = radius * 2;
            offsetPlaneDef3.SetPlane(basePlaneXOZ);
            basePlane3Offset.Create();

            //Зеркалирование первой половины относительно ZOY
            ksEntity MirrorCopyPart2 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart2Def = MirrorCopyPart2.GetDefinition();
            if (MirrorCopyPart2Def != null)
            {
                ksEntityCollection EntityCollection = MirrorCopyPart2Def.GetOperationArray();
                EntityCollection.Clear();
                EntityCollection.Add(MirrorCopyPart1);


                MirrorCopyPart2Def.SetPlane(basePlane3Offset);
                MirrorCopyPart2.Create();
            }

            ksEntityCollection ksEntityCollection53 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection53.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection53.GetByIndex(q);
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


                            if (Math.Abs(x1 + radius * 3.775) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1) <= 0.1)
                            {
                                if (Math.Abs(x2 - radius * 3.775) <= 0.1 && Math.Abs(y2 - radius * 4) <= 0.1 && Math.Abs(z2) <= 0.1)
                                {
                                    part1.name = ("PanelVerh_1_NizhTr");
                                    part1.Update();
                                    break;
                                }

                            }
                        }

                    }
                }
            }

            ksEntityCollection ksEntityCollection54 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection54.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection54.GetByIndex(q);
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


                            if (Math.Abs(x1 - radius * 1.132) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 - 500) <= 0.1)
                            {
                                if (Math.Abs(x2 + radius * 1.132) <= 0.1 && Math.Abs(y2 - radius * 4) <= 0.1 && Math.Abs(z2 - 500) <= 0.1)
                                {
                                    part1.name = ("PanelNizh_2_NizhTr");
                                    part1.Update();
                                    break;
                                }

                            }
                        }

                    }
                }
            }

            //Отверстия по середине траверсы диаметрами 240 по боками и центральное 260
            ksEntity ksCircle5ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle5ScetchDef = ksCircle5ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle5ScetchDef.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksCircle5ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle5Sketch = (ksDocument2D)ksCircle5ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle5Sketch.ksCircle(-radius * 3, radius * 2, radius * 0.45, 1);
            Circle5Sketch.ksCircle(0, radius * 2, radius * 0.49, 1);
            Circle5Sketch.ksCircle(radius * 3, radius * 2, radius * 0.45, 1);

            ksCircle5ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle5 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefCircle5 = CutCircle5.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropCircle5 = (ksExtrusionParam)CutDefCircle5.ExtrusionParam();

            if (CutPropCircle5 != null)
            {
                // эскиз для вырезания
                CutDefCircle5.SetSketch(ksCircle5ScetchDef);
                // направление вырезания (обратное)
                CutPropCircle5.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropCircle5.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle5.depthReverse = 500;
                // создадим операцию

                CutCircle5.Create();
            }


            ksEntityCollection ksEntityCollection55 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection55.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection55.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == CutCircle5)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == radius * 0.49)
                        {
                            part1.name = "CylinderNizhTr1";
                            part1.Update();
                        }
                    }
                }
            }

            //Отверстия по середине траверсы диаметром 260
            ksEntity ksCircle6ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle6ScetchDef = ksCircle6ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle6ScetchDef.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksCircle6ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle6Sketch = (ksDocument2D)ksCircle6ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle6Sketch.ksCircle(0, radius * 2, radius * 0.49, 1);

            ksCircle6ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle6 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefCircle6 = CutCircle6.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropCircle6 = (ksExtrusionParam)CutDefCircle6.ExtrusionParam();

            if (CutPropCircle6 != null)
            {
                // эскиз для вырезания
                CutDefCircle6.SetSketch(ksCircle6ScetchDef);
                // направление вырезания (обратное)
                CutPropCircle6.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropCircle6.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle6.depthReverse = 40;
                // создадим операцию

                CutCircle6.Create();
            }


            //// создадим все нужные скругления на центральной части
            //ksEntity entityFillet1 = (ksEntity)part.NewEntity((int)Obj3dType.o3d_fillet); // элемент создания скругления
            //ksFilletDefinition filletDef1 = entityFillet1.GetDefinition(); //свойства скругления

            ///////////////// создание скругления ///////////
            //filletDef1.radius = 40; //радиус скругления
            //filletDef1.tangent = false; // продолжение по каательным 

            ////коллекция рёбер
            //ksEntityCollection entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            ////коллекция рёбер, которые будут скруглены
            //ksEntityCollection entityCollectionFillet1 = (ksEntityCollection)filletDef1.array();
            //entityCollectionFillet1.Clear(); //очистить коллекцию для скругления


            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(radius * 1.887, 0, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(radius * 1.887, -370, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(radius * 4.3, -370, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(radius * 4.3, 180, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(radius * 3.775, 180, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));


            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(radius * 3.775, 880, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(radius * 4.3, 880, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(radius * 4.3, 1430, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(radius * 1.887, 1430, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(radius * 1.887, 1060, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));


            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.887, 1060, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.887, 1430, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 4.3, 1430, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 4.3, 880, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 3.775, 880, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));


            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 3.775, 180, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 4.3, 180, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 4.3, -370, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.887, -370, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.887, 0, radius * 0.755);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));


            //entityFillet1.Create(); // создать скругление



            //смещаем плоскость XOY на 300 по оси Z
            ksEntity basePlane4Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef4 = basePlane4Offset.GetDefinition();
            offsetPlaneDef4.direction = true;
            offsetPlaneDef4.offset = radius * 1.132;
            offsetPlaneDef4.SetPlane(basePlaneXOY);
            basePlane4Offset.Create();


            //4 отверстия диаметром 330
            ksEntity ksCircle7ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle7ScetchDef = ksCircle7ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle7ScetchDef.SetPlane(basePlane4Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle7ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle7Sketch = (ksDocument2D)ksCircle7ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle7Sketch.ksCircle(-radius * 3, radius * 0.68, radius * 0.62, 1);
            Circle7Sketch.ksCircle(-radius * 3, radius * 3.32, radius * 0.62, 1);

            Circle7Sketch.ksCircle(radius * 3, radius * 0.68, radius * 0.62, 1);
            Circle7Sketch.ksCircle(radius * 3, radius * 3.32, radius * 0.62, 1);

            ksCircle7ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle7 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefCircle7 = CutCircle7.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropCircle7 = (ksExtrusionParam)CutDefCircle7.ExtrusionParam();

            if (CutPropCircle7 != null)
            {
                // эскиз для вырезания
                CutDefCircle7.SetSketch(ksCircle7ScetchDef);
                // направление вырезания (обратное)
                CutPropCircle7.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropCircle7.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle7.depthNormal = 30;
                // создадим операцию

                CutCircle7.Create();
            }


            // 4 маленьких отверстия диметром 30
            ksEntity ksCircle8ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle8ScetchDef = ksCircle8ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle8ScetchDef.SetPlane(basePlane4Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle8ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle8Sketch = (ksDocument2D)ksCircle8ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle8Sketch.ksCircle(-radius * 3.51, radius * 0.68, radius * 0.057, 1);
            Circle8Sketch.ksCircle(-radius * 3.51, radius * 3.32, radius * 0.057, 1);

            Circle8Sketch.ksCircle(radius * 3.51, radius * 0.68, radius * 0.057, 1);
            Circle8Sketch.ksCircle(radius * 3.51, radius * 3.32, radius * 0.057, 1);

            ksCircle8ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle8 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefCircle8 = CutCircle8.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropCircle8 = (ksExtrusionParam)CutDefCircle8.ExtrusionParam();

            if (CutPropCircle8 != null)
            {
                // эскиз для вырезания
                CutDefCircle8.SetSketch(ksCircle8ScetchDef);
                // направление вырезания (обратное)
                CutPropCircle8.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropCircle8.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle8.depthNormal = 75;
                // создадим операцию

                CutCircle8.Create();
            }


            ksEntity ksRect9ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksRect9ScetchDef = ksRect9ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksRect9ScetchDef.SetPlane(basePlane3Offset); // установим плоскость XOZ базовой для эскиза
            ksRect9ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect9Sketch = (ksDocument2D)ksRect9ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы
            ksRectangleParam Rect9Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            Rect9Param.x = -radius * 0.6; // координата х одной из вершин прямоугольника
            Rect9Param.y = -radius * 1.887; // координата y одной из вершин прямоугольника
            Rect9Param.height = 30; // высота
            Rect9Param.width = 30; // ширина
            Rect9Param.ang = 0; // угол поворота
            Rect9Param.style = 1; // стиль линии
                                  // отправляем интерфейс параметров в функцию создания

            Rect9Sketch.ksRectangle(Rect9Param);
            Rect9Sketch.ksLineSeg(0, 0, 0, -500, 3);
            ksRect9ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity RotatedBase1 = part.NewEntity((int)Obj3dType.o3d_cutRotated);
            // получаем интерфейс операции вращения
            ksCutRotatedDefinition RotateDef1 = RotatedBase1.GetDefinition();
            RotateDef1.directionType = (short)Direction_Type.dtNormal;
            // настройки вращения (направление вращения, угол вращения) true - прямое, false - обратное
            RotateDef1.SetSideParam(true, 360);
            // устанавливаем эскиз вращения
            RotateDef1.SetSketch(ksRect9ScetchDef);
            RotatedBase1.Create(); // создаём операцию



            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Нижняя траверса.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
