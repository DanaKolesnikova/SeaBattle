using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace Морской_бой
{
    public partial class Игра : Form
    {
        Button startButton = new Button();
        Button button_NewGame = new Button();
        Label PMap = new Label();
        Label BMap = new Label();                    

        int cellSize;
        static int mapSize = 11;
        string alphabet = "АБВГДЕЖЗИК";

        int[,] myMap = new int[mapSize, mapSize];
        int[,] botMap = new int[mapSize, mapSize];
        bool[,] checkMap = new bool[mapSize + 1, mapSize + 1];

        Button[,] myButton = new Button[mapSize, mapSize];
        Button[,] botButton = new Button[mapSize, mapSize];
        
        public Игра() 
        {
            InitializeComponent();
          
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            int x = Screen.PrimaryScreen.WorkingArea.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = x;
            this.Height = y;

            this.Focus();

            cellSize = Width / 33;
            Init();
        }
        
        bool isPlayer;
        bool isBot;
        bool PShoot;
        bool BShoot;
        bool Game;

        int x_BotShoot;
        int y_BotShoot;

        StringBuilder s = new StringBuilder(206);
        string mapinfo;
        
        Button buttonSaveGame = new Button();
        Button buttonLoadGame = new Button();

        SaveFileDialog saveFileDialog = new SaveFileDialog();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        
        Button buttonExit = new Button();

        private PopupNotifier popup = null;

        public void Init()
        {
            isPlayer = isBot = false;
            PShoot = false;
            BShoot = true;

            Game = true;
           
            CreateMaps();
        }

        public void CreateMaps() 
        {
            // Создание поля игрока 
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    myMap[i, j] = 0;

                    Button button = new Button();
                    button.Location = new Point(j * cellSize, i * cellSize + cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    button.BackColor = Color.White;
                    if (j == 0 || i == 0)
                    {
                        button.BackColor = this.BackColor;
                        button.FlatAppearance.BorderSize = 0;
                        button.FlatStyle = FlatStyle.Flat;
                        if (j == 0 && i > 0)
                        {
                            button.Text = Convert.ToString(i);
                        }
                        if (i == 0 && j > 0)
                        {
                            button.Text = Convert.ToString(alphabet[j - 1]);
                        }
                    }
                    else
                    {
                        button.Click += new EventHandler(CreateShip);
                    }
                    myButton[i, j] = button;
                    this.Controls.Add(button);
                }
            }

            // Создание поля бота
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    botMap[i, j] = 0;

                    Button button = new Button();
                    button.Location = new Point(j * cellSize + cellSize * mapSize * 2 - cellSize, i * cellSize + cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    button.BackColor = Color.White;
                    if (j == 0 || i == 0)
                    {
                        button.BackColor = this.BackColor;
                        button.FlatAppearance.BorderSize = 0;
                        button.FlatStyle = FlatStyle.Flat;
                        if (j == 0 && i > 0)
                        {
                            button.Text = Convert.ToString(i);
                            button.FlatAppearance.BorderSize = 0;
                            button.FlatStyle = FlatStyle.Flat;
                        }

                        if (i == 0 && j > 0)
                        {
                            button.Text = Convert.ToString(alphabet[j - 1]);
                            button.FlatAppearance.BorderSize = 0;
                            button.FlatStyle = FlatStyle.Flat;
                        }
                    }
                    else
                    {
                        button.Click += new EventHandler(Shoot);
                    }
                    botButton[i, j] = button;
                    this.Controls.Add(button);
                }
            }
            PMap.Text = "Игрок";
            PMap.Location = new Point(mapSize * cellSize / 2, cellSize / 3);
            this.Controls.Add(PMap);

            BMap.Text = "Компьютер";
            BMap.Location = new Point(mapSize * cellSize / 2 + cellSize * 21, cellSize / 3);
            this.Controls.Add(BMap);

            PMap.Font = BMap.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            
            startButton.Location = new Point(mapSize * cellSize + 3 * cellSize, 2 * cellSize);
            startButton.Size = new Size(cellSize * 4, cellSize); startButton.Text = "Начать";
            startButton.Click += new EventHandler(Start); this.Controls.Add(startButton);

            button_NewGame.Location = new Point(mapSize * cellSize + 3 * cellSize, 3 * cellSize);
            button_NewGame.Size = new Size(cellSize * 4, cellSize);
            button_NewGame.Text = "Новая игра";
            button_NewGame.Click += new EventHandler(New_Game);
            this.Controls.Add(button_NewGame);

            buttonSaveGame.Location = new Point(mapSize * cellSize + 3 * cellSize, 4 * cellSize);
            buttonSaveGame.Size = new Size(cellSize * 4, cellSize);
            buttonSaveGame.Text = "Сохранить игру";
            buttonSaveGame.Click += new EventHandler(SaveGame);
            this.Controls.Add(buttonSaveGame);

            buttonLoadGame.Location = new Point(mapSize * cellSize + 3 * cellSize, 5 * cellSize);
            buttonLoadGame.Size = new Size(cellSize * 4, cellSize);
            buttonLoadGame.Text = "Загрузить игру";
            buttonLoadGame.Click += new EventHandler(LoadGame);
            this.Controls.Add(buttonLoadGame);

            buttonExit.Location = new Point(mapSize * cellSize + 3 * cellSize, 6 * cellSize);
            buttonExit.Size = new Size(cellSize * 4, cellSize);
            buttonExit.Text = "Выход";
            buttonExit.Click += new EventHandler(Exit);
            this.Controls.Add(buttonExit);

           
            startButton.Font = button_NewGame.Font = buttonSaveGame.Font = buttonLoadGame.Font = buttonExit.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
        }
            
        // Выход в главное меню
        public void Exit(object sender, EventArgs e) 
        {
            DialogResult result = MessageBox.Show("Покинуть игру? ", "              Выход из игры", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // Начать процесс игры
        public void Start(object sender, EventArgs e) 
        {
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    if (myMap[i, j] == 1)
                    {
                        checkMap[i, j] = true;
                    }
                    else
                    {
                        checkMap[i, j] = false;
                    }
                }

            }
            Check_Ship(); 

            if (isPlayer) 
            {
                do
                {
                    Bot_Generation();
                    for (int i = 1; i < mapSize; i++)
                    {
                        for (int j = 1; j < mapSize; j++)
                        {
                            if (botMap[i, j] == 1)
                            {
                                checkMap[i, j] = true;
                            }
                            else
                            {
                                checkMap[i, j] = false;
                            }
                        }
                    }
                    Check_Ship();
                }
                while (isBot == false);
            }
            if (isPlayer && isBot)
            {
                startButton.Enabled = false;

            }
        }
            
        // Создание флота игрока
        public void CreateShip(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            if (!isPlayer)
            {
                int x = pressedButton.Location.X / cellSize;
                int y = pressedButton.Location.Y / cellSize - 1; 
                switch (myMap[y, x]) 
                {
                    case 0:
                        pressedButton.BackColor = Color.Red; myMap[y, x] = 1;
                        break;
                        
                    case 1:
                        pressedButton.BackColor = Color.White; myMap[y, x] = 0;
                        if (y != 10 && x != 10)
                        {
                            myMap[y + 1, x + 1] = 0;
                            myMap[y + 1, x - 1] = 0;
                            myMap[y - 1, x + 1] = 0;
                            myMap[y - 1, x - 1] = 0;
                        }
                        if (y == 10 && x != 10)
                        {
                            myMap[y - 1, x + 1] = 0;
                            myMap[y - 1, x - 1] = 0;
                        }
                        if (x == 10 && y != 10)
                        {
                            myMap[y + 1, x - 1] = 0;
                            myMap[y - 1, x - 1] = 0;
                        }
                        if (x == 10 && y == 10)
                        {
                            myMap[y - 1, x - 1] = 0;
                        }
                        break;
                        
                }
            }
            for (int i = 1; i < 11; i++) // Блокировка ячеек рядом с кораблями
            {
                for (int j = 1; j < 11; j++)
                {
                    if (myMap[i, j] == 1)
                    {
                        if (j != 10 && i != 10)
                        {
                            myMap[i + 1, j + 1] = 2;
                            myMap[i + 1, j - 1] = 2;
                            myMap[i - 1, j + 1] = 2;
                            myMap[i - 1, j - 1] = 2;
                        }
                        if (i == 10 && j != 10)
                        {
                            myMap[i - 1, j + 1] = 2;
                            myMap[i - 1, j - 1] = 2;
                        }
                        if (j == 10 && i != 10)
                        {
                            myMap[i + 1, j - 1] = 2;
                            myMap[i - 1, j - 1] = 2;
                        }
                        if (j == 10 && i == 10)
                        {
                            myMap[i - 1, j - 1] = 2;
                        }
                    }
                }
            }
        }

        public void Check_Ship() // Проверка расстановки кораблей
        {
            int Ship1, Ship2, Ship3, Ship4, ShipLong;
            Ship1 = Ship2 = Ship3 = Ship4 = ShipLong = 0; // Счетчики кораблей
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    if ((checkMap[i - 1, j] == false && checkMap[i, j] == true && checkMap[i + 1, j] == true && checkMap[i + 2, j] == true && checkMap[i + 3, j] == true && checkMap[i + 4, j] == false) || (checkMap[i, j - 1] == false && checkMap[i, j] == true && checkMap[i, j + 1] == true && checkMap[i, j + 2] == true && checkMap[i, j + 3] == true && checkMap[i, j + 4] == false))
                    {
                        Ship4++;
                    }
                    
                    if ((checkMap[i - 1, j] == false && checkMap[i, j] == true && checkMap[i + 1, j] == true && checkMap[i + 2, j] == true && checkMap[i + 3, j] == false) || (checkMap[i, j - 1] == false && checkMap[i, j] == true && checkMap[i, j + 1] == true && checkMap[i, j + 2] == true && checkMap[i, j + 3] == false))
                    {
                        Ship3++;
                    }

                    if ((checkMap[i - 1, j] == false && checkMap[i, j] == true && checkMap[i + 1, j] == true && checkMap[i + 2, j] == false) || (checkMap[i, j - 1] == false && checkMap[i, j] == true && checkMap[i, j + 1] == true && checkMap[i, j + 2] == false))
                    {
                        Ship2++;
                    }

                    if (checkMap[i - 1, j] == false && checkMap[i, j] == true && checkMap[i + 1, j] == false && checkMap[i, j - 1] == false && checkMap[i, j + 1] == false)
                    {
                        Ship1++;
                    }

                    if ((checkMap[i - 1, j] == false && checkMap[i, j] == true && checkMap[i + 1, j] == true && checkMap[i + 2, j] == true && checkMap[i + 3, j] == true && checkMap[i + 4, j] == true) || (checkMap[i, j - 1] == false && checkMap[i, j] == true && checkMap[i, j + 1] == true && checkMap[i, j + 2] == true && checkMap[i, j + 3] == true && checkMap[i, j + 4] == true))
                    {
                        ShipLong++;
                    }

                }

            }

            if (!isBot && isPlayer && Ship1 == 4 && Ship2 == 3 && Ship3 == 2 && Ship4 == 1 && ShipLong == 0)
            {
                isBot = true;
               // MessageBox.Show("Игра началась", "Игра началась", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }

            if (!isPlayer)
            {
                if (Ship1 != 4 || Ship2 != 3 || Ship3 != 2 || Ship4 != 1 || ShipLong != 0)
                {
                    MessageBox.Show("           Неверно расставлены корабли. \n Вернитесь в главное меню и изучите правила игры", "                         Ошибка в расстановке кораблей!", 
                        MessageBoxButtons.OK,MessageBoxIcon.Error);
                    isPlayer = false;
                }
                else
                {
                    isPlayer = true;
                   
                }
            }


        }
        
        // Расстановка кораблей бота
        public void Bot_Generation() 
        {
            int long_boat, count_boat;
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    botMap[i, j] = 0;
                }
            }
            try
            {
                for (long_boat = 4; long_boat > 0; long_boat--)
                {
                    switch (long_boat)
                    {

                        case 1:
                            count_boat = 4; Create_Boat(count_boat, long_boat); break;
                        case 2:
                            count_boat = 3; Create_Boat(count_boat, long_boat); break;
                        case 3:
                            count_boat = 2; Create_Boat(count_boat, long_boat); break;
                        case 4:
                            count_boat = 1; Create_Boat(count_boat, long_boat); break;
                    }
                }
            }
            catch
            {
                for (int i = 0; i < mapSize; i++)
                {
                    for (int j = 0; j < mapSize; j++)
                    {
                        botMap[i, j] = 0;
                    }
                }
            }
        }

        public void Create_Boat(int count, int long_boat) // Расстановка отдельного корабля
        {
            int x0, y0;
            string napr;
            for (int j = count; j > 0; j--)
            {
                Random rnd = new Random();

                //Направление установки
                string[] napravlenie = new string[4];
                napravlenie[0] = "left";
                napravlenie[1] = "top";
                napravlenie[2] = "right";
                napravlenie[3] = "bottom";

                try
                {
                    do
                    {
                        // Координата, с которой начнется установка корабля
                        x0 = rnd.Next(1, 11);
                        y0 = rnd.Next(1, 11);
                    }
                    while (botMap[y0, x0] != 0);

                    if (long_boat != 1)
                    {
                        napr = napravlenie[rnd.Next(0, napravlenie.Length)];
                        // Создание корабля 
                        switch (napr)
                        {
                            case "left":
                                while (x0 < long_boat) x0++;
                                for (int i = x0; i > x0 - long_boat; i--) botMap[y0, i] = 1; Block();
                                break;
                            case "top":
                                while (y0 < long_boat) y0++;
                                for (int i = y0; i > y0 - long_boat; i--) botMap[i, x0] = 1; Block();
                                break;
                            case "right":
                                while (x0 > mapSize - long_boat) x0--;
                                for (int i = x0; i < x0 + long_boat; i++) botMap[y0, i] = 1; Block();
                                break;
                            case "bottom":
                                while (y0 > mapSize - long_boat) y0--;
                                for (int i = y0; i < y0 + long_boat; i++) botMap[i, x0] = 1; Block();
                                break;

                        }
                    }
                    else
                    {
                        botMap[y0, x0] = 1;
                        Block();
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show($"{error.Message}{error.StackTrace}{error.InnerException}");
                }
            }
        }

        public void Block() // Блокировка полей вокруг корабля компьютера
        {
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    if (botMap[i, j] == 1)
                    {
                        if (j != 10 && i != 10)
                        {
                            botMap[i + 1, j + 1] = 2;
                            botMap[i + 1, j - 1] = 2;
                            botMap[i - 1, j + 1] = 2;
                            botMap[i - 1, j - 1] = 2;
                        }
                        if (i == 10 && j != 10)
                        {
                            botMap[i - 1, j + 1] = 2;
                            botMap[i - 1, j - 1] = 2;
                        }

                        if (j == 10 && i != 10)
                        {
                            botMap[i + 1, j - 1] = 2;
                            botMap[i - 1, j - 1] = 2;
                        }

                        if (j == 10 && i == 10)
                        {
                            botMap[i - 1, j - 1] = 2;
                        }
                    }
                }

            }

        }

        public void Shoot(object sender, EventArgs e) // Выстрел игроока
        {
            if (isBot && BShoot && Game)
            {

                Button pressedButton = sender as Button;

                int x = pressedButton.Location.X / cellSize - 21;
                int y = pressedButton.Location.Y / cellSize - 1;
                switch (botMap[y, x])
                {
                    case 1:
                       
                        pressedButton.BackColor = Color.Red;
                        botMap[y, x] = 3;
                        Check_Killed();                      
                        break;
                    case 0:                       
                        pressedButton.BackColor = Color.Blue;
                        botMap[y, x] = 5;
                        PShoot = true;
                        BShoot = false;
                        Bot_Hit(sender, e);
                        break;
                    case 2:
                        pressedButton.BackColor = Color.Blue;
                        botMap[y, x] = 5;
                        PShoot = true;
                        BShoot = false;
                        Bot_Hit(sender, e);
                        break;
                }

                pressedButton.Enabled = false;

            }
        }

        //Ход бота
        public void Bot_Hit(object sender, EventArgs e) 
        {

            Random rnd = new Random();
            while (isBot && !BShoot && Game)
            {
                int wound = 0;
                int[] wound_y = new int[21];
                int[] wound_x = new int[21];

                // Нахождение раненых кораблей
                for (int i = 1; i < mapSize; i++)
                {
                    for (int j = 1; j < mapSize; j++)
                    {
                        if (myMap[i, j] == 3)
                        {
                            wound++;
                            wound_y[wound] = i;
                            wound_x[wound] = j;
                        }
                    }
                }
                switch (wound)
                {
                    case 0: // Нет раненых кораблей
                        x_BotShoot = rnd.Next(1, 11);
                        y_BotShoot = rnd.Next(1, 11);
                        break;
                    case 1: // одно поле с раненым кораблем
                        string[] napravlenie = new string[4];
                        napravlenie[0] = "left";
                        napravlenie[1] = "top";
                        napravlenie[2] = "right";
                        napravlenie[3] = "bottom";

                        // Добивание корабля
                    a:
                        string napr = napravlenie[rnd.Next(0, napravlenie.Length - 1)];
                        switch (napr)
                        {
                            case "left":
                                if (wound_x[1] != 1 && myMap[wound_y[1], wound_x[1] - 1] != 5)
                                {
                                    x_BotShoot = wound_x[1] - 1;
                                    y_BotShoot = wound_y[1];
                                }
                                else
                                {
                                    napravlenie[0] = "right";
                                    goto a;
                                }
                                break;
                            case "top":
                                if (wound_y[1] != 1 && myMap[wound_y[1] - 1, wound_x[1]] != 5)
                                {
                                    x_BotShoot = wound_x[1];
                                    y_BotShoot = wound_y[1] - 1;
                                }
                                else
                                {
                                    napravlenie[1] = "bottom";
                                    goto a;
                                }
                                break;
                            case "right":
                                if (wound_x[1] != 10 && myMap[wound_y[1], wound_x[1] + 1] != 5)
                                {
                                    x_BotShoot = wound_x[1] + 1;
                                    y_BotShoot = wound_y[1];
                                }
                                else
                                {
                                    napravlenie[2] = "left";
                                    goto a;
                                }
                                break;
                            case "bottom":
                                if (wound_y[1] != 10 && myMap[wound_y[1] + 1, wound_x[1]] != 5)
                                {
                                    x_BotShoot = wound_x[1];
                                    y_BotShoot = wound_y[1] + 1;
                                }
                                else
                                {
                                    napravlenie[3] = "top";
                                    goto a;
                                }
                                break;
                        }
                        break;
                    
                    // два поля с попаданиями
                    case 2:
                        // Корабли расположены горизонтально
                        if (wound_y[1] == wound_y[2])
                            {
                                string[] napr2hor = new string[2];
                                napr2hor[0] = "left";
                                napr2hor[1] = "right";
                                string naprHor = napr2hor[rnd.Next(0, napr2hor.Length - 1)];
                                switch (naprHor)
                                {
                                    case "left":
                                        if (wound_x[1] != 1 && myMap[wound_y[1], wound_x[1] - 1] != 5)
                                        {
                                            x_BotShoot = wound_x[1] - 1;
                                            y_BotShoot = wound_y[1];
                                        }
                                        else goto case "right";
                                        break;
                                    case "right":
                                        if (wound_x[2] != 10 && myMap[wound_y[2], wound_x[2] + 1] != 5)
                                        {
                                            x_BotShoot = wound_x[2] + 1;
                                            y_BotShoot = wound_y[2];
                                        }
                                        else goto case "left";
                                        break;
                                }
                        }

                        // Корабли расположены вертикально
                        if (wound_x[1] == wound_x[2])
                        {
                            string[] napr2vert = new string[2];
                            napr2vert[0] = "top";
                            napr2vert[1] = "bottom";
                            string naprVert = napr2vert[rnd.Next(0, napr2vert.Length - 1)];
                            switch (naprVert)
                            {
                                case "top":
                                    if (wound_y[1] != 1 && myMap[wound_y[1] - 1,
                                   wound_x[1]] != 5)
                                    {
                                        x_BotShoot = wound_x[1];
                                        y_BotShoot = wound_y[1] - 1;
                                    }
                                    else goto case "bottom";
                                    break;
                                case "bottom":
                                    if (wound_y[2] != 10 && myMap[wound_y[2] + 1,
                                   wound_x[2]] != 5)
                                    {
                                        x_BotShoot = wound_x[2];
                                        y_BotShoot = wound_y[2] + 1;
                                    }
                                    else goto case "top";
                                    break;
                            }
                        }
                        break;

                    // три поля с попаданиями
                    case 3:
                        // Корабли расположены горизонтально
                        if (wound_y[1] == wound_y[2] && wound_y[2] == wound_y[3])
                        {
                            string[] napr3hor = new string[2];
                            napr3hor[0] = "left";
                            napr3hor[1] = "right";
                            string naprHor = napr3hor[rnd.Next(0, napr3hor.Length - 1)];
                            switch (naprHor)
                            {
                                case "left":
                                    if (wound_x[1] != 1 && myMap[wound_y[1], wound_x[1] - 1] != 5)
                                    {
                                        x_BotShoot = wound_x[1] - 1;
                                        y_BotShoot = wound_y[1];
                                    }
                                    else goto case "right";
                                    break;

                                case "right":
                                    if (wound_x[3] != 10 && myMap[wound_y[3], wound_x[3] + 1] != 5)
                                    {
                                        x_BotShoot = wound_x[3] + 1;
                                        y_BotShoot = wound_y[3];
                                    }
                                    else goto case "left";
                                    break;
                            }
                        }

                        // Корабли расположены вертикально
                        if (wound_x[1] == wound_x[2] && wound_x[2] == wound_x[3])
                        {
                            string[] napr3vert = new string[2];
                            napr3vert[0] = "top";
                            napr3vert[1] = "bottom";
                            string naprVert = napr3vert[rnd.Next(0, napr3vert.Length - 1)];
                            switch (naprVert)
                            {
                                case "top":
                                    if (wound_y[1] != 1 && myMap[wound_y[1] - 1, wound_x[1]] != 5)
                                    {
                                        x_BotShoot = wound_x[1];
                                        y_BotShoot = wound_y[1] - 1;
                                    }
                                    else goto case "bottom";
                                    break;
                                case "bottom":
                                    if (wound_y[3] != 10 && myMap[wound_y[3] + 1, wound_x[3]] != 5)
                                    {
                                        x_BotShoot = wound_x[3];
                                        y_BotShoot = wound_y[3] + 1;
                                    }
                                    else goto case "top";
                                    break;
                            }
                        }
                        break;
                
                    // Нет полей с попаданиями
                    default:
                        x_BotShoot = rnd.Next(1, 11);
                        y_BotShoot = rnd.Next(1, 11);
                        break;
                }
            
                switch (myMap[y_BotShoot, x_BotShoot]) // Выстрел компьютера
                {
                    case 1:

                       
                        myMap[y_BotShoot, x_BotShoot] = 3;
                        myButton[y_BotShoot, x_BotShoot].Text = "X";
                        Check_Killed_OnMapPlayer();
                
                        break;
                    case 0:
 
                        myButton[y_BotShoot, x_BotShoot].BackColor = Color.Blue;
                        myMap[y_BotShoot, x_BotShoot] = 5;
                        PShoot = false;
                        BShoot = true;
                        break;
                    case 2:
       
                        myButton[y_BotShoot, x_BotShoot].BackColor = Color.Blue;
                        myMap[y_BotShoot, x_BotShoot] = 5;
                        PShoot = false;
                        BShoot = true;
                        break;
                }
               
            }
        }

       
        
        // Проверка на убийство корабля бота
        public void Check_Killed() 
        {
            int count4 = 0;
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {

                    // четырехпалубный
                    if ((this.botMap[i, j] == 3 && i < 8) && this.botMap[i + 1, j] == 3 && this.botMap[i + 2, j] == 3 && this.botMap[i + 3, j] == 3)
                    {
                        this.botButton[i, j].BackColor = this.botButton[i + 1, j].BackColor = this.botButton[i + 2, j].BackColor = this.botButton[i + 3, j].BackColor = Color.Black;
                        this.botMap[i, j] = this.botMap[i + 1, j] = this.botMap[i + 2, j] = this.botMap[i + 3, j] = 4;
                        
                        if (i == 7 || j == 10)
                        {
                            if (i == 7 && j != 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                                botMap[i, j - 1] = botMap[i, j + 1] = 5;
                                botMap[i + 1, j - 1] = botMap[i + 1, j + 1] = 5;
                                botMap[i + 2, j - 1] = botMap[i + 2, j + 1] = 5;
                                botMap[i + 3, j - 1] = botMap[i + 3, j + 1] = 5;
                            }
                            if (i == 7 && j == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i, j - 1] = botMap[i + 1, j - 1] = botMap[i + 2, j - 1] = botMap[i + 3, j - 1] = 5;
                            }
                            if (i < 7 && j == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i, j - 1] = botMap[i + 1, j - 1] = botMap[i + 2, j - 1] = botMap[i + 3, j - 1] = 5;
                                botMap[i + 4, j] = botMap[i + 4, j - 1] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                            botMap[i + 4, j - 1] = botMap[i + 4, j] = botMap[i + 4, j + 1] = 5;
                            botMap[i, j - 1] = botMap[i, j + 1] = 5;
                            botMap[i + 1, j - 1] = botMap[i + 1, j + 1] = 5;
                            botMap[i + 2, j - 1] = botMap[i + 2, j + 1] = 5;
                            botMap[i + 3, j - 1] = botMap[i + 3, j + 1] = 5;
                        }
                    }
          
                    if ((botMap[i, j] == 3 && j < 8) && botMap[i, j + 1] == 3 && botMap[i, j + 2] == 3 && botMap[i, j + 3] == 3)
                    {
                        botButton[i, j].BackColor = botButton[i, j + 1].BackColor = botButton[i, j + 2].BackColor = botButton[i, j + 3].BackColor = Color.Black;
                        botMap[i, j] = botMap[i, j + 1] = botMap[i, j + 2] = botMap[i, j + 3] = 4;
          
                        if (j == 7 || i == 10)
                        {
                            if (j == 7 && i != 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                                botMap[i - 1, j] = botMap[i + 1, j] = 5;
                                botMap[i - 1, j + 1] = botMap[i + 1, j + 1] = 5;
                                botMap[i - 1, j + 2] = botMap[i + 1, j + 2] = 5;
                                botMap[i - 1, j + 3] = botMap[i + 1, j + 3] = 5;
                            }
                            if (j == 7 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                                botMap[i - 1, j] = botMap[i - 1, j + 1] = botMap[i - 1, j + 2] = botMap[i - 1, j + 3] = 5;
                            }
                            if (j < 7 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                                botMap[i - 1, j + 4] = botMap[i, j + 4] = 5;
                                botMap[i - 1, j] = botMap[i - 1, j + 1] = botMap[i - 1, j + 2] = botMap[i - 1, j + 3] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j] = botMap[i + 1, j] = 5;
                            botMap[i - 1, j + 1] = botMap[i + 1, j + 1] = 5;
                            botMap[i - 1, j + 2] = botMap[i + 1, j + 2] = 5;
                            botMap[i - 1, j + 3] = botMap[i + 1, j + 3] = 5;
                            botMap[i - 1, j - 1] = botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                            botMap[i - 1, j + 4] = botMap[i, j + 4] = botMap[i + 1, j + 4] = 5;
                        }
                    }
                    // трехпалубный
                    if ((botMap[i - 1, j] != 1 && i < 9) && botMap[i - 1, j] != 3 && botMap[i, j] == 3 && botMap[i + 1, j] == 3 && botMap[i + 2, j] == 3 && (i == 8 || (botMap[i + 3, j] != 3 && botMap[i + 3, j] != 1)))
                    {
                        botButton[i, j].BackColor = botButton[i + 1, j].BackColor = botButton[i + 2, j].BackColor = Color.Black;
                        botMap[i, j] = botMap[i + 1, j] = botMap[i + 2, j] = 4;
           
                        if (i == 8 || j == 10)
                        {
                            if (i == 8 && j != 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                                botMap[i, j - 1] = botMap[i, j + 1] = 5;
                                botMap[i + 1, j - 1] = botMap[i + 1, j + 1] = 5;
                                botMap[i + 2, j - 1] = botMap[i + 2, j + 1] = 5;
                            }
                            if (i == 8 && j == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i, j - 1] = botMap[i + 1, j - 1] = botMap[i + 2, j - 1] = 5;
                            }
                            if (i < 8 && j == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i, j - 1] = botMap[i + 1, j - 1] = botMap[i + 2, j - 1] = 5;
                                botMap[i + 3, j] = botMap[i + 3, j - 1] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                            botMap[i + 3, j - 1] = botMap[i + 3, j] = botMap[i + 3, j + 1] = 5;
                            botMap[i, j - 1] = botMap[i, j + 1] = 5;
                            botMap[i + 1, j - 1] = botMap[i + 1, j + 1] = 5;
                            botMap[i + 2, j - 1] = botMap[i + 2, j + 1] = 5;
                        }
                    }
           
                    if ((botMap[i, j - 1] != 1 && j < 9) && botMap[i, j - 1] != 3 && botMap[i, j] == 3 && botMap[i, j + 1] == 3 && botMap[i, j + 2] == 3 && (j == 8 || (botMap[i, j + 3] != 3 && botMap[i, j + 3] != 1)))
                    {
                        botButton[i, j].BackColor = botButton[i, j + 1].BackColor = botButton[i, j + 2].BackColor = Color.Black;
                        botMap[i, j] = botMap[i, j + 1] = botMap[i, j + 2] = 4;
    
                        if (j == 8 || i == 10)
                        {
                            if (j == 8 && i != 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                                botMap[i - 1, j] = botMap[i + 1, j] = 5;
                                botMap[i - 1, j + 1] = botMap[i + 1, j + 1] = 5;
                                botMap[i - 1, j + 2] = botMap[i + 1, j + 2] = 5;
                            }
                            if (j == 8 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                                botMap[i - 1, j] = botMap[i - 1, j + 1] = botMap[i - 1, j + 2] = 5;
                            }
                            if (j < 8 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                                botMap[i - 1, j + 3] = botMap[i, j + 3] = 5;
                                botMap[i - 1, j] = botMap[i - 1, j + 1] = botMap[i - 1, j + 2] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j] = botMap[i + 1, j] = 5;
                            botMap[i - 1, j + 1] = botMap[i + 1, j + 1] = 5;
                            botMap[i - 1, j + 2] = botMap[i + 1, j + 2] = 5;
                            botMap[i - 1, j - 1] = botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                            botMap[i - 1, j + 3] = botMap[i, j + 3] = botMap[i + 1, j + 3] = 5;
                        }
                    }
                    // двухпалубный
                    if ((botMap[i - 1, j] != 1 && i < 10) && botMap[i - 1, j] != 3 && botMap[i, j] == 3 && botMap[i + 1, j] == 3 && (i == 9 || (botMap[i + 2, j] != 3 && botMap[i + 2, j] != 1)))
                    {
                        botButton[i, j].BackColor = botButton[i + 1, j].BackColor = Color.Black;
                        botMap[i, j] = botMap[i + 1, j] = 4;
       
                        if (i == 9 || j == 10)
                        {
                            if (i == 9 && j != 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                                botMap[i, j - 1] = botMap[i, j + 1] = 5;
                                botMap[i + 1, j - 1] = botMap[i + 1, j + 1] = 5;
                            }
                            if (i == 9 && j == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                            }
                            if (i < 9 && j == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                                botMap[i + 2, j] = botMap[i + 2, j - 1] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                            botMap[i + 2, j - 1] = botMap[i + 2, j] = botMap[i + 2, j + 1] = 5;
                            botMap[i, j - 1] = botMap[i, j + 1] = 5;
                            botMap[i + 1, j - 1] = botMap[i + 1, j + 1] = 5;
                        }
                    }
   
                    if ((botMap[i, j - 1] != 1 && j < 10) && botMap[i, j - 1] != 3 && botMap[i, j] == 3 && botMap[i, j + 1] == 3 && (j == 9 || (botMap[i, j + 2] != 3 && botMap[i, j + 2] != 1)))
                    {
                        botButton[i, j].BackColor = botButton[i, j + 1].BackColor = Color.Black;
                        botMap[i, j] = botMap[i, j + 1] = 4;
              
                        if (j == 9 || i == 10)
                        {
                            if (j == 9 && i != 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                                botMap[i - 1, j] = botMap[i + 1, j] = 5;
                                botMap[i - 1, j + 1] = botMap[i + 1, j + 1] = 5;
                            }
                            if (j == 9 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                                botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                            }
                            if (j < 9 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                                botMap[i - 1, j + 2] = botMap[i, j + 2] = 5;
                                botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j] = botMap[i + 1, j] = 5;
                            botMap[i - 1, j + 1] = botMap[i + 1, j + 1] = 5;
                            botMap[i - 1, j - 1] = botMap[i, j - 1] = botMap[i + 1, j - 1] = 5;
                            botMap[i - 1, j + 2] = botMap[i, j + 2] = botMap[i + 1, j + 2] = 5;
                        }
                    }
                    //однопалубный
                    if (botMap[i - 1, j] != 1 && botMap[i - 1, j] != 3 && botMap[i, j - 1] != 1 && botMap[i, j - 1] != 3 && botMap[i, j] == 3 && (j == 10 || (botMap[i, j + 1] != 3 && botMap[i, j + 1] != 1)) && (i == 10 || (botMap[i + 1, j] != 3 && botMap[i + 1, j] != 1)))
                    {
                        botButton[i, j].BackColor = Color.Black;
                        botMap[i, j] = 4;
                   
                        if (j == 10 || i == 10)
                        {
                            if (j == 10 && i != 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = 5;
                                botMap[i + 1, j] = botMap[i + 1, j - 1] = 5;
                                botMap[i, j - 1] = 5;
                            }
                            if (j == 10 && i == 10)
                            {
                                botMap[i - 1, j] = botMap[i - 1, j - 1] = botMap[i, j - 1] = 5;
                            }
                            if (j != 10 && i == 10)
                            {
                                botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                                botMap[i, j - 1] = botMap[i, j + 1] = 5;
                            }
                        }
                        else
                        {
                            botMap[i - 1, j - 1] = botMap[i - 1, j] = botMap[i - 1, j + 1] = 5;
                            botMap[i, j - 1] = botMap[i, j + 1] = 5;
                            botMap[i + 1, j - 1] = botMap[i + 1, j] = botMap[i + 1, j + 1] = 5;
                        }
                    }
                }
            }
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    switch (botMap[i, j])
                    {
                        case 4:
                            count4++;
                            break;
                        case 5:
                            botButton[i, j].Enabled = false;
                            botButton[i, j].BackColor = Color.Blue;
                            break;
                    }
                }
            }
            if (count4 == 20)
            {
                Game = false;
                DialogResult result = MessageBox.Show("Поздравляем!\n Вы великий капитан.\n Теперь вы можете покинуть игру или пройти бой снова",
                   "                           Поздравления", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                {
                    New_Game(this, new EventArgs());
                }
            }
        }
        
        // Проверка на убийство корабля игрока
        public void Check_Killed_OnMapPlayer() 
        {
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    // четырехпалубный
                    if ((myMap[i, j] == 3 && i < 8) && myMap[i + 1, j] == 3 && myMap[i + 2, j] == 3 && myMap[i + 3, j] == 3)
                    {
                        myButton[i, j].BackColor = myButton[i + 1, j].BackColor = myButton[i + 2, j].BackColor = myButton[i + 3, j].BackColor = Color.Black;
                        myMap[i, j] = myMap[i + 1, j] = myMap[i + 2, j] = myMap[i + 3, j] = 4;
                       
                        if (i == 7 || j == 10)
                        {
                            if (i == 7 && j != 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                                myMap[i, j - 1] = myMap[i, j + 1] = 5;
                                myMap[i + 1, j - 1] = myMap[i + 1, j + 1] = 5;
                                myMap[i + 2, j - 1] = myMap[i + 2, j + 1] = 5;
                                myMap[i + 3, j - 1] = myMap[i + 3, j + 1] = 5;
                            }
                            if (i == 7 && j == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i, j - 1] = myMap[i + 1, j - 1] = myMap[i + 2, j - 1] = myMap[i + 3, j - 1] = 5;
                            }
                            if (i < 7 && j == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i, j - 1] = myMap[i + 1, j - 1] = myMap[i + 2, j - 1] = myMap[i + 3, j - 1] = 5;
                                myMap[i + 4, j] = myMap[i + 4, j - 1] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                            myMap[i + 4, j - 1] = myMap[i + 4, j] = myMap[i + 4, j + 1] = 5;
                            myMap[i, j - 1] = myMap[i, j + 1] = 5;
                            myMap[i + 1, j - 1] = myMap[i + 1, j + 1] = 5;
                            myMap[i + 2, j - 1] = myMap[i + 2, j + 1] = 5;
                            myMap[i + 3, j - 1] = myMap[i + 3, j + 1] = 5;
                        }
                    }
       
                    if ((myMap[i, j] == 3 && j < 8) && myMap[i, j + 1] == 3 && myMap[i, j + 2] == 3 && myMap[i, j + 3] == 3)
                    {
                        myButton[i, j].BackColor = myButton[i, j + 1].BackColor = myButton[i, j + 2].BackColor = myButton[i, j + 3].BackColor = Color.Black;
                        myMap[i, j] = myMap[i, j + 1] = myMap[i, j + 2] = myMap[i, j + 3] = 4;
            
                        if (j == 7 || i == 10)
                        {
                            if (j == 7 && i != 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                                myMap[i - 1, j] = myMap[i + 1, j] = 5;
                                myMap[i - 1, j + 1] = myMap[i + 1, j + 1] = 5;
                                myMap[i - 1, j + 2] = myMap[i + 1, j + 2] = 5;
                                myMap[i - 1, j + 3] = myMap[i + 1, j + 3] = 5;
                            }
                            if (j == 7 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                                myMap[i - 1, j] = myMap[i - 1, j + 1] = myMap[i - 1, j + 2] = myMap[i - 1, j + 3] = 5;
                            }
                            if (j < 7 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                                myMap[i - 1, j + 4] = myMap[i, j + 4] = 5;
                                myMap[i - 1, j] = myMap[i - 1, j + 1] = myMap[i - 1, j + 2] = myMap[i - 1, j + 3] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j] = myMap[i + 1, j] = 5;
                            myMap[i - 1, j + 1] = myMap[i + 1, j + 1] = 5;
                            myMap[i - 1, j + 2] = myMap[i + 1, j + 2] = 5;
                            myMap[i - 1, j + 3] = myMap[i + 1, j + 3] = 5;
                            myMap[i - 1, j - 1] = myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                            myMap[i - 1, j + 4] = myMap[i, j + 4] = myMap[i + 1, j + 4] = 5;
                        }
                    }
                    // трехпалубный
                    if ((myMap[i - 1, j] != 1 && i < 9) && myMap[i - 1, j] != 3 && myMap[i, j] == 3 && myMap[i + 1, j] == 3 && myMap[i + 2, j] == 3 && (i == 8 || (myMap[i + 3, j] != 3 && myMap[i + 3, j] != 1)))
                    {
                        myButton[i, j].BackColor = myButton[i + 1, j].BackColor = myButton[i + 2, j].BackColor = Color.Black;
                        myMap[i, j] = myMap[i + 1, j] = myMap[i + 2, j] = 4;
           
                        if (i == 8 || j == 10)
                        {
                            if (i == 8 && j != 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                                myMap[i, j - 1] = myMap[i, j + 1] = 5;
                                myMap[i + 1, j - 1] = myMap[i + 1, j + 1] = 5;
                                myMap[i + 2, j - 1] = myMap[i + 2, j + 1] = 5;
                            }
                            if (i == 8 && j == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i, j - 1] = myMap[i + 1, j - 1] = myMap[i + 2, j - 1] = 5;
                            }
                            if (i < 8 && j == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i, j - 1] = myMap[i + 1, j - 1] = myMap[i + 2, j - 1] = 5;
                                myMap[i + 3, j] = myMap[i + 3, j - 1] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                            myMap[i + 3, j - 1] = myMap[i + 3, j] = myMap[i + 3, j + 1] = 5;
                            myMap[i, j - 1] = myMap[i, j + 1] = 5;
                            myMap[i + 1, j - 1] = myMap[i + 1, j + 1] = 5;
                            myMap[i + 2, j - 1] = myMap[i + 2, j + 1] = 5;
                        }
                    }
  
                    if ((myMap[i, j - 1] != 1 && j < 9) && myMap[i, j - 1] != 3 && myMap[i, j] == 3 && myMap[i, j + 1] == 3 && myMap[i, j + 2] == 3 && (j == 8 || (myMap[i, j + 3] != 3 && myMap[i, j + 3] != 1)))
                    {
                        myButton[i, j].BackColor = myButton[i, j + 1].BackColor = myButton[i, j + 2].BackColor = Color.Black;
                        myMap[i, j] = myMap[i, j + 1] = myMap[i, j + 2] = 4;
  
                        if (j == 8 || i == 10)
                        {
                            if (j == 8 && i != 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                                myMap[i - 1, j] = myMap[i + 1, j] = 5;
                                myMap[i - 1, j + 1] = myMap[i + 1, j + 1] = 5;
                                myMap[i - 1, j + 2] = myMap[i + 1, j + 2] = 5;
                            }
                            if (j == 8 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                                myMap[i - 1, j] = myMap[i - 1, j + 1] = myMap[i - 1, j + 2] = 5;
                            }
                            if (j < 8 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                                myMap[i - 1, j + 3] = myMap[i, j + 3] = 5;
                                myMap[i - 1, j] = myMap[i - 1, j + 1] = myMap[i - 1, j + 2] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j] = myMap[i + 1, j] = 5;
                            myMap[i - 1, j + 1] = myMap[i + 1, j + 1] = 5;
                            myMap[i - 1, j + 2] = myMap[i + 1, j + 2] = 5;
                            myMap[i - 1, j - 1] = myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                            myMap[i - 1, j + 3] = myMap[i, j + 3] = myMap[i + 1, j + 3] = 5;
                        }
                    }
                    // двухпалубный
                    if ((myMap[i - 1, j] != 1 && i < 10) && myMap[i - 1, j] != 3 && myMap[i, j] == 3 && myMap[i + 1, j] == 3 && (i == 9 || (myMap[i + 2, j] != 3 && myMap[i + 2, j] != 1)))
                    {
                        myButton[i, j].BackColor = myButton[i + 1, j].BackColor = Color.Black;
                        myMap[i, j] = myMap[i + 1, j] = 4;
    
                        if (i == 9 || j == 10)
                        {
                            if (i == 9 && j != 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                                myMap[i, j - 1] = myMap[i, j + 1] = 5;
                                myMap[i + 1, j - 1] = myMap[i + 1, j + 1] = 5;
                            }
                            if (i == 9 && j == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                            }
                            if (i < 9 && j == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                                myMap[i + 2, j] = myMap[i + 2, j - 1] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                            myMap[i + 2, j - 1] = myMap[i + 2, j] = myMap[i + 2, j + 1] = 5;
                            myMap[i, j - 1] = myMap[i, j + 1] = 5;
                            myMap[i + 1, j - 1] = myMap[i + 1, j + 1] = 5;
                        }
                    }
      
                    if ((myMap[i, j - 1] != 1 && j < 10) && myMap[i, j - 1] != 3 && myMap[i, j] == 3 && myMap[i, j + 1] == 3 && (j == 9 || (myMap[i, j + 2] != 3 && myMap[i, j + 2] != 1)))
                    {
                        myButton[i, j].BackColor = myButton[i, j + 1].BackColor = Color.Black;
                        myMap[i, j] = myMap[i, j + 1] = 4;
      
                        if (j == 9 || i == 10)
                        {
                            if (j == 9 && i != 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                                myMap[i - 1, j] = myMap[i + 1, j] = 5;
                                myMap[i - 1, j + 1] = myMap[i + 1, j + 1] = 5;
                            }
                            if (j == 9 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                                myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                            }
                            if (j < 9 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                                myMap[i - 1, j + 2] = myMap[i, j + 2] = 5;
                                myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j] = myMap[i + 1, j] = 5;
                            myMap[i - 1, j + 1] = myMap[i + 1, j + 1] = 5;
                            myMap[i - 1, j - 1] = myMap[i, j - 1] = myMap[i + 1, j - 1] = 5;
                            myMap[i - 1, j + 2] = myMap[i, j + 2] = myMap[i + 1, j + 2] = 5;
                        }
                    }
                    // однопалубный
                    if (myMap[i - 1, j] != 1 && myMap[i - 1, j] != 3 && myMap[i, j - 1] != 1 && myMap[i, j - 1] != 3 && myMap[i, j] == 3 && (j == 10 || (myMap[i, j + 1] != 3 && myMap[i, j + 1] != 1)) && (i == 10 || (myMap[i + 1, j] != 3 && myMap[i + 1, j] != 1)))
                    {
                        myButton[i, j].BackColor = Color.Black;
                        myMap[i, j] = 4;
     
                        if (j == 10 || i == 10)
                        {
                            if (j == 10 && i != 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = 5;
                                myMap[i + 1, j] = myMap[i + 1, j - 1] = 5;
                                myMap[i, j - 1] = 5;
                            }
                            if (j == 10 && i == 10)
                            {
                                myMap[i - 1, j] = myMap[i - 1, j - 1] = myMap[i, j - 1] = 5;
                            }
                            if (j != 10 && i == 10)
                            {
                                myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                                myMap[i, j - 1] = myMap[i, j + 1] = 5;
                            }
                        }
                        else
                        {
                            myMap[i - 1, j - 1] = myMap[i - 1, j] = myMap[i - 1, j + 1] = 5;
                            myMap[i, j - 1] = myMap[i, j + 1] = 5;
                            myMap[i + 1, j - 1] = myMap[i + 1, j] = myMap[i + 1, j + 1] = 5;
                        }
                    }
                }
            }

            for (int i = 1; i < mapSize; i++) // Окраска полей вокруг убитого корабля
            {
                for (int j = 1; j < mapSize; j++)
                    {
                    if (myMap[i, j] == 5)
                    {
                        myButton[i, j].Enabled = false;
                        myButton[i, j].BackColor = Color.Blue;
                    }
                }
            }

            int cell = 0;
            for (int i = 1; i < mapSize; i++) // Счетчик убитых 
            {
                for (int j = 1; j < mapSize; j++)
                {
                    if (myMap[i, j] == 4)
                    {
                        cell++;
                    }
                }
            }
            if (cell == 20) // Если убитых клеток 20, то все корабли убиты
            {
                Game = false;
                for (int i = 1; i < mapSize; i++)
                {
                    for (int j = 1; j < mapSize; j++)
                    {
                        if (botMap[i, j] == 1)
                        {
                            botButton[i, j].BackColor = Color.Green;
                        }
                    }
                }
                DialogResult result = MessageBox.Show("Вы проиграли! Начать заново? ", "Конец игры", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Restart();
                }
            }
        }

        //КНОПКИ В ФОРМЕ ИГРЫ

        //Очистка полей и начало новой игры
        public void New_Game(object sender, EventArgs e)
        {
            DialogResult restart = MessageBox.Show("Начать новую игру? ", "Новая игра", MessageBoxButtons.YesNo);
            if (restart == DialogResult.Yes)
            {
                Restart();
            }
        }

        //Сброс полей. Начать новую игру
        public void Restart()
        {
            startButton.Enabled = true;
            isPlayer = isBot = false;
            PShoot = false;
            BShoot = true;
            Game = true;
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    myMap[i, j] = 0;
                    botMap[i, j] = 0;
                    if (i != 0 && j != 0)
                    {
                        myButton[i, j].BackColor = Color.White;
                        myButton[i, j].Text = "";
                        myButton[i, j].Enabled = true;
                        botButton[i, j].BackColor = Color.White;
                        botButton[i, j].Enabled = true;
                    }
                }
            }
        }

        // Сохранить текущую игру
        public void SaveGame(object sender, EventArgs e)
        {
            mapinfo = "";
            s.Append('B');
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    s.Append($"{myMap[i, j]}");
                }
            }
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    s.Append($"{botMap[i, j]}");
                }
            }
            if (isPlayer == true) s.Append("1");
            else s.Append("0");
            if (isBot == true) s.Append("1");
            else s.Append("0");
            if (Game == true) s.Append("1");
            else s.Append("0");
            if (BShoot == true) s.Append("1");
            else s.Append("0");
            if (startButton.Enabled == true) s.Append("1");
            else s.Append("0");
            mapinfo = s.ToString();
            mapinfo += "\n||\n";

            saveFileDialog.Filter = "Text files(*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog.FileName;
            System.IO.File.WriteAllText(filename, mapinfo);
            MessageBox.Show("Игра сохранена");
        }

        // Загрузить игру
        public void LoadGame(object sender, EventArgs e)
        {

            mapinfo = "";
            int k = 0;
            openFileDialog.Filter = "Text files(*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog.FileName;
            mapinfo = System.IO.File.ReadAllText(filename);
            if (mapinfo[k] != 'B')
            {
                MessageBox.Show("Не удалось загрузить сохранение.");
                return;
            }
            k++;
            Restart();
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    myMap[i, j] = (int)(mapinfo[k] - 48);
                    switch (myMap[i, j])
                    {
                        case 1:
                            myButton[i, j].BackColor = Color.Red;
                            break;
                        case 3:
                            myButton[i, j].BackColor = Color.Red;
                            myButton[i, j].Text = "X";
                            break;
                        case 4:
                            myButton[i, j].BackColor = Color.Black;
                            break;
                        case 5:
                            myButton[i, j].BackColor = Color.Blue;
                            break;
                    }
                    k++;
                }
            }

            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    botMap[i, j] = (int)(mapinfo[k] - 48);
                    switch (botMap[i, j])
                    {
                        case 3:
                            botButton[i, j].BackColor = Color.Red;
                            botButton[i, j].Enabled = false;
                            break;
                        case 4:
                            botButton[i, j].BackColor = Color.Black;
                            botButton[i, j].Enabled = false;
                            break;
                        case 5:
                            botButton[i, j].BackColor = Color.Blue;
                            botButton[i, j].Enabled = false;
                            break;
                    }
                    k++;
                }
            }
            if (mapinfo[k] == '1') isPlayer = true;
            else isPlayer = false;
            k++;
            if (mapinfo[k] == '1') isBot = true;
            else isBot = false;
            k++;
            if (mapinfo[k] == '1') Game = true;
            else Game = false;
            k++;
            if (mapinfo[k] == '1') BShoot = true;
            else BShoot = false;
            k++;
            if (mapinfo[k] == '1') startButton.Enabled = true;
            else startButton.Enabled = false;
            for (int i = 210; i < mapinfo.Length; i++)
            {

            }
            MessageBox.Show("Игра загружена");
        }

        private void Игра_Load(object sender, EventArgs e)
        {
            popup = new PopupNotifier();
            popup.Image = Properties.Resources.d1ac13d20ebc0bac976b1c597912571e;
            popup.ImageSize = new Size(96, 96);
            popup.TitleText = "Подсказка";
            popup.ContentText = "Компьютер добивает раненые корабли. Используйте его временные промахи, чтобы побыстрее уничтожить флот соперника.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            popup.Popup();
        }
    }
}