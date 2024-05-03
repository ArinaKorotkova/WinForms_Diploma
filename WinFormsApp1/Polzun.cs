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
    internal class Polzun : BasePart
    {
        private readonly double diameter;

        public Polzun(double D)
        {
            diameter = D;
        }


        //Деталь 7 - Ползун
        public override string CreatePart(string partName = null)
        {
            //if (!File.Exists(Path.Combine(folderPath, "Ползун пресса.m3d")))
            //{
            //    return Path.Combine(folderPath, "Ползун пресса.m3d");
            //}

            CreateNew("Ползун пресса");

            var radius = diameter / 2;

            //Эскиз 1
            ksEntity ksRect1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. 
            SketchDefinition ksRect1ScetchDef = ksRect1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. 
            ksRect1ScetchDef.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksRect1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect1Sketch = (ksDocument2D)ksRect1ScetchDef.BeginEdit(); // начинаем редактирование эскиза. 
            ksRectangleParam Rect1Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            Rect1Param.x = 0; // координата х одной из вершин прямоугольника
            Rect1Param.y = 0; // координата y одной из вершин прямоугольника
            Rect1Param.height = radius * 1.43; // высота
            Rect1Param.width = radius * 2.075; // ширина
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
                extrProp1.depthNormal = 620; // глубина выдавливания
                bossExtr1.Create(); // создадим операцию
            }


            //смещаем плоскость XOZ на 620
            ksEntity basePlane1Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef1 = basePlane1Offset.GetDefinition();
            offsetPlaneDef1.direction = true;
            offsetPlaneDef1.offset = 620;
            offsetPlaneDef1.SetPlane(basePlaneXOZ);
            basePlane1Offset.Create();


            //Эскиз 2 - первое вырезание
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, 0, 0, radius * 1.26, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(0, radius * 1.26, radius * 0.67, radius * 1.26, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 0.67, radius * 1.26, radius * 1.74, radius * 0.2, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 1.74, radius * 0.2, radius * 1.74, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 1.74, 0, 0, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef1.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutScetch1 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch1 = CutScetch1.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch1 = (ksExtrusionParam)CutDefScetch1.ExtrusionParam();

            if (CutPropScetch1 != null)
            {
                // эскиз для вырезания
                CutDefScetch1.SetSketch(ksScetchDef1);
                // направление вырезания (обратное)
                CutPropScetch1.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropScetch1.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch1.depthNormal = 70;
                // создадим операцию

                CutScetch1.Create();
            }


            //Эскиз 3 - второе вырезание
            ksEntity ksScetch2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef2 = ksScetch2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef2.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch2Entity.Create(); // создадим эскиз
            ksDocument2D Scetch22D = (ksDocument2D)ksScetchDef2.BeginEdit(); // начинаем редактирование эскиза


            Scetch22D.ksLineSeg(radius * 1.204, radius * 1.45, radius * 1.445, radius * 1.208, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(radius * 1.445, radius * 1.208, radius * 1.487, radius * 1.25, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(radius * 1.487, radius * 1.25, radius * 1.887, radius * 0.849, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(radius * 1.887, radius * 0.849, radius * 1.845, radius * 0.808, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(radius * 1.845, radius * 0.808, radius * 2.075, radius * 0.58, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(radius * 2.075, radius * 0.58, radius * 2.075, radius * 1.45, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(radius * 2.075, radius * 1.45, radius * 1.204, radius * 1.45, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

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
                CutPropScetch2.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropScetch2.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch2.depthNormal = 620;
                // создадим операцию

                CutScetch2.Create();
            }


            //Эскиз 4 - первое выдавливание
            ksEntity ksScetch3Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef3 = ksScetch3Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef3.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch3Entity.Create(); // создадим эскиз
            ksDocument2D Scetch32D = (ksDocument2D)ksScetchDef3.BeginEdit(); // начинаем редактирование эскиза


            Scetch32D.ksLineSeg(radius * 1.087, radius * 0.849, radius * 1.487, radius * 1.25, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch32D.ksLineSeg(radius * 1.487, radius * 1.25, radius * 1.887, radius * 0.849, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch32D.ksLineSeg(radius * 1.887, radius * 0.849, radius * 1.487, radius * 0.45, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch32D.ksLineSeg(radius * 1.487, radius * 0.45, radius * 1.087, radius * 0.849, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef3.EndEdit(); // заканчиваем редактирование эскиза

            //Выдавливание базовой части корпуса
            ksEntity bossExtr2 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef2 = bossExtr2.GetDefinition();
            ksExtrusionParam extrProp2 = (ksExtrusionParam)ExtrDef2.ExtrusionParam();

            if (extrProp2 != null)
            {
                ExtrDef2.SetSketch(ksScetch3Entity); // эскиз операции выдавливания
                                                     // направление выдавливания (обычное)
                extrProp2.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                extrProp2.typeNormal = (short)End_Type.etBlind;
                extrProp2.depthNormal = 180; // глубина выдавливания
                bossExtr2.Create(); // создадим операцию
            }


            // создаём объект плоскости под углом
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
            PlaneAngleDef1.angle = 45;
            // создаём плоскость
            PlaneAngle1.Create();

            ksEntity basePlane3Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef3 = basePlane3Offset.GetDefinition();
            offsetPlaneDef3.direction = false;
            offsetPlaneDef3.offset =radius * 1.3686;
            offsetPlaneDef3.SetPlane(PlaneAngle1);
            basePlane3Offset.Create();



            //Эскиз 5 - Создаем эскиз первого треугольника с одной стороны куба
            ksEntity ksScetch4Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef4 = ksScetch4Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef4.SetPlane(basePlane3Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch4Entity.Create(); // создадим эскиз
            ksDocument2D Scetch42D = (ksDocument2D)ksScetchDef4.BeginEdit(); // начинаем редактирование эскиза


            Scetch42D.ksLineSeg(radius * 0.734, 620, radius * 1.087, 620, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch42D.ksLineSeg(radius * 1.087, 620, radius * 0.734, 800, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch42D.ksLineSeg(radius * 0.734, 800, radius * 0.734, 620, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef4.EndEdit();


            ksEntity basePlane4Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef4 = basePlane4Offset.GetDefinition();
            offsetPlaneDef4.direction = false;
            offsetPlaneDef4.offset = radius * 0.51;
            offsetPlaneDef4.SetPlane(basePlane3Offset);
            basePlane4Offset.Create();

            //Эскиз 6 - Создаем эскиз второго треугольника с одной стороны куба
            ksEntity ksScetch5Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef5 = ksScetch5Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef5.SetPlane(basePlane4Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch5Entity.Create(); // создадим эскиз
            ksDocument2D Scetch52D = (ksDocument2D)ksScetchDef5.BeginEdit(); // начинаем редактирование эскиза


            Scetch52D.ksLineSeg(radius * 0.734, 620, radius * 1.057, 620, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch52D.ksLineSeg(radius * 1.057, 620, radius * 0.734, 800, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch52D.ksLineSeg(radius * 0.734, 800, radius * 0.734, 620, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef5.EndEdit();


            //Эскиз 7 - Создаем эскиз первого треугольника с другой стороны куба
            ksEntity ksScetch6Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef6 = ksScetch6Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef6.SetPlane(basePlane3Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch6Entity.Create(); // создадим эскиз
            ksDocument2D Scetch62D = (ksDocument2D)ksScetchDef6.BeginEdit(); // начинаем редактирование эскиза


            Scetch62D.ksLineSeg(radius * 0.17, 620, -radius * 0.418, 620, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch62D.ksLineSeg(-radius * 0.418, 620, radius * 0.17, 800, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch62D.ksLineSeg(radius * 0.17, 800, radius * 0.17, 620, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef6.EndEdit();

            //Эскиз 8 - Создаем эскиз второго треугольника с другой стороны куба
            ksEntity ksScetch7Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef7 = ksScetch7Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef7.SetPlane(basePlane4Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch7Entity.Create(); // создадим эскиз
            ksDocument2D Scetch72D = (ksDocument2D)ksScetchDef7.BeginEdit(); // начинаем редактирование эскиза


            Scetch72D.ksLineSeg(radius * 0.17, 620, -radius * 0.15, 620, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch72D.ksLineSeg(-radius * 0.15, 620, radius * 0.17, 800, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch72D.ksLineSeg(radius * 0.17, 800, radius * 0.17, 620, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef7.EndEdit();


            //Два лофта по эскизам: 1) 5 и 6, 2) 7 и 8
            ksEntity BossLoft1 = part.NewEntity((short)Obj3dType.o3d_bossLoft); // создаём объект выдавливания
            ksBossLoftDefinition BossLoftDef1 = BossLoft1.GetDefinition();

            ksEntityCollection entCol1 = BossLoftDef1.Sketchs();
            // очищаем коллекцию
            entCol1.Clear();
            // устанавливаем первый эскиз
            entCol1.Add(ksScetch4Entity);
            // устанавливаем второй эскиз
            entCol1.Add(ksScetch5Entity);
            BossLoft1.Create();


            ksEntity BossLoft2 = part.NewEntity((short)Obj3dType.o3d_bossLoft); // создаём объект выдавливания
            ksBossLoftDefinition BossLoftDef2 = BossLoft2.GetDefinition();

            ksEntityCollection entCol2 = BossLoftDef2.Sketchs();
            // очищаем коллекцию
            entCol2.Clear();
            // устанавливаем первый эскиз
            entCol2.Add(ksScetch6Entity);
            // устанавливаем второй эскиз
            entCol2.Add(ksScetch7Entity);
            BossLoft2.Create();


            //Два зеркалирования
            //1. относительно плоскости ZOY
            ksEntity MirrorCopyPart1 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart1Def = MirrorCopyPart1.GetDefinition();

            if (MirrorCopyPart1Def != null)
            {
                ksEntityCollection EntityCollection1 = MirrorCopyPart1Def.GetOperationArray();
                EntityCollection1.Clear();
                EntityCollection1.Add(bossExtr1);
                EntityCollection1.Add(bossExtr2);
                EntityCollection1.Add(CutScetch1);
                EntityCollection1.Add(CutScetch2);
                EntityCollection1.Add(BossLoft1);
                EntityCollection1.Add(BossLoft2);

                MirrorCopyPart1Def.SetPlane(basePlaneZOY);
                MirrorCopyPart1.Create();
            }

            //2. относительно плоскости XOY
            ksEntity MirrorCopyPart2 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart2Def = MirrorCopyPart2.GetDefinition();

            if (MirrorCopyPart2Def != null)
            {
                ksEntityCollection EntityCollection2 = MirrorCopyPart2Def.GetOperationArray();
                EntityCollection2.Clear();
                EntityCollection2.Add(MirrorCopyPart1);


                MirrorCopyPart2Def.SetPlane(basePlaneXOY);
                MirrorCopyPart2.Create();
            }


            //После всех зеркалирований двигаем плоскость относительно базовой XOZ на 550
            ksEntity basePlane5Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef5 = basePlane5Offset.GetDefinition();
            offsetPlaneDef5.direction = true;
            offsetPlaneDef5.offset = 550;
            offsetPlaneDef5.SetPlane(basePlaneXOZ);
            basePlane5Offset.Create();


            //Эскиз 9 - выдавливание окружности диаметром 520 на 50
            ksEntity ksCircle1ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle1ScetchDef = ksCircle1ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle1ScetchDef.SetPlane(basePlane5Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle1ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle1Sketch = (ksDocument2D)ksCircle1ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle1Sketch.ksCircle(0, 0, diameter / 2 * 0.98, 1);

            ksCircle1ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity bossExtr3 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef3 = bossExtr3.GetDefinition();
            ksExtrusionParam extrProp3 = (ksExtrusionParam)ExtrDef3.ExtrusionParam();

            if (extrProp3 != null)
            {
                ExtrDef3.SetSketch(ksCircle1ScetchEntity); // эскиз операции выдавливания
                                                           // направление выдавливания (обычное)
                extrProp3.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                extrProp3.typeNormal = (short)End_Type.etBlind;
                extrProp3.depthNormal = 50; // глубина выдавливания
                bossExtr3.Create(); // создадим операцию
            }


            ksEntityCollection ksEntityCollection71 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection71.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection71.GetByIndex(q);
                ksFaceDefinition def1 = part1.GetDefinition();

                if (def1.GetOwnerEntity() == bossExtr3)
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

                                if (d1.IsCircle())
                                {
                                    ksVertexDefinition p1 = d1.GetVertex(true);

                                    double x1, y1, z1;

                                    p1.GetPoint(out x1, out y1, out z1);

                                    if (Math.Abs(x1 - diameter / 2 * 0.98) <= 0.1 && Math.Abs(y1 - 600) <= 0.1 && Math.Abs(z1) <= 0.1)
                                    {
                                        part1.name = ("Polzun_centr_Circle");
                                        part1.Update();
                                        break;

                                    }
                                }

                            }
                        }
                    }
                }
            }

            ksEntityCollection ksEntityCollection72 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection72.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection72.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == bossExtr3)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == diameter / 2 * 0.98)
                        {
                            part1.name = "Polzun_centr_Cylinder";
                            part1.Update();
                        }
                    }
                }
            }


            ksEntity ksCircle2ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle2ScetchDef = ksCircle2ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle2ScetchDef.SetPlane(basePlane5Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle2ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle2Sketch = (ksDocument2D)ksCircle2ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы
            
            Circle2Sketch.ksCircle(diameter / 2 * 0.42, diameter / 2 * 0.72, 16, 1);
            
            ksCircle2ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutScetch3 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch3 = CutScetch3.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch3 = (ksExtrusionParam)CutDefScetch3.ExtrusionParam();

            if (CutPropScetch3 != null)
            {
                // эскиз для вырезания
                CutDefScetch3.SetSketch(ksCircle2ScetchEntity);
                // направление вырезания (обратное)
                CutPropScetch3.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch3.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch3.depthReverse = 60;
                // создадим операцию

                CutScetch3.Create();
            }


            ksEntityCollection ksEntityCollection73 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection73.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection73.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == CutScetch3)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == 16)
                        {
                            part1.name = "CylinderD32_Polzun";
                            part1.Update();
                        }
                    }
                }
            }


            //Эскиз 10 - вырезание окружностей по центру 1 - диаметром 48 и 4 по 32
            ksEntity ksCircle21ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle21ScetchDef = ksCircle21ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle21ScetchDef.SetPlane(basePlane5Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle21ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle21Sketch = (ksDocument2D)ksCircle21ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle21Sketch.ksCircle(0, 0, 24, 1);

            Circle21Sketch.ksCircle(diameter / 2 * 0.42, -diameter / 2 * 0.72, 16, 1);
            Circle21Sketch.ksCircle(-diameter / 2 * 0.42, diameter / 2 * 0.72, 16, 1);
            Circle21Sketch.ksCircle(-diameter / 2 * 0.42, -diameter / 2 * 0.72, 16, 1);

            ksCircle21ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutScetch41 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch41 = CutScetch41.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch41 = (ksExtrusionParam)CutDefScetch41.ExtrusionParam();

            if (CutPropScetch41 != null)
            {
                // эскиз для вырезания
                CutDefScetch41.SetSketch(ksCircle21ScetchEntity);
                // направление вырезания (обратное)
                CutPropScetch41.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch41.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch41.depthReverse = 60;
                // создадим операцию

                CutScetch41.Create();
            }



            //Эскиз 10 - вырезание квадратной области под поперечину
            ksEntity ksRect2ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. 
            SketchDefinition ksRect2ScetchDef = ksRect2ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. 
            ksRect2ScetchDef.SetPlane(basePlaneZOY); // установим плоскость XOZ базовой для эскиза
            ksRect2ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Rect2Sketch = (ksDocument2D)ksRect2ScetchDef.BeginEdit(); // начинаем редактирование эскиза. 
            ksRectangleParam Rect2Param = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            Rect2Param.x = -radius * 0.471; // координата х одной из вершин прямоугольника
            Rect2Param.y = -250; // координата y одной из вершин прямоугольника
            Rect2Param.height = 250; // высота
            Rect2Param.width = diameter * 0.471; // ширина
            Rect2Param.ang = 0; // угол поворота
            Rect2Param.style = 1; // стиль линии
                                  // отправляем интерфейс параметров в функцию создания

            Rect2Sketch.ksRectangle(Rect2Param);
            ksRect2ScetchDef.EndEdit(); // заканчиваем редактирование эскиза


            ksEntity CutScetch4 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch4 = CutScetch4.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch4 = (ksExtrusionParam)CutDefScetch4.ExtrusionParam();

            if (CutPropScetch4 != null)
            {
                // эскиз для вырезания
                CutDefScetch4.SetSketch(ksRect2ScetchEntity);
                // направление вырезания (обратное)
                CutPropScetch4.direction = (short)Direction_Type.dtMiddlePlane;
                // тип вырезания (строго на глубину)
                CutPropScetch4.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch4.depthNormal = radius * 4.15;
                // создадим операцию

                CutScetch4.Create();
            }

            ksEntityCollection ksEntityCollection75 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection75.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection75.GetByIndex(q);
                ksFaceDefinition def1 = part1.GetDefinition();

                if (def1.GetOwnerEntity() == CutScetch4)
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


                                if (Math.Abs(x1 - radius * 2.075) <= 0.1 && Math.Abs(y1 - 250) <= 0.1 && Math.Abs(z1 - radius * 0.471) <= 0.1)
                                {
                                    if (Math.Abs(x2 + radius * 2.075) <= 0.1 && Math.Abs(y2 - 250) <= 0.1 && Math.Abs(z2 + radius * 0.471) <= 0.1)
                                    {
                                        part1.name = ("Plane_Nzhnaya_Polzun");
                                        part1.Update();
                                        break;
                                    }

                                }
                            }

                        }
                    }
                }

            }


            ksEntityCollection ksEntityCollection74 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection74.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection74.GetByIndex(q);
                ksFaceDefinition def1 = part1.GetDefinition();

                if (def1.GetOwnerEntity() == CutScetch4)
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


                                if (Math.Abs(x1 + radius * 2.075) <= 0.1 && Math.Abs(y1 - 0) <= 0.1 && Math.Abs(z1 + radius * 0.471) <= 0.1)
                                {
                                    if (Math.Abs(x2 - radius * 2.075) <= 0.1 && Math.Abs(y2 - 250) <= 0.1 && Math.Abs(z2 + radius * 0.471) <= 0.1)
                                    {
                                        part1.name = ("Plane_Bokovaya_Polzun");
                                        part1.Update();
                                        break;
                                    }

                                }
                            }

                        }
                    }
                }
                
            }


            //Эскиз 11.1 - вырезание отверстий под крепление поперечины диаметро 50 на расстоянии 180
            ksEntity ksCircle3ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle3ScetchDef = ksCircle3ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle3ScetchDef.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksCircle3ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle3Sketch = (ksDocument2D)ksCircle3ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle3Sketch.ksCircle(0, 0, 25, 1);
            ksCircle3ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutScetch5 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch5 = CutScetch5.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch5 = (ksExtrusionParam)CutDefScetch5.ExtrusionParam();

            if (CutPropScetch5 != null)
            {
                // эскиз для вырезания
                CutDefScetch5.SetSketch(ksCircle3ScetchEntity);
                // направление вырезания (обратное)
                CutPropScetch5.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch5.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch5.depthReverse = 336;
                // создадим операцию

                CutScetch5.Create();
            }

            ksEntityCollection ksEntityCollection76 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection76.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection76.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == CutScetch5)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == 25)
                        {
                            part1.name = "CylinderNizhOtv_Polzun";
                            part1.Update();
                        }
                    }
                }
            }

            //Эскиз 11.2 - вырезание отверстий под крепление поперечины диаметром 50 на расстоянии 180
            ksEntity ksCircle31ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle31ScetchDef = ksCircle31ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle31ScetchDef.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksCircle31ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle31Sketch = (ksDocument2D)ksCircle31ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle31Sketch.ksCircle(radius * 0.679, 0, 25, 1);
            Circle31Sketch.ksCircle(radius * 0.679 * 2, 0, 25, 1);
            Circle31Sketch.ksCircle(-radius * 0.679, 0, 25, 1);
            Circle31Sketch.ksCircle(-radius * 0.679 * 2, 0, 25, 1);

            ksCircle31ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutScetch31 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch31 = CutScetch31.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch31 = (ksExtrusionParam)CutDefScetch31.ExtrusionParam();

            if (CutPropScetch31 != null)
            {
                // эскиз для вырезания
                CutDefScetch31.SetSketch(ksCircle31ScetchEntity);
                // направление вырезания (обратное)
                CutPropScetch31.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch31.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch31.depthReverse = 336;
                // создадим операцию

                CutScetch31.Create();
            }

            //Создаем вырезы под крепления накладок

            ksEntity basePlane6Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef6 = basePlane6Offset.GetDefinition();
            offsetPlaneDef6.direction = false;
            offsetPlaneDef6.offset = 15;
            offsetPlaneDef6.SetPlane(basePlane4Offset);
            basePlane6Offset.Create();

            ksEntity ksCircle4ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle4ScetchDef = ksCircle4ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle4ScetchDef.SetPlane(basePlane6Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle4ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle4Sketch = (ksDocument2D)ksCircle4ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle4Sketch.ksCircle(radius * 0.583, 740, 8, 1);
            Circle4Sketch.ksCircle(radius * 0.319, 660, 8, 1);


            ksCircle4ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutScetch6 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch6 = CutScetch6.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch6 = (ksExtrusionParam)CutDefScetch6.ExtrusionParam();

            if (CutPropScetch6 != null)
            {
                // эскиз для вырезания
                CutDefScetch6.SetSketch(ksCircle4ScetchEntity);
                // направление вырезания (обратное)
                CutPropScetch6.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch6.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch6.depthReverse = 30;
                // создадим операцию

                CutScetch6.Create();
            }

            // операция по сетке
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
            MeshCopyDef1.step1 = -150;
            //создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection3 = MeshCopyDef1.OperationArray();
            EntityCollection3.Clear(); // очищаем её
                                       //добавляем элемент выдавливания в коллекци.
            EntityCollection3.Add(CutScetch6);
            //добавляем элемент выдавливания в коллекци.
            MeshCopy1.Create(); // создаём массив

            ksEntity circCopy1 = part.NewEntity((short)Obj3dType.o3d_circularCopy);
            // получаем свойства кругового массива
            CircularCopyDefinition circCopyDef1 = circCopy1.GetDefinition();
            // создаём ось вращения на основе базовой
            ksEntity baseAxisY2 = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOY);
            // выставляем базовую ось
            circCopyDef1.SetAxis(baseAxisY2);
            // значение для кругового массива, 3 копии 120 градусов
            circCopyDef1.SetCopyParamAlongDir(2, 180, false, false);
            // создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection4 = circCopyDef1.GetOperationArray();
            // очищаем её
            EntityCollection4.Clear();
            // добавляем элемент выдавливания в коллекци.
            EntityCollection4.Add(MeshCopy1);
            // создаём массив
            circCopy1.Create();


            // создаём объект плоскости под углом
            ksEntity PlaneAngle2 = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeAngle);
            // получаем интерфейс параметров плоскости под углом
            ksPlaneAngleDefinition PlaneAngleDef2 = PlaneAngle2.GetDefinition();
            ksEntity Axis2 = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOY);
            // настраиваем параметры
            // устанавливаем прямую или ось, через которую проходит плоскость
            PlaneAngleDef2.SetAxis(Axis2);
            // устанавливаем базовую плоскость
            PlaneAngleDef2.SetPlane(basePlaneZOY);
            // устанавливаем угол наклона плоскости
            PlaneAngleDef2.angle = -45;
            // создаём плоскость
            PlaneAngle2.Create();

            ksEntity basePlane7Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef7 = basePlane7Offset.GetDefinition();
            offsetPlaneDef7.direction = false;
            offsetPlaneDef7.offset = radius * 1.9345;
            offsetPlaneDef7.SetPlane(PlaneAngle2);
            basePlane7Offset.Create();

            ksEntity ksCircle5ScetchEntity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза. указываем имя всего эскизы
            SketchDefinition ksCircle5ScetchDef = ksCircle5ScetchEntity.GetDefinition(); // получаем интерфейс свойств эскиза. указываем имя для свойств эскизы
            ksCircle5ScetchDef.SetPlane(basePlane7Offset); // установим плоскость XOZ базовой для эскиза
            ksCircle5ScetchEntity.Create(); // создадим эскиз

            ksDocument2D Circle5Sketch = (ksDocument2D)ksCircle5ScetchDef.BeginEdit(); // начинаем редактирование эскиза. указываем название элементов эскизы

            Circle5Sketch.ksCircle(-radius * 0.583, 740, 8, 1);
            Circle5Sketch.ksCircle(-radius * 0.319, 660, 8, 1);


            ksCircle5ScetchDef.EndEdit(); // заканчиваем редактирование эскиза

            ksEntity CutScetch7 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            // получаем интерфейс определения вырезания
            ksCutExtrusionDefinition CutDefScetch7 = CutScetch7.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch7 = (ksExtrusionParam)CutDefScetch7.ExtrusionParam();

            if (CutPropScetch7 != null)
            {
                // эскиз для вырезания
                CutDefScetch7.SetSketch(ksCircle5ScetchEntity);
                // направление вырезания (обратное)
                CutPropScetch7.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch7.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch7.depthReverse = 30;
                // создадим операцию

                CutScetch7.Create();
            }

            // операция по сетке
            ksEntity MeshCopy2 = part.NewEntity((short)Obj3dType.o3d_meshCopy);
            //создаём интерфейс свойств линейного массива
            MeshCopyDefinition MeshCopyDef2 = MeshCopy2.GetDefinition();
            //создаём ось линейного массива на основе базовой Y
            ksEntity baseAxisY3 = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOY);
            //выставляем базовую ось
            MeshCopyDef2.SetAxis1(baseAxisY3);
            // количество элементов копирования вдоль первой оси
            MeshCopyDef2.count1 = 5;
            // шаг копирования вдоль первой оси
            MeshCopyDef2.step1 = -150;
            //создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection5 = MeshCopyDef2.OperationArray();
            EntityCollection5.Clear(); // очищаем её
                                       //добавляем элемент выдавливания в коллекци.
            EntityCollection5.Add(CutScetch7);
            //добавляем элемент выдавливания в коллекци.
            MeshCopy2.Create(); // создаём массив

            ksEntity circCopy2 = part.NewEntity((short)Obj3dType.o3d_circularCopy);
            // получаем свойства кругового массива
            CircularCopyDefinition circCopyDef2 = circCopy2.GetDefinition();
            // создаём ось вращения на основе базовой
            ksEntity baseAxisY4 = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOY);
            // выставляем базовую ось
            circCopyDef2.SetAxis(baseAxisY4);
            // значение для кругового массива, 3 копии 120 градусов
            circCopyDef2.SetCopyParamAlongDir(2, 180, false, false);
            // создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection6 = circCopyDef2.GetOperationArray();
            // очищаем её
            EntityCollection6.Clear();
            // добавляем элемент выдавливания в коллекци.
            EntityCollection6.Add(MeshCopy2);
            // создаём массив
            circCopy2.Create();


            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Ползун пресса.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
