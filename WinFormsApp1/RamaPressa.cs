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
    internal class RamaPressa : BasePart
    {
        //Деталь 1 - Рама пресса
        private readonly double diameter;

        public RamaPressa(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string name)
        {
            //if (File.Exists(Path.Combine(folderPath, $"{name}.m3d")))
            //{
            //    return Path.Combine(folderPath, $"{name}.m3d");
            //}
            var radius = diameter / 2;

            CreateNew("Рама пресса");
            ksEntity ksRamaScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef = ksRamaScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef.SetPlane(basePlaneZOY); // установим плоскость XOZ базовой для эскиза
            ksRamaScetchEntity.Create(); // создадим эскиз
            ksDocument2D RamaScetch2D = (ksDocument2D)ksScetchDef.BeginEdit(); // начинаем редактирование эскиза


            RamaScetch2D.ksLineSeg(0, 0, radius * 2.06, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            RamaScetch2D.ksLineSeg(radius * 2.06, 0, radius * 2.06, 1400, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            RamaScetch2D.ksLineSeg(radius * 2.06, 1400, radius * 2.132, 1400, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            RamaScetch2D.ksLineSeg(radius * 2.132, 1400, radius * 2.132, 3400, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            RamaScetch2D.ksLineSeg(radius * 2.132, 3400, radius * 2.06, 3400, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            RamaScetch2D.ksLineSeg(radius * 2.06, 3400, radius * 2.06, 4000, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            RamaScetch2D.ksLineSeg(radius * 2.06, 4000, 0, 4000, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            RamaScetch2D.ksLineSeg(0, 4000, 0, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            //Выдавливание базовой части корпуса
            ksEntity bossExtr = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef = bossExtr.GetDefinition();
            ksExtrusionParam extrProp = (ksExtrusionParam)ExtrDef.ExtrusionParam();

            if (extrProp != null)
            {
                ExtrDef.SetSketch(ksRamaScetchEntity); // эскиз операции выдавливания
                                                       // направление выдавливания (обычное)
                extrProp.direction = (short)Direction_Type.dtBoth;
                // тип выдавливания (строго на глубину)
                extrProp.typeNormal = (short)End_Type.etBlind;
                extrProp.depthNormal = radius * 1.6; // глубина выдавливания
                bossExtr.Create(); // создадим операцию
            }


            ksEntityCollection ksEntityCollection11 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection11.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection11.GetByIndex(q);
                ksFaceDefinition def1 = part1.GetDefinition();

                if (def1.GetOwnerEntity() == bossExtr)
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

                                if (Math.Abs(x1) <= 0.1 && Math.Abs(y1 + 4000) <= 0.1 && Math.Abs(z1 + radius * 2.06) <= 0.1)
                                {
                                    if (Math.Abs(x2 + radius * 1.6) <= 0.1 && Math.Abs(y2 + 3400) <= 0.1 && Math.Abs(z2 + radius * 2.06) <= 0.1)
                                    {
                                        part1.name = ("PanelRama_3_R");
                                        part1.Update();
                                        break;
                                    }

                                }
                            }

                        }
                    }
                }

            }

            //Эскиз два - отверстие снизу d=240
            ksEntity ksCircle1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle1ScetchDef = ksCircle1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle1ScetchDef.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksCircle1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle1Sketch = (ksDocument2D)ksCircle1ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle1Sketch.ksCircle(-radius * 1.6 / 2, radius * 1.32, radius * 0.45, 1);
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
                CutPropCircle1.depthNormal = 160;
                // создадим операцию

                CutCircle1.Create();
            }


            //Эскиз три - отверстие снизу d=206
            ksEntity ksCircle2ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle2ScetchDef = ksCircle2ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle2ScetchDef.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksCircle2ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle2Sketch = (ksDocument2D)ksCircle2ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle2Sketch.ksCircle(-radius * 1.6 / 2, radius * 1.32, radius * 0.388, 1);
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
                CutPropCircle2.depthNormal = 240;
                // создадим операцию

                CutCircle2.Create();
            }


            //двигаем плоскость в середину рамы на -212.5 по Х
            ksEntity basePlane1Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef1 = basePlane1Offset.GetDefinition();
            offsetPlaneDef1.direction = true;
            offsetPlaneDef1.offset = radius * 1.6 / 2;
            offsetPlaneDef1.SetPlane(basePlaneZOY);
            basePlane1Offset.Create();

            //Эскиз четыре - прямоугольник 315 на 330
            ksEntity ksRect1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksRect1ScetchDef = ksRect1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksRect1ScetchDef.SetPlane(offsetPlaneDef1); // установим плоскость XOZ базовой для эскиза
            ksRect1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect1Sketch = (ksDocument2D)ksRect1ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            ksRectangleParam recParam = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            recParam.x = radius * 0.64;
            recParam.y = 240;
            recParam.height = 330;
            recParam.width = radius * 1.19;
            recParam.ang = 0;
            recParam.style = 1;

            Rect1Sketch.ksRectangle(recParam);
            ksRect1ScetchDef.EndEdit();


            ksEntity CutRec1Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutRec1Def = CutRec1Extr.GetDefinition();
            ksExtrusionParam cutRec1Prop = (ksExtrusionParam)CutRec1Def.ExtrusionParam();

            if (cutRec1Prop != null)
            {
                CutRec1Def.SetSketch(ksRect1ScetchDef); // эскиз операции выдавливания
                                                        // направление выдавливания
                cutRec1Prop.direction = (short)Direction_Type.dtMiddlePlane;
                // тип выдавливания (строго на глубину)
                cutRec1Prop.typeNormal = (short)End_Type.etBlind;
                cutRec1Prop.depthNormal = radius * 1.3; // глубина выдавливания
                CutRec1Extr.Create(); // создадим операцию
            }


            //Эскиз пять - отверстие снизу после первого прямоугольника d=206
            ksEntity ksCircl3ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle3ScetchDef = ksCircl3ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle3ScetchDef.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksCircl3ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle3Sketch = (ksDocument2D)ksCircle3ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle3Sketch.ksCircle(-radius * 1.6 / 2, radius * 1.32, radius * 0.388, 1);
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
                CutPropCircle3.depthNormal = 650;
                // создадим операцию

                CutCircle3.Create();
            }


            //Эскиз шесть - высокий прямоугольник 2446 на 270
            ksEntity ksLongRect2ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksLongRect2ScetchDef = ksLongRect2ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksLongRect2ScetchDef.SetPlane(offsetPlaneDef1); // установим плоскость XOZ базовой для эскиза
            ksLongRect2ScetchEntity.Create(); // создадим эскиз

            ksDocument2D LongRect2Sketch = (ksDocument2D)ksLongRect2ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            ksRectangleParam LongRecParam = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            LongRecParam.x = radius * 0.8;
            LongRecParam.y = 650;
            LongRecParam.height = 2446;
            LongRecParam.width = radius * 1.02;
            LongRecParam.ang = 0;
            LongRecParam.style = 1;

            LongRect2Sketch.ksRectangle(LongRecParam);
            ksLongRect2ScetchDef.EndEdit();


            ksEntity CutRec2Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutRec2Def = CutRec2Extr.GetDefinition();
            ksExtrusionParam cutRec2Prop = (ksExtrusionParam)CutRec2Def.ExtrusionParam();

            if (cutRec2Prop != null)
            {
                CutRec2Def.SetSketch(ksLongRect2ScetchDef); // эскиз операции выдавливания
                                                            // направление выдавливания
                cutRec2Prop.direction = (short)Direction_Type.dtMiddlePlane;
                // тип выдавливания (строго на глубину)
                cutRec2Prop.typeNormal = (short)End_Type.etBlind;
                cutRec2Prop.depthNormal = radius * 1.3; // глубина выдавливания
                CutRec2Extr.Create(); // создадим операцию
            }

            //Эскиз семь - отверстие снизу после длинного прямоугольника d=206
            ksEntity ksCircl4ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle4ScetchDef = ksCircl4ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle4ScetchDef.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksCircl4ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle4Sketch = (ksDocument2D)ksCircle4ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle4Sketch.ksCircle(-radius * 1.6 / 2, radius * 1.32, radius * 0.388, 1);
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
                CutPropCircle4.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropCircle4.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle4.depthNormal = 3176;
                // создадим операцию

                CutCircle4.Create();
            }


            //Эскиз восемь - средний прямоугольник 584 на 270
            ksEntity ksMidRect3ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksMidRect3ScetchDef = ksMidRect3ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksMidRect3ScetchDef.SetPlane(offsetPlaneDef1); // установим плоскость XOZ базовой для эскиза
            ksMidRect3ScetchEntity.Create(); // создадим эскиз

            ksDocument2D MidRect3Sketch = (ksDocument2D)ksMidRect3ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            ksRectangleParam MidRectParam = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            MidRectParam.x = radius  * 0.8;
            MidRectParam.y = 3176;
            MidRectParam.height = 584;
            MidRectParam.width = radius * 1.02;
            MidRectParam.ang = 0;
            MidRectParam.style = 1;

            MidRect3Sketch.ksRectangle(MidRectParam);
            ksMidRect3ScetchDef.EndEdit();


            ksEntity CutRec3Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutRec3Def = CutRec3Extr.GetDefinition();
            ksExtrusionParam cutRec3Prop = (ksExtrusionParam)CutRec3Def.ExtrusionParam();

            if (cutRec3Prop != null)
            {
                CutRec3Def.SetSketch(ksMidRect3ScetchDef); // эскиз операции выдавливания
                                                           // направление выдавливания
                cutRec3Prop.direction = (short)Direction_Type.dtMiddlePlane;
                // тип выдавливания (строго на глубину)
                cutRec3Prop.typeNormal = (short)End_Type.etBlind;
                cutRec3Prop.depthNormal = radius * 1.3; // глубина выдавливания
                CutRec3Extr.Create(); // создадим операцию
            }


            //двигаем плоскость в XOZ наверх рамы на 4000 по y
            ksEntity basePlane2Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef2 = basePlane2Offset.GetDefinition();
            offsetPlaneDef2.direction = false;
            offsetPlaneDef2.offset = 4000;
            offsetPlaneDef2.SetPlane(basePlaneXOZ);
            basePlane2Offset.Create();


            //Эскиз девять - отверстие сверху после среднего прямоугольника d=206 на смещенной плокости
            ksEntity ksCircl5ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle5ScetchDef = ksCircl5ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle5ScetchDef.SetPlane(basePlane2Offset); // установим плоскость XOZ базовой для эскиза
            ksCircl5ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle5Sketch = (ksDocument2D)ksCircle5ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle5Sketch.ksCircle(-radius * 1.6 / 2, radius * 1.32, radius * 0.388, 1);
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
                CutPropCircle5.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle5.depthReverse = 240;
                // создадим операцию

                CutCircle5.Create();
            }


            //Эскиз десять - отверстие сверху d=240 на смещенной плокости
            ksEntity ksCircl6ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle6ScetchDef = ksCircl6ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle6ScetchDef.SetPlane(basePlane2Offset); // установим плоскость XOZ базовой для эскиза
            ksCircl6ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle6Sketch = (ksDocument2D)ksCircle6ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle6Sketch.ksCircle(-radius * 1.6 / 2, radius * 1.32, radius * 0.45, 1);
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
                CutPropCircle6.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle6.depthReverse = 160;
                // создадим операцию

                CutCircle6.Create();
            }

            //Первая половина рамы готова, далее делаем зеркалирование относительно плоскости XOY
            ksEntity MirrorCopyPart1 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart1Def = MirrorCopyPart1.GetDefinition();
            if (MirrorCopyPart1Def != null)
            {
                ksEntityCollection EntityCollection = MirrorCopyPart1Def.GetOperationArray();
                EntityCollection.Clear();
                EntityCollection.Add(bossExtr);
                EntityCollection.Add(CutCircle1);
                EntityCollection.Add(CutCircle2);
                EntityCollection.Add(CutRec1Extr);
                EntityCollection.Add(CutCircle3);
                EntityCollection.Add(CutRec2Extr);
                EntityCollection.Add(CutCircle4);
                EntityCollection.Add(CutRec3Extr);
                EntityCollection.Add(CutCircle5);
                EntityCollection.Add(CutCircle6);
                MirrorCopyPart1Def.SetPlane(basePlaneXOY);
                MirrorCopyPart1.Create();
            }

            ksEntityCollection ksEntityCollection12 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection12.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection12.GetByIndex(q);
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

                            if (Math.Abs(x1) <= 0.1 && Math.Abs(y1 + 4000) <= 0.1 && Math.Abs(z1 - radius * 2.06) <= 0.1)
                            {
                                if (Math.Abs(x2 + radius * 1.6) <= 0.1 && Math.Abs(y2 + 3400) <= 0.1 && Math.Abs(z2 - radius * 2.06) <= 0.1)
                                {
                                    part1.name = ("PanelRama_3_L");
                                    part1.Update();
                                    break;
                                }

                            }
                        }

                    }
                }
            }


            ksEntityCollection ksEntityCollection13 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection13.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection13.GetByIndex(q);
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

                            if (Math.Abs(x1) <= 0.1 && Math.Abs(y1 + 4000) <= 0.1 && Math.Abs(z1 - radius * 2.06) <= 0.1)
                            {
                                if (Math.Abs(x2 + radius * 1.6) <= 0.1 && Math.Abs(y2 + 4000) <= 0.1 && Math.Abs(z2 + radius * 2.06) <= 0.1)
                                {
                                    part1.name = ("Panel_1_Rama");
                                    part1.Update();
                                    break;
                                }

                            }
                        }

                    }
                }
            }

            ksEntityCollection ksEntityCollection15 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection15.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection15.GetByIndex(q);
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

                            if (Math.Abs(x1 + radius * 1.6) <= 0.1 && Math.Abs(y1 + 4000) <= 0.1 && Math.Abs(z1 + radius * 2.06) <= 0.1)
                            {
                                if (Math.Abs(x2 + radius * 1.6) <= 0.1 && Math.Abs(y2) <= 0.1 && Math.Abs(z2 - radius * 2.06) <= 0.1)
                                {
                                    part1.name = ("Panel_2_Rama");
                                    part1.Update();
                                    break;
                                }

                            }
                        }

                    }
                }
            }


            ksEntityCollection ksEntityCollection16 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection16.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection16.GetByIndex(q);
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


                            if (Math.Abs(x1) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 + radius * 2.06) <= 0.1)
                            {
                                if (Math.Abs(x2 + radius * 1.6) <= 0.1 && Math.Abs(y2) <= 0.1 && Math.Abs(z2 - radius * 2.06) <= 0.1)
                                {
                                    part1.name = ("Panel_4_Rama");
                                    part1.Update();
                                    break;
                                }

                            }
                        }

                    }
                }
            }

            //Далее идет построение центральной части корпуса, где свозное отверстие под штамп

            //Эскиз одиннадцать - отверстие снизу d=224
            ksEntity ksCircl7ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle7ScetchDef = ksCircl7ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle7ScetchDef.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksCircl7ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle7Sketch = (ksDocument2D)ksCircle7ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle7Sketch.ksCircle(-radius * 1.6 / 2, 0, radius * 0.42, 1);
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
                CutPropCircle7.depthNormal = 160;
                // создадим операцию

                CutCircle7.Create();
            }


            //двигаем плоскость в XOZ наверх на расстояние одного отверстия, т.е. на 160 по y
            ksEntity basePlane3Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef3 = basePlane3Offset.GetDefinition();
            offsetPlaneDef3.direction = false;
            offsetPlaneDef3.offset = 160;
            offsetPlaneDef3.SetPlane(basePlaneXOZ);
            basePlane3Offset.Create();

            //Эскиз двенадцать - отверстие снизу на созданной смещенной плоскости d=250
            ksEntity ksCircl8ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle8ScetchDef = ksCircl8ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle8ScetchDef.SetPlane(basePlane3Offset); // установим плоскость XOZ базовой для эскиза
            ksCircl8ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle8Sketch = (ksDocument2D)ksCircle8ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle8Sketch.ksCircle(-radius * 1.6 / 2, 0, radius * 0.47, 1);
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
                CutPropCircle8.depthNormal = 230;
                // создадим операцию

                CutCircle8.Create();
            }


            //двигаем плоскость в XOZ наверх на расстояние двух нижних отверстий по центру, т.е. на 160 + 230 = 390 по y
            ksEntity basePlane4Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef4 = basePlane4Offset.GetDefinition();
            offsetPlaneDef4.direction = false;
            offsetPlaneDef4.offset = 390;
            offsetPlaneDef4.SetPlane(basePlaneXOZ);
            basePlane4Offset.Create();

            //Эскиз тринадцать - отверстие снизу на созданной смещенной плоскости d=230 (послденее отверстие перед вырезанной частью)
            ksEntity ksCircl9ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle9ScetchDef = ksCircl9ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle9ScetchDef.SetPlane(basePlane4Offset); // установим плоскость XOZ базовой для эскиза
            ksCircl9ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle9Sketch = (ksDocument2D)ksCircle9ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle9Sketch.ksCircle(-radius * 1.6 / 2, 0, radius * 0.434, 1);
            ksCircle9ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle9 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefCircle9 = CutCircle9.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropCircle9 = (ksExtrusionParam)CutDefCircle9.ExtrusionParam();

            if (CutPropCircle9 != null)
            {
                // эскиз для вырезания
                CutDefCircle9.SetSketch(ksCircle9ScetchDef);
                // направление вырезания (обратное)
                CutPropCircle9.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropCircle9.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle9.depthNormal = 260;
                // создадим операцию

                CutCircle9.Create();
            }


            //Эскиз четырнадцать - центральный прямоугольник 2390 на 370 насквозь +  центральный прямоугольник 584 на 370 насквозь
            ksEntity ksRect45ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksRect45ScetchDef = ksRect45ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksRect45ScetchDef.SetPlane(offsetPlaneDef1); // установим плоскость XOZ базовой для эскиза
            ksRect45ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect45Sketch = (ksDocument2D)ksRect45ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            //центральный прямоугольник 2390 на 370
            ksRectangleParam Rect4Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            Rect4Param.x = -radius * 0.64;
            Rect4Param.y = 650;
            Rect4Param.height = 2390;
            Rect4Param.width = radius * 1.32;
            Rect4Param.ang = 0;
            Rect4Param.style = 1;

            Rect45Sketch.ksRectangle(Rect4Param);


            //центральный прямоугольник 640 на 370
            ksRectangleParam Rect5Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            Rect5Param.x = -radius * 0.64;
            Rect5Param.y = 3176;
            Rect5Param.height = 640;
            Rect5Param.width = radius * 1.32;
            Rect5Param.ang = 0;
            Rect5Param.style = 1;

            Rect45Sketch.ksRectangle(Rect5Param);
            ksRect45ScetchDef.EndEdit();


            ksEntity CutRec45Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutRec45Def = CutRec45Extr.GetDefinition();
            ksExtrusionParam cutRec45Prop = (ksExtrusionParam)CutRec45Def.ExtrusionParam();

            if (cutRec45Prop != null)
            {
                CutRec45Def.SetSketch(ksRect45ScetchDef); // эскиз операции выдавливания
                                                          // направление выдавливания
                cutRec45Prop.direction = (short)Direction_Type.dtMiddlePlane;
                // тип выдавливания (строго на глубину)
                cutRec45Prop.typeNormal = (short)End_Type.etBlind;
                cutRec45Prop.depthNormal = radius * 1.6; // глубина выдавливания
                CutRec45Extr.Create(); // создадим операцию
            }

            //двигаем плоскость в XOZ наверх на расстояние трех нижних отверстий по центру и вырезанного прямоугольника, т.е. на 650 + 2390 = 3040 по y
            ksEntity basePlane5Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef5 = basePlane5Offset.GetDefinition();
            offsetPlaneDef5.direction = false;
            offsetPlaneDef5.offset = 3040;
            offsetPlaneDef5.SetPlane(basePlaneXOZ);
            basePlane5Offset.Create();



            //Эскиз пятнадцать - отверстие между двумя центральными пярмоугольниками на созданной смещенной плоскости d=206 
            ksEntity ksCircl10ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle10ScetchDef = ksCircl10ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle10ScetchDef.SetPlane(basePlane5Offset); // установим плоскость XOZ базовой для эскиза
            ksCircl10ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle10Sketch = (ksDocument2D)ksCircle10ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle10Sketch.ksCircle(-radius * 1.6 / 2, 0, radius * 0.388, 1);
            ksCircle10ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle10 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefCircle10 = CutCircle10.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropCircle10 = (ksExtrusionParam)CutDefCircle10.ExtrusionParam();

            if (CutPropCircle10 != null)
            {
                // эскиз для вырезания
                CutDefCircle10.SetSketch(ksCircle10ScetchDef);
                // направление вырезания (обратное)
                CutPropCircle10.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropCircle10.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropCircle10.depthNormal = 260;
                // создадим операцию

                CutCircle10.Create();
            }

            //двигаем плоскость в XOZ наверх на расстояние всей рамы до верхней грани по y
            ksEntity basePlane6Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef6 = basePlane6Offset.GetDefinition();
            offsetPlaneDef6.direction = false;
            offsetPlaneDef6.offset = 4000;
            offsetPlaneDef6.SetPlane(basePlaneXOZ);
            basePlane6Offset.Create();

            //Эскиз шестнадцать - отверстие на самой верхней грани на созданной смещенной плоскости #6 d=190 
            ksEntity ksCircl11ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle11ScetchDef = ksCircl11ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle11ScetchDef.SetPlane(basePlane6Offset); // установим плоскость XOZ базовой для эскиза
            ksCircl11ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle11Sketch = (ksDocument2D)ksCircle11ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle11Sketch.ksCircle(-radius * 1.6 / 2, 0, radius * 0.358, 1);
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
                CutPropCircle11.depthReverse = 240;
                // создадим операцию

                CutCircle11.Create();
            }


            ksEntityCollection ksEntityCollection14 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection14.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection14.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == CutCircle11)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == radius * 0.358)
                        {
                            part1.name = "CylinderRama1";
                            part1.Update();
                        }
                    }
                }
            }

            //Эскиз семнадцать окно в раме верхнее 
            ksEntity ksRect6ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksRect6ScetchDef = ksRect6ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksRect6ScetchDef.SetPlane(basePlaneZOY); // установим плоскость XOZ базовой для эскиза
            ksRect6ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect6Sketch = (ksDocument2D)ksRect6ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            //центральный прямоугольник 200 на 120
            ksRectangleParam Rect6Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);

            Rect6Param.x = -radius * 1.55;
            Rect6Param.y = 3376;
            Rect6Param.height = 200;
            Rect6Param.width = radius * 0.45;
            Rect6Param.ang = 0;
            Rect6Param.style = 1;

            Rect6Sketch.ksRectangle(Rect6Param);
            ksRect6ScetchDef.EndEdit();

            ksEntity CutRec6Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutRec6Def = CutRec6Extr.GetDefinition();
            ksExtrusionParam cutRec6Prop = (ksExtrusionParam)CutRec6Def.ExtrusionParam();

            if (cutRec6Prop != null)
            {
                CutRec6Def.SetSketch(ksRect6ScetchDef); // эскиз операции выдавливания
                                                        // направление выдавливания
                cutRec6Prop.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                cutRec6Prop.typeReverse = (short)End_Type.etBlind;
                cutRec6Prop.depthReverse = radius * 0.15; // глубина выдавливания
                CutRec6Extr.Create(); // создадим операцию
            }

            //Эскиз семнадцать 5 окон в раме в середине
            ksEntity ksRect7ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksRect7ScetchDef = ksRect7ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksRect7ScetchDef.SetPlane(basePlaneZOY); // установим плоскость XOZ базовой для эскиза
            ksRect7ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect7Sketch = (ksDocument2D)ksRect7ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            //центральный прямоугольник 200 на 120
            ksRectangleParam Rect7Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);

            Rect7Param.x = -radius * 1.55;
            Rect7Param.y = 2610;
            Rect7Param.height = 200;
            Rect7Param.width = radius * 0.45;
            Rect7Param.ang = 0;
            Rect7Param.style = 1;

            Rect7Sketch.ksRectangle(Rect7Param);
            ksRect7ScetchDef.EndEdit();

            ksEntity CutRec7Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutRec7Def = CutRec7Extr.GetDefinition();
            ksExtrusionParam cutRec7Prop = (ksExtrusionParam)CutRec7Def.ExtrusionParam();

            if (cutRec7Prop != null)
            {
                CutRec7Def.SetSketch(ksRect7ScetchDef); // эскиз операции выдавливания
                                                        // направление выдавливания
                cutRec7Prop.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                cutRec7Prop.typeReverse = (short)End_Type.etBlind;
                cutRec7Prop.depthReverse = radius * 0.15; // глубина выдавливания
                CutRec7Extr.Create(); // создадим операцию
            }


            ksEntity MeshCopy1 = part.NewEntity((short)Obj3dType.o3d_meshCopy);
            //создаём интерфейс свойств линейного массива
            MeshCopyDefinition MeshCopyDef1 = MeshCopy1.GetDefinition();
            //создаём ось линейного массива на основе базовой Y
            ksEntity baseAxisY = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOY);
            MeshCopyDef1.SetAxis1(baseAxisY);
            MeshCopyDef1.count1 = 5;
            MeshCopyDef1.step1 = 430;
            ksEntityCollection EntityCollection1 = MeshCopyDef1.OperationArray();
            EntityCollection1.Clear(); // очищаем её
            //добавляем элемент выдавливания в коллекци.
            EntityCollection1.Add(CutRec7Extr);
            MeshCopy1.Create(); // создаём массив



            //Зеркалим окна относительно плоскости XOY
            ksEntity MirrorCopyPart2 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart2Def = MirrorCopyPart2.GetDefinition();
            if (MirrorCopyPart2Def != null)
            {
                ksEntityCollection EntityCollection2 = MirrorCopyPart2Def.GetOperationArray();
                EntityCollection2.Clear();
                EntityCollection2.Add(CutRec6Extr);
                EntityCollection2.Add(MeshCopy1);

                MirrorCopyPart2Def.SetPlane(basePlaneXOY);
                MirrorCopyPart2.Create();
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
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3176, radius * 0.64);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3176, -radius * 0.64);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3816, -radius * 0.64);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3816, radius * 0.64);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));


            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3040, radius * 0.64);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3040, -radius * 0.64);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -650, -radius * 0.64);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -650, radius * 0.64);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));



            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3760, radius * 1.83);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3760, radius * 0.8);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3176, radius * 0.8);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3176, radius * 1.83);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));



            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3096, radius * 1.83);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3096, radius * 0.8);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -650, radius * 0.8);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -650, radius * 1.83);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));


            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -570, radius * 1.83);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -570, radius * 0.64);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -240, radius * 0.64);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -240, radius * 1.83);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));


            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3760, -radius * 0.8);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3760, -radius * 1.83);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3176, -radius * 1.83);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3176, -radius * 0.8);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));


            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3096, -radius * 0.8);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -3096, -radius * 1.83);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -650, -radius * 1.83);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -650, -radius * 0.8);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));


            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -570, -radius * 0.64);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -570, -radius * 1.83);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -240, -radius * 1.83);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityColPart1 = (ksEntityCollection)part.EntityCollection((int)Obj3dType.o3d_edge);
            //// оставляем в массиве только ребро, содержащую точку (x,y,z)
            //entityColPart1.SelectByPoint(-radius * 1.6 / 2, -240, -radius * 0.64);
            //// добавить в коллекцию для скругления первый элемент из массива рёбер
            //entityCollectionFillet1.Add(entityColPart1.GetByIndex(0));

            //entityFillet1.Create(); // создать скругление

            //двигаем плоскость в XOZ наверх на расстояние всей рамы до верхней грани по y
            ksEntity basePlane7Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef7 = basePlane7Offset.GetDefinition();
            offsetPlaneDef7.direction = false;
            offsetPlaneDef7.offset = 3400;
            offsetPlaneDef7.SetPlane(basePlaneXOZ);
            basePlane7Offset.Create();


            ksEntity ksRect8ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksRect8ScetchDef = ksRect8ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksRect8ScetchDef.SetPlane(offsetPlaneDef7); // установим плоскость XOZ базовой для эскиза
            ksRect8ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect8Sketch = (ksDocument2D)ksRect8ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            //центральный прямоугольник 200 на 120
            ksRectangleParam Rect8Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);

            Rect8Param.x = -radius * 1.66;
            Rect8Param.y = radius * 1.31;
            Rect8Param.height = radius * 0.755;
            Rect8Param.width = radius * 0.075;
            Rect8Param.ang = 0;
            Rect8Param.style = 1;

            ksRectangleParam Rect9Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);

            Rect9Param.x = -radius * 1.66;
            Rect9Param.y = -radius * 2.06;
            Rect9Param.height = radius * 0.755;
            Rect9Param.width = radius * 0.07;
            Rect9Param.ang = 0;
            Rect9Param.style = 1;

            Rect8Sketch.ksRectangle(Rect8Param);
            Rect8Sketch.ksRectangle(Rect9Param);
            ksRect8ScetchDef.EndEdit();


            ksEntity bossExtr1 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef1 = bossExtr1.GetDefinition();
            ksExtrusionParam extrProp1 = (ksExtrusionParam)ExtrDef1.ExtrusionParam();

            if (extrProp1 != null)
            {
                ExtrDef1.SetSketch(ksRect8ScetchDef); // эскиз операции выдавливания
                                                       // направление выдавливания (обычное)
                extrProp1.direction = (short)Direction_Type.dtBoth;
                // тип выдавливания (строго на глубину)
                extrProp1.typeNormal = (short)End_Type.etBlind;
                extrProp1.depthNormal = 2000; // глубина выдавливания
                bossExtr1.Create(); // создадим операцию
            }

            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, $"{name}.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
