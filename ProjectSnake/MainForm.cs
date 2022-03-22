﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectSnake
{
    public partial class MainForm : Form
    {
        private double aspectRatio = 0.75;

        private Engine _engine;
        private WinFormsRenderer _renderer;

        private Timer _timer = new Timer();
        private ScoreLabel[] _scoreLabels;

        private Button _startGameButton = new Button();
        private ComboBox _playerCountComboBox = new ComboBox();

        public MainForm(int width = 800)
        {
            InitializeComponent();
            ClientSize = new Size(width, (int)(width * aspectRatio));
            BackColor = Gruvbox.Black;
            DoubleBuffered = true;

            _startGameButton.Text = "Start Game";
            _startGameButton.Location = new Point(ClientSize.Width / 2 - _startGameButton.Width / 2,
                ClientSize.Height / 2 - _startGameButton.Height / 2);
            _startGameButton.ForeColor = Color.White;
            _startGameButton.BackColor = Color.DimGray;
            _startGameButton.FlatStyle = FlatStyle.Flat;
            _startGameButton.FlatAppearance.BorderSize = 0;
            _startGameButton.Click += StartGameButtonOnClick;

            var playerCountStrings = new Object[Engine.MaxPlayerCount];
            for (var i = 0; i < playerCountStrings.Length; ++i)
            {
                playerCountStrings[i] = i + 1;
            }

            _playerCountComboBox.Items.AddRange(playerCountStrings);
            _playerCountComboBox.MaxDropDownItems = _playerCountComboBox.Items.Count;
            _playerCountComboBox.SelectedIndex = 0;
            _playerCountComboBox.AutoSize = true;
            _playerCountComboBox.Anchor = AnchorStyles.None;
            _playerCountComboBox.Location = new Point(ClientSize.Width / 2 - _playerCountComboBox.Width / 2,
                _startGameButton.Top - _playerCountComboBox.Height);
            _playerCountComboBox.FlatStyle = FlatStyle.Flat;
            _playerCountComboBox.BackColor = Color.DimGray;
            _playerCountComboBox.ForeColor = Color.White;
            // Tillåt inte att skriva egna värden.
            _playerCountComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            Controls.Add(_startGameButton);
        }

        private void StartGameButtonOnClick(object sender, EventArgs e)
        {
            _startGameButton.Visible = false;
            _playerCountComboBox.Visible = false;

            // När man trycker på knappen så tappar MainForm fokus
            // så för att registrera tangenttryck måste vi ta tillbaka fokus.
            Focus();

            _engine = new Engine(_playerCountComboBox.SelectedIndex + 1);
            _renderer = new WinFormsRenderer(_engine.Board);

            _scoreLabels = InitializeScoreLabels(_engine.Players);

            foreach (var label in _scoreLabels)
            {
                Controls.Add(label);
            }

            Paint += Draw;
            KeyDown += MainOnKeyDown;
            _timer.Tick += TimerEvent;
            _timer.Interval = 1000 / 60;
            _timer.Start();
        }

        private void MainOnKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            foreach (var player in _engine.Players)
            {
                var dirction = player.controls.todirection(key);
                if (dirction == null)
                {
                    continue;
                }

                player.Snake.Move((Direction)dirction);
            }
        }

        private ScoreLabel[] InitializeScoreLabels(Player[] players)
        {
            var labels = new ScoreLabel[players.Length];
            for (var i = 0; i < labels.Length; ++i)
            {
                var label = new ScoreLabel(players[i]);
                label.Location = new Point(0, i * label.Height);
                labels[i] = label;
            }

            return labels;
        }

        private void Draw(Object obj, PaintEventArgs e)
        {
            _renderer.Clear();

            _engine.Draw(_renderer);

            foreach (var label in _scoreLabels)
            {
                _renderer.Draw(label);
            }

            _renderer.Display((Control)obj, e.Graphics);
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            _engine.Tick();

            Refresh();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // NOTE(Johan): Om vi bara modifierar ClientSize.Height på detta sätt så går det bara
            // att ändra storleken genom att dra fönstret horisontellt, inte vertikalt.

            // ClientSize är storleken på fönstrets faktiska innehåll, utan title bar och liknande.
            ClientSize = new Size(ClientSize.Width, (int)(ClientSize.Width * aspectRatio));
            Refresh();
        }
    }
}
