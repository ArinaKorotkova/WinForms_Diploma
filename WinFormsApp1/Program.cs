using CurseWork;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices;
using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;
using System;
using System.Collections.Generic;
using System.Linq;
using Application = System.Windows.Forms.Application;

namespace WinFormsApp1
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }

    class PressSborka
    {
        private KompasObject kompas; // �������� ���������� ������
        private ksDocument3D ksDoc3d; // �������� ���������� 3D-���������
        private ksPart part; // �������� ���������� ������

        // ������ ���������� ��� ������� ����������
        private ksEntity basePlaneXOY;
        private ksEntity basePlaneZOY;
        private ksEntity basePlaneXOZ;

        private string folderPath = @"C:\Users\arina\Documents\��������\���������_������\�������������� �����\������1";


        public void CreateNew(string fileName) // ��������� ������
        {

            //blalblawdlkwad
            try // �������� ������������ � ��������� ����������
            {
                kompas = (KompasObject)COM.GetActiveObject("KOMPAS.Application.5");
            }
            catch // ���� �� ����������, ������ �����
            {
                kompas = (KompasObject)Activator.CreateInstance(Type.GetTypeFromProgID("KOMPAS.Application.5"));
            }
            if (kompas == null)
                return;
            kompas.Visible = true; // ������ ������ �������
            ksDoc3d = (ksDocument3D)kompas.Document3D(); // ��������� ���������� 3� ���������

            ksDoc3d.Create(false, true); // false - ������� �����, true - ������
            ksDoc3d.author = "API_Examples";   // �������� ����� ������
            ksDoc3d.fileName = $"{fileName}"; // �������� �������� �����
            part = ksDoc3d.GetPart((int)Part_Type.pTop_Part); // �������� ��������� ����� ������
            basePlaneXOY = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);// ������� ��������� ������� ��������� XOY
            basePlaneZOY = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_planeYOZ);  // ������� ��������� ������� ��������� YOZ
            basePlaneXOZ = (ksEntity)part.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ);  // ������� ��������� ������� ��������� XOZ

        }

        public void CreateNewSB(double d, string fileName) // ��������� ������
        {
            try // �������� ������������ � ��������� ����������
            {
                kompas = (KompasObject)COM.GetActiveObject("KOMPAS.Application.5");
            }
            catch // ���� �� ����������, ������ �����
            {
                kompas = (KompasObject)Activator.CreateInstance(Type.GetTypeFromProgID("KOMPAS.Application.5"));
            }
            if (kompas == null)
                return;
            kompas.Visible = true; // ������ ������ �������
            ksDocument3D sbKsDoc3d = (ksDocument3D)kompas.Document3D(); // ��������� ���������� 3� ���������

            sbKsDoc3d.Create(false, false); // false - ������� �����, true - ������
            sbKsDoc3d.author = "API_Examples";   // �������� ����� ������
            sbKsDoc3d.fileName = $"{fileName}"; // �������� �������� �����
            sbKsDoc3d = kompas.ActiveDocument3D();
            ksPart sbPart = sbKsDoc3d.GetPart((int)Part_Type.pTop_Part); // �������� ��������� ����� ������
            basePlaneXOY = (ksEntity)sbPart.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);// ������� ��������� ������� ��������� XOY
            basePlaneZOY = (ksEntity)sbPart.GetDefaultEntity((short)Obj3dType.o3d_planeYOZ);  // ������� ��������� ������� ��������� YOZ
            basePlaneXOZ = (ksEntity)sbPart.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ);  // ������� ��������� ������� ��������� XOZ

            //��������� ������ ���� � ������
            string ramaPath = new RamaPressa(d).CreatePart("���������");
            sbKsDoc3d.SetPartFromFile(ramaPath, sbPart, false);

            //��������� ������ ������� �������� � ������
            string verhtrPath = new VerhTraversa(d).CreatePart();
            sbKsDoc3d.SetPartFromFile(verhtrPath, sbPart, false);

            //��������� ������ 2 ���� � ������
            string ramaPath1 = new RamaPressa(d).CreatePart("����������");
            sbKsDoc3d.SetPartFromFile(ramaPath1, sbPart, false);

            //��������� ������ ������ �������� � ������
            string nizhtrPath = new NizhTraversa(d).CreatePart();
            sbKsDoc3d.SetPartFromFile(nizhtrPath, sbPart, false);

            //��������� ������ �������� �������� � ������
            string GlavCilPath = new GlavCylinder(d).CreatePart();
            sbKsDoc3d.SetPartFromFile(GlavCilPath, sbPart, false);

            //��������� ������ �������� �������� �������� � ������
            string PlunGlavCilPath = new PlunzherGlavCylindra(d).CreatePart();
            sbKsDoc3d.SetPartFromFile(PlunGlavCilPath, sbPart, false);

            //��������� ������ ������� � ������
            string PolzunPath = new Polzun(d).CreatePart();
            sbKsDoc3d.SetPartFromFile(PolzunPath, sbPart, false);

            //��������� ������ ���������� ������� � ������
            string PoperechinaPolzunaPath = new PoperechinaPolzuna(d).CreatePart();
            sbKsDoc3d.SetPartFromFile(PoperechinaPolzunaPath, sbPart, false);

            //��������� ������ ��������� �������� � ������
            string ReturnyCylinderPath1 = new ReturnyCylinder().CreatePart("�������� ������� �����");
            sbKsDoc3d.SetPartFromFile(ReturnyCylinderPath1, sbPart, false);

            string ReturnyCylinderPath2 = new ReturnyCylinder().CreatePart("�������� ������� ������");
            sbKsDoc3d.SetPartFromFile(ReturnyCylinderPath2, sbPart, false);

            //��������� ������ ������� 1 � ������
            string Stakan1Path = new Stakan1(d).CreatePart();
            sbKsDoc3d.SetPartFromFile(Stakan1Path, sbPart, false);

            //��������� ������ ������� ������� � ������
            string ShpilkaStyzhnayaPath = new ShpilkaStyzhnaya(d).CreatePart();
            sbKsDoc3d.SetPartFromFile(ShpilkaStyzhnayaPath, sbPart, false);

            //��������� ������ ����� ������������� � ������
            string ShtokVytalkivatelyPath = new ShtokVytalkivately().CreatePart();
            sbKsDoc3d.SetPartFromFile(ShtokVytalkivatelyPath, sbPart, false);

            //��������� ������ �������� ������������� � ������
            string TraversaVytalkivatPath = new TraversaVytalkivat().CreatePart();
            sbKsDoc3d.SetPartFromFile(TraversaVytalkivatPath, sbPart, false);

            //��������� ������ ����� 1 � ������
            string Plita1Path = new Plita1(d).CreatePart();
            sbKsDoc3d.SetPartFromFile(Plita1Path, sbPart, false);

            //��������� ������ ����� ��������� � ������
            string RebroZhPath = new RebroZh().CreatePart();
            sbKsDoc3d.SetPartFromFile(RebroZhPath, sbPart, false);

            //��������� ������ ������ 2 � ������
            string Stakan2Path = new Stakan2(d).CreatePart();
            sbKsDoc3d.SetPartFromFile(Stakan2Path, sbPart, false);

            //��������� ������ ������ 3 � ������
            string Stakan3Path = new Stakan3(d).CreatePart();
            sbKsDoc3d.SetPartFromFile(Stakan3Path, sbPart, false);

            //��������� ������ ����� 2 � ������
            string Plita2Path = new Plita2(d).CreatePart();
            sbKsDoc3d.SetPartFromFile(Plita2Path, sbPart, false);

            //��������� ������ ������� ����� � ������
            string ShpilkaShtokaPath = new ShpilkaShtoka().CreatePart();
            sbKsDoc3d.SetPartFromFile(ShpilkaShtokaPath, sbPart, false);

            //��������� ������ ������� ��������� �������� � ������
            string PlunReturnogoCylPath1 = new PlunReturnogoCyl().CreatePart("������� ��������� �������� �����");
            sbKsDoc3d.SetPartFromFile(PlunReturnogoCylPath1, sbPart, false);

            string PlunReturnogoCylPath2 = new PlunReturnogoCyl().CreatePart("������� ��������� �������� ������");
            sbKsDoc3d.SetPartFromFile(PlunReturnogoCylPath2, sbPart, false);

            //��������� ������ ������������ � ������
            string NapravlPath1 = new Napravl().CreatePart("������������(L+L)");
            sbKsDoc3d.SetPartFromFile(NapravlPath1, sbPart, false);

            string NapravlPath2 = new Napravl().CreatePart("������������(L+R)");
            sbKsDoc3d.SetPartFromFile(NapravlPath2, sbPart, false);

            string NapravlPath3 = new Napravl().CreatePart("������������(R+L)");
            sbKsDoc3d.SetPartFromFile(NapravlPath3, sbPart, false);

            string NapravlPath4 = new Napravl().CreatePart("������������(R+R)");
            sbKsDoc3d.SetPartFromFile(NapravlPath4, sbPart, false);


            //���������� �� ���� ����� �� ����� ������
            ksPartCollection sbParts = sbKsDoc3d.PartCollection(true);
            ksPart rama = sbParts.GetByIndex(0);
            ksEntityCollection ramaFaces = rama.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity rama_Panel1 = ramaFaces.GetByName("Panel_1_Rama", true, true);
            ksEntity rama_Panel2 = ramaFaces.GetByName("Panel_2_Rama", true, true);
            ksEntity rama_Cil1 = ramaFaces.GetByName("CylinderRama1", true, true);
            ksEntity rama_Panel3_L = ramaFaces.GetByName("PanelRama_3_L", true, true);
            ksEntity rama_Panel3_R = ramaFaces.GetByName("PanelRama_3_R", true, true);

            rama.name = "���� �����";
            rama.Update();


            //���������� �� ������� �������� ����� �� ����� ������
            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart verhtr = sbParts.GetByIndex(1);
            ksEntityCollection verhtrFaces = verhtr.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity verhtr_Panel1 = verhtrFaces.GetByName("Panel_1_VrTr", true, true);
            ksEntity verhtr_Cil1 = verhtrFaces.GetByName("CylinderVerhTr1", true, true);
            ksEntity verhtr_Cil2 = verhtrFaces.GetByName("CylinderVerhTr2", true, true);
            ksEntity verhtr_CilCentr = verhtrFaces.GetByName("CentrCyl_VrTr", true, true);
            ksEntity verhtr_Panel2 = verhtrFaces.GetByName("Panel_2_VrTr", true, true);
            ksEntity verhtr_Panel3 = verhtrFaces.GetByName("Panel_3_VrTr", true, true);
            verhtr.name = "������� ��������";
            verhtr.Update();


            //��������� ����������� ���� ������ 4 - ���������, -1 - ����������
            sbKsDoc3d.AddMateConstraint(0, rama_Panel1, verhtr_Panel1, -1, 1, 0);
            sbKsDoc3d.AddMateConstraint(4, rama_Cil1, verhtr_Cil1, -1, 1, 0);



            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart rama1 = sbParts.GetByIndex(2);
            ksEntityCollection ramaFaces1 = rama1.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity rama1_Panel1 = ramaFaces1.GetByName("Panel_1_Rama", true, true);
            ksEntity rama1_Cil1 = ramaFaces1.GetByName("CylinderRama1", true, true);
            ksEntity rama1_Panel2 = ramaFaces1.GetByName("Panel_2_Rama", true, true);
            ksEntity rama1_Panel3_L = ramaFaces1.GetByName("PanelRama_3_L", true, true);
            ksEntity rama1_Panel3_R = ramaFaces1.GetByName("PanelRama_3_R", true, true);
            ksEntity rama1_Panel4 = ramaFaces1.GetByName("Panel_4_Rama", true, true);

            rama1.name = "���� ������";
            rama1.Update();


            sbKsDoc3d.AddMateConstraint(5, rama1_Panel2, verhtr_Panel2, -1, 1, -d / 2 * 1.6);
            sbKsDoc3d.AddMateConstraint(5, rama1_Panel4, verhtr_Panel3, -1, 1, -4420);
            sbKsDoc3d.AddMateConstraint(0, rama1_Panel3_R, rama_Panel3_L, 1, 1, 0);



            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart nizhtr = sbParts.GetByIndex(3);
            ksEntityCollection nizhtrFaces = nizhtr.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity nizhTr_Panel1 = nizhtrFaces.GetByName("PanelVerh_1_NizhTr", true, true);
            ksEntity nizhTr_Panel2 = nizhtrFaces.GetByName("PanelNizh_2_NizhTr", true, true);
            ksEntity nizhTr_Panel3 = nizhtrFaces.GetByName("PanelBok_3_NizhTr", true, true);
            ksEntity nizhTr_Cil1 = nizhtrFaces.GetByName("CylinderNizhTr1", true, true);

            nizhtr.name = "������ ��������";
            nizhtr.Update();

            sbKsDoc3d.AddMateConstraint(1, nizhTr_Panel3, rama_Panel3_L, 1, 1, 0); //��������������
            sbKsDoc3d.AddMateConstraint(0, nizhTr_Panel1, rama1_Panel4, -1, 1, 0);
            sbKsDoc3d.AddMateConstraint(4, verhtr_CilCentr, nizhTr_Cil1, 1, 1, 0);



            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart glavCil = sbParts.GetByIndex(4);
            ksEntityCollection glavCilFaces = glavCil.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity glavCil_Cil1 = glavCilFaces.GetByName("CylinderGlavCil1", true, true);
            ksEntity glavCil_Plane1 = glavCilFaces.GetByName("Plane_1_Dno_GlavCyl", true, true);

            glavCil.name = "������� �������";
            glavCil.Update();

            sbKsDoc3d.AddMateConstraint(4, glavCil_Cil1, verhtr_CilCentr, -1, 1, 0);
            sbKsDoc3d.AddMateConstraint(5, glavCil_Plane1, verhtr_Panel1, 1, 1, 2136);


            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart PlunGlavCil = sbParts.GetByIndex(5);
            ksEntityCollection PlunGlavCilFaces = PlunGlavCil.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity plunGlCyl_1 = PlunGlavCilFaces.GetByName("CylinderPlunGlavCil1", true, true);
            ksEntity plunGlCyl_Dno = PlunGlavCilFaces.GetByName("Plun_GlCyl_Dno", true, true);
            ksEntity plunGlCyl_D32 = PlunGlavCilFaces.GetByName("CylinderD32_PlunGlCyl", true, true);

            PlunGlavCil.name = "������� �������� ��������";
            PlunGlavCil.Update();


            sbKsDoc3d.AddMateConstraint(4, plunGlCyl_1, verhtr_CilCentr, -1, 1, 0);



            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart polzun = sbParts.GetByIndex(6);
            ksEntityCollection PolzunFaces = polzun.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity polzun_Plane1_Circle = PolzunFaces.GetByName("Polzun_centr_Circle", true, true);
            ksEntity polzun_Cil1_centre = PolzunFaces.GetByName("Polzun_centr_Cylinder", true, true);
            ksEntity polzun_Cil_D32 = PolzunFaces.GetByName("CylinderD32_Polzun", true, true);
            ksEntity polzun_Plane_Bokovaya = PolzunFaces.GetByName("Plane_Bokovaya_Polzun", true, true);
            ksEntity polzun_Plane_Nizhnaya = PolzunFaces.GetByName("Plane_Nzhnaya_Polzun", true, true);
            ksEntity polzun_CylinderNizhOtv = PolzunFaces.GetByName("CylinderNizhOtv_Polzun", true, true);

            polzun.name = "������ ������";
            polzun.Update();


            sbKsDoc3d.AddMateConstraint(0, polzun_Plane1_Circle, plunGlCyl_Dno, -1, 1, 0);
            sbKsDoc3d.AddMateConstraint(4, polzun_Cil1_centre, plunGlCyl_1, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(4, plunGlCyl_D32, polzun_Cil_D32, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(1, polzun_Plane_Bokovaya, rama_Panel3_L, 1, 1, 0);


            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart poperechPolzuna = sbParts.GetByIndex(7);
            ksEntityCollection PoperechPolzunaFaces = poperechPolzuna.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity plane1_Nizh_PoperechPolzuna = PoperechPolzunaFaces.GetByName("Plane1_Nizh_PoperPolzuna", true, true);
            ksEntity Plane2_Bokovaya_PoperechPolzuna = PoperechPolzunaFaces.GetByName("Plane2_Bokovaya_PoperPolzuna", true, true);
            ksEntity cylinderOtv_PoperechPolzuna = PoperechPolzunaFaces.GetByName("CylinderOtv_PoperechPolzuna", true, true);

            poperechPolzuna.name = "���������� �������";
            poperechPolzuna.Update();


            sbKsDoc3d.AddMateConstraint(5, plane1_Nizh_PoperechPolzuna, polzun_Plane_Nizhnaya, 1, 1, -250);
            sbKsDoc3d.AddMateConstraint(4, cylinderOtv_PoperechPolzuna, polzun_CylinderNizhOtv, -1, 1, 0);
            sbKsDoc3d.AddMateConstraint(1, Plane2_Bokovaya_PoperechPolzuna, polzun_Plane_Bokovaya, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(1, Plane2_Bokovaya_PoperechPolzuna, rama1_Panel3_L, -1, 1, 0);


            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart returnyCylinder1 = sbParts.GetByIndex(8);
            ksEntityCollection returnyCylinder1Faces = returnyCylinder1.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity CylinderCentre_RetCyl1 = returnyCylinder1Faces.GetByName("CylinderCentre1_RetCyl", true, true);
            ksEntity Planel1_RetCyl1 = returnyCylinder1Faces.GetByName("Panel1_RetCyl1", true, true);
            ksEntity Panel2_Dno_RetCyl1 = returnyCylinder1Faces.GetByName("Panel2_Dno_RetCyl1", true, true);

            returnyCylinder1.name = "�������� ������� �����";
            returnyCylinder1.Update();

            sbKsDoc3d.AddMateConstraint(4, rama_Cil1, CylinderCentre_RetCyl1, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(5, rama_Panel1, Planel1_RetCyl1, -1, 1, -3350);


            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart returnyCylinder2 = sbParts.GetByIndex(9);
            ksEntityCollection returnyCylinder2Faces = returnyCylinder2.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity CylinderCentre_RetCyl2 = returnyCylinder2Faces.GetByName("CylinderCentre1_RetCyl", true, true);
            ksEntity Planel1_RetCyl2 = returnyCylinder2Faces.GetByName("Panel1_RetCyl1", true, true);
            ksEntity Panel2_Dno_RetCyl2 = returnyCylinder1Faces.GetByName("Panel2_Dno_RetCyl1", true, true);

            returnyCylinder2.name = "�������� ������� ������";
            returnyCylinder2.Update();

            sbKsDoc3d.AddMateConstraint(4, rama1_Cil1, CylinderCentre_RetCyl2, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(5, rama1_Panel1, Planel1_RetCyl2, -1, 1, -3350);

            //����� ��� ������� ��������� �������� ���������


            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart stakan1 = sbParts.GetByIndex(10);
            ksEntityCollection stakan1Faces = stakan1.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity CylinderCenterOtv_Stakan1 = stakan1Faces.GetByName("CylinderCenterOtv_Stakan1", true, true);
            ksEntity Plane1_Stakan1 = stakan1Faces.GetByName("Plane1_Stakan1", true, true);

            stakan1.name = "������ 1";
            stakan1.Update();

            sbKsDoc3d.AddMateConstraint(4, nizhTr_Cil1, CylinderCenterOtv_Stakan1, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(5, nizhTr_Panel2, Plane1_Stakan1, -1, 1, -30);


            //������� ���� � ������������� ����������� - ��������� � ������ ������� ��������
            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart shpilkaStyzhnaya = sbParts.GetByIndex(11);
            ksEntityCollection shpilkaStyzhnayaFaces = shpilkaStyzhnaya.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity Cylinder1_ShpilkaStyzh = shpilkaStyzhnayaFaces.GetByName("CylinderShpilkaStyzh", true, true);
            ksEntity Plane1_ShpilkaStyzhnaya1 = shpilkaStyzhnayaFaces.GetByName("Plane1_ShpilkaStyzhnaya1", true, true);

            shpilkaStyzhnaya.name = "������� �������";
            shpilkaStyzhnaya.Update();

            sbKsDoc3d.AddMateConstraint(4, Cylinder1_ShpilkaStyzh, verhtr_Cil2, -1, 1, 0);
            sbKsDoc3d.AddMateConstraint(5, Plane1_ShpilkaStyzhnaya1, verhtr_Panel3, 1, 1, 120);


            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart shtokVytalkivately = sbParts.GetByIndex(12);
            ksEntityCollection shtokVytalkivatelyFaces = shtokVytalkivately.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity CylinderMainBody_shtokVytalk = shtokVytalkivatelyFaces.GetByName("CylinderMainBody_shtokVytalk", true, true);
            ksEntity CylinderD30_ShtokVytalk = shtokVytalkivatelyFaces.GetByName("CylinderD30_ShtokVytalk", true, true);
            ksEntity Plane1_shtokVytalk = shtokVytalkivatelyFaces.GetByName("Plane1_shtokVytalk", true, true);

            shtokVytalkivately.name = "���� �������������";
            shtokVytalkivately.Update();

            sbKsDoc3d.AddMateConstraint(4, CylinderMainBody_shtokVytalk, CylinderCenterOtv_Stakan1, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(5, Plane1_shtokVytalk, nizhTr_Panel2, -1, 1, -110);



            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart traversaVytalkivat = sbParts.GetByIndex(13);
            ksEntityCollection traversaVytalkivatFaces = traversaVytalkivat.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity CylinderCentre_traversaVytalk = traversaVytalkivatFaces.GetByName("CylinderCentre_traversaVytalk", true, true);
            ksEntity Cylinder2_PodShpilky = traversaVytalkivatFaces.GetByName("Cylinder2_PodShpilky", true, true);
            ksEntity Plane1_traversaVytalk = traversaVytalkivatFaces.GetByName("Plane1_traversaVytalk", true, true);
            ksEntity Plane2_Bok_traversaVytalk = traversaVytalkivatFaces.GetByName("Plane2_Bok_traversaVytalk", true, true);

            traversaVytalkivat.name = "�������� �������������";
            traversaVytalkivat.Update();

            sbKsDoc3d.AddMateConstraint(4, CylinderCentre_traversaVytalk, CylinderCenterOtv_Stakan1, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(4, CylinderCentre_traversaVytalk, CylinderMainBody_shtokVytalk, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(1, Plane1_traversaVytalk, nizhTr_Panel2, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(5, Plane1_traversaVytalk, Plane1_shtokVytalk, -1, 1, -1475);
            sbKsDoc3d.AddMateConstraint(1, Plane2_Bok_traversaVytalk, nizhTr_Panel3, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(4, Cylinder2_PodShpilky, CylinderD30_ShtokVytalk, 1, 1, 0);



            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart plita1 = sbParts.GetByIndex(14);
            ksEntityCollection plita1Faces = plita1.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity Planel1_Verh_Plita1 = plita1Faces.GetByName("Planel1_Verh_Plita1", true, true);
            ksEntity CylinderPlita1 = plita1Faces.GetByName("CylinderPlita1", true, true);
            ksEntity Planel2_Bok_Plita1 = plita1Faces.GetByName("Planel2_Bok_Plita1", true, true);

            plita1.name = "����� 1";
            plita1.Update();

            sbKsDoc3d.AddMateConstraint(4, CylinderPlita1, CylinderCenterOtv_Stakan1, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(0, Planel1_Verh_Plita1, nizhTr_Panel2, -1, 1, 0);
            sbKsDoc3d.AddMateConstraint(2, Planel1_Verh_Plita1, nizhTr_Panel3, 1, 1, 0);


            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart rebroZh = sbParts.GetByIndex(15);
            ksEntityCollection rebroZhFaces = rebroZh.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity Panel1_Verh_Plita1 = rebroZhFaces.GetByName("Panel1_Verh_RebZh", true, true);
            ksEntity Panel2_Bok_Plita1 = rebroZhFaces.GetByName("Panel2_Bok_RebZh", true, true);
            ksEntity Panel3_R_Plita1 = rebroZhFaces.GetByName("Panel3_R_RebZh", true, true);

            rebroZh.name = "����� ���������";
            rebroZh.Update();

            sbKsDoc3d.AddMateConstraint(5, Panel1_Verh_Plita1, nizhTr_Panel2, -1, 1, 30);
            sbKsDoc3d.AddMateConstraint(1, Panel2_Bok_Plita1, nizhTr_Panel3, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(5, Panel2_Bok_Plita1, nizhTr_Panel3, 1, 1, 510);
            sbKsDoc3d.AddMateConstraint(5, Panel3_R_Plita1, Planel2_Bok_Plita1, 1, 1, 430);


            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart stakan2 = sbParts.GetByIndex(16);
            ksEntityCollection stakan2Faces = stakan2.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity Panel1_Dno_stakan2 = stakan2Faces.GetByName("Plane1_Dno_Stakan2", true, true);
            ksEntity CylinderMain_Stakan2 = stakan2Faces.GetByName("CylinderMain_Stakan2", true, true);

            stakan2.name = "������ 2";
            stakan2.Update();


            sbKsDoc3d.AddMateConstraint(4, CylinderMain_Stakan2, nizhTr_Cil1, -1, 1, 0);
            sbKsDoc3d.AddMateConstraint(5, Panel1_Dno_stakan2, Planel1_Verh_Plita1, -1, 1, -480);


            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart stakan3 = sbParts.GetByIndex(17);
            ksEntityCollection stakan3Faces = stakan3.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity Panel1_Dno_stakan3 = stakan3Faces.GetByName("Plane1_Dno_Stakan3", true, true);
            ksEntity CylinderMain_Stakan3 = stakan3Faces.GetByName("CylinderMain_Stakan3", true, true);

            stakan3.name = "������ 3";
            stakan3.Update();

            sbKsDoc3d.AddMateConstraint(4, CylinderMain_Stakan3, nizhTr_Cil1, -1, 1, 0);
            sbKsDoc3d.AddMateConstraint(0, Panel1_Dno_stakan3, Panel1_Dno_stakan2, 1, 1, 0);



            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart plita2 = sbParts.GetByIndex(18);
            ksEntityCollection plita2Faces = plita2.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity Plane1_Verh_Plita2 = plita2Faces.GetByName("Plane1_Plita2", true, true);
            ksEntity CylinderPlita2 = plita2Faces.GetByName("CylinderMain_Plita2", true, true);

            plita2.name = "����� 2";
            plita2.Update();

            sbKsDoc3d.AddMateConstraint(4, CylinderPlita2, CylinderCenterOtv_Stakan1, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(0, Plane1_Verh_Plita2, Panel1_Dno_stakan3, 1, 1, 0);



            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart shpilkaShtoka = sbParts.GetByIndex(19);
            ksEntityCollection shpilkaShtokaFaces = shpilkaShtoka.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity Plane1_Dno_ShpilkaSht = shpilkaShtokaFaces.GetByName("Plane1_Dno_ShpilkaSht", true, true);
            ksEntity Cylinder1_ShpilkaSht = shpilkaShtokaFaces.GetByName("Cylinder_ShpilkaSht", true, true);

            shpilkaShtoka.name = "������� �����";
            shpilkaShtoka.Update();

            sbKsDoc3d.AddMateConstraint(4, Cylinder1_ShpilkaSht, Cylinder2_PodShpilky, 1, 1, 0);
            sbKsDoc3d.AddMateConstraint(5, Plane1_Dno_ShpilkaSht, Plane2_Bok_traversaVytalk, 1, 1, -30);



            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart plunReturnogoCyl1 = sbParts.GetByIndex(20);
            ksEntityCollection plunReturnogoCylFaces1 = plunReturnogoCyl1.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity CylinderMainBody_PlunRetCyl1 = plunReturnogoCylFaces1.GetByName("CylinderMainBody_PlunRetCyl", true, true);
            ksEntity Plane1_Dno_PlunRetCyl1 = plunReturnogoCylFaces1.GetByName("Plane1_Dno_PlunRetCyl", true, true);

            plunReturnogoCyl1.name = "������� ��������� �������� �����";
            plunReturnogoCyl1.Update();

            sbKsDoc3d.AddMateConstraint(4, CylinderMainBody_PlunRetCyl1, CylinderCentre_RetCyl1, -1, 1, 0);
            sbKsDoc3d.AddMateConstraint(0, Plane1_Dno_PlunRetCyl1, Panel2_Dno_RetCyl1, -1, 1, 0);

            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart plunReturnogoCyl2 = sbParts.GetByIndex(21);
            ksEntityCollection plunReturnogoCylFaces2 = plunReturnogoCyl2.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity CylinderMainBody_PlunRetCyl2 = plunReturnogoCylFaces2.GetByName("CylinderMainBody_PlunRetCyl", true, true);
            ksEntity Plane1_Dno_PlunRetCyl2 = plunReturnogoCylFaces2.GetByName("Plane1_Dno_PlunRetCyl", true, true);

            plunReturnogoCyl2.name = "������� ��������� �������� ������";
            plunReturnogoCyl2.Update();

            sbKsDoc3d.AddMateConstraint(4, CylinderMainBody_PlunRetCyl2, CylinderCentre_RetCyl2, -1, 1, 0);
            sbKsDoc3d.AddMateConstraint(0, Plane1_Dno_PlunRetCyl2, Panel2_Dno_RetCyl2, -1, 1, 0);


            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart napravl1 = sbParts.GetByIndex(22);
            ksEntityCollection napravlFaces1 = napravl1.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity Plane1_Zad_Napr1 = napravlFaces1.GetByName("Plane1_Zad_Napr", true, true);
            ksEntity Plane2_Bok_Napr1 = napravlFaces1.GetByName("Plane2_Bok_Napr", true, true);
            ksEntity Plane3_Verh_Napr1 = napravlFaces1.GetByName("Plane3_Verh_Napr", true, true);

            napravl1.name = "������������ (L+L)";
            napravl1.Update();

            sbKsDoc3d.AddMateConstraint(5, Plane1_Zad_Napr1, rama_Panel2, -1, 1, 20);
            sbKsDoc3d.AddMateConstraint(5, Plane3_Verh_Napr1, rama_Panel1, 1, 1, 600);
            sbKsDoc3d.AddMateConstraint(5, Plane2_Bok_Napr1, rama_Panel3_L, -1, 1, 48);

            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart napravl2 = sbParts.GetByIndex(23);
            ksEntityCollection napravlFaces2 = napravl2.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity Plane1_Zad_Napr2 = napravlFaces2.GetByName("Plane1_Zad_Napr", true, true);
            ksEntity Plane2_Bok_Napr2 = napravlFaces2.GetByName("Plane2_Bok_Napr", true, true);
            ksEntity Plane3_Verh_Napr2 = napravlFaces2.GetByName("Plane3_Verh_Napr", true, true);

            napravl2.name = "������������ (L+R)";
            napravl2.Update();

            sbKsDoc3d.AddMateConstraint(5, Plane1_Zad_Napr2, rama_Panel2, -1, 1, 20);
            sbKsDoc3d.AddMateConstraint(5, Plane3_Verh_Napr2, rama_Panel1, -1, 1, -3000);
            sbKsDoc3d.AddMateConstraint(5, Plane2_Bok_Napr2, rama_Panel3_R, -1, 1, 48);


            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart napravl3 = sbParts.GetByIndex(24);
            ksEntityCollection napravlFaces3 = napravl3.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity Plane1_Zad_Napr3 = napravlFaces3.GetByName("Plane1_Zad_Napr", true, true);
            ksEntity Plane2_Bok_Napr3 = napravlFaces3.GetByName("Plane2_Bok_Napr", true, true);
            ksEntity Plane3_Verh_Napr3 = napravlFaces3.GetByName("Plane3_Verh_Napr", true, true);

            napravl3.name = "������������ (R+L)";
            napravl3.Update();

            sbKsDoc3d.AddMateConstraint(5, Plane1_Zad_Napr3, rama1_Panel2, -1, 1, 20);
            sbKsDoc3d.AddMateConstraint(5, Plane3_Verh_Napr3, rama1_Panel1, 1, 1, 600);
            sbKsDoc3d.AddMateConstraint(5, Plane2_Bok_Napr3, rama1_Panel3_L, -1, 1, 48);


            sbParts = sbKsDoc3d.PartCollection(true);
            ksPart napravl4 = sbParts.GetByIndex(25);
            ksEntityCollection napravlFaces4 = napravl4.EntityCollection((short)Obj3dType.o3d_face);
            ksEntity Plane1_Zad_Napr4 = napravlFaces4.GetByName("Plane1_Zad_Napr", true, true);
            ksEntity Plane2_Bok_Napr4 = napravlFaces4.GetByName("Plane2_Bok_Napr", true, true);
            ksEntity Plane3_Verh_Napr4 = napravlFaces4.GetByName("Plane3_Verh_Napr", true, true);

            napravl4.name = "������������ (R+R)";
            napravl4.Update();

            sbKsDoc3d.AddMateConstraint(5, Plane1_Zad_Napr4, rama1_Panel2, -1, 1, 20);
            sbKsDoc3d.AddMateConstraint(5, Plane3_Verh_Napr4, rama1_Panel1, -1, 1, -3000);
            sbKsDoc3d.AddMateConstraint(5, Plane2_Bok_Napr4, rama1_Panel3_R, -1, 1, 48);

            sbPart = sbKsDoc3d.GetPart((int)Part_Type.pTop_Part); // �������� ��������� ����� ������
            ksEntity meshArr = sbPart.NewEntity((short)Obj3dType.o3d_meshPartArray);
            ksMeshPartArrayDefinition def = meshArr.GetDefinition();
            ksEntity baseAxisZ1 = (ksEntity)sbPart.GetDefaultEntity((short)Obj3dType.o3d_axisOZ);
            ksEntity baseAxisX1 = (ksEntity)sbPart.GetDefaultEntity((short)Obj3dType.o3d_axisOX);
            def.SetAxis1(baseAxisZ1);
            def.SetAxis2(baseAxisX1);
            def.count1 = 2;
            def.count2 = 2;
            def.factor1 = true;
            def.factor2 = true;
            def.insideFlag = false;
            def.step1 = d / 2 * 2.6415;
            def.step2 = -d / 2 * 6.0377;
            ksPartCollection EntityCollection1 = def.PartArray();
            EntityCollection1.Add(shpilkaStyzhnaya);

            meshArr.Create();

        }


        public void GidravlicPress(double d, string path = null)
        {
            CreateNewSB(d, "�������������� �����");
        }
}
}