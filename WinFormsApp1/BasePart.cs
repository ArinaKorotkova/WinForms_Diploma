using Kompas6API5;
using Kompas6Constants3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1;

namespace CurseWork
{
    internal abstract class BasePart
    {
        protected string folderPath = @"C:\Users\arina\Documents\Институт\ИнжПроект_Диплом\Гидравлический пресс\Сборка1";

        protected KompasObject kompas; // создание экземпляра КОМПАС
        protected ksDocument3D ksDoc3d; // создание экземпляра 3D-документа
        protected ksPart part; // создание экземпляра детали

        // создаём экземпляры для базовых плоскостей
        protected ksEntity basePlaneXOY;
        protected ksEntity basePlaneZOY;
        protected ksEntity basePlaneXOZ;

        protected void CreateNew(string fileName) // Открываем компас
        {

            
            try // пытаемся подключиться к открытому экземпляру
            {
                kompas = (KompasObject)COM.GetActiveObject("KOMPAS.Application.5");
            }
            catch // если не получается, создаём новый
            {
                kompas = (KompasObject)Activator.CreateInstance(Type.GetTypeFromProgID("KOMPAS.Application.5"));
            }
            if (kompas == null)
                return;
            kompas.Visible = true; // делаем КОМПАС видимым
            ksDoc3d = (ksDocument3D)kompas.Document3D(); // получение интерфейса 3д документа

            ksDoc3d.Create(false, true); // false - видимый режим, true - деталь
            ksDoc3d.author = "API_Examples";   // указание имени автора
            ksDoc3d.fileName = $"{fileName}"; // указание названия файла
            part = ksDoc3d.GetPart((int)Part_Type.pTop_Part); // получаем интерфейс новой детали
            basePlaneXOY = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);// получим интерфейс базовой плоскости XOY
            basePlaneZOY = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_planeYOZ);  // получим интерфейс базовой плоскости YOZ
            basePlaneXOZ = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ);  // получим интерфейс базовой плоскости XOZ

        }

        public abstract string CreatePart(string partName = null);
    }
}
