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
    internal class Zadvizhka : BasePart
    {
        //Деталь 24 - Задвижка
        public override string CreatePart(string partName = null)
        {
            CreateNew("Задвижка");

            //Эскиз 1 - основание 
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, 0, 120, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(120, 0, 96.5, 40.7, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(96.5, 40.7, 23.5, 40.7, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(23.5, 40.7, 0, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef1.EndEdit();

            ksEntity bossExtr1 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef1 = bossExtr1.GetDefinition();
            ksExtrusionParam extrProp1 = (ksExtrusionParam)ExtrDef1.ExtrusionParam();

            if (extrProp1 != null)
            {
                ExtrDef1.SetSketch(ksScetchDef1); // эскиз операции выдавливания
                                                  // направление выдавливания (обычное)
                extrProp1.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                extrProp1.typeReverse = (short)End_Type.etBlind;
                extrProp1.depthReverse = 170; // глубина выдавливания
                bossExtr1.Create(); // создадим операцию
            }



            //Эскиз 2 - ушки 
            ksEntity ksScetch2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef2 = ksScetch2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef2.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch2Entity.Create(); // создадим эскиз
            ksDocument2D Scetch22D = (ksDocument2D)ksScetchDef2.BeginEdit(); // начинаем редактирование эскиза

            Scetch22D.ksLineSeg(0, 0, 23.5, 40.7, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(23.5, 40.7, -20.5, 40.7, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(-20.5, 40.7, -20.5, 20.7, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksArcBy3Points(-20.5, 20.7, -14.5, 6.15, 0, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)


            Scetch22D.ksLineSeg(120, 0, 96.5, 40.7, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(96.5, 40.7, 140.5, 40.7, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksLineSeg(140.5, 40.7, 140.5, 20.7, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksArcBy3Points(140.5, 20.7, 134.5, 6.15, 120, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef2.EndEdit();

            ksEntity bossExtr2 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef2 = bossExtr2.GetDefinition();
            ksExtrusionParam extrProp2 = (ksExtrusionParam)ExtrDef2.ExtrusionParam();

            if (extrProp2 != null)
            {
                ExtrDef2.SetSketch(ksScetchDef2); // эскиз операции выдавливания
                                                  // направление выдавливания (обычное)
                extrProp2.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                extrProp2.typeNormal = (short)End_Type.etBlind;
                extrProp2.depthNormal = 40; // глубина выдавливания
                bossExtr2.Create(); // создадим операцию
            }

            //Эскиз 3
            ksEntity ksScetch3Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef3 = ksScetch3Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef3.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch3Entity.Create(); // создадим эскиз
            ksDocument2D Scetch32D = (ksDocument2D)ksScetchDef3.BeginEdit(); // начинаем редактирование эскиза

            Scetch32D.ksLineSeg(0, 0, 120, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch32D.ksLineSeg(120, 0, 96.5, 40.7, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch32D.ksLineSeg(96.5, 40.7, 23.5, 40.7, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch32D.ksLineSeg(23.5, 40.7, 0, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef3.EndEdit();

            ksEntity bossExtr3 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef3 = bossExtr3.GetDefinition();
            ksExtrusionParam extrProp3 = (ksExtrusionParam)ExtrDef3.ExtrusionParam();

            if (extrProp3 != null)
            {
                ExtrDef3.SetSketch(ksScetchDef3); // эскиз операции выдавливания
                                                  // направление выдавливания (обычное)
                extrProp3.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                extrProp3.typeNormal = (short)End_Type.etBlind;
                extrProp3.depthNormal = 40; // глубина выдавливания
                bossExtr3.Create(); // создадим операцию
            }

            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Задвижка.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
