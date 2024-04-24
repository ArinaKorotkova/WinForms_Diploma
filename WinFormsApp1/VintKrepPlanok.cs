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
    internal class VintKrepPlanok : BasePart
    {
        //Деталь 15 - Винт крепления планок
        public override string CreatePart(string partName = null)
        {
            CreateNew("Винт крепления планок");

            //Эскиз 1 - основание 
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, 0, 8, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(8, 0, 8, 35, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(8, 35, 14, 41, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(14, 41, 14, 45, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(14, 45, 0, 45, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(0, 45, 0, 0, 3); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
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

            //Условное обозначение резьбы
            ksEntity Thread = part.NewEntity((short)Obj3dType.o3d_thread);
            // получаем интерфейс настроек резьбы
            ksThreadDefinition ThreadDef = Thread.GetDefinition();
            ThreadDef.allLength = false; // признак полной длины
            ThreadDef.autoDefinDr = false;// признак автоопределения диаметра
            ThreadDef.dr = 16; // номинальный диаметр резьбы
            ThreadDef.length = 30; // длина резьбы
            ThreadDef.faceValue = true; // направление построения резьбы
            ThreadDef.p = 2; // шаг резьбы
                             // получаем коллекцию рёбер детали
            ksEntityCollection EdgeECol = (ksEntityCollection)part.EntityCollection((short)Obj3dType.o3d_edge);
            // оставляем в массиве только ребро, проходящее через точку (x,y,z)
            EdgeECol.SelectByPoint(0, 8, 0);
            ThreadDef.SetBaseObject(EdgeECol.First()); // устанавливаем ребро в параметры резьбы
            // создаём резьбу
            Thread.Create();

            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Винт крепления планок.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
