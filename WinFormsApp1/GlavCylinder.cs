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
    internal class GlavCylinder : BasePart
    {
        //Деталь 2 - Главный цилиндр 
        private readonly double diameter;

        public GlavCylinder(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string partName = null)
        {
            //if (File.Exists(Path.Combine(folderPath, "Главный цилиндр.m3d")))
            //{
            //    return Path.Combine(folderPath, "Главный цилиндр.m3d");
            //}

            CreateNew("Главный цилиндр");
            double radius = diameter / 2;
            double par1 = radius * 1.08;

            //Эскиз 1 - первая четвертина базы
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Base1Scetch2D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза


            Base1Scetch2D.ksLineSeg(-2570, 0, -2570, radius * 1.33, 1);
            Base1Scetch2D.ksLineSeg(-2570, radius * 1.33, -2510, radius * 1.55, 1);
            Base1Scetch2D.ksLineSeg(-2510, radius * 1.55, -826.5, radius * 1.55, 1);
            Base1Scetch2D.ksLineSeg(-826.5, radius * 1.55, -825, radius * 1.55, 1);

            Base1Scetch2D.ksLineSeg(-825, radius * 1.55, -517.5, radius * 1.56, 1);
            Base1Scetch2D.ksLineSeg(-517.5, radius * 1.56, -515, radius * 1.57, 1);
            Base1Scetch2D.ksLineSeg(-515, radius * 1.57, -195, radius * 1.57, 1);
            Base1Scetch2D.ksLineSeg(-195, radius * 1.57, -195, radius * 1.72, 1);

            Base1Scetch2D.ksLineSeg(-195, radius * 1.72, -192, radius * 1.72, 1);
            Base1Scetch2D.ksLineSeg(-192, radius * 1.72, -192, radius * 1.75, 1);
            Base1Scetch2D.ksLineSeg(-192, radius * 1.75, 0, radius * 1.75, 1);

            Base1Scetch2D.ksLineSeg(0, radius * 1.75, 0, radius * 1.125, 1);
            Base1Scetch2D.ksLineSeg(0, radius * 1.125, -310, radius * 1.125, 1);
            Base1Scetch2D.ksLineSeg(-310, radius * 1.125, -310, radius * 1.08, 1);
            Base1Scetch2D.ksLineSeg(-310, radius * 1.08, -620, radius * 1.08, 1);
            Base1Scetch2D.ksLineSeg(-620, radius * 1.08, -620, radius * 1.125, 1);

            Base1Scetch2D.ksLineSeg(-620, radius * 1.125, -2331, radius * 1.125, 1);
            Base1Scetch2D.ksLineSeg(-2331, radius * 1.125, -2331, 0, 1);
            Base1Scetch2D.ksLineSeg(-2331, 0, -2570, 0, 3);

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


            //Эскиз 2 - отверстие для воды сверху цилиндра
            ksEntity ksCircle1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksCircleDef1 = ksCircle1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksCircleDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksCircle1Entity.Create(); // создадим эскиз
            ksDocument2D Circle1Scetch2D = (ksDocument2D)ksCircleDef1.BeginEdit(); // начинаем редактирование эскиза


            Circle1Scetch2D.ksCircle(-2256, 0, 45, 1);

            ksCircleDef1.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle1Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutCircle1Def = CutCircle1Extr.GetDefinition();
            ksExtrusionParam cutCircle1Prop = (ksExtrusionParam)CutCircle1Def.ExtrusionParam();

            if (cutCircle1Prop != null)
            {
                CutCircle1Def.SetSketch(ksCircleDef1); // эскиз операции выдавливания
                                                       // направление выдавливания
                cutCircle1Prop.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                cutCircle1Prop.typeNormal = (short)End_Type.etBlind;
                cutCircle1Prop.depthNormal = radius; // глубина выдавливания
                CutCircle1Extr.Create(); // создадим операцию
            }


            ksEntityCollection ksEntityCollection21 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection21.GetCount(); i++)
            {
                ksEntity part = ksEntityCollection21.GetByIndex(i);
                ksFaceDefinition def = part.GetDefinition();
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
                            if (Math.Abs(x1 + 620) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 + par1) <= 0.1)
                            {
                                part.name = ("CylinderGlavCil1");
                                part.Update();
                                break;
                            }
                        }
                    }
                }
            }


            ksEntityCollection ksEntityCollection22 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection22.GetCount(); i++)
            {
                ksEntity part = ksEntityCollection22.GetByIndex(i);
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
                            if (Math.Abs(x1 + 2331) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 + radius * 1.125) <= 0.1)
                            {
                                part.name = ("Plane_1_Dno_GlavCyl");
                                part.Update();
                                break;
                            }
                        }
                    }
                }
            }

            //Эскиз 3 - отверстия по центру 4 штуки
            ksEntity ksCircle2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksCircleDef2 = ksCircle2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksCircleDef2.SetPlane(basePlaneZOY); // установим плоскость XOZ базовой для эскиза
            ksCircle2Entity.Create(); // создадим эскиз
            ksDocument2D Circle2Scetch2D = (ksDocument2D)ksCircleDef2.BeginEdit(); // начинаем редактирование эскиза


            Circle2Scetch2D.ksCircle(radius * 1.35, radius * 0.9, 20, 1);
            Circle2Scetch2D.ksCircle(radius * 1.35, -radius * 0.9, 20, 1);
            Circle2Scetch2D.ksCircle(-radius * 1.35, radius * 0.9, 20, 1);
            Circle2Scetch2D.ksCircle(-radius * 1.35, -radius * 0.9, 20, 1);

            ksCircleDef2.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle2Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutCircle2Def = CutCircle2Extr.GetDefinition();
            ksExtrusionParam cutCircle2Prop = (ksExtrusionParam)CutCircle2Def.ExtrusionParam();

            if (cutCircle2Prop != null)
            {
                CutCircle2Def.SetSketch(ksCircleDef2); // эскиз операции выдавливания
                                                       // направление выдавливания
                cutCircle2Prop.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                cutCircle2Prop.typeReverse = (short)End_Type.etBlind;
                cutCircle2Prop.depthReverse = 30; // глубина выдавливания
                CutCircle2Extr.Create(); // создадим операцию
            }


            //Эскиз 4 - первый паз в Т-образном отверстии

            ksEntity ksSketchPase1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksPaseSketchDef1 = ksSketchPase1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksPaseSketchDef1.SetPlane(basePlaneZOY); // установим плоскость XOZ базовой для эскиза
            ksSketchPase1Entity.Create(); // создадим эскиз
            ksDocument2D Pase1Scetch2D = (ksDocument2D)ksPaseSketchDef1.BeginEdit(); // начинаем редактирование эскиза


            Pase1Scetch2D.ksLineSeg(20, radius * 1.75, 20, radius * 1.3, 1);
            Pase1Scetch2D.ksArcBy3Points(20, radius * 1.3, 0, radius * 1.23, -20, radius * 1.3, 1);
            Pase1Scetch2D.ksLineSeg(-20, radius * 1.3, -20, radius * 1.75, 1);
            Pase1Scetch2D.ksArcBy3Points(-20, radius * 1.75, 0, radius * 1.76, 20, radius * 1.75, 1);

            ksPaseSketchDef1.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutPase1Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutPase1Def = CutPase1Extr.GetDefinition();
            ksExtrusionParam cutPase1Prop = (ksExtrusionParam)CutPase1Def.ExtrusionParam();

            if (cutPase1Prop != null)
            {
                CutPase1Def.SetSketch(ksPaseSketchDef1); // эскиз операции выдавливания
                                                         // направление выдавливания
                cutPase1Prop.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                cutPase1Prop.typeReverse = (short)End_Type.etBlind;
                cutPase1Prop.depthReverse = 40; // глубина выдавливания
                CutPase1Extr.Create(); // создадим операцию
            }

            // операция массив вращения
            ksEntity Pase1Copy = part.NewEntity((short)Obj3dType.o3d_circularCopy);
            // получаем свойства кругового массива
            CircularCopyDefinition Pase1CopyDef = Pase1Copy.GetDefinition();
            // создаём ось вращения на основе базовой
            ksEntity baseAxisX = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOX);
            // выставляем базовую ось
            Pase1CopyDef.SetAxis(baseAxisX);
            // значение для кругового массива, 3 копии 120 градусов
            Pase1CopyDef.SetCopyParamAlongDir(16, 22.5, false, false);
            // создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection1 = Pase1CopyDef.GetOperationArray();
            // очищаем её
            EntityCollection1.Clear();
            // добавляем элемент выдавливания в коллекци.
            EntityCollection1.Add(CutPase1Extr);
            // создаём массив
            Pase1Copy.Create();



            // смещаем плоскость от базовой ZOY по оси X на 40
            ksEntity basePlane1Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef1 = basePlane1Offset.GetDefinition();
            offsetPlaneDef1.direction = true;
            offsetPlaneDef1.offset = 40;
            offsetPlaneDef1.SetPlane(basePlaneZOY);
            basePlane1Offset.Create();


            //Эскиз 5 - вторая часть паза в Т-образном отверстии

            ksEntity ksSketchPase2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksPaseSketchDef2 = ksSketchPase2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksPaseSketchDef2.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksSketchPase2Entity.Create(); // создадим эскиз
            ksDocument2D Pase2Scetch2D = (ksDocument2D)ksPaseSketchDef2.BeginEdit(); // начинаем редактирование эскиза


            Pase2Scetch2D.ksLineSeg(40, radius * 1.75, 40, radius * 1.3, 1);
            Pase2Scetch2D.ksArcBy3Points(40, radius * 1.3, 0, radius * 1.15, -40, radius * 1.3, 1);
            Pase2Scetch2D.ksLineSeg(-40, radius * 1.3, -40, radius * 1.75, 1);
            Pase2Scetch2D.ksArcBy3Points(-40, radius * 1.75, 0, radius * 1.76, 40, radius * 1.75, 1);

            ksPaseSketchDef2.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutPase2Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutPase2Def = CutPase2Extr.GetDefinition();
            ksExtrusionParam cutPase2Prop = (ksExtrusionParam)CutPase2Def.ExtrusionParam();

            if (cutPase2Prop != null)
            {
                CutPase2Def.SetSketch(ksPaseSketchDef2); // эскиз операции выдавливания
                                                         // направление выдавливания
                cutPase2Prop.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                cutPase2Prop.typeReverse = (short)End_Type.etBlind;
                cutPase2Prop.depthReverse = 40; // глубина выдавливания
                CutPase2Extr.Create(); // создадим операцию
            }

            // операция массив вращения
            ksEntity Pase2Copy = part.NewEntity((short)Obj3dType.o3d_circularCopy);
            // получаем свойства кругового массива
            CircularCopyDefinition Pase2CopyDef = Pase2Copy.GetDefinition();
            // выставляем базовую ось
            Pase2CopyDef.SetAxis(baseAxisX);
            // значение для кругового массива, 3 копии 120 градусов
            Pase2CopyDef.SetCopyParamAlongDir(16, 22.5, false, false);
            // создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection2 = Pase2CopyDef.GetOperationArray();
            // очищаем её
            EntityCollection2.Clear();
            // добавляем элемент выдавливания в коллекци.
            EntityCollection2.Add(CutPase2Extr);
            // создаём массив
            Pase2Copy.Create();



            // смещаем плоскость от базовой ZOY по оси X на 2570
            ksEntity basePlane2Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef2 = basePlane2Offset.GetDefinition();
            offsetPlaneDef2.direction = true;
            offsetPlaneDef2.offset = 2570;
            offsetPlaneDef2.SetPlane(basePlaneZOY);
            basePlane2Offset.Create();


            //отверстие сверху цилиндра узкая часть
            ksEntity ksCircle3Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksCircleDef3 = ksCircle3Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksCircleDef3.SetPlane(basePlane2Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle3Entity.Create(); // создадим эскиз
            ksDocument2D Circle3Scetch2D = (ksDocument2D)ksCircleDef3.BeginEdit(); // начинаем редактирование эскиза

            Circle3Scetch2D.ksCircle(0, 0, 40, 1);

            ksCircleDef3.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle3Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutCircle3Def = CutCircle3Extr.GetDefinition();
            ksExtrusionParam cutCircle3Prop = (ksExtrusionParam)CutCircle3Def.ExtrusionParam();

            if (cutCircle3Prop != null)
            {
                CutCircle3Def.SetSketch(ksCircleDef3); // эскиз операции выдавливания
                                                       // направление выдавливания
                cutCircle3Prop.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                cutCircle3Prop.typeNormal = (short)End_Type.etBlind;
                cutCircle3Prop.depthNormal = 200; // глубина выдавливания
                CutCircle3Extr.Create(); // создадим операцию
            }

            //отверстие сверху цилиндра широкая часть
            ksEntity ksCircle4Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksCircleDef4 = ksCircle4Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksCircleDef4.SetPlane(basePlane2Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle4Entity.Create(); // создадим эскиз
            ksDocument2D Circle4Scetch2D = (ksDocument2D)ksCircleDef4.BeginEdit(); // начинаем редактирование эскиза


            Circle4Scetch2D.ksCircle(0, 0, 57.5, 1);

            ksCircleDef4.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutCircle4Extr = part.NewEntity((short)Obj3dType.o3d_cutExtrusion); // создаём объект выдавливания
            ksCutExtrusionDefinition CutCircle4Def = CutCircle4Extr.GetDefinition();
            ksExtrusionParam cutCircle4Prop = (ksExtrusionParam)CutCircle4Def.ExtrusionParam();

            if (cutCircle4Prop != null)
            {
                CutCircle4Def.SetSketch(ksCircleDef4); // эскиз операции выдавливания
                                                       // направление выдавливания
                cutCircle4Prop.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                cutCircle4Prop.typeNormal = (short)End_Type.etBlind;
                cutCircle4Prop.depthNormal = 50; // глубина выдавливания
                CutCircle4Extr.Create(); // создадим операцию
            }

            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Главный цилиндр.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }

    }
}
