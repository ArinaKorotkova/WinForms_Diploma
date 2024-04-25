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
    internal class ShtokVytalkivately : BasePart
    {
        //Деталь 12 - Шток выталкивателя
        private readonly double diameter;

        public ShtokVytalkivately(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string partName = null)
        {
            //if (File.Exists(Path.Combine(folderPath, "Шток выталкивателя.m3d")))
            //{
            //    return Path.Combine(folderPath, "Шток выталкивателя.m3d");
            //}

            CreateNew("Шток выталкивателя");
            var radius = diameter / 2;

            //Эскиз 1 - основание направляющей
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, 0, 1375, 0, 3); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(1375, 0, 1375, -radius * 1.132 / 4, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(1375, -radius * 1.132 / 4, 75, -radius * 1.132 / 4, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksArcBy3Points(75, -radius * 1.132 / 4, 22, -radius * 0.2, 0, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
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


            ksEntityCollection ksEntityCollection121 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection121.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection121.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == RotatedBase1)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == radius * 1.132 / 4)
                        {
                            part1.name = "CylinderMainBody_shtokVytalk";
                            part1.Update();
                        }
                    }
                }
            }

            ksEntityCollection ksEntityCollection122 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection122.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection122.GetByIndex(q);
                ksFaceDefinition def1 = part1.GetDefinition();

                if (def1.GetOwnerEntity() == RotatedBase1)
                {
                    if (def1.IsPlanar())
                    {
                        ksEdgeCollection col1 = def1.EdgeCollection();

                        for (int k = 0; k < col1.GetCount(); k++)
                        {
 
                                ksEdgeDefinition d1 = col1.GetByIndex(k);

                                if (d1.IsCircle())
                                {


                                    ksVertexDefinition p1 = d1.GetVertex(true);


                                    double x1, y1, z1;

                                    p1.GetPoint(out x1, out y1, out z1);


                                    if (Math.Abs(x1 - 1375) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 - radius * 1.132 / 4) <= 0.1)
                                    {
                                        part1.name = ("Plane1_shtokVytalk");
                                        part1.Update();
                                        break;
                                    }
                                }
                        }
                    }
                }
            }


            //Эскиз 2 
            ksEntity ksScetch2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef2 = ksScetch2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef2.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch2Entity.Create(); // создадим эскиз
            ksDocument2D Scetch22D = (ksDocument2D)ksScetchDef2.BeginEdit(); // начинаем редактирование эскиза

            Scetch22D.ksCircle(35, 0, radius * 0.0566, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

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
                CutPropScetch2.direction = (short)Direction_Type.dtMiddlePlane;
                // тип вырезания (строго на глубину)
                CutPropScetch2.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch2.depthNormal = radius * 1.132 / 2;
                // создадим операцию

                CutScetch2.Create();

            }

            ksEntityCollection ksEntityCollection23 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection23.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection23.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == CutScetch2)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == radius * 0.0566)
                        {
                            part1.name = "CylinderD30_ShtokVytalk";
                            part1.Update();
                        }
                    }
                }
            }


            ksEntity basePlane1Offset = (ksEntity)part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition offsetPlaneDef1 = basePlane1Offset.GetDefinition();
            offsetPlaneDef1.direction = false;
            offsetPlaneDef1.offset = radius * 1.132 / 4;
            offsetPlaneDef1.SetPlane(basePlaneXOZ);
            basePlane1Offset.Create();

            ksEntity ksScetch3Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef3 = ksScetch3Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef3.SetPlane(basePlane1Offset); // установим плоскость XOZ базовой для эскиза
            ksScetch3Entity.Create(); // создадим эскиз
            ksDocument2D Scetch32D = (ksDocument2D)ksScetchDef3.BeginEdit(); // начинаем редактирование эскиза

            ksRectangleParam recParam = (ksRectangleParam)kompas.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
            recParam.x = 1195;
            recParam.y = -radius * 0.0377;
            recParam.height = radius * 0.075;
            recParam.width = 150;
            recParam.ang = 0;
            recParam.style = 1;

            Scetch32D.ksRectangle(recParam);
            ksScetchDef3.EndEdit();

            ksEntity bossExtr1 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef1 = bossExtr1.GetDefinition();
            ksExtrusionParam extrProp1 = (ksExtrusionParam)ExtrDef1.ExtrusionParam();

            if (extrProp1 != null)
            {
                ExtrDef1.SetSketch(ksScetchDef3); // эскиз операции выдавливания
                                                  // направление выдавливания (обычное)
                extrProp1.direction = (short)Direction_Type.dtReverse;
                // тип выдавливания (строго на глубину)
                extrProp1.typeReverse = (short)End_Type.etBlind;
                extrProp1.depthReverse = radius * 0.169; // глубина выдавливания
                bossExtr1.Create(); // создадим операцию
            }

            ksEntity bossExtr2 = part.NewEntity((short)Obj3dType.o3d_bossExtrusion); // создаём объект выдавливания
            ksBossExtrusionDefinition ExtrDef2 = bossExtr2.GetDefinition();
            ksExtrusionParam extrProp2 = (ksExtrusionParam)ExtrDef2.ExtrusionParam();

            if (extrProp2 != null)
            {
                ExtrDef2.SetSketch(ksScetchDef3); // эскиз операции выдавливания
                                                  // направление выдавливания (обычное)
                extrProp2.direction = (short)Direction_Type.dtNormal;
                // тип выдавливания (строго на глубину)
                extrProp2.typeNormal = (short)End_Type.etBlind;
                extrProp2.depthNormal = radius * 0.075; // глубина выдавливания
                bossExtr2.Create(); // создадим операцию
            }

            // операция массив вращения
            ksEntity Circle1Copy = part.NewEntity((short)Obj3dType.o3d_circularCopy);
            // получаем свойства кругового массива
            CircularCopyDefinition Circle1CopyDef = Circle1Copy.GetDefinition();
            // создаём ось вращения на основе базовой
            ksEntity baseAxisX = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOX);
            // выставляем базовую ось
            Circle1CopyDef.SetAxis(baseAxisX);
            // значение для кругового массива, 3 копии 120 градусов
            Circle1CopyDef.SetCopyParamAlongDir(4, 90, false, false);
            // создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection1 = Circle1CopyDef.GetOperationArray();
            // очищаем её
            EntityCollection1.Clear();
            // добавляем элемент выдавливания в коллекци.
            EntityCollection1.Add(bossExtr1);
            EntityCollection1.Add(bossExtr2);
            // создаём массив
            Circle1Copy.Create();

            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Шток выталкивателя.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
