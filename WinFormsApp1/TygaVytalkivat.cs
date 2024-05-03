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
    internal class TygaVytalkivat : BasePart
    {
        //Деталь 14 - Тяга выталкивателя
        private readonly double diameter;

        public TygaVytalkivat(double D)
        {
            diameter = D;
        }
        public override string CreatePart(string partName = null)
        {
            CreateNew("Тяга выталкивателя");

            //Эскиз 1 - основание 
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksCircle(0, 0, diameter / 2 * 0.132, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

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
                extrProp1.depthReverse = 4710; // глубина выдавливания
                bossExtr1.Create(); // создадим операцию
            }
            ksEntityCollection ksEntityCollection1 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection1.GetCount(); i++)
            {
                ksEntity part1 = ksEntityCollection1.GetByIndex(i);
                ksFaceDefinition def = part1.GetDefinition();

                if (def.IsCylinder())
                {
                    double h1, r;
                    def.GetCylinderParam(out h1, out r);

                    if (r == diameter / 2 * 0.132)
                    {
                        part1.name = "Cylinder_TygaVytalk";
                        part1.Update();
                    }
                }
            }

            ksEntityCollection ksEntityCollection2 = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_face);
            for (int i = 0; i < ksEntityCollection2.GetCount(); i++)
            {
                ksEntity part = ksEntityCollection2.GetByIndex(i);
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
                            if (Math.Abs(x1 - diameter / 2 * 0.132) <= 0.1 && Math.Abs(y1) <= 0.1 && Math.Abs(z1) <= 0.1)
                            {
                                part.name = ("Plane1_Dno_TygaVyt");
                                part.Update();
                                break;
                            }
                        }
                    }
                }
            }

            ksEntity MeshCopyE = part.NewEntity((short)Obj3dType.o3d_meshCopy);
            //создаём интерфейс свойств линейного массива
            MeshCopyDefinition MeshCopyDef = MeshCopyE.GetDefinition();
            //создаём ось линейного массива на основе базовой X
            ksEntity baseAxisX = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_axisOX);
            //выставляем базовую ось для первого направления
            MeshCopyDef.SetAxis1(baseAxisX);
            // количество элементов копирования вдоль первой оси
            MeshCopyDef.count1 = 2;
            // шаг поворота вдоль первой оси
            MeshCopyDef.angle1 = 0;
            // шаг копирования вдоль первой оси
            MeshCopyDef.step1 = -diameter / 2 * 8.332;
            //создаём коллекцию для копируемых элементов
            ksEntityCollection EntityCollection = MeshCopyDef.OperationArray();
            EntityCollection.Clear(); // очищаем её
                                      //добавляем элемент выдавливания в коллекци.
            EntityCollection.Add(bossExtr1);
            MeshCopyE.Create(); // создаём массив

            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Тяга выталкивателя.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
