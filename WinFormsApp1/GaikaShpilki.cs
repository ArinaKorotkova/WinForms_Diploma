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
    internal class GaikaShpilki : BasePart
    {
        //Деталь 25 - Гайка шпильки
        public override string CreatePart(string partName = null)
        {
            CreateNew("Гайка Шпильки");

            //Эскиз 1 - основание 
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, 0, 10, 0, 3); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(0, -105, 0, -155, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(0, -155, 260, -155, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(260, -155, 260, -105, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(260, -105, 230, -105, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(230, -105, 230, -100, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(230, -100, 30, -100, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(30, -100, 30, -105, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(30, -105, 0, -105, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef1.EndEdit();

            ksEntity RotatedBase1 = part.NewEntity((int)Obj3dType.o3d_bossRotated);
            // получаем интерфейс операции вращения
            ksBossRotatedDefinition RotateDef1 = RotatedBase1.GetDefinition();
            RotateDef1.directionType = (short)Direction_Type.dtNormal;
            // настройки вращения (направление вращения, угол вращения) true - прямое, false - обратное
            RotateDef1.SetSideParam(true, 360);
            // устанавливаем эскиз вращения
            RotateDef1.SetSketch(ksScetch1Entity);
            RotatedBase1.Create(); // создаём операцию


            ksEntity basePlane1Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef1 = basePlane1Offset.GetDefinition();
            offsetPlaneDef1.direction = false;
            offsetPlaneDef1.offset = 260;
            offsetPlaneDef1.SetPlane(basePlaneZOY);
            basePlane1Offset.Create();

            //Эскиз 2 - отверстие диаметром 30
            ksEntity ksScetch2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef2 = ksScetch2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef2.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch2Entity.Create(); // создадим эскиз
            ksDocument2D Scetch22D = (ksDocument2D)ksScetchDef2.BeginEdit(); // начинаем редактирование эскиза

            Scetch22D.ksCircle(130, 0, 15, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            ksScetchDef2.EndEdit();

            ksEntity CutScetch2 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            ksCutExtrusionDefinition CutDefScetch2 = CutScetch2.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch2 = (ksExtrusionParam)CutDefScetch2.ExtrusionParam();

            if (CutPropScetch2 != null)
            {
                // эскиз для вырезания
                CutDefScetch2.SetSketch(ksScetch2Entity);
                // направление вырезания (обратное)
                CutPropScetch2.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch2.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch2.depthReverse = 45;
                // создадим операцию

                CutScetch2.Create();

            }


            ksEntity basePlane2Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef2 = basePlane2Offset.GetDefinition();
            offsetPlaneDef2.direction = false;
            offsetPlaneDef2.offset = 155;
            offsetPlaneDef2.SetPlane(basePlaneXOY);
            basePlane2Offset.Create();

            //Эскиз 2 - отверстие диаметром 30
            ksEntity ksScetch3Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef3 = ksScetch3Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef3.SetPlane(basePlane2Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch3Entity.Create(); // создадим эскиз
            ksDocument2D Scetch32D = (ksDocument2D)ksScetchDef3.BeginEdit(); // начинаем редактирование эскиза

            Scetch32D.ksCircle(40, 0, 15, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            ksScetchDef3.EndEdit();


            ksEntity CutScetch3 = part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            ksCutExtrusionDefinition CutDefScetch3 = CutScetch3.GetDefinition();
            // получаем интерфейс настроек вырезания
            ksExtrusionParam CutPropScetch3 = (ksExtrusionParam)CutDefScetch3.ExtrusionParam();

            if (CutPropScetch3 != null)
            {
                // эскиз для вырезания
                CutDefScetch3.SetSketch(ksScetch3Entity);
                // направление вырезания (обратное)
                CutPropScetch3.direction = (short)Direction_Type.dtReverse;
                // тип вырезания (строго на глубину)
                CutPropScetch3.typeReverse = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch3.depthReverse = 45;
                // создадим операцию

                CutScetch3.Create();

            }

            ksEntity MirrorCopyPart1 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart1Def = MirrorCopyPart1.GetDefinition();
            if (MirrorCopyPart1Def != null)
            {
                ksEntityCollection EntityCollection = MirrorCopyPart1Def.GetOperationArray();
                EntityCollection.Clear();
                EntityCollection.Add(CutScetch3);
                MirrorCopyPart1Def.SetPlane(basePlaneXOY);
                MirrorCopyPart1.Create();
            }


            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Гайка шпильки.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
