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
    internal class PoperechinaPolzuna : BasePart
    {

        //Деталь 6 - Поперечина ползуна
        private readonly double diameter;

        public PoperechinaPolzuna(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string partName = null)
        {
            //if (File.Exists(Path.Combine(folderPath, "Поперечина ползуна.m3d")))
            //{
            //    return Path.Combine(folderPath, "Поперечина ползуна.m3d");
            //}

            CreateNew("Поперечина ползуна");
            var radius = diameter / 2;

            //Эскиз 1 - первая половина поперечины
            ksEntity ksRect1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. 
            SketchDefinition ksRect1ScetchDef = ksRect1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. 
            ksRect1ScetchDef.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksRect1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect1Sketch = (ksDocument2D)ksRect1ScetchDef.BeginEdit(); // начинаем редактирование эскиза. 
            ksRectangleParam Rect1Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            Rect1Param.x = 0; // координата х одной из вершин прямоугольника
            Rect1Param.y = 0; // координата y одной из вершин прямоугольника
            Rect1Param.height = radius * 0.94; // высота
            Rect1Param.width = radius * 4.44; // ширина
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
                extrProp1.depthNormal = 250; // глубина выдавливания
                bossExtr1.Create(); // создадим операцию
            }

            ksEntityCollection ksEntityCollection61 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection61.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection61.GetByIndex(q);
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


                                if (Math.Abs(x1) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 - 250) <= 0.1)
                                {
                                    if (Math.Abs(x2 - radius * 4.44) <= 0.1 && Math.Abs(y2 - radius * 0.94) <= 0.1 && Math.Abs(z2 - 250) <= 0.1)
                                    {
                                        part1.name = ("Plane1_Nizh_PoperPolzuna");
                                        part1.Update();
                                        break;
                                    }

                                }
                            }

                        }
                    }
                }

            }

            ksEntityCollection ksEntityCollection62 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection62.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection62.GetByIndex(q);
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


                                if (Math.Abs(x1 - radius * 4.44) <= 0.1 && Math.Abs(y1 - radius * 0.94) <= 0.1 && Math.Abs(z1 - 250) <= 0.1)
                                {
                                    if (Math.Abs(x2) <= 0.1 && Math.Abs(y2 - radius * 0.94) <= 0.1 && Math.Abs(z2) <= 0.1)
                                    {
                                        part1.name = ("Plane2_Bokovaya_PoperPolzuna");
                                        part1.Update();
                                        break;
                                    }

                                }
                            }

                        }
                    }
                }

            }

            ksEntity ksCircle1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle1ScetchDef = ksCircle1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle1ScetchDef.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksCircle1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle1Sketch = (ksDocument2D)ksCircle1ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle1Sketch.ksCircle(radius * 4.44, radius * 0.94 / 2, 25, 1);

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
                CutPropCircle1.depthReverse = 250;
                // создадим операцию

                CutCircle1.Create();
            }

            ksEntityCollection ksEntityCollection63 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection63.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection63.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == CutCircle1)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == 25)
                        {
                            part1.name = "CylinderOtv_PoperechPolzuna";
                            part1.Update();
                        }
                    }
                }
            }

            //Эскиз 2 - 3 отверстия по 50 и большое на 142 насквозь
            ksEntity ksCircle11ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle11ScetchDef = ksCircle11ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle11ScetchDef.SetPlane(basePlaneXOY); // установим плоскость XOZ базовой для эскиза
            ksCircle11ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle11Sketch = (ksDocument2D)ksCircle11ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы


            Circle11Sketch.ksCircle(radius * 3.758, radius * 0.94 / 2, 25, 1);
            Circle11Sketch.ksCircle(radius * 3.079, radius * 0.94 / 2, 25, 1);


            Circle11Sketch.ksCircle(radius * 1.419, radius * 0.94 / 2, radius * 0.2679, 1);

            ksCircle11ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle11 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefCircle11 = CutCircle11.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropCircle11 = (ksExtrusionParam)CutDefCircle11.ExtrusionParam();

            if (CutPropCircle11 != null)
            {
                // эскиз для вырезания
                CutDefCircle11.SetSketch(ksCircle11ScetchDef);
                // направление вырезания (обратное)
                CutPropCircle11.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropCircle11.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle11.depthReverse = 250;
                // создадим операцию

                CutCircle11.Create();
            }


            //смещаем плоскость XOY на 300 по оси Z
            ksEntity basePlane1Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef1 = basePlane1Offset.GetDefinition();
            offsetPlaneDef1.direction = true;
            offsetPlaneDef1.offset = 250;
            offsetPlaneDef1.SetPlane(basePlaneXOY);
            basePlane1Offset.Create();


            //Эскиз 3 - 3 отверстия по 120 на 60
            ksEntity ksCircle2ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle2ScetchDef = ksCircle2ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle2ScetchDef.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle2ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle2Sketch = (ksDocument2D)ksCircle2ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle2Sketch.ksCircle(radius * 4.44, radius * 0.94 / 2, 60, 1);
            Circle2Sketch.ksCircle(radius * 3.758, radius * 0.94 / 2, 60, 1);
            Circle2Sketch.ksCircle(radius * 3.079, radius * 0.94 / 2, 60, 1);

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
                CutPropCircle2.depthNormal = 60;
                // создадим операцию

                CutCircle2.Create();
            }


            //Эскиз 4 - отверстие под тягу выталкивателя
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, radius * 0.332, 40, radius * 0.245, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(40, radius * 0.245, 40, radius * 0.336, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(40, radius * 0.336, 76, radius * 0.336, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(76, radius * 0.336, 76, radius * 0.61, 1);

            Scetch12D.ksLineSeg(76, radius * 0.61, 40, radius * 0.61, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(40, radius * 0.61, 40, radius * 0.698, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(40, radius * 0.698, 0, radius * 0.61, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(0, radius * 0.61, 0, radius * 0.332, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef1.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutExtr1 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutExtrDef1 = CutExtr1.GetDefinition();
            ksExtrusionParam CutExtrProp1 = (ksExtrusionParam)CutExtrDef1.ExtrusionParam();

            if (CutExtrProp1 != null)
            {
                CutExtrDef1.SetSketch(ksScetch1Entity); // эскиз операции выдавливания
                                                        // направление выдавливания (обычное)
                CutExtrProp1.direction = (short)Direction_Type.dtBoth;
                // тип выдавливания (строго на глубину)
                CutExtrProp1.typeNormal = (short)End_Type.etBlind;
                CutExtrProp1.depthNormal = 250; // глубина выдавливания
                CutExtr1.Create(); // создадим операцию
            }


            //продолжение под тягу 
            ksEntity ksCircle21ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle21ScetchDef = ksCircle21ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle21ScetchDef.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle21ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle21Sketch = (ksDocument2D)ksCircle21ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle21Sketch.ksCircle(76, radius * 0.4716, radius * 0.1358, 1);

            ksCircle21ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle21 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefCircle21 = CutCircle21.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropCircle21 = (ksExtrusionParam)CutDefCircle21.ExtrusionParam();

            if (CutPropCircle21 != null)
            {
                // эскиз для вырезания
                CutDefCircle21.SetSketch(ksCircle21ScetchDef);
                // направление вырезания (обратное)
                CutPropCircle21.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropCircle21.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle21.depthNormal = 250;
                // создадим операцию

                CutCircle21.Create();
            }

            ksEntityCollection ksEntityCollection4 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection4.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection4.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.IsCylinder())
                {
                    double h1, r;
                    def.GetCylinderParam(out h1, out r);

                    if (r == radius * 0.1358)
                    {
                        part1.name = "CylinderTyga_PoperechPolzuna";
                        part1.Update();
                    }
                }
            }


            ////Эскиз 5 - вырез под задвижку
            //ksEntity ksScetch2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            //SketchDefinition ksScetchDef2 = ksScetch2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            //ksScetchDef2.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            //ksScetch2Entity.Create(); // создадим эскиз
            //ksDocument2D Scetch22D = (ksDocument2D)ksScetchDef2.BeginEdit(); // начинаем редактирование эскиза


            //Scetch22D.ksLineSeg(0, 44, 20, 44, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            //Scetch22D.ksArcBy3Points(20, 44, 34.5, 50.2, 40, 65, 1);
            //Scetch22D.ksLineSeg(40, 65, 0, 88, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            //Scetch22D.ksLineSeg(0, 88, 0, 44, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)


            //Scetch22D.ksLineSeg(0, 204, 20, 204, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            //Scetch22D.ksArcBy3Points(20, 204, 33.8, 198.5, 40, 185, 1);
            //Scetch22D.ksLineSeg(40, 185, 0, 161, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            //Scetch22D.ksLineSeg(0, 161, 0, 204, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            //ksScetchDef2.EndEdit(); // заканчиваем редактирование эскиза


            ////Выдавливание базовой части корпуса
            //ksEntity CutExtr2 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            //ksCutExtrusionDefinition CutExtrDef2 = CutExtr2.GetDefinition();
            //ksExtrusionParam CutExtrProp2 = (ksExtrusionParam)CutExtrDef2.ExtrusionParam();

            //if (CutExtrProp2 != null)
            //{
            //    CutExtrDef2.SetSketch(ksScetch2Entity); // эскиз операции выдавливания
            //                                            // направление выдавливания (обычное)
            //    CutExtrProp2.direction = (short)Direction_Type.dtBoth;
            //    // тип выдавливания (строго на глубину)
            //    CutExtrProp2.typeNormal = (short)End_Type.etBlind;
            //    CutExtrProp2.depthNormal = 40; // глубина выдавливания
            //    CutExtr2.Create(); // создадим операцию
            //}

            //смещаем плоскость ZOY
            ksEntity basePlane2Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef2 = basePlane2Offset.GetDefinition();
            offsetPlaneDef2.direction = false;
            offsetPlaneDef2.offset = radius * 4.44;
            offsetPlaneDef2.SetPlane(basePlaneZOY);
            basePlane2Offset.Create();


            //Зеркальный массив относительно смещенной плоскости 2
            ksEntity MirrorCopyPart1 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart1Def = MirrorCopyPart1.GetDefinition();

            if (MirrorCopyPart1Def != null)
            {
                ksEntityCollection EntityCollection1 = MirrorCopyPart1Def.GetOperationArray();
                EntityCollection1.Clear();
                EntityCollection1.Add(bossExtr1);
                EntityCollection1.Add(CutCircle1);
                EntityCollection1.Add(CutCircle11);
                EntityCollection1.Add(CutCircle2);
                EntityCollection1.Add(CutExtr1);
                EntityCollection1.Add(CutCircle21);


                MirrorCopyPart1Def.SetPlane(basePlane2Offset);
                MirrorCopyPart1.Create();
            }



            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Поперечина ползуна.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
