/*
     ___       _______   ______         __________   ___ 
    /   \     /  _____| /      |       |   ____\  \ /  / 
   /  ^  \   |  |  __  |  ,----' ______|  |__   \  V  /  
  /  /_\  \  |  | |_ | |  |     |______|   __|   >   <   
 /  _____  \ |  |__| | |  `----.       |  |____ /  .  \  
/__/     \__\ \______|  \______|       |_______/__/ \__\ 
                                                          
                                                                                           
             -+#*:                                                                        
           =**###%*:                                                                      
         :******###%#-                                                                    
        -*+++*****#%%%#-          .-+#*=:                                                 
       :+++++****#****#%*:       -+*=--::--:.                                             
      .*+++++++**++++***#%+    :-=+::+*+:=**+=-::.                                        
      =+++++++**=++++++**##*.:----..:-+=*++*****#=-::.                                    
      #*******#++++++++++++=+-::.     ..--===+=::*#*=-==:-:.:                             
      -+*****##***+++++++-=-:..        :.:=----=*#**=+***+++*=:                           
          .:-=+===+*****+--.          ..:----==--==*++*+++*+*++-.                         
                    ..::--:.          ..:::::----=----==+++++++=--=-:                     
                        =-::         .......::::-:-----==-====+=-::=*+:                   
                       :--:.        ...........::-:::-+=-=---===+==*++++                  
                       +-=-       ................:-:*++-=--..:-=*#++#**=                 
                      .===:      :.::::...........:.-=.:--:....:+#+++++**=                
                      .+++.. .. :.:==-::::............:::. ...:**==+++*****-              
                    =#%#*+... ..::++++-:-::::::::......:.....:+*+=+==+*#***#-             
                    **=**+*+-..:.=++++-::-=-------:::::. ...:+*+*+====+++++**-            
                    :+=--+*#*=.::=+***+=======----:--:.    .+*####**+=+++++**#=           
                   +%##*.####*     .-+*++++=====-:::::    :*##%%%#%%%#*+++++++*+          
                   +#*+- *#*=:        .:=++*+++=-:::-:.. .*##%%%%%%%%%%##*+===+*:         
                                         :.::=+++=---=:--=##%%%%%%%%%%%%%%%#*+*#-         
                                           --=--+++::-:-==**#########%%#%%%%##*#**:       
                                              .  .--:..=#+++++++++++++****+++=.-..:       
                                                   =-:-=+++**=--:::::...                  
                                                    .:::::::-                             
                                                                                          
  External AGC DSKY for ReEntry CM/CSM
  Compiled on VS2022 17.10.3 // .NET Framework 4.8.1
  by Sputterfish
  This project uses fonts from the DSKY-FONTS project @ https://github.com/ehdorrii/dsky-fonts
  This project uses some code from the ReEntryUDP example project @ https://github.com/ReentryGame/ReentryUDP
*/
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Text.Json;
using System.IO;
#if DEBUG
using System.Diagnostics;
#endif
#pragma warning disable CS0168


namespace AGC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool AreWeDarkMode;

        public MainWindow()
        {
            InitializeComponent();
            AreWeDarkMode = false;
            MouseDown += Window_MouseDown;
        }

        //MainWindow movement
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        //Updates all the displayed values and flashes the anun lights
        public void UpdateStoredValues()
        {

            Verb.Content = AGCStorage.VerbD1 + AGCStorage.VerbD2;
            Noun.Content = AGCStorage.NounD1 + AGCStorage.NounD2;
            Prog.Content = AGCStorage.ProgramD1 + AGCStorage.ProgramD2;
            Register1.Content = AGCStorage.Register1Sign + " " + AGCStorage.Register1D1 + AGCStorage.Register1D2 + AGCStorage.Register1D3 + AGCStorage.Register1D4 + AGCStorage.Register1D5;
            Register2.Content = AGCStorage.Register2Sign + " " + AGCStorage.Register2D1 + AGCStorage.Register2D2 + AGCStorage.Register2D3 + AGCStorage.Register2D4 + AGCStorage.Register2D5;
            Register3.Content = AGCStorage.Register3Sign + " " + AGCStorage.Register3D1 + AGCStorage.Register3D2 + AGCStorage.Register3D3 + AGCStorage.Register3D4 + AGCStorage.Register3D5;

            // COMP ACTY
            if (AGCStorage.IlluminateCompLight==true)
            {
                CompActy.Visibility = Visibility.Visible;
            }
            else { CompActy.Visibility = Visibility.Hidden; }

            //LEFT SIDE LIGHTS
            //UPLINK ACTY
            if (AGCStorage.IlluminateUplinkActy > 0) 
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/lit/uplinkacty.png", UriKind.Relative);
                UplinkActy.Source = new BitmapImage(uriSource);
            }
            else
            {
                if (AreWeDarkMode == true)
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                    UplinkActy.Source = new BitmapImage(uriSource);
                }
                else
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/uplinkacty.png", UriKind.Relative);
                    UplinkActy.Source = new BitmapImage(uriSource);
                }
            }
            //NO ATT
            if (AGCStorage.IlluminateNoAtt > 0)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/lit/noatt.png", UriKind.Relative);
                NoAtt.Source = new BitmapImage(uriSource);
            }
            else
            {
                if (AreWeDarkMode == true)
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                    NoAtt.Source = new BitmapImage(uriSource);
                }
                else
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/noatt.png", UriKind.Relative);
                    NoAtt.Source = new BitmapImage(uriSource);
                }
            }
            //STBY
            if (AGCStorage.IlluminateStby > 0)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/lit/stby.png", UriKind.Relative);
                Stby.Source = new BitmapImage(uriSource);
            }
            else
            {
                if (AreWeDarkMode == true)
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                    Stby.Source = new BitmapImage(uriSource);
                }
                else
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/stby.png", UriKind.Relative);
                    Stby.Source = new BitmapImage(uriSource);
                }
            }
            //KEY REL
            if (AGCStorage.IlluminateKeyRel > 0)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/lit/keyrel.png", UriKind.Relative);
                KeyRel.Source = new BitmapImage(uriSource);
            }
            else
            {
                if (AreWeDarkMode == true)
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                    KeyRel.Source = new BitmapImage(uriSource);
                }
                else
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/keyrel.png", UriKind.Relative);
                    KeyRel.Source = new BitmapImage(uriSource);
                }
            }
            //OPR ERR
            if (AGCStorage.IlluminateOprErr > 0)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/lit/oprerr.png", UriKind.Relative);
                OprErr.Source = new BitmapImage(uriSource);
            }
            else
            {
                if (AreWeDarkMode == true)
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                    OprErr.Source = new BitmapImage(uriSource);
                }
                else
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/oprerr.png", UriKind.Relative);
                    OprErr.Source = new BitmapImage(uriSource);
                }
            }

            //RIGHT SIDE LIGHTS
            //TEMP
            if (AGCStorage.IlluminateTemp > 0)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/lit/temp.png", UriKind.Relative);
                Temp.Source = new BitmapImage(uriSource);
            }
            else
            {
                if (AreWeDarkMode == true)
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                    Temp.Source = new BitmapImage(uriSource);
                }
                else
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/temp.png", UriKind.Relative);
                    Temp.Source = new BitmapImage(uriSource);
                }
            }
            //GIMBAL LOCK
            if (AGCStorage.IlluminateGimbalLock > 0)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/lit/gimballock.png", UriKind.Relative);
                Gimballock.Source = new BitmapImage(uriSource);
            }
            else
            {
                if (AreWeDarkMode == true)
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                    Gimballock.Source = new BitmapImage(uriSource);
                }
                else
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/gimballock.png", UriKind.Relative);
                    Gimballock.Source = new BitmapImage(uriSource);
                }
            }
            //PROG
            if (AGCStorage.IlluminateProg > 0)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/lit/prog.png", UriKind.Relative);
                Program.Source = new BitmapImage(uriSource);
            }
            else
            {
                if (AreWeDarkMode == true)
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                    Program.Source = new BitmapImage(uriSource);
                }
                else
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/prog.png", UriKind.Relative);
                    Program.Source = new BitmapImage(uriSource);
                }
            }
            //RESTART
            if (AGCStorage.IlluminateRestart > 0)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/lit/restart.png", UriKind.Relative);
                Restart.Source = new BitmapImage(uriSource);
            }
            else
            {
                if (AreWeDarkMode == true)
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                    Restart.Source = new BitmapImage(uriSource);
                }
                else
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/restart.png", UriKind.Relative);
                    Restart.Source = new BitmapImage(uriSource);
                }
            }
            //TRACKER
            if (AGCStorage.IlluminateTracker > 0)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/lit/tracker.png", UriKind.Relative);
                Tracker.Source = new BitmapImage(uriSource);
            }
            else
            {
                if (AreWeDarkMode == true)
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                    Tracker.Source = new BitmapImage(uriSource);
                }
                else
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/tracker.png", UriKind.Relative);
                    Tracker.Source = new BitmapImage(uriSource);
                }
            }
            /*
             * Lights for ALT and VEL not present
            //ALT
            if (AGCStorage.IlluminateAlt > 0)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/lit/alt.png", UriKind.Relative);
                Alt.Source = new BitmapImage(uriSource);
            }
            else
            {
                if (AreWeDarkMode == true)
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                    Alt.Source = new BitmapImage(uriSource);
                }
                else
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/alt.png", UriKind.Relative);
                    Alt.Source = new BitmapImage(uriSource);
                }
            }
            //VEL
            if (AGCStorage.IlluminateVel > 0)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/lit/vel.png", UriKind.Relative);
                Vel.Source = new BitmapImage(uriSource);
            }
            else
            {
                if (AreWeDarkMode == true)
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                    Vel.Source = new BitmapImage(uriSource);
                }
                else
                {
                    var uriSource = new Uri(@"/AGC;component/images/lights/unlit/vel.png", UriKind.Relative);
                    Vel.Source = new BitmapImage(uriSource);
                }
            }*/
//Handles the remaining blank anun lights..
if (AreWeDarkMode==true)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                Unlit1.Source = new BitmapImage(uriSource);
                Unlit2.Source = new BitmapImage(uriSource);
                Vel.Source = new BitmapImage(uriSource);
                Alt.Source = new BitmapImage(uriSource);
            }
            else if (AreWeDarkMode==false)
            {
                var uriSource = new Uri(@"/AGC;component/images/lights/unlit/unlit.png", UriKind.Relative);
                Unlit1.Source = new BitmapImage(uriSource);
                Unlit2.Source = new BitmapImage(uriSource);
                Vel.Source = new BitmapImage(uriSource);
                Alt.Source = new BitmapImage(uriSource);
            }
        }

        //DESERIALIZE OUR JSON AND MOVE TO STORAGE
        public static void GetAGCdata()
        {
            string folderRelativeFromLocalAppData = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "LocalLow"));
            string fileName = folderRelativeFromLocalAppData + "//Wilhelmsen Studios//ReEntry//Export//Apollo//outputAGC.json";//Maybe subject to change in future ReEntry Updates

            try
            {
                string jsonString = File.ReadAllText(fileName);
                AGCValues AGCValues = JsonSerializer.Deserialize<AGCValues>(jsonString);
                AGCStorage.VerbD1 = AGCValues.VerbD1;
                AGCStorage.VerbD2 = AGCValues.VerbD2;
                AGCStorage.NounD1 = AGCValues.NounD1;
                AGCStorage.NounD2 = AGCValues.NounD2;
                AGCStorage.ProgramD1 = AGCValues.ProgramD1;
                AGCStorage.ProgramD2 = AGCValues.ProgramD2;
                AGCStorage.Register1D1 = AGCValues.Register1D1;
                AGCStorage.Register1D2 = AGCValues.Register1D2;
                if (String.IsNullOrEmpty(AGCValues.Register1D3)) { AGCStorage.Register1D3 = " "; }
                else { AGCStorage.Register1D3 = AGCValues.Register1D3; }
                AGCStorage.Register1D4 = AGCValues.Register1D4;
                AGCStorage.Register1D5 = AGCValues.Register1D5;
                AGCStorage.Register1Sign = AGCValues.Register1Sign;
                AGCStorage.Register2D1 = AGCValues.Register2D1;
                AGCStorage.Register2D2 = AGCValues.Register2D2;
                if (String.IsNullOrEmpty(AGCValues.Register2D3)) { AGCStorage.Register2D3 = " "; }
                else { AGCStorage.Register2D3 = AGCValues.Register2D3; }
                AGCStorage.Register2D4 = AGCValues.Register2D4;
                AGCStorage.Register2D5 = AGCValues.Register2D5;
                AGCStorage.Register2Sign = AGCValues.Register2Sign;
                AGCStorage.Register3D1 = AGCValues.Register3D1;
                AGCStorage.Register3D2 = AGCValues.Register3D2;
                if (String.IsNullOrEmpty(AGCValues.Register3D3)) { AGCStorage.Register3D3 = " "; }
                else { AGCStorage.Register3D3 = AGCValues.Register3D3; }
                AGCStorage.Register3D4 = AGCValues.Register3D4;
                AGCStorage.Register3D5 = AGCValues.Register3D5;
                AGCStorage.Register3Sign = AGCValues.Register3Sign;
                AGCStorage.IlluminateCompLight = AGCValues.IlluminateCompLight;
                AGCStorage.IlluminateTemp = AGCValues.IlluminateTemp;
                AGCStorage.IlluminateGimbalLock = AGCValues.IlluminateGimbalLock;
                AGCStorage.IlluminateProg = AGCValues.IlluminateProg;
                AGCStorage.IlluminateRestart = AGCValues.IlluminateRestart;
                AGCStorage.IlluminateTracker = AGCValues.IlluminateTracker;
                //AGCStorage.IlluminateAlt = AGCValues.IlluminateAlt;
                //AGCStorage.IlluminateVel = AGCValues.IlluminateVel;
                AGCStorage.IlluminateUplinkActy = AGCValues.IlluminateUplinkActy;
                AGCStorage.IlluminateNoAtt = AGCValues.IlluminateNoAtt;
                AGCStorage.IlluminateStby = AGCValues.IlluminateStby;
                AGCStorage.IlluminateKeyRel = AGCValues.IlluminateKeyRel;
                AGCStorage.IlluminateOprErr = AGCValues.IlluminateOprErr;
            }
#if DEBUG
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                Debug.WriteLine("There is a sharing violation.");
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 80)
            {
                Debug.WriteLine("The file already exists.");
            }
            catch (IOException e)
            {
                Debug.WriteLine($"An exception occurred:\nError code: " +
                                  $"{e.HResult & 0x0000FFFF}\nMessage: {e.Message}");
            }
#endif
            catch (Exception e)
            { 
                
            }
        }

        //POWER BUTTON - CLICK
        //GET AGC/LGC DATA & UPDATE THE RENDERER EVERY 100 ms.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Task.Run(() =>
            {
                while (true)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            GetAGCdata();
                            UpdateStoredValues();
                        }
                        catch (IOException ex)
                        { }
                        catch (Exception ex) 
                        { }
                    });
                    Task.Delay(100).Wait();//Testing 100ms
                }
            });
        }

        //This will handle the initial darkmode click and setup the boolean
        private void DarkMode_Click(object sender, RoutedEventArgs e)
        {
            if (AreWeDarkMode == false) { AreWeDarkMode = true; }
            else { AreWeDarkMode = false; }
            if (AreWeDarkMode == true)
            {
                //var uriSource = new Uri(@"/ExternalViewer/AGC/agc/images/agc-bg-darkLGC.png", UriKind.Relative);//This is a bug.. ImageSource needs to direct to the resource not this resolved file location..
                //pack://application:,,,/WpfApplication1;component/Resources/CINTRA2016.png
                var uriSource = new Uri(@"pack://application:,,,/AGC;component/images/agc-bg-dark.png", UriKind.Absolute);//Test to resolve above bug present in AGC/LGC builds..
                MainBackground.ImageSource = new BitmapImage(uriSource);
                var uriSource1 = new Uri(@"/AGC;component/images/lights/unlit/unlit-darkmode.png", UriKind.Relative);
                UplinkActy.Source = new BitmapImage(uriSource1);
                Alt.Source = new BitmapImage(uriSource1);
                Gimballock.Source = new BitmapImage(uriSource1);
                KeyRel.Source = new BitmapImage(uriSource1);
                NoAtt.Source = new BitmapImage(uriSource1);
                OprErr.Source = new BitmapImage(uriSource1);
                Restart.Source = new BitmapImage(uriSource1);
                Stby.Source = new BitmapImage(uriSource1);
                Temp.Source = new BitmapImage(uriSource1);
                Tracker.Source = new BitmapImage(uriSource1);
                Vel.Source = new BitmapImage(uriSource1);
                Program.Source = new BitmapImage(uriSource1);
                Unlit1.Source = new BitmapImage(uriSource1);
                Unlit2.Source = new BitmapImage(uriSource1);
            }
            else
            {
                var uriSource = new Uri(@"pack://application:,,,/AGC;component/images/agc-bg.png", UriKind.Absolute);
                MainBackground.ImageSource = new BitmapImage(uriSource);
                var uriSource1 = new Uri(@"/AGC;component/images/lights/unlit/uplinkacty.png", UriKind.Relative);
                UplinkActy.Source = new BitmapImage(uriSource1);
                var uriSource2 = new Uri(@"/AGC;component/images/lights/unlit/noatt.png", UriKind.Relative);
                NoAtt.Source = new BitmapImage(uriSource2);
                var uriSource3 = new Uri(@"/AGC;component/images/lights/unlit/unlit.png", UriKind.Relative);
                Alt.Source = new BitmapImage(uriSource3);
                var uriSource4 = new Uri(@"/AGC;component/images/lights/unlit/gimballock.png", UriKind.Relative);
                Gimballock.Source = new BitmapImage(uriSource4);
                var uriSource5 = new Uri(@"/AGC;component/images/lights/unlit/keyrel.png", UriKind.Relative);
                KeyRel.Source = new BitmapImage(uriSource5);
                var uriSource6 = new Uri(@"/AGC;component/images/lights/unlit/oprerr.png", UriKind.Relative);
                OprErr.Source = new BitmapImage(uriSource6);
                var uriSource7 = new Uri(@"/AGC;component/images/lights/unlit/prog.png", UriKind.Relative);
                Program.Source = new BitmapImage(uriSource7);
                var uriSource8 = new Uri(@"/AGC;component/images/lights/unlit/restart.png", UriKind.Relative);
                Restart.Source = new BitmapImage(uriSource8);
                var uriSource9 = new Uri(@"/AGC;component/images/lights/unlit/stby.png", UriKind.Relative);
                Stby.Source = new BitmapImage(uriSource9);
                var uriSource10 = new Uri(@"/AGC;component/images/lights/unlit/temp.png", UriKind.Relative);
                Temp.Source = new BitmapImage(uriSource10);
                var uriSource11 = new Uri(@"/AGC;component/images/lights/unlit/tracker.png", UriKind.Relative);
                Tracker.Source = new BitmapImage(uriSource11);
                var uriSource12 = new Uri(@"/AGC;component/images/lights/unlit/unlit.png", UriKind.Relative);
                Vel.Source = new BitmapImage(uriSource12);
                var uriSource13 = new Uri(@"/AGC;component/images/lights/unlit/unlit.png", UriKind.Relative);
                Unlit1.Source = new BitmapImage(uriSource13);
                Unlit2.Source = new BitmapImage(uriSource13);
            }
        }

        //call these from buttons to send the ReEntry UDP API cmds
        private void Pro_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGCPro();
        }

        private void Verb_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGCVerb();
        }

        private void KeyNoun_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGCNoun();
        }

        private void Key3_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGC3();
        }

        private void Key2_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGC2();
        }

        private void Key1_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGC1();
        }

        private void Key0_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGC0();
        }

        private void KeyRSET_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGCRset();
        }

        private void KeyKeyRel_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGCKeyRel();
        }

        private void KeyEntr_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGCEntr();
        }

        private void KeyClear_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGCClear();
        }

        private void KeyMinus_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGCMinus();
        }

        private void Key4_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGC4();
        }

        private void Key5_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGC5();
        }

        private void Key6_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGC6();
        }

        private void Key7_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGC7();
        }

        private void Key8_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGC8();
        }

        private void Key9_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGC9();
        }

        private void KeyPluss_Click(object sender, RoutedEventArgs e)
        {
            StoredCmds.AGCPlus();
        }
    }

    //values to store the serialized data
    public class AGCStorage
    {
        public static bool IsInCM;
        //public static bool IsInLM;
        public static string ProgramD1;
        public static string ProgramD2;
        public static string VerbD1;
        public static string VerbD2;
        public static string NounD1;
        public static string NounD2;
        public static string Register1D1;
        public static string Register1D2;
        public static string Register1D3;
        public static string Register1D4;
        public static string Register1D5;
        public static string Register1Sign;
        public static string Register2D1;
        public static string Register2D2;
        public static string Register2D3;
        public static string Register2D4;
        public static string Register2D5;
        public static string Register2Sign;
        public static string Register3D1;
        public static string Register3D2;
        public static string Register3D3;
        public static string Register3D4;
        public static string Register3D5;
        public static string Register3Sign;
        public static bool IlluminateCompLight;
        public static int IlluminateUplinkActy;
        public static int IlluminateNoAtt;
        public static int IlluminateStby;
        public static int IlluminateKeyRel;
        public static int IlluminateOprErr;
        public static int IlluminateTemp;
        public static int IlluminateGimbalLock;
        public static int IlluminateProg;
        public static int IlluminateRestart;
        public static int IlluminateTracker;
        //public static int IlluminateAlt;
        //public static int IlluminateVel;
    }

    //Set of values for serializing the jsons
    public class AGCValues
    {
        public bool IsInCM { get; set; }
        //public bool IsInLM { get; set; }
        public string ProgramD1 { get; set; }
        public string ProgramD2 { get; set; }
        public string VerbD1 { get; set; }
        public string VerbD2 { get; set; }
        public string NounD1 { get; set; }
        public string NounD2 { get; set; }
        public string Register1D1 { get; set; }
        public string Register1D2 { get; set; }
        public string Register1D3 { get; set; }
        public string Register1D4 { get; set; }
        public string Register1D5 { get; set; }
        public string Register1Sign { get; set; }
        public string Register2D1 { get; set; }
        public string Register2D2 { get; set; }
        public string Register2D3 { get; set; }
        public string Register2D4 { get; set; } 
        public string Register2D5 { get; set; }
        public string Register2Sign { get; set; }
        public string Register3D1 { get; set; }
        public string Register3D2 { get; set; }
        public string Register3D3 { get; set; }
        public string Register3D4 { get; set; }
        public string Register3D5 { get; set; }
        public string Register3Sign { get; set; }
        public bool IlluminateCompLight { get; set; }
        public int IlluminateUplinkActy { get; set; }
        public int IlluminateNoAtt { get; set; }
        public int IlluminateStby { get; set; }
        public int IlluminateKeyRel { get; set; }
        public int IlluminateOprErr { get; set; }
        public int IlluminateTemp { get; set; }
        public int IlluminateGimbalLock { get; set; }
        public int IlluminateProg { get; set; }
        public int IlluminateRestart { get; set; }
        public int IlluminateTracker { get; set; }
        //public int IlluminateAlt { get; set; }
        //public int IlluminateVel { get; set; }
    }
}
