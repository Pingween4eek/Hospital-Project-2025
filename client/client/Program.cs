using System;
using System.Collections.Generic;
using System.Drawing; // Для работы с изображениями
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Linq;
// класс для считывания hex-кодов



public class HexToColor
{

    public HexToColor()
    {
    }

    public static Color HexStringToColor(string hexColor)
    {
        string hc = ExtractHexDigits(hexColor);
        if (hc.Length != 6)
        {
            return Color.Empty;
        }
        string r = hc.Substring(0, 2);
        string g = hc.Substring(2, 2);
        string b = hc.Substring(4, 2);
        Color color = Color.Empty;
        try
        {
            int ri
               = Int32.Parse(r, System.Globalization.NumberStyles.HexNumber);
            int gi
               = Int32.Parse(g, System.Globalization.NumberStyles.HexNumber);
            int bi
               = Int32.Parse(b, System.Globalization.NumberStyles.HexNumber);
            color = Color.FromArgb(ri, gi, bi);
        }
        catch
        {
            return Color.Empty;
        }
        return color;
    }

    public static string ExtractHexDigits(string input)
    {
        Regex isHexDigit
           = new Regex("[abcdefABCDEF\\d]+", RegexOptions.Compiled);
        string newnum = "";
        foreach (char c in input)
        {
            if (isHexDigit.IsMatch(c.ToString()))
                newnum += c.ToString();
        }
        return newnum;
    }

}

namespace WindowsFormsApp1
{
    public class MainForm : Form
    {
        private Button startButton;
        private Button exitButton;

        // универсальная кнопка выхода
        private Button universalBack;

        // кнопки на главном экране
        private Button patientActions;
        private Button timeActions;

        // кнопки работы с пациентами
        private Button createPatientList;
        private Button searchPatient;
        private Button addPatient;
        private Button deletePatient;
        private Button printPatients;
        private Button exitButton2;

        //кнопки в скипе дней
        private Button senddaysButton;
        private Button daysexitButton;
        private TrackBar numberTrackBar; // Ползунок
        private Label valueLabel;        // Метка для отображения значения
        private Label countofdays;

        //кнопки для креатива
        private Button createsend1;
        private Label createlabel1;
        private TextBox createtextbox1;
        private Label errorLabel;
        private Label numberOfPatients;

        //кнопки для ввода данных пациента
        private Label patientNumber;
        private Label patientCounter;

        private Label enterId;
        private Label enterName;
        private Label enterSurname;
        private Label enterGender;
        private Label enterAge;
        private Label enterDiagnosis;
        private Label enterStatus;
        private Label enterDoctor;
        private Label enterDepartment;
        private Label enterDays;

        private Label errorEnterPatient;

        private TextBox enterTextId;
        private TextBox enterTextName;
        private TextBox enterTextSurname;
        private TextBox enterTextGender;
        private TextBox enterTextAge;
        private TextBox enterTextDiagnosis;
        private TextBox enterTextStatus;
        private TextBox enterTextDoctor;
        private TextBox enterTextDepartment;
        private TextBox enterTextDays;

        private Button sendPatientInfo;

        //кнопки для работы с поиском
        private Button send;
        private Button exitButton3;
        private Label label_fio;
        private TextBox textbox_fio;
        private Label label_id;
        private Label label_name;
        private Label label_fio_2;
        private Label label_id_otvet;
        private Label label_name_otvet;
        private Label label_fio_2_otvet;
        private Label label_no_patient;

        private Label otvet_id;
        private Label otvet_name;
        private Label otvet_surname;
        private Label otvet_gender;
        private Label otvet_age;
        private Label otvet_diagnosis;
        private Label otvet_status;
        private Label otvet_doctor;
        private Label otvet_department;
        private Label otvet_days;


        //delete
        private Button send_delete;

        // название проги
        private Label name;

        // Для списка (тестово)
        public ListBox spisok;

        private static TcpClient client = new TcpClient("127.0.0.1", 8080);
        private NetworkStream stream = client.GetStream();

        public MainForm()
        {

            // Устанавливаем заголовок и размеры окна
            this.Text = "Пример интерфейса с изображением";
            this.Size = new System.Drawing.Size(1000, 600);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.MaximumSize = new System.Drawing.Size(1000, 600);

            startButton = new Button();
            startButton.Location = new System.Drawing.Point(340, 250);
            startButton.Width = 300;
            startButton.Height = 50;
            this.Controls.Add(startButton);
            startButton.Text = "START";
            startButton.Font = new Font("Century", 25, FontStyle.Bold);
            startButton.Click += Start_Programm;
            startButton.ForeColor = Color.White;
            //startButton.BackColor = System.Drawing.Color.Red;

            this.Resize += (sender, e) => CenterButton();

            try
            {
                startButton.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                startButton.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                startButton.BackColor = HexToColor.HexStringToColor("#ffd059");
            }

            spisok = new ListBox()
            {
                FormattingEnabled = true,
                ItemHeight = 20,
                Location = new System.Drawing.Point(32, 134),
                Name = "ListPatient",
                Size = new System.Drawing.Size(733, 304),
                TabIndex = 4,
            };
            // Заголовок HOSPITAL BOLNITSA
            name = new Label()
            {
                Text = "HOSPITAL BOLNITSA",
                Font = new Font("Century", 52, FontStyle.Bold), //шрифт
                ForeColor = Color.White, //цвет текста
                BackColor = Color.Transparent, //прозрачный фон
                TextAlign = ContentAlignment.TopCenter, //выравниваем по центру
                Location = new Point(50, 20), //ставим в точке (50, 50)
                Size = new Size(900, 110), //рамзер
            };
            this.Controls.Add(name); //добавление на экран

            // кнопка выхода из проги
            exitButton = new Button()
            {
                Text = "ВЫХОД",
                Font = new Font("Century", 24, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.White,
                Location = new Point(385, 500),
                Size = new Size(200, 50),
            };
            exitButton.Click += Exit_Programm; // функция, закрывающая прогу
            this.Controls.Add(exitButton);

            // кнопка, возвращающая на экран выбора действия
            exitButton2 = new Button()
            {
                Text = "НАЗАД",
                Font = new Font("Century", 24, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.White,
                Location = new Point(385, 500),
                Size = new Size(200, 50),
            };
            exitButton2.Click += Back; // функция, возвращающая на экран выбора действия

            exitButton3 = new Button()
            {
                Text = "НАЗАД",
                Font = new Font("Century", 24, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.White,
                Location = new Point(525, 500),
                Size = new Size(200, 50),
            };
            exitButton3.Click += Back2; // функция, возвращающая на экран выбора действия

            // кнопка "Работа с пациентами"
            patientActions = new Button()
            {
                Text = "Работа с пациентами",
                Font = new Font("Century", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(100, 250),
                Size = new Size(375, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            try
            {
                patientActions.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                patientActions.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                patientActions.BackColor = HexToColor.HexStringToColor("#69a0c7");
            }
            patientActions.Click += Patient_Actions; // функция, удаляющая элементы экрана "Выбор действия" и добавляющая элементы экрана "Работа с пациентами"

            daysexitButton = new Button()
            {
                Text = "Назад",
                Font = new Font("Century", 24, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#2e2e2e"),
                BackColor = HexToColor.HexStringToColor("#c23838"),
                Location = new Point(385, 500),
                Size = new Size(200, 50),
            };
            daysexitButton.Click += day_Back; // функция, возвращающая на экран выбора действия

            // кнопка "Работа со временем"
            timeActions = new Button()
            {
                Text = "Промотать время",
                Font = new Font("Century", 24, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#75e093"),
                Location = new Point(500, 250),
                Size = new Size(375, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            try
            {
                timeActions.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                timeActions.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                timeActions.BackColor = HexToColor.HexStringToColor("#75e093");
            }
            timeActions.Click += time_Actions;

            // кнопка отправления кол-ва дней
            senddaysButton = new Button()
            {
                Text = "Отправить",
                Font = new Font("Century", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(385, 450),
                Size = new Size(200, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            try
            {
                senddaysButton.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                senddaysButton.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                senddaysButton.BackColor = HexToColor.HexStringToColor("#fffa61");
            }
            senddaysButton.Click += daySend;

            //ползунок
            numberTrackBar = new TrackBar()
            {
                Location = new Point(150, 280),
                Width = 500,
                Minimum = 0,
                Maximum = 50,
                TickFrequency = 5,
                Value = 25,
            };
            numberTrackBar.Scroll += NumberTrackBar_Scroll;

            valueLabel = new Label()
            {
                Text = "Дни: ",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Location = new Point(680, 270),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            countofdays = new Label()
            {
                Text = "Выберите количество дней.",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Location = new Point(150, 230),
                Size = new Size(400, 50),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            universalBack = new Button()
            {
                Text = "НАЗАД",
                Font = new Font("Century", 24, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.White,
                Location = new Point(385, 500),
                Size = new Size(200, 50),
            };
            universalBack.Click += BackToPatient;











            // кнопка "Создать список пациентов"
            createPatientList = new Button()
            {
                Text = "Создать список пациентов",
                Font = new Font("Century", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 200),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            try
            {
                createPatientList.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                createPatientList.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                createPatientList.BackColor = HexToColor.HexStringToColor("#fffa61");
            }
            createPatientList.Click += create;

            //отправить кол-во пациентов
            createsend1 = new Button()
            {
                Text = "Отправить",
                Font = new Font("Century", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(385, 450),
                Size = new Size(200, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            try
            {
                createsend1.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                createsend1.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                createsend1.BackColor = HexToColor.HexStringToColor("#fffa61");
            }
            createsend1.Click += create_send_1;

            createlabel1 = new Label()
            {
                Text = "Впишите количество пациентов:",
                Font = new Font("Century", 16, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Location = new Point(50, 210),
                Size = new Size(400, 50),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            createtextbox1 = new TextBox()
            {
                Name = "createtextbox1",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(450, 220),
                Size = new Size(100, 100),
                TextAlign = HorizontalAlignment.Center,
            };

            errorLabel = new Label()
            {
                Font = new Font("Century", 16, FontStyle.Bold),
                Text = "Некорректный ввод! Введите число от 1 до 100",
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                Location = new Point(20, 270),
                Size = new Size(400, 100),
                TextAlign = ContentAlignment.MiddleLeft,
            };
            numberOfPatients = new Label()
            {
                Text = "0",
            };

            patientCounter = new Label()
            {
                Text = "1",
            };






            // окно заполнения данных пациента
            enterId = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.TopRight,
                Text = "Id:",
                Location = new Point(50, 150)
            };

            enterTextId = new TextBox()
            {
                Name = "Id",
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(245, 150),
                Size = new Size(175, 50),
                TextAlign = HorizontalAlignment.Left,
            };

            otvet_id = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.Transparent,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(245, 150),
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            enterName = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.TopRight,
                Text = "Имя:",
                Location = new Point(50, 220)
            };

            enterTextName = new TextBox()
            {
                Name = "Name",
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(245, 220),
                Size = new Size(175, 50),
                TextAlign = HorizontalAlignment.Left,
            };

            otvet_name = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.Transparent,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(245, 220),
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            enterSurname = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.TopRight,
                Text = "Фамилия:",
                Location = new Point(50, 290)
            };

            enterTextSurname = new TextBox()
            {
                Name = "Surname",
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(245, 290),
                Size = new Size(175, 50),
                TextAlign = HorizontalAlignment.Left,
            };

            otvet_surname = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.Transparent,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(245, 290),
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            enterGender = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.TopRight,
                Text = "Пол:",
                Location = new Point(50, 360)
            };

            enterTextGender = new TextBox()
            {
                Name = "Gender",
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(245, 360),
                Size = new Size(175, 50),
                TextAlign = HorizontalAlignment.Left,
            };

            otvet_gender = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.Transparent,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(245, 360),
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            enterAge = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.TopRight,
                Text = "Возраст:",
                Location = new Point(50, 430)
            };

            enterTextAge = new TextBox()
            {
                Name = "Age",
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(245, 430),
                Size = new Size(175, 50),
                TextAlign = HorizontalAlignment.Left,
            };

            otvet_age = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.Transparent,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(245, 430),
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            enterDiagnosis = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.TopRight,
                Text = "Диагноз:",
                Location = new Point(450, 150)
            };

            otvet_diagnosis = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.Transparent,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(645, 150),
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            enterTextDiagnosis = new TextBox()
            {
                Name = "Diagnosis",
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(645, 150),
                Size = new Size(175, 50),
                TextAlign = HorizontalAlignment.Left,
            };

            enterStatus = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.TopRight,
                Text = "Статус:",
                Location = new Point(450, 220)
            };

            enterTextStatus = new TextBox()
            {
                Name = "Status",
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(645, 220),
                Size = new Size(175, 50),
                TextAlign = HorizontalAlignment.Left,
            };

            otvet_status = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.Transparent,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(645, 220),
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            enterDoctor = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.TopRight,
                Text = "Лечащий врач:",
                Location = new Point(450, 290)
            };

            enterTextDoctor = new TextBox()
            {
                Name = "Doctor",
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(645, 290),
                Size = new Size(175, 50),
                TextAlign = HorizontalAlignment.Left,
            };

            otvet_doctor = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.Transparent,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(645, 290),
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            enterDepartment = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.TopRight,
                Text = "Отделение:",
                Location = new Point(450, 360)
            };

            enterTextDepartment = new TextBox()
            {
                Name = "Department",
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(645, 360),
                Size = new Size(175, 50),
                TextAlign = HorizontalAlignment.Left,
            };

            otvet_department = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.Transparent,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(645, 360),
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            enterDays = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.TopRight,
                Text = "Оставшиеся Дни:",
                Location = new Point(450, 430)
            };

            enterTextDays = new TextBox()
            {
                Name = "Days",
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(645, 430),
                Size = new Size(175, 50),
                TextAlign = HorizontalAlignment.Left,
            };

            otvet_days = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = Color.Transparent,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(645, 430),
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.MiddleLeft,
            };

            sendPatientInfo = new Button()
            {
                Text = "Отправить",
                Font = new Font("Century", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(700, 500),
                Size = new Size(200, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            try
            {
                sendPatientInfo.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                sendPatientInfo.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                sendPatientInfo.BackColor = HexToColor.HexStringToColor("#fffa61");
            }
            this.sendPatientInfo.Click += SendPatient;

            patientNumber = new Label()
            {
                Font = new Font("Century", 14, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Size = new Size(175, 50),
                TextAlign = ContentAlignment.TopRight,
                Text = "Пациент N1",
                Location = new Point(350, 100)
            };

            errorEnterPatient = new Label()
            {
                Font = new Font("Century", 16, FontStyle.Bold),
                Text = "",
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                Location = new Point(50, 450),
                Size = new Size(350, 100),
                TextAlign = ContentAlignment.MiddleLeft,
            };











            // кнопка "Поиск пациента"
            searchPatient = new Button()
            {
                Text = "Поиск пациента",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(350, 200),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            try
            {
                searchPatient.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                searchPatient.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                searchPatient.BackColor = HexToColor.HexStringToColor("#e6a9f5");
            }
            searchPatient.Click += search;

            label_fio = new Label()
            {
                Text = "Введите фамилию пациента:",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(350, 200),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            textbox_fio = new TextBox()
            {
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(350, 350),
                Size = new Size(275, 50),

            };

            label_id = new Label()
            {
                Text = "ID пациента:",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Location = new Point(150, 230),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            label_name = new Label()
            {
                Text = "Имя пациента:",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Location = new Point(150, 290),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            label_no_patient = new Label()
            {
                Text = "Пациента с данной фамилией не найдено",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Location = new Point(290, 200),
                Size = new Size(400, 100),
                TextAlign = ContentAlignment.MiddleCenter,
            };


            label_fio_2 = new Label()
            {
                Text = "Фамилия пациента:",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#FFFFFF"),
                BackColor = HexToColor.HexStringToColor("#2e2e2e"),
                Location = new Point(150, 350),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            label_id_otvet = new Label()
            {
                Text = "Здесь данные пациента",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Location = new Point(500, 230),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            label_name_otvet = new Label()
            {
                Text = "Здесь данные пациента",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Location = new Point(500, 290),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            label_fio_2_otvet = new Label()
            {
                Text = "Здесь данные пациента",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = HexToColor.HexStringToColor("#138fff"),
                BackColor = Color.Transparent,
                Location = new Point(500, 350),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            //кнопка Отправить
            send = new Button()
            {
                Text = "Отправить",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(385, 450),
                Size = new Size(200, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            try
            {
                send.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                send.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                send.BackColor = HexToColor.HexStringToColor("#e6a9f5");
            }
            send.Click += send_func;

            // кнопка "Добавить пациента"
            addPatient = new Button()
            {
                Text = "Добавить пациента",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(650, 200),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            try
            {
                addPatient.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                addPatient.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                addPatient.BackColor = HexToColor.HexStringToColor("#8aff9d");
            }
            addPatient.Click += add;

            // кнопка "Удалить пациента"
            deletePatient = new Button()
            {
                Text = "Удалить пациента",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(185, 275),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            try
            {
                deletePatient.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                deletePatient.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                deletePatient.BackColor = HexToColor.HexStringToColor("#edb47e");
            }
            deletePatient.Click += delete;

            //кнопка Отправить
            send_delete = new Button()
            {
                Text = "Отправить",
                Font = new Font("Century", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(385, 450),
                Size = new Size(200, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            try
            {
                send_delete.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                send_delete.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                send_delete.BackColor = HexToColor.HexStringToColor("#e6a9f5");
            }
            send_delete.Click += sendDelete;


            // кнопка "Вывести список пациентов"
            printPatients = new Button()
            {
                Text = "Показать список пациентов",
                Font = new Font("Century", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(485, 275),
                Size = new Size(275, 50),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            try
            {
                printPatients.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\button.png"); // Путь к изображению
                printPatients.BackgroundImageLayout = ImageLayout.Stretch; // Растянуть изображение по кнопке
            }
            catch (Exception ex)
            {
                printPatients.BackColor = HexToColor.HexStringToColor("#8bb3e0");
            }
            printPatients.Click += print;

            // Фоновое изображение
            try
            {
                this.BackgroundImage = Image.FromFile("..\\..\\..\\..\\client with interface\\WindowsFormsApp1\\background.jpg"); // Путь к изображению
                this.BackgroundImageLayout = ImageLayout.Stretch; // Растягиваем изображение по форме
            }
            catch (Exception ex)
            {
                this.BackColor = HexToColor.HexStringToColor("#ffe299");
            }

        }

        // Выход из программы
        private void Exit_Programm(object sender, EventArgs e)
        {
            this.Close();
            byte[] data = Encoding.ASCII.GetBytes("3");
            stream.Write(data, 0, data.Length);
        }

        private void CenterButton()
        {
            // Вычисляем координаты для центрирования кнопки
            startButton.Location = new Point(
                (this.ClientSize.Width - startButton.Width) / 2, // Горизонтально
                (this.ClientSize.Height - startButton.Height) / 2 // Вертикально
            );
        }

        // Нажатие стартовой кнопки
        private void Start_Programm(object sender, EventArgs e)
        {
            this.Controls.Remove(startButton); // Удаляем стартовую кнопку

            this.Controls.Add(patientActions); // Добавляем кнопку "Работа с пациентами"
            this.Controls.Add(timeActions); // Добавляем кнопку "Работа со временем"

            string message = "start";
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        // Нажатие кнопки работы с пациентами
        private void Patient_Actions(object sender, EventArgs e)
        {
            this.Controls.Remove(exitButton); // Удаляем кнопку выхода из проги
            this.Controls.Remove(patientActions); // Удаляем кнопку "Работа с пациентами"
            this.Controls.Remove(timeActions); // Удаляем кнопку "Работа со временем"

            this.Controls.Add(exitButton2); // Добавляем кнопку "Назад"
            this.Controls.Add(createPatientList); // Добавляем кнопку "Создать список пациентов"
            this.Controls.Add(searchPatient); // Добавляем кнопку "Поиск пациента"
            this.Controls.Add(addPatient); // Добавляем кнопку "Добавить пациента"
            this.Controls.Add(deletePatient); // Добавляем кнопку "Удалить пациента"
            this.Controls.Add(printPatients); // Добавляем кнопку "Вывести список пациентов"

            byte[] data = Encoding.ASCII.GetBytes("1");
            stream.Write(data, 0, data.Length);
        }

        private void BackToPatient(object sender, EventArgs e)
        {
            this.Controls.Clear();
            this.Controls.Add(exitButton2); // Добавляем кнопку "Назад"
            this.Controls.Add(createPatientList); // Добавляем кнопку "Создать список пациентов"
            this.Controls.Add(searchPatient); // Добавляем кнопку "Поиск пациента"
            this.Controls.Add(addPatient); // Добавляем кнопку "Добавить пациента"
            this.Controls.Add(deletePatient); // Добавляем кнопку "Удалить пациента"
            this.Controls.Add(printPatients);
            this.Controls.Add(name);

            byte[] data = Encoding.ASCII.GetBytes("BACK");
            stream.Write(data, 0, data.Length);

            client = new TcpClient("127.0.0.1", 8080);
            stream = client.GetStream();
            //this.Controls.Remove(createsend1);// button
            //this.Controls.Remove(createlabel1);// label
            //this.Controls.Remove(createtextbox1);// label
            //this.Controls.Remove(universalBack);
            this.label_no_patient.Text = "Пациента с данной фамилией не найдено";
        }

        // Нажатие кнопки поиск пациента
        private void search(object sender, EventArgs e)
        {
            this.Controls.Remove(createPatientList); // Удаляем кнопку "Создать список пациентов"
            this.Controls.Remove(searchPatient); // Удаляем кнопку "Поиск пациента"
            this.Controls.Remove(addPatient); // Удаляем кнопку "Добавить пациента"
            this.Controls.Remove(deletePatient); // Удаляем кнопку "Удалить пациента"
            this.Controls.Remove(printPatients); // Удаляем кнопку "Вывести список пациентов"
            this.Controls.Remove(exitButton2);

            this.Controls.Add(send);
            this.Controls.Add(label_fio);
            this.Controls.Add(textbox_fio);
            this.Controls.Add(universalBack);

            byte[] data = Encoding.ASCII.GetBytes("3");
            stream.Write(data, 0, data.Length);

        }

        private void create(object sender, EventArgs e)
        {
            this.Controls.Remove(createPatientList); // Удаляем кнопку "Создать список пациентов"
            this.Controls.Remove(searchPatient); // Удаляем кнопку "Поиск пациента"
            this.Controls.Remove(addPatient); // Удаляем кнопку "Добавить пациента"
            this.Controls.Remove(deletePatient); // Удаляем кнопку "Удалить пациента"
            this.Controls.Remove(printPatients); // Удаляем кнопку "Вывести список пациентов"
            this.Controls.Remove(exitButton2);

            this.Controls.Add(createsend1);// button
            this.Controls.Add(createlabel1);// label
            this.Controls.Add(createtextbox1);// label
            this.Controls.Add(universalBack);

            byte[] data = Encoding.ASCII.GetBytes("1");
            stream.Write(data, 0, data.Length);
        }

        private void add(object sender, EventArgs e)
        {
            this.numberOfPatients.Text = "1";
            this.Controls.Remove(createPatientList); // Удаляем кнопку "Создать список пациентов"
            this.Controls.Remove(searchPatient); // Удаляем кнопку "Поиск пациента"
            this.Controls.Remove(addPatient); // Удаляем кнопку "Добавить пациента"
            this.Controls.Remove(deletePatient); // Удаляем кнопку "Удалить пациента"
            this.Controls.Remove(printPatients); // Удаляем кнопку "Вывести список пациентов"
            this.Controls.Remove(exitButton2);

            this.Controls.Add(enterId);
            this.Controls.Add(enterTextId);

            this.Controls.Add(enterName);
            this.Controls.Add(enterTextName);

            this.Controls.Add(enterSurname);
            this.Controls.Add(enterTextSurname);

            this.Controls.Add(enterGender);
            this.Controls.Add(enterTextGender);

            this.Controls.Add(enterAge);
            this.Controls.Add(enterTextAge);

            this.Controls.Add(enterDiagnosis);
            this.Controls.Add(enterTextDiagnosis);

            this.Controls.Add(enterStatus);
            this.Controls.Add(enterTextStatus);

            this.Controls.Add(enterDoctor);
            this.Controls.Add(enterTextDoctor);

            this.Controls.Add(enterDepartment);
            this.Controls.Add(enterTextDepartment);

            this.Controls.Add(enterDays);
            this.Controls.Add(enterTextDays);

            this.Controls.Add(sendPatientInfo);
            this.Controls.Add(universalBack);

            byte[] data = Encoding.ASCII.GetBytes("2");
            stream.Write(data, 0, data.Length);
        }
        private void delete(object sender, EventArgs e)
        {
            this.Controls.Remove(createPatientList); // Удаляем кнопку "Создать список пациентов"
            this.Controls.Remove(searchPatient); // Удаляем кнопку "Поиск пациента"
            this.Controls.Remove(addPatient); // Удаляем кнопку "Добавить пациента"
            this.Controls.Remove(deletePatient); // Удаляем кнопку "Удалить пациента"
            this.Controls.Remove(printPatients); // Удаляем кнопку "Вывести список пациентов"
            this.Controls.Remove(exitButton2);

            this.Controls.Add(send_delete);
            this.Controls.Add(label_fio);
            this.Controls.Add(textbox_fio);
            this.Controls.Add(universalBack);

            byte[] data = Encoding.ASCII.GetBytes("4");
            stream.Write(data, 0, data.Length);
        }
        private void create_send_1(object sender, EventArgs e)
        {
            string input = this.createtextbox1.Text;
            if (int.TryParse(input, out int number))
            {
                int num = Convert.ToInt32(input);
                if (num <= 100 && num > 0)
                {
                    this.numberOfPatients.Text = num.ToString();
                    this.Controls.Remove(name);
                    this.Controls.Remove(createlabel1);
                    this.Controls.Remove(createsend1);
                    this.Controls.Remove(createtextbox1);
                    this.Controls.Remove(errorLabel);
                    this.Controls.Remove(universalBack);

                    this.Controls.Add(enterId);
                    this.Controls.Add(enterTextId);

                    this.Controls.Add(enterName);
                    this.Controls.Add(enterTextName);

                    this.Controls.Add(enterSurname);
                    this.Controls.Add(enterTextSurname);

                    this.Controls.Add(enterGender);
                    this.Controls.Add(enterTextGender);

                    this.Controls.Add(enterAge);
                    this.Controls.Add(enterTextAge);

                    this.Controls.Add(enterDiagnosis);
                    this.Controls.Add(enterTextDiagnosis);

                    this.Controls.Add(enterStatus);
                    this.Controls.Add(enterTextStatus);

                    this.Controls.Add(enterDoctor);
                    this.Controls.Add(enterTextDoctor);

                    this.Controls.Add(enterDepartment);
                    this.Controls.Add(enterTextDepartment);

                    this.Controls.Add(enterDays);
                    this.Controls.Add(enterTextDays);

                    this.Controls.Add(sendPatientInfo);

                    this.Controls.Add(patientNumber);
                    this.Controls.Add(name);

                    byte[] data = Encoding.ASCII.GetBytes(this.createtextbox1.Text);
                    stream.Write(data, 0, data.Length);
                }
                else
                {
                    this.Controls.Add(errorLabel);
                    this.createtextbox1.Text = "";
                }
            }
            else
            {
                this.Controls.Add(errorLabel);
                this.createtextbox1.Text = "";
            }





        }

        private void print(object sender, EventArgs e)
        {
            spisok.Items.Clear();

            this.Controls.Remove(createPatientList); // Удаляем кнопку "Создать список пациентов"
            this.Controls.Remove(searchPatient); // Удаляем кнопку "Поиск пациента"
            this.Controls.Remove(addPatient); // Удаляем кнопку "Добавить пациента"
            this.Controls.Remove(deletePatient); // Удаляем кнопку "Удалить пациента"
            this.Controls.Remove(printPatients); // Удаляем кнопку "Вывести список пациентов"
            this.Controls.Remove(exitButton2);

            this.Controls.Add(spisok);
            this.Controls.Add(universalBack);

            byte[] data = Encoding.ASCII.GetBytes("5");
            stream.Write(data, 0, data.Length);

            byte[] bytes = new byte[256];
            int bytesRead = stream.Read(bytes, 0, bytes.Length);
            string responseData = Encoding.UTF8.GetString(bytes, 0, bytesRead);
            responseData = Encoding.UTF8.GetString(bytes, 0, bytesRead);
            int c = Convert.ToInt32(responseData);

            if (c == 0)
            {
                spisok.Items.Add("Список пациентов пустой");
            }
            else
            {
                for (int i = 0; i < c; i++)
                {
                    bytesRead = stream.Read(bytes, 0, bytes.Length);
                    responseData = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                    string[] pat = split(responseData);

                    string pat_n = "Пациент номер " + Convert.ToString(i + 1);
                    string id = "ID = " + pat[0];
                    string name = "Имя = " + pat[1];
                    string surname = "Фамилия = " + pat[2];
                    string gender = "Пол = " + pat[3];
                    string age = "Возраст = " + pat[4];
                    string diagnosis = "Диагноз = " + pat[5];
                    string status = "Статус = " + pat[6];
                    string doctor = "Доктор = " + pat[7];
                    string department = "Отделение = " + pat[8];
                    string days = "Оставшиеся дни = " + pat[9];

                    spisok.Items.Add(pat_n);
                    spisok.Items.Add(id);
                    spisok.Items.Add(name);
                    spisok.Items.Add(surname);
                    spisok.Items.Add(gender);
                    spisok.Items.Add(age);
                    spisok.Items.Add(diagnosis);
                    spisok.Items.Add(status);
                    spisok.Items.Add(doctor);
                    spisok.Items.Add(department);
                    spisok.Items.Add(days);
                    spisok.Items.Add("-----------------------------");
                }
            }
        }

        private void SendPatient(object sender, EventArgs e)
        {
            if (!(int.TryParse(this.enterTextId.Text, out int number)))
            {
                this.errorEnterPatient.Text = "ID не является числом!";
                this.Controls.Add(errorEnterPatient);
            }
            else if (Convert.ToInt32(this.enterTextId.Text) > 100 || Convert.ToInt32(this.enterTextId.Text) < 0)
            {
                this.errorEnterPatient.Text = "Некорректный ID!";
                this.Controls.Add(errorEnterPatient);
            }
            else if (!(int.TryParse(this.enterTextAge.Text, out int numbe)))
            {
                this.errorEnterPatient.Text = "Возраст не является числом!";
                this.Controls.Add(errorEnterPatient);
            }
            else if (Convert.ToInt32(this.enterTextAge.Text) > 120 || Convert.ToInt32(this.enterTextAge.Text) <= 0)
            {
                this.errorEnterPatient.Text = "Некорректный Возраст!";
                this.Controls.Add(errorEnterPatient);
            }
            else if (!(int.TryParse(this.enterTextDays.Text, out int numb)))
            {
                this.errorEnterPatient.Text = "Кол-во дней не является числом!";
                this.Controls.Add(errorEnterPatient);
            }
            else if (Convert.ToInt32(this.enterTextDays.Text) > 365 || Convert.ToInt32(this.enterTextDays.Text) <= 0)
            {
                this.errorEnterPatient.Text = "Некорректное кол-во дней!";
                this.Controls.Add(errorEnterPatient);
            }
            else if (this.enterTextName.Text == "")
            {
                this.errorEnterPatient.Text = "Введите имя!";
                this.Controls.Add(errorEnterPatient);
            }
            else if (this.enterTextSurname.Text == "")
            {
                this.errorEnterPatient.Text = "Введите фамилию!";
                this.Controls.Add(errorEnterPatient);
            }
            else if (this.enterTextGender.Text == "")
            {
                this.errorEnterPatient.Text = "Введите пол!";
                this.Controls.Add(errorEnterPatient);
            }
            else if (this.enterTextDiagnosis.Text == "")
            {
                this.errorEnterPatient.Text = "Введите диагноз!";
                this.Controls.Add(errorEnterPatient);
            }
            else if (this.enterTextStatus.Text == "")
            {
                this.errorEnterPatient.Text = "Введите статус!";
                this.Controls.Add(errorEnterPatient);
            }
            else if (this.enterTextDoctor.Text == "")
            {
                this.errorEnterPatient.Text = "Введите врача!";
                this.Controls.Add(errorEnterPatient);
            }
            else if (this.enterTextDepartment.Text == "")
            {
                this.errorEnterPatient.Text = "Введите отделение!";
                this.Controls.Add(errorEnterPatient);
            }
            else
            {
                int count = Convert.ToInt32(this.numberOfPatients.Text);
                if (count == 1)
                {
                    this.Controls.Clear();

                    string patient = this.enterTextId.Text + " " + this.enterTextName.Text + " " + this.enterTextSurname.Text + " " + this.enterTextGender.Text + " " + this.enterTextAge.Text + " " + this.enterTextDiagnosis.Text + " " + this.enterTextStatus.Text + " " + this.enterTextDoctor.Text + " " + this.enterTextDepartment.Text + " " + this.enterTextDays.Text;

                    byte[] data = Encoding.ASCII.GetBytes(patient);
                    stream.Write(data, 0, data.Length);

                    client = new TcpClient("127.0.0.1", 8080);
                    stream = client.GetStream();

                    this.Controls.Add(name);
                    this.Controls.Add(exitButton2); // Добавляем кнопку "Назад"
                    this.Controls.Add(createPatientList); // Добавляем кнопку "Создать список пациентов"
                    this.Controls.Add(searchPatient); // Добавляем кнопку "Поиск пациента"
                    this.Controls.Add(addPatient); // Добавляем кнопку "Добавить пациента"
                    this.Controls.Add(deletePatient); // Добавляем кнопку "Удалить пациента"
                    this.Controls.Add(printPatients); // Добавляем кнопку "Вывести список пациентов"

                    this.enterTextId.Text = "";
                    this.enterTextName.Text = "";
                    this.enterTextSurname.Text = "";
                    this.enterTextGender.Text = "";
                    this.enterTextAge.Text = "";
                    this.enterTextDiagnosis.Text = "";
                    this.enterTextStatus.Text = "";
                    this.enterTextDoctor.Text = "";
                    this.enterTextDepartment.Text = "";
                    this.enterTextDays.Text = "";
                }
                else
                {
                    this.Controls.Remove(errorEnterPatient);
                    this.numberOfPatients.Text = Convert.ToString(count - 1);

                    string patient = this.enterTextId.Text + " " + this.enterTextName.Text + " " + this.enterTextSurname.Text + " " + this.enterTextGender.Text + " " + this.enterTextAge.Text + " " + this.enterTextDiagnosis.Text + " " + this.enterTextStatus.Text + " " + this.enterTextDoctor.Text + " " + this.enterTextDepartment.Text + " " + this.enterTextDays.Text;

                    byte[] data = Encoding.ASCII.GetBytes(patient);
                    stream.Write(data, 0, data.Length);

                    this.enterTextId.Text = "";
                    this.enterTextName.Text = "";
                    this.enterTextSurname.Text = "";
                    this.enterTextGender.Text = "";
                    this.enterTextAge.Text = "";
                    this.enterTextDiagnosis.Text = "";
                    this.enterTextStatus.Text = "";
                    this.enterTextDoctor.Text = "";
                    this.enterTextDepartment.Text = "";
                    this.enterTextDays.Text = "";

                    int now_count_0 = Convert.ToInt32(patientCounter.Text) + 1;
                    this.patientNumber.Text = "Пациент  N" + Convert.ToString(now_count_0);

                    int now_count = Convert.ToInt32(this.patientCounter.Text) + 1;
                    this.patientCounter.Text = Convert.ToString(now_count);
                }
            }

        }

        private void sendDelete(object sender, EventArgs e)
        {
            this.Controls.Remove(send_delete);
            this.Controls.Remove(label_fio);
            this.Controls.Remove(textbox_fio);

            byte[] data = Encoding.ASCII.GetBytes(textbox_fio.Text);
            stream.Write(data, 0, data.Length);

            this.textbox_fio.Text = "";

            byte[] bytes = new byte[256];
            int bytesRead = stream.Read(bytes, 0, bytes.Length);
            string responseData = Encoding.UTF8.GetString(bytes, 0, bytesRead);
            responseData = Encoding.UTF8.GetString(bytes, 0, bytesRead);

            if (responseData == "YES")
            {
                this.label_no_patient.Text = "Пациент удален успешно";
                this.Controls.Add(label_no_patient);
            }
            if (responseData == "NO")
            {
                this.label_no_patient.Text = "Такой пациент не найден";
                this.Controls.Add(label_no_patient);
            }
        }

        // Нажатие кнопки пропуск дня
        private void time_Actions(object sender, EventArgs e)
        {
            this.Controls.Remove(exitButton); // Удаляем кнопку выхода из проги
            this.Controls.Remove(patientActions); // Удаляем кнопку "Работа с пациентами"
            this.Controls.Remove(timeActions); // Удаляем кнопку "Работа со временем"

            this.Controls.Add(daysexitButton); // Добавляем кнопку "Назад"
            this.Controls.Add(senddaysButton); // Добавляем кнопку отправить
            this.Controls.Add(numberTrackBar);
            this.Controls.Add(valueLabel);
            this.Controls.Add(countofdays);

            byte[] data = Encoding.ASCII.GetBytes("2");
            stream.Write(data, 0, data.Length);
        }

        private void NumberTrackBar_Scroll(object sender, EventArgs e)
        {
            // Обновляем текст метки при изменении значения ползунка
            valueLabel.Text = "Дни: " + numberTrackBar.Value;
        }

        private void send_func(object sender, EventArgs e)
        {
            this.Controls.Remove(label_fio);
            this.Controls.Remove(textbox_fio);
            this.Controls.Remove(send);
            this.Controls.Remove(exitButton3);


            byte[] data = Encoding.ASCII.GetBytes(this.textbox_fio.Text);
            stream.Write(data, 0, data.Length);

            byte[] bytes = new byte[256];
            int bytesRead = stream.Read(bytes, 0, bytes.Length);
            string responseData = Encoding.UTF8.GetString(bytes, 0, bytesRead);
            responseData = Encoding.UTF8.GetString(bytes, 0, bytesRead);

            if (responseData == "NO PATIENT")
            {
                this.Controls.Add(label_no_patient);
                this.Controls.Add(universalBack);
            }
            else
            {
                string[] patient = split(responseData);

                this.otvet_id.Text = patient[0];
                this.otvet_name.Text = patient[1];
                this.otvet_surname.Text = patient[2];
                this.otvet_gender.Text = patient[3];
                this.otvet_age.Text = patient[4];
                this.otvet_diagnosis.Text = patient[5];
                this.otvet_status.Text = patient[6];
                this.otvet_doctor.Text = patient[7];
                this.otvet_department.Text = patient[8];
                this.otvet_days.Text = patient[9];

                this.Controls.Add(exitButton);

                this.Controls.Add(enterId);
                this.Controls.Add(enterName);
                this.Controls.Add(enterSurname);
                this.Controls.Add(enterGender);
                this.Controls.Add(enterAge);
                this.Controls.Add(enterDiagnosis);
                this.Controls.Add(enterStatus);
                this.Controls.Add(enterDoctor);
                this.Controls.Add(enterDepartment);
                this.Controls.Add(enterDays);

                this.Controls.Add(otvet_id);
                this.Controls.Add(otvet_name);
                this.Controls.Add(otvet_surname);
                this.Controls.Add(otvet_gender);
                this.Controls.Add(otvet_age);
                this.Controls.Add(otvet_diagnosis);
                this.Controls.Add(otvet_status);
                this.Controls.Add(otvet_doctor);
                this.Controls.Add(otvet_department);
                this.Controls.Add(otvet_days);

            }

            this.textbox_fio.Text = "";
        }

        // Нажатие кнопки назад, тут мы возвращаем убранные и убираем добавленные кнопки в прошлой функции
        private void Back(object sender, EventArgs e)
        {
            this.Controls.Add(exitButton);
            this.Controls.Add(patientActions);
            this.Controls.Add(timeActions);

            this.Controls.Remove(exitButton2);
            this.Controls.Remove(createPatientList);
            this.Controls.Remove(searchPatient);
            this.Controls.Remove(addPatient);
            this.Controls.Remove(deletePatient);
            this.Controls.Remove(printPatients);
            this.Controls.Remove(send);

            byte[] data = Encoding.ASCII.GetBytes("8");
            stream.Write(data, 0, data.Length);
        }
        private void Back2(object sender, EventArgs e)
        {
            this.Controls.Add(exitButton);
            this.Controls.Add(patientActions);
            this.Controls.Add(timeActions);

            this.Controls.Remove(exitButton3);
            this.Controls.Remove(createPatientList);
            this.Controls.Remove(searchPatient);
            this.Controls.Remove(addPatient);
            this.Controls.Remove(deletePatient);
            this.Controls.Remove(printPatients);
            this.Controls.Remove(send);
            this.Controls.Remove(label_fio);
            this.Controls.Remove(textbox_fio);
        }

        private void day_Back(object sender, EventArgs e)
        {
            this.Controls.Add(exitButton);
            this.Controls.Add(patientActions);
            this.Controls.Add(timeActions);

            this.Controls.Remove(daysexitButton);
            this.Controls.Remove(senddaysButton);
            this.Controls.Remove(valueLabel);
            this.Controls.Remove(numberTrackBar);
            this.Controls.Remove(countofdays);

            byte[] data = Encoding.ASCII.GetBytes("BACK");
            stream.Write(data, 0, data.Length);
        }

        private void daySend(object sender, EventArgs e)
        {
            byte[] data = Encoding.ASCII.GetBytes(Convert.ToString(this.numberTrackBar.Value));
            stream.Write(data, 0, data.Length);

            this.Controls.Add(exitButton);
            this.Controls.Add(patientActions);
            this.Controls.Add(timeActions);

            this.Controls.Remove(daysexitButton);
            this.Controls.Remove(senddaysButton);
            this.Controls.Remove(valueLabel);
            this.Controls.Remove(numberTrackBar);
            this.Controls.Remove(countofdays);

        }

        private string[] split(string input)
        {

            string[] result = input
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            return result;
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}