﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProjectSnake
{
    public partial class MainForm : Form
    {
        private double aspectRatio = 0.75;

        private Engine _engine;
        private WinFormsRenderer _renderer;

        private readonly Timer _frameTimer = new Timer();
        private ScoreLabel[] _scoreLabels;

        private readonly Button _startGameButton = new Button();
        private readonly ComboBox _playerCountComboBox = new ComboBox();
        private readonly FlowLayoutPanel _mainMenuControls = new FlowLayoutPanel();
        // EXTENDED
        private readonly FlowLayoutPanel _gameOverPanel = new FlowLayoutPanel();

        private readonly Label _pauseLabel = new Label();

        public MainForm(int width = 800)
        {
            InitializeComponent();
            ClientSize = new Size(width, (int)(width * aspectRatio));
            BackColor = Gruvbox.Black;
            DoubleBuffered = true;

            Text = "Snake 🐍";

            SetupUi();
        }

        private void SetupUi()
        {
            var titleLabel = new Label();
            titleLabel.Text = "Snake";
            titleLabel.Anchor = AnchorStyles.None;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.AutoSize = true;
            titleLabel.ForeColor = Gruvbox.White;
            titleLabel.Font = new Font(this.Font.FontFamily, 40);
            titleLabel.ForeColor = Gruvbox.Green;

            _pauseLabel.Visible = false;
            _pauseLabel.Text = "Paused";
            _pauseLabel.TextAlign = ContentAlignment.MiddleCenter;
            _pauseLabel.Font = new Font(this.Font.FontFamily, 20);
            _pauseLabel.Anchor = AnchorStyles.None;
            _pauseLabel.AutoSize = true;
            _pauseLabel.ForeColor = Gruvbox.White;
            _pauseLabel.BackColor = Gruvbox.DarkGray;
            Controls.Add(_pauseLabel);
            _pauseLabel.Location = GetCenterPointForSizeInSize(ClientSize, _pauseLabel.Size);

            _startGameButton.Text = "Start Game";
            _startGameButton.ForeColor = Gruvbox.White;
            _startGameButton.BackColor = Gruvbox.DarkGray;
            _startGameButton.TextAlign = ContentAlignment.MiddleCenter;
            _startGameButton.Anchor = AnchorStyles.None;
            _startGameButton.AutoSize = true;
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
            _playerCountComboBox.FlatStyle = FlatStyle.Flat;
            _playerCountComboBox.BackColor = Gruvbox.DarkGray;
            _playerCountComboBox.ForeColor = Gruvbox.White;
            // Tillåt inte att skriva egna värden.
            _playerCountComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            var playerCountLabel = new Label();
            playerCountLabel.Text = "Player Count: ";
            playerCountLabel.ForeColor = Gruvbox.White;
            playerCountLabel.TextAlign = ContentAlignment.MiddleCenter;
            playerCountLabel.Anchor = AnchorStyles.None;
            playerCountLabel.AutoSize = true;

            FlowLayoutPanel playerCountControls = new FlowLayoutPanel();
            playerCountControls.FlowDirection = FlowDirection.LeftToRight;
            playerCountControls.Controls.Add(playerCountLabel);
            playerCountControls.Controls.Add(_playerCountComboBox);
            playerCountControls.Anchor = AnchorStyles.None;
            playerCountControls.AutoSize = true;

            _mainMenuControls.FlowDirection = FlowDirection.TopDown;
            _mainMenuControls.Anchor = AnchorStyles.None;
            _mainMenuControls.AutoSize = true;
            _mainMenuControls.Controls.Add(titleLabel);
            _mainMenuControls.Controls.Add(playerCountControls);
            _mainMenuControls.Controls.Add(_startGameButton);

            Controls.Add(_mainMenuControls);

            _mainMenuControls.Location = GetCenterPointForSizeInSize(ClientSize, _mainMenuControls.Size);
        }

        // Ger tillbaka nordvästra hörnet för contained om den skulle centreras i container.
        private Point GetCenterPointForSizeInSize(Size container, Size contained)
        {
            return new Point(container.Width / 2 - contained.Width / 2, container.Height / 2 - contained.Height / 2);
        }

        private void StartGameButtonOnClick(object sender, EventArgs e)
        {
            _mainMenuControls.Visible = false;

            // När man trycker på knappen så tappar MainForm fokus
            // så för att registrera tangenttryck måste vi ta tillbaka fokus.
            Focus();

            // EXTENDED
            InitializeGame();

            foreach (var label in _scoreLabels)
            {
                Controls.Add(label);
            }

            Paint += MainFormOnPaint;
            KeyDown += MainFormOnKeyDown;
            _frameTimer.Tick += FrameTimerOnTick;
            _frameTimer.Interval = 1000 / 60;
            _frameTimer.Start();
        }

        // EXTENDED
        private void InitializeGame()
        {
            _engine = new Engine(_playerCountComboBox.SelectedIndex + 1);
            _renderer = new WinFormsRenderer(_engine.Board);
            _scoreLabels = InitializeScoreLabels(_engine.Players);
        }

        private void MainFormOnKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            if (key == Keys.Escape && !_engine.GameOver)
            {
                _engine.IsPaused = !_engine.IsPaused;
                _pauseLabel.Visible = _engine.IsPaused;
                return;
            }

            // Tillåt inte spelarinput när spelet är pausat.
            if (_engine.IsPaused)
            {
                return;
            }

            foreach (var player in _engine.Players)
            {
                var direction = player.CurrentControls.ToDirection(key);
                if (direction == null)
                {
                    continue;
                }

                player.Snake.Move((Direction)direction);
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

        private void MainFormOnPaint(Object obj, PaintEventArgs e)
        {
            _renderer.Clear();

            _engine.Draw(_renderer);

            foreach (var label in _scoreLabels)
            {
                _renderer.Draw(label);
            }

            _renderer.Display((Control)obj, e.Graphics);
        }

        private void FrameTimerOnTick(object sender, EventArgs e)
        {
            if (_engine.GameOver)
            {
                _engine.ClearBoard();
                GameOverDisplay();
                Refresh();
                _frameTimer.Stop();
            }
            else
            {
                _engine.Tick();
            }

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

        private void GameOverDisplay()
        {
            // FlowOutPanel som allt kommer läggas in i
            _gameOverPanel.FlowDirection = FlowDirection.TopDown;
            _gameOverPanel.Anchor = AnchorStyles.None;
            _gameOverPanel.AutoSize = true;
            _gameOverPanel.BackColor = Gruvbox.DarkGray;
            _gameOverPanel.Location = GetCenterPointForSizeInSize(ClientSize, _gameOverPanel.Size);

            // Text som säger att spelet är över
            var gameOverLabel = new Label();
            gameOverLabel.Text = "Game over!";
            gameOverLabel.ForeColor = Gruvbox.White;
            gameOverLabel.Font = new Font(this.Font.FontFamily, 24);
            gameOverLabel.Size = new Size(200, 50);
            gameOverLabel.TextAlign = ContentAlignment.MiddleCenter;
            _gameOverPanel.Controls.Add(gameOverLabel);


            // Lägg till spelarnas poäng som en ny Label. I ordning från högst till lägst poäng
            var sortedPlayers = _engine.Players.OrderByDescending(p => p.Score).ToList();
            for (var i = 0; i < sortedPlayers.Count; i++)
            {
                var player = sortedPlayers[i];
                Label playerScore = new Label();

                playerScore.TextAlign = ContentAlignment.MiddleCenter;
                playerScore.Dock = DockStyle.Fill;
                playerScore.ForeColor = Gruvbox.White;
                playerScore.Font = new Font(this.Font.FontFamily, 14);

                // Ändra bakgrunden till samma färg som spelarens orm
                playerScore.BackColor = player.Snake.ColorScheme.Head;
                playerScore.Text = $"{player.Score}";

                _gameOverPanel.Controls.Add(playerScore);
            }

            // EXTENDED
            var restartButton = new Button();
            restartButton.Text = "Restart";
            restartButton.ForeColor = Gruvbox.White;
            restartButton.BackColor = Gruvbox.Gray;
            restartButton.Anchor = AnchorStyles.None;
            restartButton.FlatStyle = FlatStyle.Flat;
            restartButton.FlatAppearance.BorderSize = 0;
            restartButton.Font = new Font(this.Font.FontFamily, 20);
            restartButton.TextAlign = ContentAlignment.MiddleCenter;
            //restartButton.Size = new Size(200, 50);
            restartButton.Margin = new Padding(20);
            restartButton.AutoSize = true;
            restartButton.Click += RestartButtonOnClick;

            _gameOverPanel.Controls.Add(restartButton);
            _gameOverPanel.Visible = true;

            Controls.Add(_gameOverPanel);
        }

        // EXTENDED
        private void RestartButtonOnClick(object sender, EventArgs e)
        {
            _gameOverPanel.Visible = false;
            _gameOverPanel.Controls.Clear();
            Controls.Remove(_gameOverPanel);
            foreach (var scoreLabel in _scoreLabels)
            {
                Controls.Remove(scoreLabel);
            }

            InitializeGame();
            foreach (var scoreLabel in _scoreLabels)
            {
                Controls.Add(scoreLabel);
            }

            _frameTimer.Start();
            Focus();
        }
    }
}
