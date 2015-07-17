using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Excel = Microsoft.Office.Interop.Excel; 

namespace Planning
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            TypeOfEmployment typeOfEmployment = new TypeOfEmployment();
            addDaysToList();
            if (File.Exists(File0))
            {
                Stream FileStream0 = File.OpenRead(Application.StartupPath + "\\" + File0);
                XmlSerializer deserializer0 = new XmlSerializer(profilesClass.profiles.GetType());
                profilesClass.profiles = (List<string>)deserializer0.Deserialize(FileStream0);
                FileStream0.Close();
                for (int i = 0; i < profilesClass.profiles.Count; i++)
                    comboBoxProfiles.Items.Add(profilesClass.profiles[i]);
                currentProfile = comboBoxProfiles.Items[0].ToString();
                comboBoxProfiles.SelectedItem = comboBoxProfiles.Items[0];
            }
            else
            {
                MessageBox.Show("Ще не створено жодного профілю для збереження даних та результату", "Вітаю ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //File.Create(File0);
                addProfile();
            }
        }
        bool start = true;
        // ---------------------------- Serializating & profiles
        Profiles profilesClass = new Profiles();
        public string currentProfile { get { return currentProfilePrivate; } set { currentProfilePrivate = value;  } }
        private string currentProfilePrivate;
        const string File0 = "profiles.xml";
        const string File1 = "SavedDisciplines.xml";
        const string File2 = "SavedSchedule.xml";
        const string File3 = "SavedDatesHolydays.xml";
        void addProfile()
        {
            NewProfileForm newProfile = new NewProfileForm(profilesClass.profiles);
            newProfile.Owner = this;
            newProfile.ShowDialog();
            //if (profilesClass.profiles.Exists(Convert.ToString((profilesClass.profiles[profilesClass.profiles.Count - 1]))) == true)
            //{
            //    // щоб не було профілів з однаковими назвами
            //}
            comboBoxProfiles.Items.Add(profilesClass.profiles[profilesClass.profiles.Count - 1]);
            comboBoxProfiles.SelectedItem = comboBoxProfiles.Items[profilesClass.profiles.Count - 1];
            currentProfile = Convert.ToString(comboBoxProfiles.SelectedItem);
            Directory.CreateDirectory(Application.StartupPath + "\\" + currentProfile);
            // якщо вже є дані до цього профілю
            if (File.Exists(Application.StartupPath + "\\" + currentProfile + "\\" + File2) && File.Exists(Application.StartupPath + "\\" + currentProfile + "\\" + "disciplines.xls") && File.Exists(Application.StartupPath + "\\" + currentProfile + "\\" + File3))
            {
                DialogResult result = MessageBox.Show("Знайдено файли, що містять дані до профілю \"" + currentProfile + "\". Відновити їх (натисніть Так) чи створити новий профіль без даних?",
                    "Знайдено дані профілю",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    loadDefaultData();
                    string path = Application.StartupPath + "\\" + currentProfile + "\\" + File1;
                    File.Create(path);
                    path = "\\" + currentProfile + File2;
                    File.Create(path);
                    path = "\\" + currentProfile + File3;
                    File.Create(path);
                }
                else
                {
                    Stream FileStream1 = File.OpenRead("\\" + currentProfile + "\\" + File1);
                    XmlSerializer deserializer1 = new XmlSerializer(disciplines.GetType());
                    disciplines = (Disciplines)deserializer1.Deserialize(FileStream1);
                    FileStream1.Close();
                    loadFromXls();
                    Stream FileStream2 = File.OpenRead("\\" + currentProfile + "\\" + File2);
                    XmlSerializer deserializer2 = new XmlSerializer(schedule.GetType());
                    schedule = (Schedule)deserializer2.Deserialize(FileStream2);
                    FileStream2.Close();
                    Stream FileStream3 = File.OpenRead("\\" + currentProfile + "\\" + File3);
                    XmlSerializer deserializer3 = new XmlSerializer(datesHolydays.GetType());
                    datesHolydays = (DatesHolydays)deserializer3.Deserialize(FileStream3);
                    FileStream3.Close();
                    datesDataToControls();
                    holydaysDataToControls();
                    disciplinesDataToControls();
                    Refresh();
                }
            }
            else
                loadDefaultData();
            // серіалізуємо новий профіль
            Stream FileStream0 = File.Create(Application.StartupPath + "\\" + File0);
            XmlSerializer serializer0 = new XmlSerializer(profilesClass.profiles.GetType());
            serializer0.Serialize(FileStream0, profilesClass.profiles);
            FileStream0.Close();
            schedule.allSubjects = subjects[1].Items.Cast<string>().ToList();
            holydaysControlsToData();
            datesControlsToData();
            //disciplineControlsToData();

            //Stream FileStream4 = File.OpenWrite(Application.StartupPath + "\\" + currentProfile + "\\" + File1);
            //XmlSerializer serializer4 = new XmlSerializer(disciplines.GetType());
            //serializer4.Serialize(FileStream4, disciplines);
            //FileStream4.Close();
            saveToXls();

            Stream FileStream5  = File.OpenWrite(Application.StartupPath + "\\" + currentProfile + "\\" + File2);
            XmlSerializer serializer5 = new XmlSerializer(typeof(Schedule));
            serializer5.Serialize(FileStream5, schedule);
            FileStream5.Close();
            Stream FileStream6 = File.OpenWrite(Application.StartupPath + "\\" + currentProfile + "\\" + File3);
            XmlSerializer serializer6 = new XmlSerializer(datesHolydays.GetType());
            serializer6.Serialize(FileStream6, datesHolydays);
            FileStream6.Close();
        }
        void delProfile()
        {
            if (File.Exists(Application.StartupPath + "\\" + currentProfile + "\\" + File2) && File.Exists(Application.StartupPath + "\\" + currentProfile + "\\" + "dicsiplines.xls") && File.Exists(Application.StartupPath + "\\" + currentProfile + "\\" + File3))
            {
                File.Delete(Application.StartupPath + "\\" + currentProfile + "\\" + "disciplines.xls");
                File.Delete(Application.StartupPath + "\\" + currentProfile + "\\" + File2);
                File.Delete(Application.StartupPath + "\\" + currentProfile + "\\" + File3);
                Directory.Delete(Application.StartupPath + "\\" + currentProfile);
            }
            profilesClass.profiles.Remove(currentProfile);
            comboBoxProfiles.Items.Remove(currentProfile);
            if (comboBoxProfiles.Items.Count == 0)
                addProfile();
            else
                currentProfile = comboBoxProfiles.Items[0].ToString();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stream FileStream0 = File.Create(Application.StartupPath + "\\" + File0);
            XmlSerializer serializer0 = new XmlSerializer(profilesClass.profiles.GetType());
            serializer0.Serialize(FileStream0, profilesClass.profiles);
            FileStream0.Close();
            schedule.allSubjects = subjects[1].Items.Cast<string>().ToList();
            holydaysControlsToData();
            datesControlsToData();
            //disciplineControlsToData();

            //File.Delete(Application.StartupPath + "\\" + currentProfile + "\\" + File1);
            //Stream FileStream4 = File.OpenWrite(Application.StartupPath + "\\" + currentProfile + "\\" + File1);
            //XmlSerializer serializer4 = new XmlSerializer(disciplines.GetType());
            //serializer4.Serialize(FileStream4, disciplines);
            //FileStream4.Close();

            saveToXls();

            File.Delete(Application.StartupPath + "\\" + currentProfile + "\\" + File2);
            Stream FileStream5 = File.OpenWrite(Application.StartupPath + "\\" + currentProfile + "\\" + File2);
            XmlSerializer serializer5 = new XmlSerializer(typeof(Schedule));
            serializer5.Serialize(FileStream5, schedule);
            FileStream5.Close();
            File.Delete(Application.StartupPath + "\\" + currentProfile + "\\" + File3);
            Stream FileStream6 = File.OpenWrite(Application.StartupPath + "\\" + currentProfile + "\\" + File3);
            XmlSerializer serializer6 = new XmlSerializer(datesHolydays.GetType());
            serializer6.Serialize(FileStream6, datesHolydays);
            FileStream6.Close();
        }
        void loadDefaultData()
        {
            panelDisciplines.Controls.Clear();
            disciplinesControls.Clear();
            panelHolydays.Controls.Clear();
            holydaysControls.Clear();
            datesHolydays.holydays.Clear();
            disciplines.disciplineList.Clear();
            addHolyday(new DateTime(2014, 1, 1, 0, 0, 0, 0));
            addHolyday(new DateTime(2014, 3, 8, 17, 3, 0, 0));
            addHolyday(new DateTime(2014, 5, 23, 17, 3, 0, 0));
            datesHolydays.startStuding = pickerStartStuding.Value;
            datesHolydays.endStuding = pickerEndStuding.Value;
            addDisciplineControl("-=введіть назву=-", "---");
            for (int i = 0; i < subjects.Count; i++)
            {
                subjects[i].Items.Clear();
                subjects[i].Items.Add("---");
            }
        }
        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            if (!start)
            {
                start = false;
                //серіалізуємо теперішній профіль
                Stream FileStream0 = File.Create(Application.StartupPath + "\\" + File0);
                XmlSerializer serializer0 = new XmlSerializer(profilesClass.profiles.GetType());
                serializer0.Serialize(FileStream0, profilesClass.profiles);
                FileStream0.Close();
                schedule.allSubjects = subjects[1].Items.Cast<string>().ToList();
                holydaysControlsToData();
                datesControlsToData();
                //disciplineControlsToData();

                //if (File.Exists(Application.StartupPath + "\\" + currentProfile + "\\" + File1))
                //    File.Delete(Application.StartupPath + "\\" + currentProfile + "\\" + File1);
                //Stream FileStream4 = File.OpenWrite(Application.StartupPath + "\\" + currentProfile + "\\" + File1);
                //XmlSerializer serializer4 = new XmlSerializer(disciplines.GetType());
                //serializer4.Serialize(FileStream4, disciplines);
                //FileStream4.Close();

                saveToXls();

                if (File.Exists(Application.StartupPath + "\\" + currentProfile + "\\" + File2))
                    File.Delete(Application.StartupPath + "\\" + currentProfile + "\\" + File2);
                Stream FileStream5 = File.OpenWrite(Application.StartupPath + "\\" + currentProfile + "\\" + File2);
                XmlSerializer serializer5 = new XmlSerializer(typeof(Schedule));
                serializer5.Serialize(FileStream5, schedule);
                FileStream5.Close();
                if (File.Exists(Application.StartupPath + "\\" + currentProfile + "\\" + File3))
                    File.Delete(Application.StartupPath + "\\" + currentProfile + "\\" + File3);
                Stream FileStream6 = File.OpenWrite(Application.StartupPath + "\\" + currentProfile + "\\" + File3);
                XmlSerializer serializer6 = new XmlSerializer(datesHolydays.GetType());
                serializer6.Serialize(FileStream6, datesHolydays);
                FileStream6.Close();
            }
            
            //відновлюємо вибраний профіль
            ComboBox combobox = (ComboBox)sender;
            currentProfile = combobox.SelectedItem.ToString();
            if (File.Exists(Application.StartupPath + "\\" + currentProfile + "\\" + File2) && File.Exists(Application.StartupPath + "\\" + currentProfile + "\\" + File3))
            {
                //Stream FileStream1 = File.OpenRead(Application.StartupPath + "\\" + currentProfile + "\\" + File1);
                //XmlSerializer deserializer1 = new XmlSerializer(disciplines.GetType());
                //disciplines = (Disciplines)deserializer1.Deserialize(FileStream1);
                //FileStream1.Close();
                loadFromXls();

                Stream FileStream2 = File.OpenRead(Application.StartupPath + "\\" + currentProfile + "\\" + File2);
                XmlSerializer deserializer2 = new XmlSerializer(schedule.GetType());
                schedule = (Schedule)deserializer2.Deserialize(FileStream2);
                FileStream2.Close();
                Stream FileStream3 = File.OpenRead(Application.StartupPath + "\\" + currentProfile + "\\" + File3);
                XmlSerializer deserializer3 = new XmlSerializer(datesHolydays.GetType());
                datesHolydays = (DatesHolydays)deserializer3.Deserialize(FileStream3);
                FileStream3.Close();
                datesDataToControls();
                holydaysDataToControls();
                disciplinesDataToControls();
                this.Refresh();
            }
            else
                loadDefaultData();
            refreshScheduleAllCollections();
            this.Refresh();
            pictureBox1.Visible = false;
        }


        // --------------------- Exporting
        void saveToXls()
        {
            pictureBox1.Visible = true;
            if (!String.IsNullOrEmpty(currentProfile))
            {
                string fileName = Application.StartupPath + "\\" + currentProfile + "\\" + "disciplines.xls";
                if (File.Exists(fileName))
                    File.Delete(fileName);
                File.Create(fileName).Close();

                Excel.Application excelApp = new Excel.Application();
                excelApp.DisplayAlerts = false;
                excelApp.Workbooks.Add();
                Excel.Workbook excelappworkbook = excelApp.Workbooks.get_Item(1);
                Excel.Worksheet excelWorkSheet = excelappworkbook.Sheets.get_Item(1);

                Excel.Range currentRange = excelWorkSheet.get_Range("H7");
                currentRange.Value2 = "Лекції";
                currentRange = excelWorkSheet.get_Range("I7");
                currentRange.Value2 = "Практичні зан.";
                currentRange = excelWorkSheet.get_Range("J7");
                currentRange.Value2 = "Семінарські";
                currentRange = excelWorkSheet.get_Range("K7");
                currentRange.Value2 = "Лабораторні";
                currentRange = excelWorkSheet.get_Range("L7");
                currentRange.Value2 = "Інд. робота";
                currentRange = excelWorkSheet.get_Range("M7");
                currentRange.Value2 = "Самостійна";
                currentRange = excelWorkSheet.get_Range("N7");
                currentRange.Value2 = "Конс. з дисц.";
                currentRange = excelWorkSheet.get_Range("O7");
                currentRange.Value2 = "Екзамени";
                currentRange = excelWorkSheet.get_Range("P7");
                currentRange.Value2 = "Конс. до екз.";
                currentRange = excelWorkSheet.get_Range("Q7");
                currentRange.Value2 = "Заліки";
                currentRange = excelWorkSheet.get_Range("R7");
                currentRange.Value2 = "Інд.завд./зан.";
                currentRange = excelWorkSheet.get_Range("S7");
                currentRange.Value2 = "Контр. роботи";
                currentRange = excelWorkSheet.get_Range("T7");
                currentRange.Value2 = "Дипл.(Маг) роб";
                currentRange = excelWorkSheet.get_Range("U7");
                currentRange.Value2 = "Керівн. практ.";
                currentRange = excelWorkSheet.get_Range("V7");
                currentRange.Value2 = "Участь в ДЕК";
                currentRange = excelWorkSheet.get_Range("W7");
                currentRange.Value2 = "Скорочення годин";
                currentRange = excelWorkSheet.get_Range("X7");
                currentRange.Value2 = "Навантаження в годинах";
                currentRange = excelWorkSheet.get_Range("A9", "X9");
                currentRange.Merge();
                currentRange.Value2 = "Перше  півріччя";

                int row = 10;
                int col = 2;
                currentRange = (Excel.Range)excelWorkSheet.Cells[row, col];

                Dictionary<string, string> employments = new Dictionary<string, string>();
                employments.Add("H", "Лекції");
                employments.Add("I", "Практичні зан.");
                employments.Add("J", "Семінарські");
                employments.Add("K", "Лабораторні");
                employments.Add("L", "Інд. робота");
                employments.Add("M", "Самостійна");
                employments.Add("N", "Конс. з дисц.");
                employments.Add("O", "Екзамени");
                employments.Add("P", "Конс. до екз.");
                employments.Add("Q", "Заліки");
                employments.Add("R", "Інд.завд./зан.");
                employments.Add("S", "Контр. роботи");
                employments.Add("T", "Дипл.(Маг) роб");
                employments.Add("U", "Керівн. практ.");
                employments.Add("V", "Участь в ДЕК");

                for (int i = 0; i < disciplines.disciplineList.Count; i++)
                {
                    currentRange = excelWorkSheet.get_Range("B" + row);
                    currentRange.Value2 = disciplines.disciplineList[i].discipline;
                    currentRange = excelWorkSheet.get_Range("D" + row);
                    currentRange.Value2 = disciplines.disciplineList[i].group;
                    for (int j = 0; j < disciplines.disciplineList[i].employments.Count; j++)
                    {
                        for (int k = 0; k < employments.Count; k++)
                        {
                            currentRange = excelWorkSheet.get_Range(employments.ElementAt(k).Key + 7);
                            if (disciplines.disciplineList[i].employments[j].type == employments.ElementAt(k).Value)
                            {
                                currentRange = excelWorkSheet.get_Range(employments.ElementAt(k).Key + row);
                                currentRange.Value2 = disciplines.disciplineList[i].employments[j].hoursNeedToTeach;
                                break;
                            }
                        }
                    }
                    row++;
                }
                excelappworkbook.SaveAs(fileName, Excel.XlFileFormat.xlExcel8, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                
                excelApp.Quit();
            }

            pictureBox1.Visible = false;
        }

        // --------------------- Importing
        void loadFromXls()
        {
            pictureBox1.Visible = true;
            string fileName = Application.StartupPath + "\\" + currentProfile + "\\" + "disciplines.xls";
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook excelappworkbook = excelApp.Workbooks.Open(fileName,
                Type.Missing, true, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);
            Excel.Sheets excelsheets = excelappworkbook.Worksheets;
            Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelsheets.get_Item(1);

            string startOn = "Перше  півріччя";
            for (byte i = 0; i < 1; i++)
            {
                int row = 10;
                Excel.Range startAt = excelWorkSheet.Cells.Find(startOn, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSearchDirection.xlNext, Type.Missing, Type.Missing, Type.Missing);
                if (startAt != null)
                {
                    string cellDiscipline;
                    string cellGroup;
                    bool stop = false;
                    do
                    {
                        cellDiscipline = "B" + row;
                        cellGroup = "D" + row;
                        Excel.Range excelRangeDiscipline = excelWorkSheet.get_Range(cellDiscipline, Type.Missing);
                        Excel.Range excelRangeGroup = excelWorkSheet.get_Range(cellGroup, Type.Missing);
                        if (!String.IsNullOrEmpty(Convert.ToString(excelRangeDiscipline.Value2)))
                        {
                            //перевірка на повтори
                            if (disciplines.disciplineList.Count > 0)
                            {
                                if (!disciplines.disciplineList[disciplines.disciplineList.Count - 1].discipline.Equals(Convert.ToString(excelRangeDiscipline.Value2))
                                    && !disciplines.disciplineList[disciplines.disciplineList.Count - 1].group.Equals(Convert.ToString(excelRangeGroup.Value2)))
                                {
                                    disciplines.disciplineList.Add(new DisciplineType()
                                    {
                                        discipline = Convert.ToString(excelRangeDiscipline.Value2),
                                        group = Convert.ToString(excelRangeGroup.Value2),
                                    });
                                }
                            }
                            else
                                disciplines.disciplineList.Add(new DisciplineType()
                                {
                                    discipline = Convert.ToString(excelRangeDiscipline.Value2),
                                    group = Convert.ToString(excelRangeGroup.Value2),
                                });
                            //додаємо години з видами занять, якщо не пусті комірки
                            for (byte j = 72; j < 86; j++)
                            {
                                Excel.Range cell = excelWorkSheet.get_Range(Convert.ToChar(j).ToString() + row, Type.Missing);
                                if (!String.IsNullOrEmpty(Convert.ToString(cell.Value2)))
                                {
                                    bool newEmployment = true;
                                    if (disciplines.disciplineList.Count > 0)
                                    {
                                        for (int k = 0; k < disciplines.disciplineList[disciplines.disciplineList.Count - 1].employments.Count; k++)
                                            if (disciplines.disciplineList[disciplines.disciplineList.Count - 1].employments[k].type == Convert.ToString(excelWorkSheet.get_Range(Convert.ToChar(j).ToString() + 7, Type.Missing).Value2))
                                            {
                                                newEmployment = false;
                                                disciplines.disciplineList[disciplines.disciplineList.Count - 1].employments[k].hoursNeedToTeach += Convert.ToDecimal(cell.Value2);
                                            }
                                        if (newEmployment)
                                            disciplines.disciplineList[disciplines.disciplineList.Count - 1].employments.Add(new Employment()
                                            {
                                                hoursNeedToTeach = Convert.ToDecimal(cell.Value2),
                                                type = Convert.ToString(excelWorkSheet.get_Range(Convert.ToChar(j).ToString() + 7, Type.Missing).Value2),
                                            });
                                    }
                                }
                            }
                        }
                        else
                            stop = true;
                        row++;
                    } while (!stop);
                }
                startOn = "Друге  півріччя";
            }
            excelappworkbook.Close();
            excelApp.Quit();
            
            disciplinesDataToControls();
            refreshScheduleAllCollections();
            pictureBox1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            OpenFileDialog OPF = new OpenFileDialog();
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook excelappworkbook = excelApp.Workbooks.Open(OPF.FileName,
                    Type.Missing, true, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);
                Excel.Sheets excelsheets = excelappworkbook.Worksheets;
                Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelsheets.get_Item(1);

                string startOn = "Перше  півріччя";
                Excel.Range startAt;
                for (byte i = 0; i < 2; i++)
                {
                    int row = 1;
                    startAt = excelWorkSheet.Cells.Find(startOn, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSearchDirection.xlNext, Type.Missing, Type.Missing, Type.Missing);
                    if (startAt != null)
                    {
                        row = startAt.Row + 1;
                        string cellDiscipline;
                        string cellGroup;
                        bool stop = false;
                        do
                        {
                            cellDiscipline = "B" + row;
                            cellGroup = "D" + row;
                            Excel.Range excelRangeDiscipline = excelWorkSheet.get_Range(cellDiscipline, Type.Missing);
                            Excel.Range excelRangeGroup = excelWorkSheet.get_Range(cellGroup, Type.Missing);
                            if (!String.IsNullOrEmpty(Convert.ToString(excelRangeDiscipline.Value2)))
                            {
                                //перевірка на повтори
                                if (disciplines.disciplineList.Count > 0)
                                {
                                    if (!disciplines.disciplineList[disciplines.disciplineList.Count - 1].discipline.Equals(Convert.ToString(excelRangeDiscipline.Value2))
                                        && !disciplines.disciplineList[disciplines.disciplineList.Count - 1].group.Equals(Convert.ToString(excelRangeGroup.Value2)))
                                    {
                                        disciplines.disciplineList.Add(new DisciplineType()
                                        {
                                            discipline = Convert.ToString(excelRangeDiscipline.Value2),
                                            group = Convert.ToString(excelRangeGroup.Value2),
                                        });
                                    }
                                }
                                else
                                    disciplines.disciplineList.Add(new DisciplineType()
                                    {
                                        discipline = Convert.ToString(excelRangeDiscipline.Value2),
                                        group = Convert.ToString(excelRangeGroup.Value2),
                                    });
                                //додаємо години з видами занять, якщо не пусті комірки
                                for (byte j = 72; j < 86; j++)
                                {
                                    Excel.Range cell = excelWorkSheet.get_Range(Convert.ToChar(j).ToString() + row, Type.Missing);
                                    if (!String.IsNullOrEmpty(Convert.ToString(cell.Value2)))
                                    {
                                        bool newEmployment = true;
                                        if (disciplines.disciplineList.Count > 0)
                                        {
                                            for (int k = 0; k < disciplines.disciplineList[disciplines.disciplineList.Count - 1].employments.Count; k++)
                                                if (disciplines.disciplineList[disciplines.disciplineList.Count - 1].employments[k].type == Convert.ToString(excelWorkSheet.get_Range(Convert.ToChar(j).ToString() + 7, Type.Missing).Value2))
                                                {
                                                    newEmployment = false;
                                                    disciplines.disciplineList[disciplines.disciplineList.Count - 1].employments[k].hoursNeedToTeach += Convert.ToDecimal(cell.Value2);
                                                }
                                            if (newEmployment)
                                                disciplines.disciplineList[disciplines.disciplineList.Count - 1].employments.Add(new Employment()
                                                {
                                                    hoursNeedToTeach = Convert.ToDecimal(cell.Value2),
                                                    type = Convert.ToString(excelWorkSheet.get_Range(Convert.ToChar(j).ToString() + 7, Type.Missing).Value2),
                                                });
                                        }
                                    }
                                }
                            }
                            else
                                stop = true;
                            row++;
                        } while (!stop);
                    }
                    startOn = "Друге  півріччя";
                }
                //excelappworkbook.Close();
                excelApp.Quit();
                disciplinesDataToControls();
                refreshScheduleAllCollections();
            }
            pictureBox1.Visible = false;
        }





        // --------------------- Disciplines
        Disciplines disciplines = new Disciplines();
        // Controls
        List<disciplinesControlsClass> disciplinesControls = new List<disciplinesControlsClass>();
        class disciplinesControlsClass
        {
            public TextBox discipline;
            public TextBox group;
            public Button btnSetHours;
        }
        
        private void btnAddDiscipline_Click(object sender, EventArgs e)
        {
            addDisciplineControl();
        }
        void addDisciplineControl()
        {
            int num = disciplinesControls.Count;
            TextBox textBoxGroup = new TextBox();
            // textBoxDiscipline
            TextBox textBoxDiscipline = new TextBox();
            textBoxDiscipline.Location = new System.Drawing.Point(0, num * 25 - panelDisciplines.VerticalScroll.Value);
            textBoxDiscipline.Name = "textBoxDiscipline" + (num + 1);
            textBoxDiscipline.Size = new System.Drawing.Size(320, 20);
            panelDisciplines.Controls.Add(textBoxDiscipline);
            // textBoxGroup
            textBoxGroup.Location = new System.Drawing.Point(325, num * 25 - panelDisciplines.VerticalScroll.Value);
            textBoxGroup.Name = "textBoxGroup" + (num + 1);
            textBoxGroup.Size = new System.Drawing.Size(83, 20);
            panelDisciplines.Controls.Add(textBoxGroup);
            // buttonAddHours
            Button buttonAddHours = new Button();
            buttonAddHours.Location = new System.Drawing.Point(415, num * 25 - panelDisciplines.VerticalScroll.Value);
            buttonAddHours.Name = "btnSetHours" + (num + 1);
            buttonAddHours.Size = new System.Drawing.Size(40, 20);
            buttonAddHours.BackColor = System.Drawing.SystemColors.Control;
            buttonAddHours.BackgroundImage = global::Planning.Properties.Resources.time_add_icon;
            buttonAddHours.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            buttonAddHours.UseVisualStyleBackColor = false;
            buttonAddHours.Click += new System.EventHandler(this.btnSetHours_Click);
            panelDisciplines.Controls.Add(buttonAddHours);
            disciplinesControls.Add(new disciplinesControlsClass()
            {
                discipline = textBoxDiscipline,
                group = textBoxGroup,
                btnSetHours = buttonAddHours,
            });
            disciplines.disciplineList.Add(new DisciplineType() { discipline = textBoxDiscipline.Text, group = textBoxGroup.Text });
        }
        void addDisciplineControl(string disciplineText, string groupText)
        {
            int num = disciplinesControls.Count;
            TextBox textBoxGroup = new TextBox();
            // textBoxGroup
            textBoxGroup.Location = new System.Drawing.Point(325, num * 25 - panelDisciplines.VerticalScroll.Value);
            textBoxGroup.Name = "textBoxGroup" + (num + 1);
            textBoxGroup.Text = groupText;
            textBoxGroup.Size = new System.Drawing.Size(83, 20);
            panelDisciplines.Controls.Add(textBoxGroup);
            // textBoxDiscipline
            TextBox textBoxDiscipline = new TextBox();
            textBoxDiscipline.Location = new System.Drawing.Point(0, num * 25 - panelDisciplines.VerticalScroll.Value);
            textBoxDiscipline.Name = "textBoxDiscipline" + (num + 1);
            textBoxDiscipline.Text = disciplineText;
            textBoxDiscipline.Size = new System.Drawing.Size(320, 20);
            panelDisciplines.Controls.Add(textBoxDiscipline);
            // buttonAddHours
            Button buttonAddHours = new Button();
            buttonAddHours.Location = new System.Drawing.Point(415, num * 25 - panelDisciplines.VerticalScroll.Value);
            buttonAddHours.Name = "btnSetHours" + (num + 1);
            buttonAddHours.Size = new System.Drawing.Size(40, 20);
            buttonAddHours.BackColor = System.Drawing.SystemColors.Control;
            buttonAddHours.BackgroundImage = global::Planning.Properties.Resources.time_add_icon;
            buttonAddHours.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            buttonAddHours.UseVisualStyleBackColor = false;
            buttonAddHours.Click += new System.EventHandler(this.btnSetHours_Click);
            panelDisciplines.Controls.Add(buttonAddHours);
            disciplinesControls.Add(new disciplinesControlsClass()
            {
                discipline = textBoxDiscipline,
                group = textBoxGroup,
                btnSetHours = buttonAddHours,
            });
        }
        void disciplinesDataToControls()
        {
            // clean current disciplineControls
            disciplinesControls.Clear();
            panelDisciplines.Controls.Clear();
            // наповнення новими даними
            int count = disciplines.disciplineList.Count;
            for (int i = 0; i < count; i++)
            {
                string d = disciplines.disciplineList[i].discipline;
                string g = disciplines.disciplineList[i].group;
                addDisciplineControl(d, g);
            }
        }
        bool disciplineControlToData(int i)
        {
            string d = disciplinesControls[i].discipline.Text;
            string g = disciplinesControls[i].group.Text;
            if (d == "" && g == "")
                return true;
            else
                if (i >= disciplines.disciplineList.Count)
                    disciplines.disciplineList.Add(new DisciplineType() { discipline = d, group = g });
                else
                {
                    disciplines.disciplineList[i].discipline = d;
                    disciplines.disciplineList[i].group = g;
                }
            return false;
        }
        //void disciplineControlsToData()
        //{
        //    disciplinesControlsInFormToList();
        //    for (int j = (disciplines.disciplineList.Count - 1); j >=0; j--)
        //        disciplines.disciplineList.RemoveAt(j);
        //    for (int i = 0; i < disciplinesControls.Count; i++)
        //    {
        //        string d = disciplinesControls[i].discipline.Text;
        //        string g = disciplinesControls[i].group.Text;
        //        if (d != "" && g != "")
        //            disciplines.disciplineList.Add(new DisciplineType() { discipline = d, group = g });
        //    }
        //}
        //void disciplinesControlsInFormToList()
        //{
        //    for (int i = 0; i < (panelDisciplines.Controls.Count/3); i++)
        //    {
        //        string currentProfile = "textBoxDiscipline" + (i + 1);
        //        if (this.panelDisciplines.Controls.ContainsKey(Name))
        //            disciplinesControls[i].discipline = (TextBox)this.Controls.Find(Name, true)[0];
        //        currentProfile = "textBoxGroup" + (i + 1);
        //        if (this.panelDisciplines.Controls.ContainsKey(Name))
        //            disciplinesControls[i].group = (TextBox)this.Controls.Find(Name, true)[0];
        //        currentProfile = "btnSetHours" + (i + 1);
        //        if (this.panelDisciplines.Controls.ContainsKey(Name))
        //            disciplinesControls[i].btnSetHours = (Button)this.Controls.Find(Name, true)[0];
        //    }
        //}
        void disciplineControlsToData()
        {
            disciplinesControlsInFormToList();
            for (int j = (disciplines.disciplineList.Count - 1); j >= 0; j--)
                disciplines.disciplineList.RemoveAt(j);
            for (int i = 0; i < disciplinesControls.Count; i++)
            {
                string d = disciplinesControls[i].discipline.Text;
                string g = disciplinesControls[i].group.Text;
                if (d != "" && g != "")
                    disciplines.disciplineList.Add(new DisciplineType() { discipline = d, group = g });
            }
        }
        void disciplinesControlsInFormToList()
        {
            string Name;
            for (int i = 0; i < (panelDisciplines.Controls.Count / 3); i++)
            {
                Name = "textBoxDiscipline" + (i + 1);
                if (this.panelDisciplines.Controls.ContainsKey(Name))
                    disciplinesControls[i].discipline = (TextBox)this.Controls.Find(Name, true)[0];
                Name = "textBoxGroup" + (i + 1);
                if (this.panelDisciplines.Controls.ContainsKey(Name))
                    disciplinesControls[i].group = (TextBox)this.Controls.Find(Name, true)[0];
                Name = "btnSetHours" + (i + 1);
                if (this.panelDisciplines.Controls.ContainsKey(Name))
                    disciplinesControls[i].btnSetHours = (Button)this.Controls.Find(Name, true)[0];
            }
        }
        
        private void btnSetHours_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int index = getIndexOfDisciplineControl(btn);
            if (!disciplineControlToData(index))
            {
                DetailOfDescipline f = new DetailOfDescipline(disciplines.disciplineList, index);
                f.Owner = this;
                f.ShowDialog();
                refreshScheduleCollection(index);  
            }
        }
        int getIndexOfDisciplineControl(Control ctr)
        {
            for (int i = 0; i < disciplinesControls.Count; i++)
            {
                if (ctr is Button)
                    if (ctr.Name == disciplinesControls[i].btnSetHours.Name)
                        return i;
                if (ctr is TextBox)
                    if (ctr.Name == disciplinesControls[i].discipline.Name || ctr.Name == disciplinesControls[i].group.Name)
                        return i;
            }
            return -1;
        }
        void refreshScheduleCollection(int index)
        {
            for (int k = 0; k < subjects.Count; k++)
            {
                subjects[k].Items.Clear();
            }
            for (int i = 0; i < disciplines.disciplineList[index].employments.Count; i++)
                for (int j = 0; j < subjects.Count; j++)
                {
                    string str = disciplines.disciplineList[index].discipline + "-" + disciplines.disciplineList[index].group + "-" + disciplines.disciplineList[index].employments[i].type;
                    // Видаляємо всі елементи колекції що належали дисципліні попередньої редакції
                    //for (int k = 0; k < subjects[j].Items.Count; k++)
                    //{
                    //    string elOfItems = subjects[j].Items[k].ToString();
                    //    if (elOfItems.Contains(disciplines.disciplineList[index].discipline))
                    //        subjects[j].Items.RemoveAt(k);
                    //}
                    if (!subjects[j].Items.Contains(str))
                        subjects[j].Items.Add(str);
                }
        }
        void refreshScheduleAllCollections()
        {
            for (int q = 0; q < subjects.Count; q++)
            {
                subjects[q].Items.Clear();
            }
            for (int index = 0; index < disciplines.disciplineList.Count; index++)
            {
                for (int i = 0; i < disciplines.disciplineList[index].employments.Count; i++)
                {
                    for (int j = 0; j < subjects.Count; j++)
                    {
                        subjects[j].Items.Add(disciplines.disciplineList[index].discipline + "-" + disciplines.disciplineList[index].group + "-" + disciplines.disciplineList[index].employments[i].type);
                    }
                }
            }
        }
        void refreshScheduleCollection(string text)
        {
            for (int q = 0; q < subjects.Count; q++)
            {
                subjects[q].Items.Clear();
            }
            for (int index = 0; index < disciplines.disciplineList.Count; index++)
            {
                for (int i = 0; i < disciplines.disciplineList[index].employments.Count; i++)
                {
                    for (int j = 0; j < subjects.Count; j++)
                    {
                        subjects[j].Items.Add(disciplines.disciplineList[index].discipline + "-" + disciplines.disciplineList[index].group + "-" + disciplines.disciplineList[index].employments[i].type);
                    }
                }
            }
        }

        // ---------------------------- Holydays, Start-end
        DatesHolydays datesHolydays = new DatesHolydays();
        List<DateTimePicker> holydaysControls = new List<DateTimePicker>();

        void datesControlsToData()
        {
            datesHolydays.startStuding = pickerStartStuding.Value;
            datesHolydays.endStuding = pickerEndStuding.Value;
        }
        void datesDataToControls()
        {
            pickerStartStuding.Value = datesHolydays.startStuding;
            pickerEndStuding.Value = datesHolydays.endStuding;
        }

        void holydaysControlsToData()
        {
            for (int i = (datesHolydays.holydays.Count - 1); i >= 0; i--)
                datesHolydays.holydays.RemoveAt(i);
            for (int i = 0; i < holydaysControls.Count; i++)
                datesHolydays.holydays.Add(holydaysControls[i].Value);
        }
        void holydaysDataToControls()
        {
            for (int i = (holydaysControls.Count - 1); i >= 0; i--)
                holydaysControls.RemoveAt(i);
            panelHolydays.Controls.Clear();
            for (int i = 0; i < datesHolydays.holydays.Count; i++)
                addHolyday(datesHolydays.holydays[i]);            
        }
        void addHolyday()
        {
            int num = holydaysControls.Count();
            DateTimePicker dateTimePicker = new DateTimePicker();
            dateTimePicker.Location = new System.Drawing.Point(0, num * 25 - panelHolydays.VerticalScroll.Value);
            dateTimePicker.Name = "dateTimePicker" + (num + 1);
            dateTimePicker.ShowCheckBox = true;
            dateTimePicker.Size = new System.Drawing.Size(165, 20);
            dateTimePicker.TabIndex = 0;
            dateTimePicker.Value = new System.DateTime(2014, 3, 8, 17, 3, 0, 0);
            holydaysControls.Add(dateTimePicker);
            panelHolydays.Controls.Add(dateTimePicker);
        }
        void addHolyday(DateTime dataTime)
        {
            int num = holydaysControls.Count();
            DateTimePicker dateTimePicker = new DateTimePicker();
            dateTimePicker.Location = new System.Drawing.Point(0, num * 25 - panelHolydays.VerticalScroll.Value);
            dateTimePicker.Name = "dateTimePicker" + (num + 1);
            dateTimePicker.ShowCheckBox = true;
            dateTimePicker.Size = new System.Drawing.Size(165, 20);
            dateTimePicker.TabIndex = 0;
            dateTimePicker.Value = dataTime;
            holydaysControls.Add(dateTimePicker);
            panelHolydays.Controls.Add(dateTimePicker);
        }

        private void addHolyday_Click(object sender, EventArgs e)
        {
            addHolyday();
        }
        private void pickerEndStuding_ValueChanged_1(object sender, EventArgs e)
        {
            datesHolydays.endStuding = pickerEndStuding.Value;
        }
        private void pickerStartStuding_ValueChanged(object sender, EventArgs e)
        {
            datesHolydays.startStuding = pickerStartStuding.Value;
        }


        // ----------------------------- Schedule
        Schedule schedule = new Schedule();
        public List<ComboBox> subjects = new List<ComboBox>();
        private void addDaysToList()
        {
            subjects.Add(monday1);
            subjects.Add(monday2);
            subjects.Add(monday3);
            subjects.Add(monday4);
            subjects.Add(monday5);
            subjects.Add(monday6);
            subjects.Add(monday7);
            subjects.Add(monday8);
            subjects.Add(tuesday1);
            subjects.Add(tuesday2);
            subjects.Add(tuesday3);
            subjects.Add(tuesday4);
            subjects.Add(tuesday5);
            subjects.Add(tuesday6);
            subjects.Add(tuesday7);
            subjects.Add(tuesday8);
            subjects.Add(wednesday1);
            subjects.Add(wednesday2);
            subjects.Add(wednesday3);
            subjects.Add(wednesday4);
            subjects.Add(wednesday5);
            subjects.Add(wednesday6);
            subjects.Add(wednesday7);
            subjects.Add(wednesday8);
            subjects.Add(thursday1);
            subjects.Add(thursday2);
            subjects.Add(thursday3);
            subjects.Add(thursday4);
            subjects.Add(thursday5);
            subjects.Add(thursday6);
            subjects.Add(thursday7);
            subjects.Add(thursday8);
            subjects.Add(friday1);
            subjects.Add(friday2);
            subjects.Add(friday3);
            subjects.Add(friday4);
            subjects.Add(friday5);
            subjects.Add(friday6);
            subjects.Add(friday7);
            subjects.Add(friday8);

            subjects.Add(monday9);
            subjects.Add(monday10);
            subjects.Add(monday11);
            subjects.Add(monday12);
            subjects.Add(monday13);
            subjects.Add(monday14);
            subjects.Add(monday15);
            subjects.Add(monday16);
            subjects.Add(tuesday9);
            subjects.Add(tuesday10);
            subjects.Add(tuesday11);
            subjects.Add(tuesday12);
            subjects.Add(tuesday13);
            subjects.Add(tuesday14);
            subjects.Add(tuesday15);
            subjects.Add(tuesday16);
            subjects.Add(wednesday9);
            subjects.Add(wednesday10);
            subjects.Add(wednesday11);
            subjects.Add(wednesday12);
            subjects.Add(wednesday13);
            subjects.Add(wednesday14);
            subjects.Add(wednesday15);
            subjects.Add(wednesday16);
            subjects.Add(thursday9);
            subjects.Add(thursday10);
            subjects.Add(thursday11);
            subjects.Add(thursday12);
            subjects.Add(thursday13);
            subjects.Add(thursday14);
            subjects.Add(thursday15);
            subjects.Add(thursday16);
            subjects.Add(friday9);
            subjects.Add(friday10);
            subjects.Add(friday11);
            subjects.Add(friday12);
            subjects.Add(friday13);
            subjects.Add(friday14);
            subjects.Add(friday15);
            subjects.Add(friday16);

            for (int i = 0; i < subjects.Count; i++)
                subjects[i].Items.Add("---");
        }
        private void btnCopyToDenominator_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 39; i++)
            {
                int j = i+40;
                subjects[j].Text = subjects[i].Text;
            }
        }
        void scheduleDataToControls()
        {
            for (int i = 0; i < schedule.allSubjects.Count; i++)
            {
                refreshScheduleCollection(schedule.allSubjects[i].ToString());
            }
        }


        // ПОШУК КІЛЬКОСТІ ГОДИН ЗА РОЗКЛАДОМ
        public string getHoursOfYear(DisciplineType discipline, int employmentIndex)
        {
            int MaxEmploymentIndex = discipline.employments.Count;
            decimal allHours = discipline.allHours;
            List<int> hoursForDaysOfWeek = new List<int>() { 0, 0, 0, 0, 0, 0, 0, };
            // calculate hours for each day of week from schedule
            for (int i = 0; i < 7; i++)
                if (subjects[i].Text == discipline.discipline + "-" + discipline.group + "-" + discipline.employments[employmentIndex].type)
                    discipline.employments[employmentIndex].hoursForWeek[0]++;
            for (int i = 8; i < 15; i++)
                if (subjects[i].Text == discipline.discipline + "-" + discipline.group + "-" + discipline.employments[employmentIndex].type)
                    discipline.employments[employmentIndex].hoursForWeek[1]++;
            for (int i = 16; i < 23; i++)
                if (subjects[i].Text == discipline.discipline + "-" + discipline.group + "-" + discipline.employments[employmentIndex].type)
                    discipline.employments[employmentIndex].hoursForWeek[2]++;
            for (int i = 24; i < 31; i++)
                if (subjects[i].Text == discipline.discipline + "-" + discipline.group + "-" + discipline.employments[employmentIndex].type)
                    discipline.employments[employmentIndex].hoursForWeek[3]++;
            for (int i = 32; i < 39; i++)
                if (subjects[i].Text == discipline.discipline + "-" + discipline.group + "-" + discipline.employments[employmentIndex].type)
                    discipline.employments[employmentIndex].hoursForWeek[4]++;
            for (int i = 40; i < 47; i++)
                if (subjects[i].Text == discipline.discipline + "-" + discipline.group + "-" + discipline.employments[employmentIndex].type)
                    discipline.employments[employmentIndex].hoursForWeek[0]++;
            for (int i = 48; i < 55; i++)
                if (subjects[i].Text == discipline.discipline + "-" + discipline.group + "-" + discipline.employments[employmentIndex].type)
                    discipline.employments[employmentIndex].hoursForWeek[1]++;
            for (int i = 56; i < 63; i++)
                if (subjects[i].Text == discipline.discipline + "-" + discipline.group + "-" + discipline.employments[employmentIndex].type)
                    discipline.employments[employmentIndex].hoursForWeek[2]++;
            for (int i = 64; i < 71; i++)
                if (subjects[i].Text == discipline.discipline + "-" + discipline.group + "-" + discipline.employments[employmentIndex].type)
                    discipline.employments[employmentIndex].hoursForWeek[3]++;
            for (int i = 72; i < 79; i++)
                if (subjects[i].Text == discipline.discipline + "-" + discipline.group + "-" + discipline.employments[employmentIndex].type)
                    discipline.employments[employmentIndex].hoursForWeek[4]++;
            
            DateTime tempDate = datesHolydays.startStuding;
            while (tempDate != datesHolydays.endStuding || discipline.employments[employmentIndex].hoursNeedToTeach != 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    bool itsHolyday = false;
                    // This day is holyday?
                    for (int ind = 0; ind < holydaysControls.Count; ind++)
                        if (tempDate == holydaysControls[ind].Value) itsHolyday = true;
                    // If isn't holyday, then...
                    if (!itsHolyday)
                    {
                        discipline.employments[employmentIndex].hoursNeedToTeach -= discipline.employments[employmentIndex].hoursForWeek[i];
                        if (tempDate == datesHolydays.endStuding || discipline.employments[employmentIndex].hoursNeedToTeach <= 0) break;
                        tempDate = tempDate.Add(new TimeSpan(1, 0, 0, 0));
                        if (tempDate == datesHolydays.endStuding || discipline.employments[employmentIndex].hoursNeedToTeach <= 0) break;
                    }
                }
                if (tempDate == datesHolydays.endStuding || discipline.employments[employmentIndex].hoursNeedToTeach <= 0) break;
            }
            if (tempDate == datesHolydays.endStuding)
            {
                if (discipline.employments[employmentIndex].hoursNeedToTeach == 1) return "НЕ вистачає годин для опрацювання плану. Залишилось " + discipline.employments[employmentIndex].hoursNeedToTeach + " година";
                if (discipline.employments[employmentIndex].hoursNeedToTeach >= 2 && discipline.employments[employmentIndex].hoursNeedToTeach <= 4) return "НЕ вистачає годин для опрацювання плану. Залишилось " + discipline.employments[employmentIndex].hoursNeedToTeach + " години";
                else return "НЕ вистачає годин для опрацювання плану. Залишилось " + discipline.employments[employmentIndex].hoursNeedToTeach + " годин";
            }
            else
                return "Для опрацювання плану вистачає годин за розкладом. Останнє заняття відбудеться: " + tempDate + ".";




            //while (tempDate != datesHolydays.endStuding || discipline.allHours != 0)
            //{
            //    for (int i = 0; i < 6; i++)
            //    {
            //        bool itsHolyday = false;
            //        // This day is holyday?
            //        for (int ind = 0; ind < holydaysControls.Count; ind++)
            //            if (tempDate == holydaysControls[ind].Value) itsHolyday = true;
            //        // If isn't holyday, then...
            //        if (!itsHolyday)
            //        {
            //            discipline.employments[employmentIndex].hoursNeedToTeach -= discipline.employments[employmentIndex].hoursForWeek[i];
            //            if (tempDate == datesHolydays.endStuding || discipline.allHours == 0) break;
            //            tempDate = tempDate.Add(new TimeSpan(1, 0, 0, 0));
            //            if (tempDate == datesHolydays.endStuding || discipline.allHours == 0) break;
            //        }
            //    }
            //    if (tempDate == datesHolydays.endStuding || discipline.allHours == 0) break;
            //}
            //if (tempDate == datesHolydays.endStuding)
            //{
            //    if (discipline.allHours == 1) return "НЕ вистачає годин для опрацювання плану. Залишилось " + discipline.allHours + " зайва година";
            //    if (discipline.allHours >= 2 && discipline.allHours <= 4) return "НЕ вистачає годин для опрацювання плану. Залишилось " + discipline.allHours + " зайві години";
            //    else return "НЕ вистачає годин для опрацювання плану. Залишилось " + discipline.allHours + " зайвих годин";
            //}
            //else
            //    return "Для опрацювання плану вистачає годин за розкладом. Останнє заняття відбудеться: " + tempDate + ".";
        }

        // numerator and denominator in schedule
        public void changeWeekSchedule(string typeOfWeek)
        {
            if (typeOfWeek == "numerator")
            {
                tabGroupDenominator.Visible = false;
                tabGroupNumerator.Visible = true;
            }
            else
            {
                tabGroupNumerator.Visible = false;
                tabGroupDenominator.Visible = true;
            }
        }


        /* ---------------EVENTS--------------- */
        private void startStuding_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker newDate = (DateTimePicker) sender;
            datesHolydays.startStuding = newDate.Value;
        }

        

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            holydaysControlsToData();
            string textBox = "";
            for (int i = 0; i < disciplines.disciplineList.Count; i++)
            {
                for (int j = 0; j < disciplines.disciplineList[i].employments.Count; j++)
                {
                    textBox += ("---------------------------------------------------\n");
                    textBox += ("Спеціальність:" + disciplines.disciplineList[i].discipline + "\n");
                    textBox += ("              Група: " + disciplines.disciplineList[i].group + "\n");
                    textBox += ("        Вид робіт: " + disciplines.disciplineList[i].employments[j].type + "\n");
                    textBox += getHoursOfYear(disciplines.disciplineList[i], j) + "\n";
                }
            }
            Results resultForm = new Results(textBox);
            resultForm.Owner = this;
            resultForm.ShowDialog();
        }

        private void radioButtonNumerator_CheckedChanged(object sender, EventArgs e)
        {
            changeWeekSchedule("numerator");
        }

        private void radioButtonDenominator_CheckedChanged(object sender, EventArgs e)
        {
            changeWeekSchedule("denominator");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void BtnProfileAdd_Click(object sender, EventArgs e)
        {
            addProfile();
        }

        private void BtnProfileDel_Click(object sender, EventArgs e)
        {
            delProfile();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            loadDefaultData();
            Stream FileStream0 = File.Create(Application.StartupPath + "\\" + File0);
            XmlSerializer serializer0 = new XmlSerializer(profilesClass.profiles.GetType());
            serializer0.Serialize(FileStream0, profilesClass.profiles);
            FileStream0.Close();
            schedule.allSubjects = subjects[1].Items.Cast<string>().ToList();
            holydaysControlsToData();
            datesControlsToData();
            //disciplineControlsToData();

            //File.Delete(Application.StartupPath + "\\" + currentProfile + "\\" + File1);
            //Stream FileStream4 = File.OpenWrite(Application.StartupPath + "\\" + currentProfile + "\\" + File1);
            //XmlSerializer serializer4 = new XmlSerializer(disciplines.GetType());
            //serializer4.Serialize(FileStream4, disciplines);
            //FileStream4.Close();
            saveToXls();

            File.Delete(Application.StartupPath + "\\" + currentProfile + "\\" + File2);
            Stream FileStream5 = File.OpenWrite(Application.StartupPath + "\\" + currentProfile + "\\" + File2);
            XmlSerializer serializer5 = new XmlSerializer(typeof(Schedule));
            serializer5.Serialize(FileStream5, schedule);
            FileStream5.Close();
            File.Delete(Application.StartupPath + "\\" + currentProfile + "\\" + File3);
            Stream FileStream6 = File.OpenWrite(Application.StartupPath + "\\" + currentProfile + "\\" + File3);
            XmlSerializer serializer6 = new XmlSerializer(datesHolydays.GetType());
            serializer6.Serialize(FileStream6, datesHolydays);
            FileStream6.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 newform = new Form2();
            newform.ShowDialog();
        }

        public string cellDiscipline { get; set; }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void tuesday7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tuesday6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tuesday5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void tuesday8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSaveToXls_Click_1(object sender, EventArgs e)
        {
            saveToXls();
        }

        private void btnLoadFromXls_Click(object sender, EventArgs e)
        {
            loadFromXls();
        }

        private void btnSetProfile_Click(object sender, EventArgs e)
        {
            currentProfile = profilesClass.profiles[0];
        }
    }
}
