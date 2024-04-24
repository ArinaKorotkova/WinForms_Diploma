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
    internal class BoltLapa : BasePart
    {
        //Деталь 13 - Болт-лапа
        public override string CreatePart(string partName = null)
        {
            CreateNew("Болт-Лапа");

            //Эскиз 1 - основание 
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, 40, 0, -40, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(0, -40, 132.5, -40, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(132.5, -40, 162.5, -19, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(162.5, -19, 162.5, 19, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(162.5, 19, 132.5, 40, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(132.5, 40, 0, 40, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

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
                extrProp1.depthReverse = 60; // глубина выдавливания
                bossExtr1.Create(); // создадим операцию
            }

            ksEntity basePlane1Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef1 = basePlane1Offset.GetDefinition();
            offsetPlaneDef1.direction = false;
            offsetPlaneDef1.offset = 60;
            offsetPlaneDef1.SetPlane(basePlaneXOZ);
            basePlane1Offset.Create();


            ksEntity ksScetch2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef2 = ksScetch2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef2.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch2Entity.Create(); // создадим эскиз
            ksDocument2D Scetch22D = (ksDocument2D)ksScetchDef2.BeginEdit(); // начинаем редактирование эскиза

            Scetch22D.ksCircle(70, 0, 30, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef2.EndEdit();

            ksEntity bossExtr2 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef2 = bossExtr2.GetDefinition();
            ksExtrusionParam extrProp2 = (ksExtrusionParam)ExtrDef2.ExtrusionParam();

            if (extrProp2 != null)
            {
                ExtrDef2.SetSketch(ksScetchDef2); // эскиз операции выдавливания
                                                  // направление выдавливания (обычное)
                extrProp2.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                extrProp2.typeReverse = (short)End_Type.etBlind;
                extrProp2.depthReverse = 940; // глубина выдавливания
                bossExtr2.Create(); // создадим операцию
            }

            ksEntity ksScetch3Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef3 = ksScetch3Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef3.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch3Entity.Create(); // создадим эскиз
            ksDocument2D Scetch32D = (ksDocument2D)ksScetchDef3.BeginEdit(); // начинаем редактирование эскиза

            ksRectangleParam recParam = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            recParam.x = 20;
            recParam.y = -40;
            recParam.height = 80;
            recParam.width = 20;
            recParam.ang = 0;
            recParam.style = 1;

            Scetch32D.ksRectangle(recParam);
            ksScetchDef3.EndEdit();


            ksEntity bossExtr3 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef3 = bossExtr3.GetDefinition();
            ksExtrusionParam extrProp3 = (ksExtrusionParam)ExtrDef3.ExtrusionParam();

            if (extrProp3 != null)
            {
                ExtrDef3.SetSketch(ksScetchDef3); // эскиз операции выдавливания
                                                  // направление выдавливания (обычное)
                extrProp3.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                extrProp3.typeReverse = (short)End_Type.etBlind;
                extrProp3.depthReverse = 195; // глубина выдавливания
                bossExtr3.Create(); // создадим операцию
            }

            ksEntity ksScetch4Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef4 = ksScetch4Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef4.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch4Entity.Create(); // создадим эскиз
            ksDocument2D Scetch42D = (ksDocument2D)ksScetchDef4.BeginEdit(); // начинаем редактирование эскиза

            Scetch42D.ksCircle(135.5, 0, 19, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            ksScetchDef4.EndEdit();

            ksEntity bossExtr4 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef4 = bossExtr4.GetDefinition();
            ksExtrusionParam extrProp4 = (ksExtrusionParam)ExtrDef4.ExtrusionParam();

            if (extrProp4 != null)
            {
                ExtrDef4.SetSketch(ksScetchDef4); // эскиз операции выдавливания
                                                  // направление выдавливания (обычное)
                extrProp4.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                extrProp4.typeReverse = (short)End_Type.etBlind;
                extrProp4.depthReverse = 30; // глубина выдавливания
                bossExtr4.Create(); // создадим операцию
            }

            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Болт-лапа.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
