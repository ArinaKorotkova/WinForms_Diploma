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
    internal class PlunzherGlavCylindra : BasePart
    {
        //Деталь 3 - Плунжер главного цилиндра
        private readonly double diameter;

        public PlunzherGlavCylindra(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string partName = null)
        {
            //if (File.Exists(Path.Combine(folderPath, "Плунжер главного цилиндра.m3d")))
            //{
            //    return Path.Combine(folderPath, "Плунжер главного цилиндра.m3d");
            //}

            CreateNew("Плунжер главного цилиндра");

            var dMal = diameter / 2 * 0.98;

            //Эскиз 1 - главное тело вращения
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Base1Scetch2D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза


            Base1Scetch2D.ksLineSeg(0, 0, 0, -diameter / 2, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Base1Scetch2D.ksLineSeg(0, -diameter / 2, 2350, -diameter / 2, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Base1Scetch2D.ksLineSeg(2350, -diameter / 2, 2630, -dMal, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Base1Scetch2D.ksLineSeg(2630, -dMal, 2630, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Base1Scetch2D.ksLineSeg(2630, 0, 0, 0, 3); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef1.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity Rotate1Extr = part.NewEntity((short)Obj3dType.o3d_bossRotated); //создание интерфейса объекта вращения
            ksBossRotatedDefinition Rotated1Def = Rotate1Extr.GetDefinition();// получаем интерфейс операции вращения
            Rotated1Def.directionType = (short)Direction_Type.dtNormal; //устанавливаем направление вращения - прямо
            Rotated1Def.SetSideParam(false, 360);// настройки вращения (направление вращения, угол вращения)
            Rotated1Def.SetSketch(ksScetch1Entity);// устанавливаем эскиз вращения
            Rotate1Extr.Create();// создаём операцию

            ksEntityCollection ksEntityCollection31 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection31.GetCount(); i++)
            {
                ksEntity part = ksEntityCollection31.GetByIndex(i);
                ksFaceDefinition def = part.GetDefinition();
                if (def.GetOwnerEntity() == Rotate1Extr)
                {
                    if (def.IsCone())
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
                                if (Math.Abs(x1 - 2630) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 - dMal) <= 0.1)
                                {
                                    part.name = ("CylinderPlunGlavCil1");
                                    part.Update();
                                    break;
                                }
                            }
                        }
                    }

                }
            }


            ksEntityCollection ksEntityCollection32 = 
                (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection32.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection32.GetByIndex(q);
                ksFaceDefinition def1 = part1.GetDefinition();

                if (def1.GetOwnerEntity() == Rotate1Extr)
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


                                    if (Math.Abs(x1 - 2630) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 - dMal) <= 0.1)
                                    {
                                        if (Math.Abs(x2 - 2630) <= 0.1 && Math.Abs(y2) <= 0.1 && Math.Abs(z2 - dMal) <= 0.1)
                                        {
                                            part1.name = ("Plun_GlCyl_Dno");
                                            part1.Update();
                                            break;
                                        }

                                    }
                                }

                            }
                        }
                    }
                }
            }

            //Эскиз 2 - отверстие d=95
            ksEntity ksCircl1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle1ScetchDef = ksCircl1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle1ScetchDef.SetPlane(basePlaneZOY); // установим плоскость XOZ базовой для эскиза
            ksCircl1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle1Sketch = (ksDocument2D)ksCircle1ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle1Sketch.ksCircle(0, 0, 47.5, 1);
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
                CutPropCircle1.depthNormal = 20;
                // создадим операцию

                CutCircle1.Create();
            }

            //Эскиз 3 - отверстие d=80
            ksEntity ksCircl2ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle2ScetchDef = ksCircl2ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle2ScetchDef.SetPlane(basePlaneZOY); // установим плоскость XOZ базовой для эскиза
            ksCircl2ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle2Sketch = (ksDocument2D)ksCircle2ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle2Sketch.ksCircle(0, 0, 40, 1);
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
                CutPropCircle2.depthNormal = 180;
                // создадим операцию

                CutCircle2.Create();
            }


            //двигаем плоскость в XOY наверх по Z на dMal
            ksEntity basePlane1Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef1 = basePlane1Offset.GetDefinition();
            offsetPlaneDef1.direction = false;
            offsetPlaneDef1.offset = dMal;
            offsetPlaneDef1.SetPlane(basePlaneXOY);
            basePlane1Offset.Create();


            //Эскиз 4 - прямоугольники
            ksEntity ksRect1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksRect1ScetchDef = ksRect1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksRect1ScetchDef.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksRect1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect1Sketch = (ksDocument2D)ksRect1ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            //центральный прямоугольник 140 на 100
            ksRectangleParam Rect1Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);

            Rect1Param.x = 2495;
            Rect1Param.y = -70;
            Rect1Param.height = 140;
            Rect1Param.width = 100;
            Rect1Param.ang = 0;
            Rect1Param.style = 1;

            Rect1Sketch.ksRectangle(Rect1Param);
            ksRect1ScetchDef.EndEdit();

            ksEntity CutRec1Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutRec1Def = CutRec1Extr.GetDefinition();
            ksExtrusionParam cutRec1Prop = (ksExtrusionParam)CutRec1Def.ExtrusionParam();

            if (cutRec1Prop != null)
            {
                CutRec1Def.SetSketch(ksRect1ScetchDef); // эскиз операции выдавливания
                                                        // направление выдавливания
                cutRec1Prop.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                cutRec1Prop.typeReverse = (short)End_Type.etBlind;
                cutRec1Prop.depthReverse = 85; // глубина выдавливания
                CutRec1Extr.Create(); // создадим операцию
            }

            ksEntity CutRec2Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutRec2Def = CutRec2Extr.GetDefinition();
            ksExtrusionParam cutRec2Prop = (ksExtrusionParam)CutRec2Def.ExtrusionParam();

            if (cutRec2Prop != null)
            {
                CutRec2Def.SetSketch(ksRect1ScetchDef); // эскиз операции выдавливания
                                                        // направление выдавливания
                cutRec2Prop.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                cutRec2Prop.typeNormal = (short)End_Type.etBlind;
                cutRec2Prop.depthNormal = 10; // глубина выдавливания
                CutRec2Extr.Create(); // создадим операцию
            }

            //Эскиз 5 - массив 1 эскиза 4
            ksEntity circCopy1 = part.NewEntity((short)Obj3dType.o3d_circularCopy);
            CircularCopyDefinition circCopyDef1 = circCopy1.GetDefinition();
            ksEntity baseAxisX = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOX);
            circCopyDef1.SetCopyParamAlongDir(2, 180, false, false);
            circCopyDef1.SetAxis(baseAxisX);
            ksEntityCollection EntityCollection1 = circCopyDef1.GetOperationArray();
            // очищаем её
            EntityCollection1.Clear();
            // добавляем элемент выдавливания в коллекци.
            EntityCollection1.Add(CutRec1Extr);
            EntityCollection1.Add(CutRec2Extr);
            // создаём массив
            circCopy1.Create();


            //Эскиз 6 - массив 1 эскиза 5 на 60*
            ksEntity circCopy2 = part.NewEntity((short)Obj3dType.o3d_circularCopy);
            CircularCopyDefinition circCopyDef2 = circCopy2.GetDefinition();
            circCopyDef2.SetCopyParamAlongDir(2, 60, false, false);
            circCopyDef2.SetAxis(baseAxisX);
            ksEntityCollection EntityCollection2 = circCopyDef2.GetOperationArray();
            // очищаем её
            EntityCollection2.Clear();
            // добавляем элемент выдавливания в коллекци.
            EntityCollection2.Add(CutRec1Extr);
            EntityCollection2.Add(CutRec2Extr);
            // создаём массив
            circCopy2.Create();


            //Эскиз 7 - массив 1 эскиза 5 на 120*
            ksEntity circCopy3 = part.NewEntity((short)Obj3dType.o3d_circularCopy);
            CircularCopyDefinition circCopyDef3 = circCopy3.GetDefinition();
            circCopyDef3.SetCopyParamAlongDir(2, 240, false, false);
            circCopyDef3.SetAxis(baseAxisX);
            ksEntityCollection EntityCollection3 = circCopyDef3.GetOperationArray();
            // очищаем её
            EntityCollection3.Clear();
            // добавляем элемент выдавливания в коллекци.
            EntityCollection3.Add(CutRec1Extr);
            EntityCollection3.Add(CutRec2Extr);
            // создаём массив
            circCopy3.Create();


            //двигаем плоскость в ZOY 
            ksEntity basePlane2Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef2 = basePlane2Offset.GetDefinition();
            offsetPlaneDef2.direction = false;
            offsetPlaneDef2.offset = 2630;
            offsetPlaneDef2.SetPlane(basePlaneZOY);
            basePlane2Offset.Create();


            //Эскиз 8 - 4 отверстия d=32
            ksEntity ksCircl3ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle3ScetchDef = ksCircl3ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle3ScetchDef.SetPlane(basePlane2Offset); // установим плоскость XOZ базовой для эскиза
            ksCircl3ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle3Sketch = (ksDocument2D)ksCircle3ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle3Sketch.ksCircle(-diameter / 2 * 0.42, diameter / 2 * 0.72, 16, 1);

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
                CutPropCircle3.depthReverse = 40;
                // создадим операцию

                CutCircle3.Create();
            }

            ksEntityCollection ksEntityCollection33 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection33.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection33.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == CutCircle3)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == 16)
                        {
                            part1.name = "CylinderD32_PlunGlCyl";
                            part1.Update();
                        }
                    }
                }
            }


            //Эскиз 8 - 4 отверстия d=32
            ksEntity ksCircl4ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle4ScetchDef = ksCircl4ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle4ScetchDef.SetPlane(basePlane2Offset); // установим плоскость XOZ базовой для эскиза
            ksCircl4ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle4Sketch = (ksDocument2D)ksCircle4ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle4Sketch.ksCircle(-diameter / 2 * 0.82, 0, 16, 1);
            Circle4Sketch.ksCircle(diameter / 2 * 0.42, -diameter / 2 * 0.72, 16, 1);
            Circle4Sketch.ksCircle(diameter / 2 * 0.82, 0, 16, 1);
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
                CutPropCircle4.depthReverse = 40;
                // создадим операцию

                CutCircle4.Create();
            }

            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Плунжер главного цилиндра.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
