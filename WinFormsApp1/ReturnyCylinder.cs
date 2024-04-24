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
    internal class ReturnyCylinder : BasePart
    {

        //Деталь 9 - Ретурный цилиндр
        public override string CreatePart(string name)
        {
            if (File.Exists(Path.Combine(folderPath, $"{name}.m3d")))
            {
                return Path.Combine(folderPath, $"{name}.m3d");
            }
            CreateNew("Ретурный цилиндр");


            //Эскиз 1 - Базовое вращение
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, 0, 10, 0, 3); // создаём первый отрезок (x1,y1,x2,y2,стиль линии) сось

            Scetch12D.ksLineSeg(0, -87.5, 0, -130, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(0, -130, 350, -130, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(350, -130, 350, -115, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(350, -115, 600, -115, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(600, -115, 603, -112, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(603, -112, 1023, -112, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(1023, -112, 1025, -110, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(1025, -110, 2320, -110, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(2320, -110, 2320, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(2320, 0, 2180, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(2180, 0, 2180, -75, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(2180, -75, 530, -75, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(530, -75, 530, -78, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(530, -78, 280, -78, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(280, -78, 280, -87.5, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(280, -87.5, 0, -87.5, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)


            ksScetchDef1.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity RotatedBase1 = part.NewEntity((int)Obj3dType.o3d_bossRotated);
            // получаем интерфейс операции вращения
            ksBossRotatedDefinition RotateDef1 = RotatedBase1.GetDefinition();
            RotateDef1.directionType = (short)Direction_Type.dtNormal;
            // настройки вращения (направление вращения, угол вращения) true - прямое, false - обратное
            RotateDef1.SetSideParam(true, 360);
            // устанавливаем эскиз вращения
            RotateDef1.SetSketch(ksScetch1Entity);
            RotatedBase1.Create(); // создаём операцию


            ksEntityCollection ksEntityCollection91 = 
                (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection91.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection91.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == RotatedBase1)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == 87.5 && h1 == 280)
                        {
                            part1.name = "CylinderCentre1_RetCyl";
                            part1.Update();
                        }
                    }
                }
            }

            ksEntityCollection ksEntityCollection92 = 
                (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection92.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection92.GetByIndex(q);
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

                                ksVertexDefinition p1 = d1.GetVertex(true);
                                ksVertexDefinition p2 = d2.GetVertex(true);

                                double x1, y1, z1;
                                double x2, y2, z2;

                                p1.GetPoint(out x1, out y1, out z1);
                                p2.GetPoint(out x2, out y2, out z2);

                                if (Math.Abs(x1 - 350) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 - 130) <= 0.1)
                                {
                                    if (Math.Abs(x2 - 350) <= 0.1 && Math.Abs(y2) <= 0.1 && Math.Abs(z2 - 130) <= 0.1)
                                    {
                                        part1.name = ("Panel1_RetCyl1");
                                        part1.Update();
                                        break;
                                    }

                                }
                            }

                        }
                    }
                }

            }


            ksEntityCollection ksEntityCollection21 = 
                (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection21.GetCount(); i++)
            {
                ksEntity part = ksEntityCollection21.GetByIndex(i);
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
                            if (Math.Abs(x1 - 2180) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 - 75) <= 0.1)
                            {
                                part.name = ("Panel2_Dno_RetCyl1");
                                part.Update();
                                break;
                            }
                        }
                    }
                }
            }
            //Эскиз 2 - Вырез пазов
            ksEntity ksScetch2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef2 = ksScetch2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef2.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch2Entity.Create(); // создадим эскиз
            ksDocument2D Scetch22D = (ksDocument2D)ksScetchDef2.BeginEdit(); // начинаем редактирование эскиза

            Scetch22D.ksLineSeg(0, -12.5, 45, -12.5, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(45, -12.5, 45, -21, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(45, -21, 70, -21, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(70, -21, 70, 21, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(70, 21, 45, 21, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(45, 21, 45, 12.5, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(45, 12.5, 0, 12.5, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(0, 12.5, 0, -12.5, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef2.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutScetch2 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch2 = CutScetch2.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch2 = (ksExtrusionParam)CutDefScetch2.ExtrusionParam();

            if (CutPropScetch2 != null)
            {
                // эскиз для вырезания
                CutDefScetch2.SetSketch(ksScetchDef2);
                // направление вырезания (обратное)
                CutPropScetch2.direction = (short)Direction_Type.dtMiddlePlane;
                // тип вырезания (строго на глубину)
                CutPropScetch2.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch2.depthNormal = 260;
                // создадим операцию

                CutScetch2.Create();
            }

            ksEntity circCopy1 = part.NewEntity((short)Obj3dType.o3d_circularCopy);
            // получаем свойства кругового массива
            CircularCopyDefinition circCopyDef1 = circCopy1.GetDefinition();
            // создаём ось вращения на основе базовой
            ksEntity baseAxisX = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOX);
            // выставляем базовую ось
            circCopyDef1.SetAxis(baseAxisX);
            // значение для кругового массива, 3 копии 120 градусов
            circCopyDef1.SetCopyParamAlongDir(6, 60, false, false);
            // создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection1 = circCopyDef1.GetOperationArray();
            // очищаем её
            EntityCollection1.Clear();
            // добавляем элемент выдавливания в коллекци.
            EntityCollection1.Add(CutScetch2);
            // создаём массив
            circCopy1.Create();

            ksEntity basePlane1Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef1 = basePlane1Offset.GetDefinition();
            offsetPlaneDef1.direction = false;
            offsetPlaneDef1.offset = 2320;
            offsetPlaneDef1.SetPlane(basePlaneZOY);
            basePlane1Offset.Create();

            //Эскиз 3 - Пазы с противположной стороны
            ksEntity ksScetch3Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef3 = ksScetch3Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef3.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch3Entity.Create(); // создадим эскиз
            ksDocument2D Scetch32D = (ksDocument2D)ksScetchDef3.BeginEdit(); // начинаем редактирование эскиза

            Scetch32D.ksLineSeg(12.5, 109.3, 12.5, 45, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch32D.ksArcBy3Points(12.5, 45, 0, 32.5, -12.5, 45, 1);
            Scetch32D.ksLineSeg(-12.5, 45, -12.5, 109.3, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch32D.ksArcBy3Points(-12.5, 109.3, 0, 110, 12.5, 109.3, 1);

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
                CutPropScetch3.depthReverse = 45;
                // создадим операцию

                CutScetch3.Create();
            }

            ksEntity basePlane2Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef2 = basePlane2Offset.GetDefinition();
            offsetPlaneDef2.direction = false;
            offsetPlaneDef2.offset = 2275;
            offsetPlaneDef2.SetPlane(basePlaneZOY);
            basePlane2Offset.Create();

            //Эскиз 4 - Пазы с противположной стороны
            ksEntity ksScetch4Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef4 = ksScetch4Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef4.SetPlane(basePlane2Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch4Entity.Create(); // создадим эскиз
            ksDocument2D Scetch42D = (ksDocument2D)ksScetchDef4.BeginEdit(); // начинаем редактирование эскиза

            Scetch42D.ksLineSeg(21, 108, 21, 45, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch42D.ksArcBy3Points(21, 45, 0, 24, -21, 45, 1);
            Scetch42D.ksLineSeg(-21, 45, -21, 108, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch42D.ksArcBy3Points(-21, 108, 0, 110, 21, 108, 1);

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
                CutPropScetch4.depthReverse = 25;
                // создадим операцию

                CutScetch4.Create();
            }


            ksEntity circCopy2 = part.NewEntity((short)Obj3dType.o3d_circularCopy);
            // получаем свойства кругового массива
            CircularCopyDefinition circCopyDef2 = circCopy2.GetDefinition();
            // создаём ось вращения на основе базовой
            ksEntity baseAxisX1 = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOX);
            // выставляем базовую ось
            circCopyDef2.SetAxis(baseAxisX1);
            // значение для кругового массива, 3 копии 120 градусов
            circCopyDef2.SetCopyParamAlongDir(4, 90, false, false);
            // создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection2 = circCopyDef2.GetOperationArray();
            // очищаем её
            EntityCollection2.Clear();
            // добавляем элемент выдавливания в коллекци.
            EntityCollection2.Add(CutScetch3);
            EntityCollection2.Add(CutScetch4);
            // создаём массив
            circCopy2.Create();


            //Эскиз 5 - Отверстие диаметром 20
            ksEntity ksCircle5ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle5ScetchDef = ksCircle5ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle5ScetchDef.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle5ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle5Sketch = (ksDocument2D)ksCircle5ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle5Sketch.ksCircle(0, 0, 10, 1);

            ksCircle5ScetchDef.EndEdit(); // заканчиваем редактирование эскиза


            ksEntity CutScetch5 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch5 = CutScetch5.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch5 = (ksExtrusionParam)CutDefScetch5.ExtrusionParam();

            if (CutPropScetch5 != null)
            {
                // эскиз для вырезания
                CutDefScetch5.SetSketch(ksCircle5ScetchEntity);
                // направление вырезания (обратное)
                CutPropScetch5.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch5.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch5.depthReverse = 150;
                // создадим операцию

                CutScetch5.Create();
            }


            //Эскиз 6 - Отверстие диаметром 40
            ksEntity ksCircle6ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle6ScetchDef = ksCircle6ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle6ScetchDef.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle6ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle6Sketch = (ksDocument2D)ksCircle6ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle6Sketch.ksCircle(0, 0, 20, 1);

            ksCircle6ScetchDef.EndEdit(); // заканчиваем редактирование эскиза


            ksEntity CutScetch6 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch6 = CutScetch6.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch6 = (ksExtrusionParam)CutDefScetch6.ExtrusionParam();

            if (CutPropScetch6 != null)
            {
                // эскиз для вырезания
                CutDefScetch6.SetSketch(ksCircle6ScetchEntity);
                // направление вырезания (обратное)
                CutPropScetch6.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch6.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch6.depthReverse = 10;
                // создадим операцию

                CutScetch6.Create();
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
