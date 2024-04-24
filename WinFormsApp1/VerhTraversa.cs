using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CurseWork
{
    internal class VerhTraversa : BasePart
    {
        private readonly double diameter;

        public VerhTraversa(double D)
        {
            diameter = D;
        }
        //Деталь 4 - Верхняя траверса
        public override string CreatePart(string partName = null)
        {
            //if (File.Exists(Path.Combine(folderPath, "Верхняя траверса.m3d")))
            //{
            //    return Path.Combine(folderPath, "Верхняя траверса.m3d");
            //}

            CreateNew("Верхняя траверса");
            var radious = diameter / 2;

            //Эскиз 1 - первая четвертина базы
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Base1Scetch2D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза


            Base1Scetch2D.ksLineSeg(0, 0, 0, -radious * 2.265, 1);
            Base1Scetch2D.ksLineSeg(0, -radious * 2.265, radious * 1.515, -radious * 2.265, 1);
            Base1Scetch2D.ksLineSeg(radious * 1.515, -radious * 2.265, radious * 3.775, -diameter, 1);
            Base1Scetch2D.ksLineSeg(radious * 3.775, -diameter, radious * 3.775, 0, 1);
            Base1Scetch2D.ksLineSeg(radious * 3.775, 0, 0, 0, 1);

            ksScetchDef1.EndEdit(); // заканчиваем редактирование эскиза

            //Выдавливание базовой части корпуса
            ksEntity bossExtr1 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef1 = bossExtr1.GetDefinition();
            ksExtrusionParam extrProp1 = (ksExtrusionParam)ExtrDef1.ExtrusionParam();

            if (extrProp1 != null)
            {
                ExtrDef1.SetSketch(ksScetch1Entity); // эскиз операции выдавливания
                                                     // направление выдавливания (обычное)
                extrProp1.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                extrProp1.typeReverse = (short)End_Type.etBlind;
                extrProp1.depthReverse = 420; // глубина выдавливания
                bossExtr1.Create(); // создадим операцию
            }



            //Эскиз 2 - выступающая часть сверху 
            ksEntity ksScetch2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef2 = ksScetch2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef2.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksScetch2Entity.Create(); // создадим эскиз
            ksDocument2D Base2Scetch2D = (ksDocument2D)ksScetchDef2.BeginEdit(); // начинаем редактирование эскиза

            Base2Scetch2D.ksLineSeg(0, 0, 0, -radious * 2.265, 1);
            Base2Scetch2D.ksLineSeg(0, -radious * 2.265, radious * 1.515, -radious * 2.265, 1);
            Base2Scetch2D.ksLineSeg(radious * 1.515, -radious * 2.265, radious * 2.075, -radious * 1.32, 1);
            Base2Scetch2D.ksLineSeg(radious * 2.075, -radious * 1.32, radious * 2.075, 0, 1);
            Base2Scetch2D.ksLineSeg(radious * 2.075, 0, 0, 0, 1);

            ksScetchDef2.EndEdit(); // заканчиваем редактирование эскиза


            //Выдавливание базовой части корпуса
            ksEntity bossExtr2 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef2 = bossExtr2.GetDefinition();
            ksExtrusionParam extrProp2 = (ksExtrusionParam)ExtrDef2.ExtrusionParam();

            if (extrProp2 != null)
            {
                ExtrDef2.SetSketch(ksScetch2Entity); // эскиз операции выдавливания
                                                     // направление выдавливания (обычное)
                extrProp2.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                extrProp2.typeNormal = (short)End_Type.etBlind;
                extrProp2.depthNormal = 220; // глубина выдавливания
                bossExtr2.Create(); // создадим операцию
            }


            //Первый зеркальный массив относительно плоскости ZOX
            ksEntity MirrorCopyPart1 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart1Def = MirrorCopyPart1.GetDefinition();

            if (MirrorCopyPart1Def != null)
            {
                ksEntityCollection EntityCollection1 = MirrorCopyPart1Def.GetOperationArray();
                EntityCollection1.Clear();
                EntityCollection1.Add(bossExtr1);
                EntityCollection1.Add(bossExtr2);

                MirrorCopyPart1Def.SetPlane(basePlaneXOZ);
                MirrorCopyPart1.Create();
            }


            //Второй зеркальный массив относительно плоскости ZOY
            ksEntity MirrorCopyPart2 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart2Def = MirrorCopyPart2.GetDefinition();

            if (MirrorCopyPart2Def != null)
            {
                ksEntityCollection EntityCollection2 = MirrorCopyPart2Def.GetOperationArray();
                EntityCollection2.Clear();
                EntityCollection2.Add(MirrorCopyPart1);

                MirrorCopyPart2Def.SetPlane(basePlaneZOY);
                MirrorCopyPart2.Create();
            }

            ksEntityCollection ksEntityCollection26 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection26.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection26.GetByIndex(q);
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

                            if (Math.Abs(x1 - radious * 1.515) <= 0.1 && Math.Abs(y1 - radious * 2.265) <= 0.1 && Math.Abs(z1 + 420) <= 0.1)
                            {
                                if (Math.Abs(x2 - radious * 3.775) <= 0.1 && Math.Abs(y2 + diameter) <= 0.1 && Math.Abs(z2 + 420) <= 0.1)
                                {
                                    part1.name = ("Panel_1_VrTr");
                                    part1.Update();
                                    break;
                                }

                            }
                        }

                    }
                }
            }

            ksEntityCollection ksEntityCollection22 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection22.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection22.GetByIndex(q);
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

                            if (Math.Abs(x1 + radious * 3.775) <= 0.1 && Math.Abs(y1 + diameter) <= 0.1 && Math.Abs(z1 + 420) <= 0.1)
                            {
                                if (Math.Abs(x2 + radious * 3.775) <= 0.1 && Math.Abs(y2 - diameter) <= 0.1 && Math.Abs(z2) <= 0.1)
                                {
                                    part1.name = ("Panel_2_VrTr");
                                    part1.Update();
                                    break;
                                }

                            }
                        }

                    }
                }
            }


            ksEntityCollection ksEntityCollection23 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection23.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection23.GetByIndex(q);
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

                            if (Math.Abs(x1 + radious * 3.775) <= 0.1 && Math.Abs(y1 - diameter) <= 0.1 && Math.Abs(z1) <= 0.1)
                            {
                                if (Math.Abs(x2 + radious * 2.075) <= 0.1 && Math.Abs(y2 + radious * 1.32) <= 0.1 && Math.Abs(z2) <= 0.1)
                                {
                                    part1.name = ("Panel_3_VrTr");
                                    part1.Update();
                                    break;
                                }

                            }
                        }

                    }
                }
            }

            //Эскиз 3 - отверстия крепежные по бокам внутренние диаметрами 204 и центральное 150

            ksEntity ksCircle1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза.
            SketchDefinition ksCircle1ScetchDef = ksCircle1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза.
            ksCircle1ScetchDef.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksCircle1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle1Sketch = (ksDocument2D)ksCircle1ScetchDef.BeginEdit(); // начинаем редактирование эскиза.

            Circle1Sketch.ksCircle(radious * 3, -radious * 1.32, radious * 0.38, 1);
            Circle1Sketch.ksCircle(radious * 3, 0, radious * 0.28, 1);
            Circle1Sketch.ksCircle(radious * 3, radious * 1.32, radious * 0.38, 1);

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
                CutPropCircle1.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropCircle1.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle1.depthNormal = 420;
                // создадим операцию

                CutCircle1.Create();
            }


            ksEntityCollection ksEntityCollection21 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection21.GetCount(); i++)
            {
                ksEntity part2 = ksEntityCollection21.GetByIndex(i);
                ksFaceDefinition def = part2.GetDefinition();

                if (def.GetOwnerEntity() == CutCircle1)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == radious * 0.28)
                        {
                            part2.name = "CylinderVerhTr1";
                            part2.Update();
                        }
                    }
                }
            }

            ksEntityCollection ksEntityCollection24 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection24.GetCount(); i++)
            {
                ksEntity part2 = ksEntityCollection24.GetByIndex(i);
                ksFaceDefinition def = part2.GetDefinition();

                if (def.GetOwnerEntity() == CutCircle1)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == radious * 0.38)
                        {
                            part2.name = "CylinderVerhTr2";
                            part2.Update();
                        }
                    }
                }
            }


            //Эскиз 4 - два отверстия поверх 204 диаметром 240
            ksEntity ksCircle2ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle2ScetchDef = ksCircle2ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle2ScetchDef.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksCircle2ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle2Sketch = (ksDocument2D)ksCircle2ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle2Sketch.ksCircle(radious * 3, -radious * 1.32, radious * 0.45, 1);
            Circle2Sketch.ksCircle(radious * 3, radious * 1.32, radious * 0.45, 1);

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
                CutPropCircle2.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropCircle2.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle2.depthNormal = 160;
                // создадим операцию

                CutCircle2.Create();
            }




            //Эскиз 5 - два отверстия поверх 240 диаметром 330
            ksEntity ksCircle3ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle3ScetchDef = ksCircle3ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle3ScetchDef.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksCircle3ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle3Sketch = (ksDocument2D)ksCircle3ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle3Sketch.ksCircle(radious * 3, -radious * 1.32, radious * 0.62, 1);
            Circle3Sketch.ksCircle(radious * 3, radious * 1.32, radious * 0.62, 1);

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
                CutPropCircle3.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropCircle3.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle3.depthNormal = 20;
                // создадим операцию

                CutCircle3.Create();
            }


            //Третий зеркальный массив относительно плоскости ZOY (зеркалим отверстия)
            ksEntity MirrorCopyPart3 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart3Def = MirrorCopyPart3.GetDefinition();

            if (MirrorCopyPart3Def != null)
            {
                ksEntityCollection EntityCollection3 = MirrorCopyPart3Def.GetOperationArray();
                EntityCollection3.Clear();
                EntityCollection3.Add(CutCircle1);
                EntityCollection3.Add(CutCircle2);
                EntityCollection3.Add(CutCircle3);

                MirrorCopyPart3Def.SetPlane(basePlaneZOY);
                MirrorCopyPart3.Create();
            }



            //двигаем плоскость в XOY наверх по Z на 220
            ksEntity basePlane1Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef1 = basePlane1Offset.GetDefinition();
            offsetPlaneDef1.direction = false;
            offsetPlaneDef1.offset = 220;
            offsetPlaneDef1.SetPlane(basePlaneXOY);
            basePlane1Offset.Create();


            //Эскиз 6 - мелкие отверстия сверху всех траверсы диаметром по 62

            ksEntity ksCircle4ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle4ScetchDef = ksCircle4ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle4ScetchDef.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle4ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle4Sketch = (ksDocument2D)ksCircle4ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle4Sketch.ksCircle(radious * 1.55, radious * 1.03, radious * 0.117, 1);
            Circle4Sketch.ksCircle(radious * 1.55, -radious * 1.03, radious * 0.117, 1);
            Circle4Sketch.ksCircle(-radious * 1.55, radious * 1.03, radious * 0.117, 1);
            Circle4Sketch.ksCircle(-radious * 1.55, -radious * 1.03, radious * 0.117, 1);

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
                CutPropCircle4.direction = (short)Direction_Type.dtMiddlePlane;
                // тип вырезания (строго на глубину)
                CutPropCircle4.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle4.depthReverse = 1000;
                // создадим операцию

                CutCircle4.Create();
            }


            //Эскиз 7 - центральное отверстие 

            ksEntity ksScetch3Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef3 = ksScetch3Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef3.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch3Entity.Create(); // создадим эскиз
            ksDocument2D Base3Scetch2D = (ksDocument2D)ksScetchDef3.BeginEdit(); // начинаем редактирование эскиза

            Base3Scetch2D.ksLineSeg(0, -220, 0, 420, 3);//ось вращения троечка!!!!!!!!!!
            Base3Scetch2D.ksLineSeg(0, 420, -radious * 1.5, 420, 1);
            Base3Scetch2D.ksLineSeg(-radious * 1.5, 420, -radious * 1.5, 160, 1);
            Base3Scetch2D.ksLineSeg(-radious * 1.5, 160, -radious * 1.54, 120, 1);
            Base3Scetch2D.ksLineSeg(-radious * 1.54, 120, -radious * 1.54, -206, 1);
            Base3Scetch2D.ksLineSeg(-radious * 1.54, -206, -radious * 1.6, -220, 1);
            Base3Scetch2D.ksLineSeg(-radious * 1.6, -220, 0, -220, 1);

            ksScetchDef3.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity Rotate1Extr = part.NewEntity((short)Obj3dType.o3d_cutRotated); //создание интерфейса объекта вращения
            ksCutRotatedDefinition Rotated1Def = Rotate1Extr.GetDefinition();// получаем интерфейс операции вращения
            Rotated1Def.directionType = (short)Direction_Type.dtNormal; //устанавливаем направление вращения - прямо
            Rotated1Def.SetSideParam(false, 360);// настройки вращения (направление вращения, угол вращения) 
            Rotated1Def.SetSketch(ksScetch3Entity);// устанавливаем эскиз вращения
            Rotate1Extr.Create();// создаём операцию


            ksEntityCollection ksEntityCollection25 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection25.GetCount(); i++)
            {
                ksEntity part = ksEntityCollection25.GetByIndex(i);
                ksFaceDefinition def = part.GetDefinition();

                if (def.GetOwnerEntity() == Rotate1Extr)
                {
                    if (def.IsCylinder())
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
                                if (Math.Abs(x1 + radious * 1.5) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 + 420) <= 0.1)
                                {
                                    part.name = ("CentrCyl_VrTr");
                                    part.Update();
                                    break;
                                }
                            }
                        }
                    }

                }
            }


            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Верхняя траверса.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
