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
    internal class Plita1 : BasePart
    {

        //Деталь 19 - Плита 1
        private readonly double diameter;

        public Plita1(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string partName = null)
        {

            //if (File.Exists(Path.Combine(folderPath, "Плита1_025.m3d")))
            //{
            //    return Path.Combine(folderPath, "Плита1_025.m3d");
            //}
            CreateNew("Плита1_025");
            var radius = diameter / 2;

            //Эскиз 1 - основание 
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(radius * 1.132, diameter, -radius * 1.132, diameter, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(-radius * 1.132, diameter, -radius * 1.132, -diameter, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(-radius * 1.132, -diameter, radius * 1.132, -diameter, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(radius * 1.132, -diameter, radius * 1.132, diameter, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

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
                extrProp1.depthReverse = 30; // глубина выдавливания
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

                            if (Math.Abs(x1 - radius * 1.132) <= 0.1 && Math.Abs(y1 + 30) <= 0.1 && Math.Abs(z1 - diameter) <= 0.1)
                            {
                                if (Math.Abs(x2 + radius * 1.132) <= 0.1 && Math.Abs(y2 + 30 ) <= 0.1 && Math.Abs(z2 + diameter) <= 0.1)
                                {
                                    part1.name = ("Planel1_Verh_Plita1");
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

                            if (Math.Abs(x1 + radius * 1.132) <= 0.1 && Math.Abs(y1 + 30) <= 0.1 && Math.Abs(z1 - diameter) <= 0.1)
                            {
                                if (Math.Abs(x2 + radius * 1.132) <= 0.1 && Math.Abs(y2) <= 0.1 && Math.Abs(z2 + diameter) <= 0.1)
                                {
                                    part1.name = ("Planel2_Bok_Plita1");
                                    part1.Update();
                                    break;
                                }

                            }

                        }
                    }
                }
            }

            ksEntity ksScetch2Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef2 = ksScetch2Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef2.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch2Entity.Create(); // создадим эскиз
            ksDocument2D Scetch22D = (ksDocument2D)ksScetchDef2.BeginEdit(); // начинаем редактирование эскиза

            Scetch22D.ksCircle(0, 0, radius * 0.49, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch22D.ksCircle(radius * 1.132 / 2, radius * 1.51, 15, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksCircle(0, radius * 1.51, 15, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksCircle(-radius * 1.132 / 2, radius * 1.51, 15, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch22D.ksCircle(-radius * 1.132 / 2, -radius * 1.51, 15, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksCircle(0, -radius * 1.51, 15, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch22D.ksCircle(radius * 1.132 / 2, -radius * 1.51, 15, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

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
                CutPropScetch2.direction = (short)Direction_Type.dtNormal;
                // тип вырезания (строго на глубину)
                CutPropScetch2.typeNormal = (short)End_Type.etBlind;
                // глубина вырезания
                CutPropScetch2.depthNormal = 30;
                // создадим операцию

                CutScetch2.Create();

            }

            ksEntityCollection ksEntityCollection3 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection3.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection3.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.GetOwnerEntity() == CutScetch2)
                {
                    if (def.IsCylinder())
                    {
                        double h1, r;
                        def.GetCylinderParam(out h1, out r);

                        if (r == radius * 0.49)
                        {
                            part1.name = "CylinderPlita1";
                            part1.Update();
                        }
                    }
                }
            }


            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Плита1_025.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
