using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DZ7_NickPustovoy_Var4_IVT_21B
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            gameTimer.Tick += GameTimer_Elapsed;
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            buttons[0] = red;
            buttons[1] = green;
            buttons[2] = blue;
            buttons[3] = yellow;

        }

        private void GameTimer_Elapsed(object sender, EventArgs e)
        {
            bool lightUp = currentStep % 2 == 0;
            int buttonIdx = sequence[currentStep / 2];
            setImageLight(buttons[buttonIdx], lightUp);
            currentStep++;
            if(currentStep/2 == sequenceLength)
            {
                gameTimer.Stop();
                currentStep = 0;
                awaitingUserInput = true;
            }
        }

        private static void setImageLight(Image image, bool lightUp)
        {
            string packUri;
            if (lightUp)
                packUri = $"pack://application:,,,/assets/{image.Name}Pressed.png";
            else
                packUri = $"pack://application:,,,/assets/{image.Name}.png";
            image.Source = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
        }

        private void red_MouseDown(object sender, MouseButtonEventArgs e)
        {
            setImageLight(sender as Image, true);
        }

        private void red_MouseUp(object sender, MouseButtonEventArgs e)
        {
            setImageLight(sender as Image, false);
            if(awaitingUserInput)
            {
                int buttonIdx = Array.IndexOf(buttons, sender);
                if (buttonIdx != sequence[currentStep])
                {

                    gameOver();
                    return;
                }
                else
                {
                    currentStep++;
                    pnt = pnt + 100;
                    points.Content = pnt;
                }
                if (currentStep == sequenceLength)
                    startRound();
            }
        }

        private void gameOver()
        {
            awaitingUserInput = false;
            start.Visibility = Visibility.Visible;
            if (mpnt < pnt)
            {
                mpnt = pnt;
            }
            mxpnt.Content = mpnt;
            round = 0;
            Lb.Content = $"Вы проиграли";
            points.Content = "0";
            pnt = 0;
        }

        List<int> sequence;
        int round = 0;
        int pnt = 0;
        int mpnt = 0;
        int sequenceLength;
        int currentStep;
        bool awaitingUserInput;
        DispatcherTimer gameTimer = new DispatcherTimer();
        Image[] buttons = new Image[4];
        private void startGame()
        {
            start.Visibility = Visibility.Hidden;
            sequenceLength = 0;
            startRound();
        }
        private void startRound()
        {
            
            sequenceLength++;
            currentStep = 0;
            awaitingUserInput = false;
            sequence = generateSequence(sequenceLength);
            gameTimer.Start();
            round = round + 1;
            Lb.Content = $"Раунд {round}";
        }

        private List<int> generateSequence(int Length)
        {
            List<int> sequence = new List<int>();
            var rand = new Random();
            for(int i = 0; i < Length; i++)
            {
                sequence.Add(rand.Next(4));
            }
            return sequence;
        }

        private void start_MouseUp(object sender, MouseButtonEventArgs e)
        {
            startGame();
        }
    }
    
}
