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
    internal class RebroZh : BasePart
    {
        //Деталь 23 - Ребро жесткости
        public override string CreatePart(string partName = null)
        {
            if (File.Exists(Path.Combine(folderPath, "Ребро жесткости.m3d")))
            {
                return Path.Combine(folderPath, "Ребро жесткости.m3d");
            }
            CreateNew("Ребро жесткости");

            //Эскиз 1 - основание 
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(119, 0, 190, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(190, 0, 250, 410, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(250, 410, 130, 410, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(130, 410, 130, 220, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(130, 220, 120, 160, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(120, 160, 119, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

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
                extrProp1.depthReverse = 20; // глубина выдавливания
                bossExtr1.Create(); // создадим операцию
            }

            ksEntityCollection ksEntityCollection1 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection1.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection1.GetByIndex(q);
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

                            if (Math.Abs(x1 - 250) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 + 410) <= 0.1)
                            {
                                if (Math.Abs(x2 - 130) <= 0.1 && Math.Abs(y2 + 20) <= 0.1 && Math.Abs(z2 + 410) <= 0.1)
                                {
                                    part1.name = ("Panel1_Verh_RebZh");
                                    part1.Update();
                                    break;
                                }

                            }

                        }
                    }
                }
            }


            ksEntityCollection ksEntityCollection2 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection2.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection2.GetByIndex(q);
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

                            if (Math.Abs(x1 - 130) <= 0.1 && Math.Abs(y1 + 20) <= 0.1 && Math.Abs(z1 + 410) <= 0.1)
                            {
                                if (Math.Abs(x2 - 190) <= 0.1 && Math.Abs(y2 + 20) <= 0.1 && Math.Abs(z2) <= 0.1)
                                {
                                    part1.name = ("Panel2_Bok_RebZh");
                                    part1.Update();
                                    break;
                                }

                            }

                        }
                    }
                }
            }


            ksEntityCollection ksEntityCollection3 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int q = 0; q < ksEntityCollection3.GetCount(); q++)
            {
                ksEntity part1 = ksEntityCollection3.GetByIndex(q);
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

                            if (Math.Abs(x1 - 130) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1 + 410) <= 0.1)
                            {
                                if (Math.Abs(x2 - 130) <= 0.1 && Math.Abs(y2 + 20) <= 0.1 && Math.Abs(z2 + 220) <= 0.1)
                                {
                                    part1.name = ("Panel3_R_RebZh");
                                    part1.Update();
                                    break;
                                }

                            }

                        }
                    }
                }
            }


            //Первый зеркальный массив относительно плоскости ZOX
            ksEntity MirrorCopyPart1 = part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            // получаем интерфейс определения вырезания
            ksMirrorCopyDefinition MirrorCopyPart1Def = MirrorCopyPart1.GetDefinition();

            if (MirrorCopyPart1Def != null)
            {
                ksEntityCollection EntityCollection1 = MirrorCopyPart1Def.GetOperationArray();
                EntityCollection1.Clear();
                EntityCollection1.Add(bossExtr1);

                MirrorCopyPart1Def.SetPlane(basePlaneZOY);
                MirrorCopyPart1.Create();
            }


            ksEntity circCopy1 = part.NewEntity((short)Obj3dType.o3d_circularCopy);
            // получаем свойства кругового массива
            CircularCopyDefinition circCopyDef1 = circCopy1.GetDefinition();
            // создаём ось вращения на основе базовой
            ksEntity baseAxisZ = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOZ);
            // выставляем базовую ось
            circCopyDef1.SetAxis(baseAxisZ);
            // значение для кругового массива, 3 копии 120 градусов
            circCopyDef1.SetCopyParamAlongDir(2, 90, false, false);
            // создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection4 = circCopyDef1.GetOperationArray();
            // очищаем её
            EntityCollection4.Clear();
            // добавляем элемент выдавливания в коллекци.
            EntityCollection4.Add(MirrorCopyPart1);
            // создаём массив
            circCopy1.Create();

            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Ребро жесткости.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
