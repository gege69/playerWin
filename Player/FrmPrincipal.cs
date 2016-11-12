using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using Newtonsoft.Json;
using RestSharp;
using System.Net;


namespace Player
{
    public partial class Frm_Principal : Form
    {
        public Frm_Principal()
        {
            InitializeComponent();

            //recebe o valor inicial programado do volume
            trbVolume.Value = 100;
            //carrega o form full
            this.WindowState = FormWindowState.Maximized;     
        }
        string[] files, paths;
        int volume = 100;
        // Começa em 0
        // Valores são:
        // 0 = pause;
        // 1 = play;
        // 2 = stop;

        //Contole das músicas e locução
        int statusMusic = 0;
        int RetMusic = 0;
        int statusLocucao = 0;

        private void btnStart_Click(object sender, EventArgs e)
        {
            MidiaPlayer.Ctlcontrols.play();
            statusMusic = 1;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            MidiaPlayer.Ctlcontrols.pause();
            statusMusic = 0;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            MidiaPlayer.Ctlcontrols.stop();
            statusMusic = 2;
        }

        private void btn_list_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();

            OpenFileDialog1.Multiselect = true;

            if (OpenFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
            {
                files = OpenFileDialog1.SafeFileNames;
                paths = OpenFileDialog1.FileNames;
                
                listBox.Items.Clear(); //Limpa a lista sempre antes de adicionar outras músicas
                MidiaPlayer.Ctlcontrols.stop(); // Faz o player parar quando selecionar uma nova pasta

                for (int i = 0; i < files.Length; i++)
                {
                    listBox.Items.Add(files[i]); //Adiciona as músicas na lista
                }

            }

            //Display frmAbout as a modal dialog
            //Form frmAbout = new Form();
            //frmAbout.ShowDialog();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MidiaPlayer.URL = paths[listBox.SelectedIndex];

            //música em play
            statusMusic = 1;
        }

        private void MidiaPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (MidiaPlayer.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                timer1.Interval = 100;
                timer1.Enabled = true;
            }  
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex < files.Length - 1)
            {
                listBox.SelectedIndex++; //Pula de música assim que acabar a anterior
                timer1.Enabled = false;
            }
            else
            {
                listBox.SelectedIndex = 0; //Se não houver mais música na lista volta para o começo
                timer1.Enabled = false;
            } 
        }

        private void MidiaPlayer_Enter(object sender, EventArgs e)
        {
            MidiaPlayer.uiMode = "invisible";
            MidiaPlayer.settings.volume = volume;
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            try
            {

                if (listBox.SelectedIndex < files.Length - 1)
                    listBox.SelectedIndex++; //Pula de música assim que acabar a anterior
                else
                    listBox.SelectedIndex = 0; //Se não houver mais música na lista volta para o começo
            }
            catch
            {
                Console.WriteLine("Sem música");
            }
        }

        private void btn_ant_Click(object sender, EventArgs e)
        {
            try
            {

                if (listBox.SelectedIndex < files.Length - 1)
                    listBox.SelectedIndex--; //Pula de música assim que acabar a anterior
                else
                    listBox.SelectedIndex = 0; //Se não houver mais música na lista volta para o começo
            }
            catch
            {
                Console.WriteLine("Sem música");
            }
        }

        private void desconectarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void testeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChamadaFunc chamafunc = new ChamadaFunc();
            chamafunc.ShowDialog();
        }

        private void MidiaPlayerChamada_Enter(object sender, EventArgs e)
        {
            MidiaPlayerChamada.uiMode = "invisible";
        }

        private void trbVolume_Scroll(object sender, EventArgs e)
        {
            MidiaPlayer.settings.volume = trbVolume.Value;            
        }

        private void btnChamada_Click(object sender, EventArgs e)
        {
            // inicia o timer
            timerLocucao.Start();
            //mostra o label
            lblLocucao.Visible = true;
            // realiza o controle das musicas
            ControlaMusica(statusMusic);

            WindowsMicrophoneMuteLibrary.WindowsMicMute micMute = new WindowsMicrophoneMuteLibrary.WindowsMicMute();
            micMute.UnMuteMic();
        }

        private void timerLocucao_Tick(object sender, EventArgs e)
        {
            string vlFinal = "01:00";
            
            WindowsMicrophoneMuteLibrary.WindowsMicMute micMute = new WindowsMicrophoneMuteLibrary.WindowsMicMute();

            if (statusLocucao < 60)
            {
                statusLocucao = statusLocucao + 1;                
                lblLocucao.Text = "Parar Locução \n" + TimeSpan.FromSeconds(statusLocucao).ToString("mm\\:ss") + " até " + vlFinal;
            }
            else
            {
                lblLocucao.Visible = false;
                lblLocucao.Text = "";
                //para a locução
                timerLocucao.Stop();
                statusLocucao = 0;
                micMute.UnMuteMic();
                //retorna a música caso esteja tocando
                RetornaMusica(RetMusic);                
            }

        }

        private void lblLocucao_Click(object sender, EventArgs e)
        {
            WindowsMicrophoneMuteLibrary.WindowsMicMute micMute = new WindowsMicrophoneMuteLibrary.WindowsMicMute();

            lblLocucao.Visible = false;
            lblLocucao.Text = "";
            //para a locução
            timerLocucao.Stop();
            statusLocucao = 0;
            micMute.MuteMic();
            //retorna a música caso esteja tocando
            RetornaMusica(RetMusic);
        }

        private void ControlaMusica(int controle)
        {
            if (controle == 1)
            {
                MidiaPlayer.Ctlcontrols.pause();
                string caminho = Path.GetFullPath(@"Sounds\sensui.wav");

                MidiaPlayerChamada.URL = caminho;

                MidiaPlayerChamada.Ctlcontrols.play();
                RetMusic = 1;
            }

            //música parada, stop ou pause
            if (statusMusic == 0 || statusMusic == 2)
            {
                RetMusic = 0;
            }
        }

        private void RetornaMusica(int controle)
        {
            if (controle == 1)
            {
                MidiaPlayer.Ctlcontrols.play();
            }

        }

        private void proprietárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProprietarioVeiculo proprietarioVeiculo = new ProprietarioVeiculo();
            proprietarioVeiculo.ShowDialog();
        }

        private void chamdaInstanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChamadaInstantanea chamadaInstant = new ChamadaInstantanea();
            chamadaInstant.ShowDialog();
        }

        private void horóscopoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Horoscopo horoscopoMensal = new Horoscopo();
            horoscopoMensal.ShowDialog();
        }

        private void comerciaisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Comercial comercial = new Comercial();
            comercial.ShowDialog();
        }

        private void institucionaisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Institucional institucional = new Institucional();
            institucional.ShowDialog();
        }

        private void programetesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Programetes programetes = new Programetes();
            programetes.ShowDialog();
        }

        private void eventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Eventos evento = new Eventos();
            evento.ShowDialog();
        }

        private void testeToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Genero genero = new Genero();
            genero.ShowDialog();
        }

        private void blocosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Blocos blocos = new Blocos();
            blocos.ShowDialog();
        }

        private void downloadsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Download download = new Download();
            download.ShowDialog();
        }

        private void atendimentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Atendimento atendiment = new Atendimento();
            atendiment.ShowDialog();
        }

        private void relatóriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Relatorios relatorio = new Relatorios();
            relatorio.ShowDialog();
        }

        private void noBreakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Nobreak nobreak = new Nobreak();
            nobreak.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            var client = new RestClient("http://fisica.ufpr.br/kurumin");
            var request = new RestRequest("kurumin-light-7.0.iso", Method.GET);
            byte[] x = client.DownloadData(request);

            try
            {
                File.WriteAllBytes(@"C:\Users\pazin\teste\doido.txt", x);
            }
            catch (Exception)
            {
                throw;
            }
            */

            //string tempFile = Path.GetTempFileName();
            /*
            using (var writer = File.OpenWrite(@"C:\Users\pazin\teste\yahoo.iso"))
            {
                var client = new RestClient("https://download.documentfoundation.org/libreoffice/stable/4.4.5/deb/x86");
                var request = new RestRequest("LibreOffice_4.4.5_Linux_x86_deb_helppack_am.tar.gz");
                request.ResponseWriter = (responseStream) => responseStream.CopyTo(writer);
                var response = client.DownloadData(request);

            }
            */

            Byte[] result = new WebClient().DownloadData("https://download.documentfoundation.org/libreoffice/stable/4.4.5/deb/x86/LibreOffice_4.4.5_Linux_x86_deb_helppack_am.tar.gz");
            File.WriteAllBytes(@"C:\Users\pazin\teste\helppack.tar.gz", result);
        }

        private void Frm_Principal_FormClosing(object sender, FormClosingEventArgs e)
        {
            //quando fechar a aplicação no "X" para encerrar e tirar da memória
            Application.Exit();
        }
    }
}
