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
    internal class ShaibaRetCyl : BasePart
    {
        //Деталь 21 - Шайба на плунжер ретурного цилиндра
        public override string CreatePart(string partName = null)
        {
            CreateNew("Шайба на плунжер ретурного цилиндра");

            //Эскиз 1 - основание 
            ksEntity ksScetch1Entity = part.NewEntity((int)Obj3dType.o3d_sketch); // создание нового эскиза
            SketchDefinition ksScetchDef1 = ksScetch1Entity.GetDefinition(); // получаем интерфейс свойств эскиза
            ksScetchDef1.SetPlane(basePlaneXOZ); // установим плоскость XOZ базовой для эскиза
            ksScetch1Entity.Create(); // создадим эскиз
            ksDocument2D Scetch12D = (ksDocument2D)ksScetchDef1.BeginEdit(); // начинаем редактирование эскиза

            Scetch12D.ksLineSeg(0, 0, 0, 10, 3); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

            Scetch12D.ksLineSeg(30.5, 0, 70.5, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(70.5, 0, 70.5, 15, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(70.5, 15, 90, 15, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(90, 15, 90, 30, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(90, 30, 30.5, 30, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)
            Scetch12D.ksLineSeg(30.5, 30, 30.5, 0, 1); // создаём первый отрезок (x1,y1,x2,y2,стиль линии)

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

            ksDoc3d.hideAllPlanes = true; // скрыть все плоскости
            ksDoc3d.hideAllAxis = true; // скрыть все оси


            string path = Path.Combine(folderPath, "Шайба на плунжер ретурного цилиндра.m3d");
            ksDoc3d.SaveAs(path);
            ksDoc3d.close();

            return path;
        }
    }
}
